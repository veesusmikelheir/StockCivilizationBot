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

        // maybe replace this method with something that defers to a chain of responsibility pattern? (thanks waterboi)
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
            return sourcePortfolio.ValidateTransactionAmount(transaction.TradedSecurity, BigRational.Negate(transaction.TradedSecurityAmount)) // check if we can withdraw the traded security
                && targetPortfolio.ValidateTransactionAmount(transaction.TradedSecurity, transaction.TradedSecurityAmount) // check that we can deposit the traded security into the other account
                && sourcePortfolio.ValidateTransactionAmount(transaction.BackingSecurity, transaction.BackingSecurityAmount) // check that we can deposit the backing security into the source account
                && targetPortfolio.ValidateTransactionAmount(transaction.BackingSecurity, BigRational.Negate(transaction.BackingSecurityAmount)); // check that there's actually enough backing security to take out 
        }

        public bool IsTransactionAuthorized(Transaction transaction)
        {
            // check that both accounts are ready for this transaction
            return transaction.Source.IsTransactionAuthorized(transaction) && transaction.Target.IsTransactionAuthorized(transaction);
        }

        public void AttemptToAuthorize(Transaction transaction) { } // todo

        // if the transaction is already stored, something went majorly wrong, abort
        public bool IsTransactionUnique(Transaction transaction)
        {
            return !Transactions.IsAlreadyPresent(transaction);
        }
        // create a new trasnaction without having to deal with assigning it its ID
        public Transaction CreateNewTransaction(Account source, Account Target, Security tradedSecurity, BigRational tradedSecurityAmount, Security backingSecurity, BigRational backingSecurityAmount)
        {
            return new Transaction(Transactions.GetNextIdentifier(), source, Target, tradedSecurity, tradedSecurityAmount, backingSecurity, backingSecurityAmount);
        }
    }
}
