
using AutoMapper;
using Financiera.Application.DTO.Usuarios;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financiera.Application.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepositoty _repositoty;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuarios> _userManager;

        public UsuarioService(IUsuarioRepositoty repositoty, IMapper mapper, UserManager<Usuarios> userManager)
        {
            _repositoty = repositoty;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<int> ContarAsync()
        {
            return await _repositoty.ContarAsync();
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser un número entero mayor a cero.");
            }

            var registro = await _repositoty.ObtenerPorIdAsync(id);
            if (registro == null)
            {
                throw new ArgumentException("Usuario no encontrado.");
            }

            // 1. Mapeamos la entidad base al DTO
            var usuarioDto = _mapper.Map<UsuarioDto>(registro);

            // 2. Buscamos el rol usando UserManager de Identity
            var roles = await _userManager.GetRolesAsync(registro);

            // 3. Asignamos el primer rol encontrado (o una cadena vacía si no tiene)
            usuarioDto.Rol = roles.FirstOrDefault() ?? "Sin Rol";

            return usuarioDto;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int Pagina, int Tamano)
        {
            var registros = await _repositoty.ObtenerTodosAsync(Pagina, Tamano);
            return _mapper.Map<IEnumerable<UsuarioDto>>(registros);
        }
    }
}
