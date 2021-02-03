using System.Collections.Generic;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// collection wrapper for securities
    /// </summary>
    public class Securities : IdentifierSupplier
    {
        Dictionary<Identifier, Security> securitiesDictionary = new Dictionary<Identifier, Security>();
        Dictionary<string, Security> shortNameToSecurities = new Dictionary<string, Security>();

        public void LoadSecuritys(IEnumerable<Security> securities)
        {
            ResetHighestIdentifier();
            ClearSecurities();
            foreach (var v in securities)
            {
                AddSecurity(v);
            }
        }

        public Security Get(Identifier identifier)
        {
            if (securitiesDictionary.TryGetValue(identifier, out var account)) return account;
            return null;
        }

        public Security Get(string shortName)
        {
            if (shortNameToSecurities.TryGetValue(shortName, out var account)) return account;
            return null;
        }

        public void ClearSecurities()
        {
            securitiesDictionary.Clear();
            shortNameToSecurities.Clear();
        }

        public bool AddSecurity(Security account)
        {
            if (securitiesDictionary.ContainsKey(account.SecurityID) || shortNameToSecurities.ContainsKey(account.ShortName)) return false;

            securitiesDictionary.Add(account.SecurityID, account);
            shortNameToSecurities.Add(account.ShortName, account);

            MaybeReplaceHighestIdentifier(account.SecurityID);
            return true;
        }
    }
}
