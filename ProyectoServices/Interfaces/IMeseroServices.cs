using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoModelo;
using Microsoft.EntityFrameworkCore; 
using ProyectoServices.Implementaciones;

namespace ProyectoServices
{
    public interface IMeseroServices
    {
        Task<IEnumerable<Mesero>> GetAllAsync();
        Task<Mesero?> GetByIdAsync(int id);
        Task<Mesero> CreateAsync(Mesero mesero);
        Task<Mesero> UpdateAsync(Mesero mesero);
        Task<bool> DeleteAsync(int id);
    }
}