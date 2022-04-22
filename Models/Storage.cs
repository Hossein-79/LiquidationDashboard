using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LiquidationDashboard.Models
{
    public class Storage
    {
        public int StorageId { get; set; }

        public string Address { get; set; }

        public decimal UserBorrow { get; set; }

        public decimal MaxBorrow { get; set; }

        public decimal Health { get => UserBorrow * 100 / MaxBorrow; }

        //[ForeignKey(nameof(Symbol))]
        //public int SymbolId { get; set; }

        //public Symbol Symbol { get; set; }
    }
}
