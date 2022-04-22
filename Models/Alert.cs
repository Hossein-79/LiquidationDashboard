using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiquidationDashboard.Models
{
    public class Alert
    {
        public int AlertId { get; set; }

        public decimal AlertLimit { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsSend { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
