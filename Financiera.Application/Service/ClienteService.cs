using AutoMapper;
using Financiera.Application.DTO.Clientes;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Service
{
    public class ClienteService: IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClienteDto> ActualizarAsync(int id, ClienteActualizarDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registo fue eliminado");

            var nuevoNombre = dto.Nombres.Trim();


            //Validar duplicados solamente si el nombre cambio.
            if (string.Equals(registro.Nombres.Trim(), nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExisteNombreAsync(nuevoNombre);
                if (siExiste)
                    throw new KeyNotFoundException($"Ya existe un registro con el Titulo: {dto.Nombres}.");
            }



            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);
            return _mapper.Map<ClienteDto>(registro);
        }

        public async Task<IEnumerable<ClienteDto>> BuscarClienteAsync(string nombre)
        {
            var registros = await _repository.BuscarClienteAsync(nombre);

            return _mapper.Map<IEnumerable<ClienteDto>>(registros);
        }

        public async Task<ClienteDto> CrearAsync(ClienteCrearDto dto)
        {
            var siExiste = await _repository.ExisteNombreAsync(dto.Nombres);
            if (siExiste)
                throw new KeyNotFoundException($"Ya existe un registro con el Titulo: {dto.Nombres}.");

            var registro = _mapper.Map<Clientes>(dto);
            await _repository.CrearAsync(registro);

            return _mapper.Map<ClienteDto>(registro);
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);

            if (registro == null)
                throw new KeyNotFoundException("El registo no existe o fue eliminado");

            if (registro.Nombres.Any())
                throw new InvalidOperationException($"No se puede eliminar la categoria: {registro.Rutas} porque tiene peliculas" +
                    $"asociadas");
            await _repository.EliminarAsync(id);
        }

        public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser u numero mayor a cero");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registo no existe o fue eliminado");


            return _mapper.Map<ClienteDto>(registro);
        }

        public async Task<IEnumerable<ClienteDto>> ObtenerTodasAsync()
        {
            var registros = await _repository.ObtenerTodasAsync();

            return _mapper.Map<IEnumerable<ClienteDto>>(registros);
        }
    }
}
