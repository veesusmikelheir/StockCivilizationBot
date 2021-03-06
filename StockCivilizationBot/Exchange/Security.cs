﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// An entity exchangeable between accounts
    /// </summary>
    public class Security
    {
        public static readonly Security NOTHING = new Security(-1,"NIL", "Nothing");

        public Identifier SecurityID { get; }
        public string ShortName { get; }
        public string ReadableName { get; }

        public Security(Identifier securityID)
        {
            SecurityID = securityID;
        }

        public Security(Identifier securityID, string shortName, string readableName) : this(securityID)
        {
            ShortName = shortName;
            ReadableName = readableName;
        }

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

    public static class SecurityExtensions
    {
        // make it an extension so securities doesnt need to care about how to initialize a security properly
        public static Security GetOrCreateSecurity(this Securities securities, string name, string shortName)
        {
            Security security = new Security(securities.GetNextIdentifier(), shortName, name);
            if (!securities.AddSecurity(security)) return securities.Get(shortName);
            return security;
        }
    }
}
