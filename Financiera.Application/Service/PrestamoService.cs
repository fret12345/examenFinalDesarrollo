using AutoMapper;
using Financiera.Application.DTO.Clientes;
using Financiera.Application.DTO.Prestamo;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Service
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _repository;
        private readonly ICuotasRepository _repositoryCuotas;
        private readonly ICuetnasPorCobrarRepository _cuetnasPorCobrarRepository;
        private readonly IMapper _mapper;

        public PrestamoService(IPrestamoRepository repository, ICuotasRepository repositoryCuotas, ICuetnasPorCobrarRepository cuentasPorCobrarRepository, IMapper mapper)
        {
            _repository = repository;
            _repositoryCuotas = repositoryCuotas;
            _cuetnasPorCobrarRepository = cuentasPorCobrarRepository;
            _mapper = mapper;
        }

        public async Task<PrestamoDto> ActualizarAsync(int id, PrestamoActualizarDto prestamos)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registo fue eliminado");

            //var nuevoNumero = prestamos.NumPrestamos.Trim();


            ////Validar duplicados solamente si el nombre cambio.
            //if (string.Equals(registro.NumPrestamos.Trim(), nuevoNumero, StringComparison.OrdinalIgnoreCase))
            //{
            //    var siExiste = await _repository.ExistePrestamo(nuevoNumero);
            //    if (siExiste)
            //        throw new KeyNotFoundException($"Ya existe un registro con el Numero de prestamo: {prestamos.NumPrestamos}.");
            //}



            _mapper.Map(prestamos, registro);
            await _repository.ActualizarAsync(registro);
            return _mapper.Map<PrestamoDto>(registro);
        }

        //public async Task<PrestamoDto> ActualizarEstado(int id, PrestamoActualizarEstadoDto dto)
        //{
        //    var registro = await _repository.ObtenerPorIdAsync(id);
        //    if (registro == null)
        //        throw new KeyNotFoundException("El registro no existe o fue eliminado");

        //    // Guardamos el estado anterior para comparar más abajo
        //    string estadoAnterior = registro.EstadoPrestamo?.Trim() ?? "";
        //    string nuevoEstado = dto.EstadoPrestamo.ToString().Trim();

        //    // Validar duplicados solamente si el nombre cambió.
        //    if (!string.Equals(estadoAnterior, nuevoEstado, StringComparison.OrdinalIgnoreCase))
        //    {
        //        var siExiste = await _repository.ExistePrestamo(nuevoEstado);
        //        if (siExiste)
        //            throw new KeyNotFoundException($"El registro ya tiene asignado el estado: {nuevoEstado}.");
        //    }

        //    // Actualizamos el estado en el registro
        //    registro.EstadoPrestamo = nuevoEstado;
        //    await _repository.ActualizarEstado(registro);

        //    // CRUCIAL: Solo generamos cuotas si el estado NUEVO es "Aprovado" 
        //    // Y el estado ANTERIOR NO era "Aprovado" (Evita duplicados al actualizar de nuevo)
        //    if (string.Equals(nuevoEstado, "Aprovado", StringComparison.OrdinalIgnoreCase) &&
        //        !string.Equals(estadoAnterior, "Aprovado", StringComparison.OrdinalIgnoreCase))
        //    {
        //        // No necesitas volver a ir a la base de datos con 'ObtenerPorIdAsync' 
        //        // porque ya tienes toda la información en la variable 'registro'
        //        var CantidadTotal = registro.TotalAPAgar;
        //        var PlasoMEses = registro.PladoMeses;

        //        if (PlasoMEses > 0)
        //        {
        //            decimal TotalCuotas = CantidadTotal / PlasoMEses;

        //            for (int i = 0; i < PlasoMEses; i++)
        //            {
        //                Cuota nuevaCuota = new Cuota
        //                {
        //                    IdPrestamos = registro.Id,
        //                    NumCuota = i + 1,
        //                    MontoCuota = TotalCuotas,
        //                    FechaVensimiento = DateOnly.FromDateTime(DateTime.Now.AddMonths(i + 1)),
        //                    CobroMora = 0,
        //                    DiasAtraso = 0,
        //                    MontoMora = 0,
        //                    Estado = true // Asegúrate de setear el estado en true si tu base de datos lo pide
        //                };
        //                await _repositoryCuotas.CrearAsync(nuevaCuota);
        //            }
        //        }
        //    }

        //    return _mapper.Map<PrestamoDto>(registro);
        //}

        public async Task<PrestamoDto> ActualizarEstado(int id, PrestamoActualizarEstadoDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");

            // Guardamos el estado anterior para comparar más abajo
            string estadoAnterior = registro.EstadoPrestamo?.Trim() ?? "";
            string nuevoEstado = dto.EstadoPrestamo.ToString().Trim();

            // Validar duplicados solamente si el nombre cambió.
            if (!string.Equals(estadoAnterior, nuevoEstado, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExistePrestamo(nuevoEstado);
                if (siExiste)
                    throw new KeyNotFoundException($"El registro ya tiene asignado el estado: {nuevoEstado}.");
            }

            // Actualizamos el estado en el registro de Préstamo
            registro.EstadoPrestamo = nuevoEstado;
            await _repository.ActualizarEstado(registro);

            // CRUCIAL: Si el estado cambia a "Aprobado", disparamos la automatización
            if (string.Equals(nuevoEstado, "Aprovado", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(estadoAnterior, "Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                // =========================================================================
                // PASO A: CREAR AUTOMÁTICAMENTE LA CUENTA POR COBRAR
                // =========================================================================
                CuentasPorCobrar nuevaCuenta = new CuentasPorCobrar
                {
                    IdPrestammo = registro.Id, // Mantenemos tu propiedad con doble 'm'
                    MontoTotal = registro.TotalAPAgar, // Extraemos directo del préstamo aprobado
                    SaldoPendiente = registro.TotalAPAgar, // Inicia debiendo el total del préstamo
                    TotalMoraAcumuada = 0,
                    TotalImpuestoExtra = 0,
                    Estado = true
                };

                // Guardamos la cuenta por cobrar en su respectivo repositorio
                // REVISÁ: Asegurate de que el nombre de este repositorio coincida con el que declaraste arriba
                await _cuetnasPorCobrarRepository.CrearAsync(nuevaCuenta);


                // =========================================================================
                // PASO B: GENERACIÓN DE CUOTAS (Tu lógica original optimizada)
                // =========================================================================
                var CantidadTotal = registro.TotalAPAgar;
                var PlasoMEses = registro.PladoMeses; // Tu propiedad original 'PladoMeses'

                if (PlasoMEses > 0)
                {
                    decimal TotalCuotas = (decimal)CantidadTotal / PlasoMEses;

                    for (int i = 0; i < PlasoMEses; i++)
                    {
                        Cuota nuevaCuota = new Cuota
                        {
                            IdPrestamos = registro.Id,
                            NumCuota = i + 1,
                            MontoCuota = TotalCuotas,
                            FechaVensimiento = DateOnly.FromDateTime(DateTime.Now.AddMonths(i + 1)),
                            CobroMora = 0,
                            DiasAtraso = 0,
                            MontoMora = 0,
                            Estado = true
                        };
                        await _repositoryCuotas.CrearAsync(nuevaCuota);
                    }
                }
            }

            return _mapper.Map<PrestamoDto>(registro);
        }

        public async Task<PrestamoDto> CrearAsync(PrestamoCrearDto prestamos)
        {
            var siExiste = await _repository.ExistePrestamo(prestamos.NumPrestamos);
            if (siExiste)
                throw new KeyNotFoundException($"Ya existe un registro con el Titulo: {prestamos.NumPrestamos}.");

            var registro = _mapper.Map<Prestamos>(prestamos);
            await _repository.CrearAsync(registro);

            return _mapper.Map<PrestamoDto>(registro);
        }

        public async Task<PrestamoDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser u numero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registo no existe o fue eliminado");


            return _mapper.Map<PrestamoDto>(registro);
        }

        public async Task<IEnumerable<PrestamoDto>> ObtenerPorNoPrestamoAsync(string Numero)
        {
            if (Numero == null)
                throw new ArgumentException("Ingrese el numero del prestamo.");

            var registro = await _repository.ObtenerPorNoPrestamoAsync(Numero);

            return _mapper.Map < IEnumerable<PrestamoDto>>(registro);
        }

        public async Task<IEnumerable<PrestamoDto>> ObtenerTodasAsync()
        {
            var registros = await _repository.ObtenerTodasAsync();

            return _mapper.Map<IEnumerable<PrestamoDto>>(registros);
        }
    }
}
