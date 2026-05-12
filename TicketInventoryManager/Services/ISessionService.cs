using System;
using System.Collections.Generic;
using System.Text;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface ISessionService
    {
        UserDTO? CurrentUser { get; set; }
    }
}
