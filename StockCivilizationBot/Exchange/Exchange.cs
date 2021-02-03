using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// Core class for exchanging securities
    /// </summary>
    public class Exchange
    {
        public Accounts Accounts { get; } = new Accounts();
        public Securities Securities { get; } = new Securities();

        public TransactionProcessor TransactionProcessor { get; } = new TransactionProcessor();

        public void LoadExchange()
        {
            // todo
            // in the mean time just initialize the money security
            
            Securities.GetOrCreateSecurity("Dollars","$");
 
        }

        

    }
}
