using AutoMapper;
using Financiera.Application.DTO.Rutas;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;


namespace Financiera.Application.Service
{
    public class RutasService : IRutasService
    {
        private readonly IRutasRepository _repository;
        private readonly IMapper _mapper;

        public RutasService(IRutasRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RutasDto> ActualizarAsync(int id, RutasActualizarDto dto)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado.");

            var nuevoNombre = dto.NombreRuta.Trim();

            // Validar duplicados solo si el nombre cambió
            if (!string.Equals(registro.NombreRuta.Trim(), nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                var siExiste = await _repository.ExisteNombreAsync(nuevoNombre);
                if (siExiste)
                    throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.NombreRuta}.'");
            }

            _mapper.Map(dto, registro);
            await _repository.ActualizarAsync(registro);

            return _mapper.Map<RutasDto>(registro);
        }

        public async Task<IEnumerable<RutasDto>> BuscarRutasAsync(string nombre)
        {
            var registros = await _repository.BuscarRutasAsync(nombre);

            return _mapper.Map<IEnumerable<RutasDto>>(registros);
        }

        public async Task<RutasDto> CrearAsync(RutasCrearDto dto)
        {
            var siExiste = await _repository.ExisteNombreAsync(dto.NombreRuta);
            if (siExiste)
                throw new InvalidOperationException($"Ya existe un registro con el nombre: '{dto.NombreRuta}.'");

            var registro = _mapper.Map<Rutas>(dto);
            await _repository.CrearAsync(registro);

            return _mapper.Map<RutasDto>(registro);
        }

        public async Task EliminarAsync(int id)
        {
            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado.");

            if (registro.Clientes.Any())
                throw new InvalidOperationException($"No se puede eliminar la categoría: '{registro.NombreRuta}' porque tiene películas asociadas.");

            await _repository.EliminarAsync(id);
        }

        public async Task<RutasDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser un número entero mayor a cero.");

            var registro = await _repository.ObtenerPorIdAsync(id);
            if (registro == null)
                throw new KeyNotFoundException("El registro no existe o fue eliminado.");

            return _mapper.Map<RutasDto>(registro);
        }

        public async Task<IEnumerable<RutasDto>> ObtenerTodasAsync()
        {
            var registros = await _repository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<RutasDto>>(registros);
        }
    }
}
