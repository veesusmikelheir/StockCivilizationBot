using ExtendedNumerics;
using System.Collections.Generic;

namespace StockCivilizationBot.Economy
{
    public class Portfolio
    {
        Dictionary<Security, BigRational> AvailableSecurities = new Dictionary<Security, BigRational>();

        public Portfolio(Dictionary<Security, BigRational> availableSecurities)
        {
            AvailableSecurities = availableSecurities;
        }

        /// <summary>
        /// Gets the current amount of a given security
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        public BigRational Get(Security security)
        {
            if (AvailableSecurities.TryGetValue(security, out var rational)) return rational;
            return BigRational.Zero;
        }

        /// <summary>
        /// Adds <paramref name="amount"/> of the security <paramref name="security"/> to this portfolio
        /// Positive values add to the portfolio, while negative numbers subtract from
        /// Does not protect against negative ending values
        /// </summary>
        /// <param name=""></param>
        public void Transact(Security security,BigRational amount)
        {
            AvailableSecurities[security] = BigRational.Add(Get(security), amount);
        }
    }
}