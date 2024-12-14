using RBAC_API.Models;

namespace RBAC_API.Repositories
{
    public interface IRoleRepository
    {
        Role GetById(int id);
        List<Role> GetAll();
        void Add(Role role);
        void Update(Role role);
        void Delete(int id);
    }
}
