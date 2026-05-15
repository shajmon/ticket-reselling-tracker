using DAL;
using DAL.Entities;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            return await Task.Run(() =>
            {
                var target = _context.Users.FirstOrDefault(user => user.Username == username);
                return target == null ? null : ToDTO(target);
            });
        }

        public async Task<UserDTO?> LoginAsync(string username, string password)
        {
            return await Task.Run(() =>
            {
                var target = _context.Users.FirstOrDefault(user => user.Username == username);
                if (target == null || !BCrypt.Net.BCrypt.Verify(password, target.PasswordHash))
                    return null;
                return ToDTO(target);
            });
        }

        public async Task<UserDTO> RegisterAsync(string username, string password)
        {
            return await Task.Run(() =>
            {
                var newUser = new User
                {
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = false
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return ToDTO(newUser);
            });
        }

        private static UserDTO ToDTO(User userToMap)
        {
            return new UserDTO
            {
                Id = userToMap.Id,
                Username = userToMap.Username,
                IsAdmin = userToMap.IsAdmin
            };
        }
    }
}
