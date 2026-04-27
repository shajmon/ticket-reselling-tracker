using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string PasswordHash { get; init; }
        public bool IsAdmin { get; init; }

        public List<InventoryLog> InventoryLogs { get; init; }
    }
}
