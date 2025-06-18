using System;

namespace DataLayer
{
    public class CurrencyPairModel
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; } 
        public string ToCurrency { get; set; }   
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}
