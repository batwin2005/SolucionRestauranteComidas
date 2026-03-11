using ProyectoModelo;

public interface IMeseroService
{
    Task<IEnumerable<Mesero>> GetAllAsync();
    Task<Mesero?> GetByIdAsync(int id);
    Task<Mesero> CreateAsync(Mesero mesero);
    Task<Mesero> UpdateAsync(Mesero mesero);
    Task<bool> DeleteAsync(int id);
}