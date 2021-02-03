using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockCivilizationBot.Economy
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

        public Portfolio Portfolio;

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
}
