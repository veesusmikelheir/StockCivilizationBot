using System.Collections.Generic;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// Keeps track of known transactions in program memory, and provides new ID's when needed
    /// </summary>
    public class TransactionLog
    {
        HashSet<Transaction> Transactions = new HashSet<Transaction>();
        Identifier highestFreeIdentifier = new Identifier(0); // stores the next unused identifier (as far as we know)

        internal void ClearTransactions()
        {
            Transactions.Clear();

        }
        internal void LoadTransactions(IEnumerable<Transaction> transactions)
        {
            ResetHighestIdentifier();
            ClearTransactions();
            // iterator through the enumerable instead of creating a new hashset in case I add more advanced push transaction logic
            foreach(var v in transactions)
            {
                PushTransaction(v);
                
            }
        }

        public bool IsAlreadyPresent(Transaction transaction) => Transactions.Contains(transaction);

        void ResetHighestIdentifier() => highestFreeIdentifier = Identifier.NULLIDENTIFIER;
        void MaybeReplaceHighestIdentifier(Identifier potentialHighestIdentifier)
        {
            if (potentialHighestIdentifier >= highestFreeIdentifier) highestFreeIdentifier = potentialHighestIdentifier.NextIdentifier();
        }
        public bool PushTransaction(Transaction transaction)
        {
            if (IsAlreadyPresent(transaction)) return false;
            Transactions.Add(transaction);

            MaybeReplaceHighestIdentifier(transaction.TransactionID);

            return true;
        }

        public Identifier GetNextTransactionIdentifier(bool increment = false)
        {
            var ident = highestFreeIdentifier;
            if (increment) highestFreeIdentifier = highestFreeIdentifier.NextIdentifier();
            return ident;
        }

    }
}