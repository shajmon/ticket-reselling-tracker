using System;
using System.Collections.Generic;
using System.Text;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class SessionService : ISessionService
    {
        public UserDTO? CurrentUser { get; set; }
    }
}
