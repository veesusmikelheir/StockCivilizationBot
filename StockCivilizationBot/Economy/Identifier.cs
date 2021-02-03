using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockCivilizationBot.Economy
{
    /// <summary>
    /// class that abstracts the idea of an identifier in case integers dont cut it later on 
    /// </summary>
    public struct Identifier
    {
        public int ID { get; }

        public Identifier(int iD)
        {
            ID = iD;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier identifier &&
                   ID == identifier.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString() => ID.ToString();

        public static bool operator ==(Identifier left, Identifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !(left == right);
        }

        public static implicit operator Identifier(int val)
        {
            return new Identifier(val);
        }
    }
}
