using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// Collection wrapper for accounts
    /// </summary>
    public class Accounts : IdentifierSupplier
    {
        Dictionary<Identifier, Account> accountsDictionary = new Dictionary<Identifier, Account>();
        Dictionary<string, Account> shortNameToAccounts = new Dictionary<string, Account>();

        public void LoadAccounts(IEnumerable<Account> accounts)
        {
            ResetHighestIdentifier();
            ClearAccounts();
            foreach(var v in accounts)
            {
                AddAccount(v);
            }
        }

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

        public void ClearAccounts()
        {
            accountsDictionary.Clear();
            shortNameToAccounts.Clear();
        }

        public bool AddAccount(Account account)
        {
            if (accountsDictionary.ContainsKey(account.AccountID) || shortNameToAccounts.ContainsKey(account.ShortName)) return false;

            accountsDictionary.Add(account.AccountID, account);
            shortNameToAccounts.Add(account.ShortName, account);

            MaybeReplaceHighestIdentifier(account.AccountID);
            return true;
        }

        
    }
}
