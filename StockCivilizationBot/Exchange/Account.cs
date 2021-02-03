using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockCivilizationBot.Exchange
{
    public class Account
    {
        public Identifier AccountID { get; }

        public Account(Identifier accountID)
        {
            AccountID = accountID;
        }

        public string ReadableName;
        public string ShortName;

        public Portfolio Portfolio = new Portfolio();

        public bool ValidateTransactionAmount(Security security,BigRational rational)
        {
            return Portfolio.Validate(security, rational); 
        }
        
        public bool IsTransactionAuthorized(Transaction transaction)
        {
            if (transaction.Source != this && transaction.Target != this) throw new Exception("This account has no bearing on this transaction");
            // no validation logic yet, 
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   AccountID == account.AccountID;
        }

        public override int GetHashCode()
        {
            return -1940269095 + AccountID.GetHashCode();
        }

        public static bool operator ==(Account left, Account right)
        {
            return EqualityComparer<Account>.Default.Equals(left, right);
        }

        public static bool operator !=(Account left, Account right)
        {
            return !(left == right);
        }

    }
    public static class AccountExtensions
    {
        // make it an extension so accounts doesnt need to care about how to initialize an account properly
        public static Account GetOrCreateAccount(this Accounts accounts, string name, string shortName)
        {
            Account account = new Account(accounts.GetNextIdentifier()) { ReadableName = name, ShortName = shortName };
            if (!accounts.AddAccount(account)) return accounts.Get(shortName);
            return account;
        }
    }
}
