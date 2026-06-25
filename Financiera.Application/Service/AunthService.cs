
using AutoMapper;
using Financiera.Application.DTO.Usuarios;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Application.Response;
using Financiera.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Financiera.Application.Service
{
    public class AunthService : IAuthService
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refresRepo;
        private readonly IMapper _mapper;

        public AunthService(UserManager<Usuarios> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration config
            , IRefreshTokenRepository refresRepo, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _refresRepo = refresRepo;
            _mapper = mapper;
        }

        #region Metodo Privado

        private async Task<UsuarioDto> MapearUsuariosDtoAsync(Usuarios usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuarios);

            return new UsuarioDto
            {
                Id = Convert.ToString(usuarios.Id),
                NombreCompleto = usuarios.NombresCompleto,
                Email = usuarios.Email!,
                Rol = roles.FirstOrDefault() ?? "",
                PhonNumber = usuarios.PhoneNumber ?? ""
            };
        }

        private static void ValidarResultado(IdentityResult resultado, string mensajeError)
        {
            if(!resultado.Succeeded)
            {
                var Errores = string.Join(" | ", resultado.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"{mensajeError}: '{Errores}'");
            }
        }

        private string GenerarToken(Usuarios usuarios, string rol, DateTime expiracion)
        {
            //Leer la configuracion de variables de entorno
            var key = _config["JWT_kEY"]
                ?? throw new Exception("JWT_KEY no configurado");

            var issuer = _config["JWT_ISSUER"]
               ?? throw new Exception("JWT_ISSUER no configurado");

            var audience = _config["JWT_AUDIENCE"]
               ?? throw new Exception("JWT_AUDIENCE no configurado");

            //Convertit la clave secreta en bytes
            //Es necesario para crear la configuracion del token
            var keyBytes = Encoding.ASCII.GetBytes(key);

            // Crear claims (Informacion que incluira el token)
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuarios.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarios.NombresCompleto ?? ""),
                new Claim(ClaimTypes.Email, usuarios.Email ?? ""),
                new Claim(ClaimTypes.Role, rol)
            };

            // preparar el descriptor del token
            // Aqui se define la informacion del token: claims, expiracion, issuer, audience y firma

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiracion,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // Crear el token
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));

            // Devolver el token como string
        }

        // generar token refresh
        private string GenerarRefreshToken()
        {
           return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        #endregion

        public async Task<RespuestaLoginDto> loginAsync(UsuarioLoginDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto), "El campo es requerido.");

            var email = dto.Email.Trim().ToLower();

            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null)
                throw new UnauthorizedAccessException("Usuario no registrado");

            if (!await _userManager.CheckPasswordAsync(usuario, dto.Password))
                throw new UnauthorizedAccessException("Contraseña incorrecta. Verifique Por Favor.");

            var rol = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault() ?? "";

            var expiracion = DateTime.UtcNow.AddMinutes(15);
            var jwtToken = GenerarToken(usuario, rol, expiracion);
            var refresh = GenerarRefreshToken();

            var refresEntity = new RefreshToken
            {
                Token = refresh,
                UsuarioId = usuario.Id,
                Expiracion = DateTime.UtcNow.AddDays(7)
            };

            await _refresRepo.GuardarAsync(refresEntity);

            return new RespuestaLoginDto
            {
                Usuario = await MapearUsuariosDtoAsync(usuario),
                AccessToken = jwtToken,
                RefreshToken = refresh,
                ExpiraEn = expiracion
            };
        }

        public async Task<UsuarioDto> ReegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "El campo es requerido.");

            dto.Email = dto.Email.Trim().ToLower();

            var existeEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existeEmail != null)
                throw new InvalidOperationException("El email ya esta registrado.");

            var rolExistente = await _roleManager.RoleExistsAsync(dto.Rol);
            if (!rolExistente)
                throw new InvalidOperationException($"El rol: {dto.Rol} no existe.");

            var usuario = _mapper.Map<Usuarios>(dto);
            usuario.NombresCompleto = dto.NombreCompleto;
            usuario.Email = dto.Email;
            usuario.PhoneNumber = dto.PhonNumber;
            usuario.UserName = dto.Email;
            usuario.EmailConfirmed = true;
            usuario.PhoneNumberConfirmed = true;

            // Crear el usuario
            var usuariocreado = await _userManager.CreateAsync(usuario, dto.Password);
            ValidarResultado(usuariocreado, "Error al crear el usuario");

            //asignar el rol al usuario
            var rolAsignado = await _userManager.AddToRoleAsync(usuario, dto.Rol);
            ValidarResultado(rolAsignado, "Error al asignar el rol al usuario");

            return await MapearUsuariosDtoAsync(usuario);



        }

        public async Task<RespuestaLoginDto> RefreshTokenAsync(string refreshToken)
        {
            var tokenDB = await _refresRepo.ObtenerAsync(refreshToken);
            if (tokenDB == null || tokenDB.Expiracion < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token no válido o expirado.");

            var usuario = tokenDB.Usuario;
            var rol = (await _userManager.GetRolesAsync(usuario)).FirstOrDefault() ?? "";

            var expiracion = DateTime.UtcNow.AddMinutes(15);

            var niveoJwt = GenerarToken(usuario, rol, expiracion);
            var nuveoRefresh = GenerarRefreshToken();

            tokenDB.Token = nuveoRefresh;
            tokenDB.Expiracion = DateTime.UtcNow.AddDays(7);

            await _refresRepo.ActualizarAsync(tokenDB);

            return new RespuestaLoginDto
            {
                Usuario = await MapearUsuariosDtoAsync(usuario),
                AccessToken = niveoJwt,
                RefreshToken = nuveoRefresh,
                ExpiraEn = expiracion
            };
        }
    }
}
