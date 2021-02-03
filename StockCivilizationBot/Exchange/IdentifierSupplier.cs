using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCivilizationBot.Exchange
{
    public abstract class IdentifierSupplier
    {
        protected Identifier HighestFreeIdentifier = new Identifier(0);
        public void ResetHighestIdentifier() => HighestFreeIdentifier = Identifier.NULLIDENTIFIER;

        public void MaybeReplaceHighestIdentifier(Identifier potentialHighestIdentifier)
        {
            if (potentialHighestIdentifier >= HighestFreeIdentifier) HighestFreeIdentifier = potentialHighestIdentifier.NextIdentifier();
        }


        public Identifier GetNextIdentifier(bool increment = false)
        {
            var ident = HighestFreeIdentifier;
            if (increment) HighestFreeIdentifier = HighestFreeIdentifier.NextIdentifier();
            return ident;
        }
    }
}
