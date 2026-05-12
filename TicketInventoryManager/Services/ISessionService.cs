using System;
using System.Collections.Generic;
using System.Text;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    internal interface ISessionService
    {
        UserDTO? CurrentUser { get; set; }
    }
}
