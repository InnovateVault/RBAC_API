using RBAC_API.Models;

namespace RBAC_API.Repositories
{
    public interface IPermissionRepository
    {
        Permission GetById(int id);
        List<Permission> GetAll();
        void Add(Permission permission);
        void Update(Permission permission);
        void Delete(int id);
    }
}
