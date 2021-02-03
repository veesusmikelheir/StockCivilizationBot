using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// Represents a transaction between two accounts
    /// Involves two securities, the security being traded for (dubbed TradedSecurity) and the security being given in exchange (dubbed BackingSecurity)
    /// Source account is the account giving up the TradedSecurity
    /// Target account is the account giving up the BackingSecurity
    /// Note: Since every transaction has an inverse where source and target are swapped and the signs of tradedsecurityamount and backingsecurityamount are flipped, all transactions will be the versions that can be most easily deemed as a "Purchase" (ie, the traded security is being transferred from source to target)
    /// </summary>
    public class Transaction
    {
        public Identifier TransactionID { get; }

        /// <summary>
        /// Account traded security is being withdrawn from, and backing security is being deposited into
        /// </summary>
        public Account Source { get; }
        /// <summary>
        /// Account backing security is being withdrawn from, and traded security is being deposited into
        /// </summary>
        public Account Target { get; }
        /// <summary>
        /// Primary security of this transaction, the "good" being bought or sold
        /// </summary>
        public Security TradedSecurity { get; }
        /// <summary>
        /// Amount of the primary security being withrawn from source account and deposited into target account
        /// </summary>
        public BigRational TradedSecurityAmount { get; }
        /// <summary>
        /// Secondary security of this transaction, the "currency" facilitating the exchange of the traded security
        /// </summary>
        public Security BackingSecurity { get; }
        /// <summary>
        /// Amount of the secondary security being deposited into source account and withdrawn from target account
        /// </summary>
        public BigRational BackingSecurityAmount { get; }

        public Transaction(Identifier transactionID, Account source, Account target, Security tradedSecurity, BigRational tradedSecurityAmount, Security backingSecurity, BigRational backingSecurityAmount)
        {
            TransactionID = transactionID;
            Source = source;
            Target = target;
            TradedSecurity = tradedSecurity;
            TradedSecurityAmount = tradedSecurityAmount;
            BackingSecurity = backingSecurity;
            BackingSecurityAmount = backingSecurityAmount;
        }

        
    }
}
