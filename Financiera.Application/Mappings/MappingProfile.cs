using AutoMapper;
using Financiera.Application.DTO.Clientes;
using Financiera.Application.DTO.CuentasPorCobrar;
using Financiera.Application.DTO.Cuota;
using Financiera.Application.DTO.Pagos;
using Financiera.Application.DTO.Prestamo;
using Financiera.Application.DTO.Rutas;
using Financiera.Application.DTO.Usuarios;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Mapeo de Usuario
            CreateMap<Usuarios, UsuarioDto>()
            // Mapea la propiedad personalizada si hereda de Identity o usa otra columna
            .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombresCompleto))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhonNumber, opt => opt.MapFrom(src => src.PhoneNumber)); // Ojo: verifica si tu DTO dice 'PhonNumber' o 'PhoneNumber'
            CreateMap<UsuarioRegistroDto, Usuarios>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            #endregion

            #region Mapeo de Rutas
            CreateMap<Rutas, RutasDto>();
            CreateMap<RutasCrearDto, Rutas>();
            CreateMap<RutasActualizarDto, Rutas>()
                .ForMember(c => c.id, opt => opt.Ignore());
            #endregion

            #region Mapeo de Clientes
            CreateMap<Clientes, ClienteDto>();
            CreateMap<ClienteCrearDto, Clientes>();
            CreateMap<ClienteActualizarDto, Clientes>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region Mapeo de Prestamos
            CreateMap<Prestamos, PrestamoDto>();
            CreateMap<PrestamoCrearDto, Prestamos>();
            CreateMap<PrestamoActualizarDto, Prestamos>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            CreateMap<PrestamoActualizarEstadoDto, Prestamos>()
                .ForMember(e => e.EstadoPrestamo, opt => opt.MapFrom(src => src.EstadoPrestamo.ToString()));
            #endregion

            #region Mepeo de Cuotas
            CreateMap<Cuota, CuotaDto>();
            CreateMap<CuotaCrearDto, Cuota>();
            CreateMap<CuotaActualizarDto, Cuota>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region Mepeo de CuentasPorCobrar
            CreateMap<CuentasPorCobrar, CuentasPorCobrarDto>();
            CreateMap<CuentasPorCobrarCrearDto, CuentasPorCobrar>();
            CreateMap<CuentasPorCobrarActualizarDto, CuentasPorCobrar>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion

            #region Mepeo de Pagos
            CreateMap<Pagos, PagosDto>();
            CreateMap<PagosCrearDto, Pagos>();
            CreateMap<PagosActualizarDto, Pagos>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            #endregion
        }
    }
}
