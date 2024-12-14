using RBAC_API.Data;
using RBAC_API.Models;

namespace RBAC_API.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Permission GetById(int id)
        {
            return _context.Permissions.SingleOrDefault(p => p.Id == id);
        }

        public List<Permission> GetAll()
        {
            return _context.Permissions.ToList();
        }

        public void Add(Permission permission)
        {
            _context.Permissions.Add(permission);
            _context.SaveChanges();
        }

        public void Update(Permission permission)
        {
            _context.Permissions.Update(permission);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var permission = GetById(id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                _context.SaveChanges();
            }
        }
    }
}
