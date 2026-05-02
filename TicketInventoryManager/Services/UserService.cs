using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task DeleteAsync(int id)
        {
            var toDelete = await _context.Users.FindAsync(id);
            if (toDelete == null)
            {
                return;
            }
            _context.Users.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            return await _context.Users
                .Select(user => ToDTO(user))
                .ToListAsync();
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var target = await _context.Users.FindAsync(id);
            return target == null ? null : ToDTO(target);
        }

        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            var target = await _context.Users
                            .FirstOrDefaultAsync(user => user.Username == username);
            return target == null ? null : ToDTO(target);

        }

        public async Task<UserDTO?> LoginAsync(string username, string password)
        {
            var target = await _context.Users
                            .FirstOrDefaultAsync(user => user.Username == username);
            if (target == null || 
                !BCrypt.Net.BCrypt.Verify(password, target.PasswordHash))
            {
                return null;
            }
            return ToDTO(target);
        }

        public async Task<UserDTO> RegisterAsync(string username, string password)
        {
            var newUser = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                IsAdmin = false
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return ToDTO(newUser);
        }

        private UserDTO ToDTO(User userToMap)
        {
            return new UserDTO
            {
                Id = userToMap.Id,
                Username = userToMap.Username,
                IsAdmin = userToMap.IsAdmin
            };
        }

        private User FromDTO(UserDTO userToMap, string passwordHash)
        {
            return new User
            {
                Username = userToMap.Username,
                PasswordHash = passwordHash,
                IsAdmin = userToMap.IsAdmin
            };
        }
    }
}
