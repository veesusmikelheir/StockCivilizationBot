using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    public class Accounts : IdentifierSupplier
    {
        Dictionary<Identifier, Account> accountsDictionary = new Dictionary<Identifier, Account>();
        Dictionary<string, Account> shortNameToAccounts = new Dictionary<string, Account>();



        public Account Get(Identifier identifier)
        {
            if (accountsDictionary.TryGetValue(identifier, out var account)) return account;
            return null;
        }

        public Account Get(string shortName)
        {
            if (shortNameToAccounts.TryGetValue(shortName, out var account)) return account;
            return null;
        }


    }
}
