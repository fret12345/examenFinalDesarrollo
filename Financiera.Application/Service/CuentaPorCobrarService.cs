using AutoMapper;
using Financiera.Application.DTO.CuentasPorCobrar;
using Financiera.Application.DTO.Cuota;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Financiera.Application.Service
{
    public class CuentaPorCobrarService : ICuentasPorCobrarService
    {
        private readonly ICuetnasPorCobrarRepository _repository;
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IMapper _mapper;
        
        public CuentaPorCobrarService(ICuetnasPorCobrarRepository repository, IPrestamoRepository prestamoRepository ,IMapper mapper)
        {
            _repository = repository;
            _prestamoRepository = prestamoRepository;
            _mapper = mapper;
        }

        public async Task<CuentasPorCobrarDto> ActualizarAsync(int id, CuentasPorCobrarActualizarDto Dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if(registro == null)
                throw new KeyNotFoundException("El registo no existe o fue eliminado");
            _mapper.Map(Dto, registro);
            await _repository.ActualizarAsync(registro);
            return _mapper.Map<CuentasPorCobrarDto>(registro);

        }

        public async Task<int> ContarAsync()
        {
            return await _repository.ContarAsync();
        }

        public async Task<int> ContarBusquedaAsync(int valor)
        {
            return await _repository.ContarBusquedaAsync(valor);
        }

        public async Task<CuentasPorCobrarDto> CrearAsync(CuentasPorCobrarCrearDto Dto)
        {
            // 1. Validamos si ya existe una cuenta asociada a ese préstamo
            var siExiste = await _repository.ExistePrestamo(Dto.IdPrestammo);
            if (siExiste)
                throw new KeyNotFoundException($"Ya existe un registro con el número de cuota: {Dto.IdPrestammo}");

            // 2. Vamos a buscar el préstamo original a la base de datos para extraer los montos reales
            var DatosPrestamo = await _prestamoRepository.ObtenerPorIdAsync(Dto.IdPrestammo);
            if (DatosPrestamo == null)
                throw new KeyNotFoundException("El préstamo especificado no existe.");

            // 3. AutoMapper inicializa la entidad con el IdPrestammo y los demás campos vacíos del DTO
            var registro = _mapper.Map<CuentasPorCobrar>(Dto);

            // 4. ¡Automatización directa! Inyectamos los montos que extrajimos del préstamo
            registro.MontoTotal = DatosPrestamo.TotalAPAgar;
            registro.SaldoPendiente = DatosPrestamo.TotalAPAgar;
            registro.Estado = true; // Dejamos la cuenta activa por defecto

            // 5. Guardamos la entidad completa en la base de datos
            await _repository.CrearAsync(registro);

            // 6. Retornamos el DTO de respuesta con los datos calculados visibles
            return _mapper.Map<CuentasPorCobrarDto>(registro);
        }

        public async Task<CuentasPorCobrarDto?> obtenerPorIdAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("El ID debe ser un numero mayor a cero.");
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");
            return _mapper.Map<CuentasPorCobrarDto>(registro);
        }

        public async Task<ICollection<CuentasPorCobrarDto>> ObtenerPorIdPrestamoAsync(int IdPrestamos, int pagina, int tamano)
        {
            var registros = await _repository
                .ObtenerPorNumPrestamoAsync(IdPrestamos, pagina, tamano);

            return _mapper.Map<List<CuentasPorCobrarDto>>(registros);
        }

        public async Task<IEnumerable<CuentasPorCobrarDto>> ObtenerTodasAsync(int pagina, int tamano)
        {
            var registros = await _repository.ObtenerTodasAsync(pagina, tamano);
            return _mapper.Map<IEnumerable<CuentasPorCobrarDto>>(registros);
        }
    }
}
