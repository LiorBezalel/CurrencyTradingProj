using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string CurrencyName { get; set; }
        public string Abbreviation { get; set; }
        public decimal CurrentValue { get; set; }
    }
}
