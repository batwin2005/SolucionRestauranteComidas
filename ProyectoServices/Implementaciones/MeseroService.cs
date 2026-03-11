using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoModelo;
using ProyectoData;

namespace ProyectoServices.Implementaciones
{
    public class MeseroService : IMeseroService
    {
        private readonly MeseroData _meseroData;

        public MeseroService(MeseroData meseroData)
        {
            _meseroData = meseroData;
        }

        public Task<IEnumerable<Mesero>> GetAllAsync()
            => Task.FromResult(_meseroData.GetAll());

        public Task<Mesero?> GetByIdAsync(int id)
            => Task.FromResult(_meseroData.GetById(id));

        public Task<Mesero> CreateAsync(Mesero mesero)
        {
            var id = _meseroData.Create(mesero);
            mesero.IdMesero = id;
            return Task.FromResult(mesero);
        }

        public Task<Mesero> UpdateAsync(Mesero mesero)
        {
            _meseroData.Update(mesero);
            return Task.FromResult(mesero);
        }

        public Task<bool> DeleteAsync(int id)
            => Task.FromResult(_meseroData.Delete(id));
    }
}