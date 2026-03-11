using Microsoft.EntityFrameworkCore;
using ProyectoModelo;
using ProyectoWebAPI.Data;

public class MeseroService : IMeseroService
{
    private readonly ApplicationDbContext _context;

    public MeseroService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Mesero>> GetAllAsync()
        => await _context.Meseros.ToListAsync();

    public async Task<Mesero?> GetByIdAsync(int id)
        => await _context.Meseros.FindAsync(id);

    public async Task<Mesero> CreateAsync(Mesero mesero)
    {
        _context.Meseros.Add(mesero);
        await _context.SaveChangesAsync();
        return mesero;
    }

    public async Task<Mesero> UpdateAsync(Mesero mesero)
    {
        _context.Meseros.Update(mesero);
        await _context.SaveChangesAsync();
        return mesero;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var mesero = await _context.Meseros.FindAsync(id);
        if (mesero == null) return false;
        _context.Meseros.Remove(mesero);
        await _context.SaveChangesAsync();
        return true;
    }
}