using System;
using System.Collections.Generic;
using System.Text;

namespace TicketInventoryManager.Models.Entities
{
    public class UserDTO
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public bool IsAdmin { get; init; }
    }
}
