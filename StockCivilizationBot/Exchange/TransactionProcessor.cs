using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    public class TransactionProcessor
    {
        public TransactionLog Transactions = new TransactionLog();

        public bool TryProcessTransaction(Transaction transaction)
        {

            if (!ValidateTransaction(transaction)) return false;

            if (!CommitTransactionAmount(transaction)) return false;

            if (!Transactions.PushTransaction(transaction))
            {
                UndoTransaction(transaction);
                return false;
            }

            return true;
        }

        public bool ValidateTransaction(Transaction transaction)
        {
            if (!IsTransactionUnique(transaction) || !IsTransactionPossible(transaction)) return false;
            if (!IsTransactionAuthorized(transaction))
            {
                AttemptToAuthorize(transaction);
                if (!IsTransactionAuthorized(transaction)) return false;
            }
            return true;
        }

        bool CommitTransactionAmount(Transaction transaction)
        {
            return Portfolio.TryTwoWayTrade(transaction.Source.Portfolio, transaction.Target.Portfolio, transaction.TradedSecurity, transaction.TradedSecurityAmount, transaction.BackingSecurity, transaction.BackingSecurityAmount);
        }

        bool UndoTransaction(Transaction transaction)
        {
            return Portfolio.TryTwoWayTrade(transaction.Target.Portfolio, transaction.Source.Portfolio, transaction.TradedSecurity, transaction.TradedSecurityAmount, transaction.BackingSecurity, transaction.BackingSecurityAmount);

        }

        public bool IsTransactionPossible(Transaction transaction)
        {
            var targetPortfolio = transaction.Target;
            var sourcePortfolio = transaction.Source;
            return sourcePortfolio.ValidateTransactionAmount(transaction.TradedSecurity, BigRational.Negate(transaction.TradedSecurityAmount)) && targetPortfolio.ValidateTransactionAmount(transaction.TradedSecurity, transaction.TradedSecurityAmount)
            && sourcePortfolio.ValidateTransactionAmount(transaction.BackingSecurity, transaction.BackingSecurityAmount) && targetPortfolio.ValidateTransactionAmount(transaction.BackingSecurity, BigRational.Negate(transaction.BackingSecurityAmount));
        }

        public bool IsTransactionAuthorized(Transaction transaction)
        {
            return transaction.Source.IsTransactionAuthorized(transaction) && transaction.Target.IsTransactionAuthorized(transaction);
        }

        public void AttemptToAuthorize(Transaction transaction) { }

        public bool IsTransactionUnique(Transaction transaction)
        {
            return !Transactions.IsAlreadyPresent(transaction);
        }
    }
}
