using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface ISeederRepository
    {
        Task SeedAsync();
    }
}
