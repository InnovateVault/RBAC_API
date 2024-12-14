using Microsoft.EntityFrameworkCore;
using RBAC_API.Data;
using RBAC_API.Models;

namespace RBAC_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            return _context.Users.Include(u => u.Roles).SingleOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.Include(u => u.Roles).SingleOrDefault(u => u.Username == username);
        }

        public List<User> GetAll()
        {
            return _context.Users.Include(u => u.Roles).ToList();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
