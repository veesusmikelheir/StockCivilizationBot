using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockCivilizationBot.Exchange
{
    /// <summary>
    /// class that abstracts the idea of an identifier in case integers dont cut it later on 
    /// </summary>
    public struct Identifier : IComparable<Identifier>
    {

        public static readonly Identifier NULLIDENTIFIER = new Identifier(int.MinValue);
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

        public Identifier NextIdentifier()
        {
            return new Identifier(ID + 1);
        }

        public override string ToString() => ID.ToString();

        public int CompareTo(Identifier other)
        {
            return ID.CompareTo(other.ID);
        }

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

        // Define the is greater than operator.
        public static bool operator >(Identifier operand1, Identifier operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Identifier operand1, Identifier operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Identifier operand1, Identifier operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Identifier operand1, Identifier operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }


    }
}
