using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Economy
{
    /// <summary>
    /// An entity exchangeable between accounts
    /// </summary>
    public class Security
    {
        public static readonly Security NOTHING = new Security(-1) { SecurityName = "Nothing" };

        public Identifier SecurityID { get; }


        public Security(Identifier securityID)
        {
            SecurityID = securityID;
        }

        public string SecurityName;

        public override bool Equals(object obj)
        {
            return obj is Security security &&
                   SecurityID == security.SecurityID;
        }

        public override int GetHashCode()
        {
            return 1664755014 + SecurityID.GetHashCode();
        }

        public static bool operator ==(Security left, Security right)
        {
            return EqualityComparer<Security>.Default.Equals(left, right);
        }

        public static bool operator !=(Security left, Security right)
        {
            return !(left == right);
        }
    }
}
