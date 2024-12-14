using Microsoft.EntityFrameworkCore;
using RBAC_API.Data;
using RBAC_API.Models;

namespace RBAC_API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Role GetById(int id)
        {
            return _context.Roles.Include(r => r.Permissions).SingleOrDefault(r => r.Id == id);
        }

        public List<Role> GetAll()
        {
            return _context.Roles.Include(r => r.Permissions).ToList();
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var role = GetById(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        }
    }
}
