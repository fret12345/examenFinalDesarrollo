using AutoMapper;
using Financiera.Application.DTO.Cuota;
using Financiera.Application.DTO.Pagos;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Service
{
    public class PagosService : IPagosService
    {
        private readonly IPagosRepository _repository;
        private readonly ICuotasRepository _Cuotarepository;
        private readonly ICuetnasPorCobrarRepository _CuentasPorCobrarrepository;
        private readonly IMapper _mapper;

        public PagosService(IPagosRepository repository, ICuotasRepository cuotasRepository, ICuetnasPorCobrarRepository cuetnasPorCobrarRepository, IMapper mapper)
        {
            _repository = repository;
            _Cuotarepository = cuotasRepository;
            _CuentasPorCobrarrepository = cuetnasPorCobrarRepository;
            _mapper = mapper;
        }

        public async Task<PagosDto> ActualizarAsync(int id, PagosActualizarDto dto)
        {
            var registro = await _repository.obtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registo no existe o fue eliminado");

            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);
            return _mapper.Map<PagosDto>(registro);
        }

        public async Task<int> ContarAsync()
        {
            return await _repository.ContarAsync();
        }

        public async Task<int> ContarBusquedaAsync(int valor)
        {
            return await _repository.ContarBusquedaAsync(valor);
        }

        public async Task<PagosDto> CrearAsync(PagosCrearDto dto)
        {
            // 1. Validar si la cuota ya fue pagada anteriormente
            var yaFuePagada = await _repository.ExisteCuotaAsync(dto.IdCuota);
            if (yaFuePagada)
            {
                throw new InvalidOperationException($"La cuota con ID {dto.IdCuota} ya cuenta con un pago registrado.");
            }

            // 2. Obtener los datos de la cuota desde su repositorio
            var datosCuota = await _Cuotarepository.obtenerPorIdAsync(dto.IdCuota);
            if (datosCuota == null)
            {
                throw new KeyNotFoundException($"La cuota con ID {dto.IdCuota} no existe.");
            }

            // 3. 📅 Validación de fechas y cálculo de mora
            DateTime fechaHoy = DateTime.Now.Date;
            DateTime fechaVencimiento = datosCuota.FechaVensimiento.ToDateTime(TimeOnly.MinValue);

            int diasAtraso = 0;
            decimal montoMora = 0;

            if (fechaHoy > fechaVencimiento)
            {
                diasAtraso = (fechaHoy - fechaVencimiento).Days;
                decimal porcentajeMoraDiario = 0.01m;
                montoMora = datosCuota.MontoCuota * porcentajeMoraDiario * diasAtraso;
            }

            // 4. Calcular el monto EXACTO requerido (Cuota original + Mora del día)
            decimal totalDeudaCuota = datosCuota.MontoCuota + montoMora;

            // 🪙 Redondeamos ambos montos a 2 decimales para evitar problemas con fracciones como 133.33333
            decimal montoRecibidoRedondeado = Math.Round(dto.MontoRecibido, 2);
            decimal totalDeudaRedondeado = Math.Round(totalDeudaCuota, 2);

            // 🛑 REGLA DE NEGOCIO: Comparación segura de montos exactos redondeados
            if (montoRecibidoRedondeado != totalDeudaRedondeado)
            {
                throw new InvalidOperationException(
                    $"El monto recibido ({montoRecibidoRedondeado:0.00}) no coincide con el total requerido " +
                    $"de la cuota más la mora acumulada ({totalDeudaRedondeado:0.00}). Se debe pagar la cantidad exacta."
                );
            }

            // 5. Actualizar los campos de la entidad Cuota
            datosCuota.DiasAtraso = diasAtraso;
            datosCuota.MontoMora = montoMora;
            datosCuota.CobroMora = montoMora;
            datosCuota.Estado = false;         // 🚫 INACTIVO: Al pagarse exacta, sale del listado de pendientes

            // 6. Mapear y configurar las propiedades del nuevo Pago
            var pago = _mapper.Map<Pagos>(dto);
            pago.MontoCuota = datosCuota.MontoCuota;
            pago.SaldoPendiente = 0;
            pago.ImpuestoExtra = montoMora.ToString("0.00");
            pago.FechaPago = DateOnly.FromDateTime(fechaHoy);

            // Guardar registros principales en la base de datos
            await _repository.CrearAsync(pago);
            await _Cuotarepository.ActualizarAsync(datosCuota);

            // 7. 🏦 Actualizar los balances globales en CuentasPorCobrar
            // Nota: Usamos el nombre exacto de tu repositorio: _CuentasPorCobrarrepository (revisa mayúsculas/minúsculas según lo tengas declarado arriba)
            var cuentaPorCobrar = await _CuentasPorCobrarrepository.ObtenerPorIdPrestamoAsync(datosCuota.IdPrestamos);

            if (cuentaPorCobrar != null)
            {
                // 🔥 CORRECCIÓN: Usamos "TotalMoraAcumurada" con 'r' como está en tu base de datos
                cuentaPorCobrar.TotalImpuestoExtra += montoMora;

                // Actualizar el saldo pendiente global (Saldo anterior + Mora - Monto Pagado)
                cuentaPorCobrar.SaldoPendiente = (cuentaPorCobrar.SaldoPendiente + montoMora) - dto.MontoRecibido;

                // Si la cuenta por cobrar global llegó a cero, se marca como cancelada por completo
                if (cuentaPorCobrar.SaldoPendiente <= 0)
                {
                    cuentaPorCobrar.SaldoPendiente = 0;
                    cuentaPorCobrar.Estado = false; // Ajustado a booleano si tu campo Estado en CuentasPorCobrar es bool
                }

                await _CuentasPorCobrarrepository.ActualizarAsync(cuentaPorCobrar);
            }

            // Retornar el DTO de respuesta mapeado con los datos del pago insertado
            return _mapper.Map<PagosDto>(pago);
        }

        public async Task<PagosDto?> obtenerPorIdAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("El ID debe ser un numero mayor a cero.");
            var registro = await _repository.obtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");
            return _mapper.Map<PagosDto>(registro);
        }

        public async Task<ICollection<PagosDto>> ObtenerPorIdCuotaAsync(int IdCuota, int pagina, int tamano)
        {
            var registros = await _repository
                .ObtenerPorIdCuotaAsync(IdCuota, pagina, tamano);

            return _mapper.Map<List<PagosDto>>(registros);
        }

        public async Task<IEnumerable<PagosDto>> ObtenerTodasAsync(int pagina, int tamano)
        {
            var registros = await _repository.ObtenerTodasAsync(pagina, tamano);
            return _mapper.Map<IEnumerable<PagosDto>>(registros);
        }
    }
}
