using AutoMapper;
using Financiera.Application.DTO.Cuota;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Service
{
    public class CuotaService : ICuotaService
    {
        private readonly ICuotasRepository _repository;
        private readonly IMapper _mapper;

        public CuotaService(ICuotasRepository repository ,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CuotaDto> ActualizarAsync(int id, CuotaActualizarDto cuota)
        {
            var registro = await _repository.obtenerPorIdAsync(id);
            if(registro == null )
                throw new KeyNotFoundException("El registo no existe o fue eliminado");

            _mapper.Map(cuota, registro);
            await _repository.ActualizarAsync(registro);
            return _mapper.Map<CuotaDto>(registro);
        }

        public async Task<int> ContarAsync()
        {
            return await _repository.ContarAsync();
        }

        public async Task<int> ContarBusquedaAsync(int valor)
        {
            return await _repository.ContarBusquedaAsync(valor);
        }
        public async Task<CuotaDto> CrearAsync(CuotaCrearDto cuota)
        {
            var siExiste = await _repository.ExisteCuotaAsync(cuota.NumCuota);
            if (siExiste)
                throw new KeyNotFoundException($"Ya Existe un resgistro con el numero de cuota: {cuota.NumCuota}.");

            var registro = _mapper.Map<Cuota>(cuota);
            await _repository.CrearAsync(registro);

            return _mapper.Map<CuotaDto>(registro);
        }

        public async Task<CuotaDto?> obtenerPorIdAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("El ID debe ser un numero mayor a cero.");
            var registro = await _repository.obtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado");
            return _mapper.Map<CuotaDto>(registro);
        }

        public async Task<ICollection<CuotaDto>> ObtenerPorIdPrestamoAsync(int IdPrestamos, int pagina, int tamano)
        {
            var registros = await _repository
                .ObtenerPorIdPrestamoAsync(IdPrestamos, pagina, tamano);

            return _mapper.Map<List<CuotaDto>>(registros);
        }
        
        public async Task<IEnumerable<CuotaDto>> ObtenerTodasAsync(int pagina, int tamano)
        {
            var registros = await _repository.ObtenerTodasAsync(pagina, tamano);
            return _mapper.Map<IEnumerable<CuotaDto>>(registros);
        }
    }
}
