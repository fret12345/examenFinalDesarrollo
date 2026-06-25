using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Infrestructure.Repository
{
    public class SeederRepository : ISeederRepository
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public SeederRepository(UserManager<Usuarios> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Crear roles
            string[] roles = { "Admin"};
            foreach (var rol in roles)
            {
                var rolExistente = await _roleManager.RoleExistsAsync(rol);
                if (!rolExistente)
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(rol));
                }
            }

            if (!await _userManager.Users.AnyAsync())
            {
                var usuarioAdmin = new Usuarios
                {
                    NombresCompleto = "Administrador",
                    UserName = "Admin",
                    Email = "fret@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = await _userManager.CreateAsync(usuarioAdmin, "Fret123+-*");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuarioAdmin, "Admin");
                }
            }
        }
    }
}
