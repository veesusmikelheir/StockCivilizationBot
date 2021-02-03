using ExtendedNumerics;
using System;
using System.Collections.Generic;

namespace StockCivilizationBot.Economy
{
    /// <summary>
    /// Represents a collection of securities
    /// </summary>
    public class Portfolio
    {
        Dictionary<Security, BigRational> AvailableSecurities = new Dictionary<Security, BigRational>();

        public IEnumerable<KeyValuePair<Security, BigRational>> GetNonZeroSecurities() => AvailableSecurities;

        public Portfolio(Dictionary<Security, BigRational> availableSecurities)
        {
            AvailableSecurities = availableSecurities;
        }

        /// <summary>
        /// Gets the current amount of a given security
        /// </summary>
        /// <param name="security"></param>
        /// <returns>Amount of given security</returns> 
        public BigRational Get(Security security)
        {
            if (AvailableSecurities.TryGetValue(security, out var rational)) return rational;
            return BigRational.Zero;
        }

        /// <summary>
        /// Adds <paramref name="amount"/> of the security <paramref name="security"/> to this portfolio and returns the new value
        /// Positive values add to the portfolio, while negative numbers subtract from
        /// Does not protect against negative ending values or validate transact
        /// </summary>
        /// <param name=""></param>
        /// <returns>New value for this security</returns>
        public BigRational Transact(Security security,BigRational amount)
        {
            return AvailableSecurities[security] = BigRational.Add(Get(security), amount);
        }

        /// <summary>
        /// Checks to make sure an amount can be successfully be transacted to or from this portfolio
        /// Negative numbers for withdrawals and positive numbers for deposits
        /// </summary>
        /// <param name="security"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Validate(Security security, BigRational amount)
        {
            return BigRational.Negate(amount) <= Get(security);
        }

        /// <summary>
        /// Validate a transaction and apply it if valid, otherwise return false
        /// </summary>
        /// <param name="security"></param>
        /// <param name="amount">Negative numbers for withdrawals and positive numbers for deposits</param>
        /// <param name="amountAfter">If transaction is valid, amount in the account after the transaction is processed, otherwise, the amount currently in the account</param>
        /// <returns>If transaction completed or not </returns>
        public bool AttemptTransact(Security security, BigRational amount, out BigRational amountAfter)
        {
            amountAfter = Get(security);
            if (!Validate(security, amount)) return false;
            amountAfter = Transact(security, amount);
            return true;
        }

        public static bool TryTrade(Security security, Portfolio source, Portfolio target, BigRational amount, out BigRational sourceNew,out BigRational targetNew)
        {
            if (amount < BigRational.Zero) throw new ArgumentException("Must be greater than zero (swap source and target)", "amount");
            targetNew = target.Get(security); // so if source transaction fails targetnew isnt null
            // if the source is denying the transaction then we can end the whole thing without much harm
            if (!source.AttemptTransact(security, BigRational.Negate(amount), out sourceNew)) return false;
            // if the target is denying the transaction then we need to rollback the state 
            if(!target.AttemptTransact(security,amount,out targetNew))
            {
                // naive rollback
                // dont bother checking if its valid, we need to force this through
                sourceNew = target.Transact(security, amount);
            }
            return true;
        }
    }
}