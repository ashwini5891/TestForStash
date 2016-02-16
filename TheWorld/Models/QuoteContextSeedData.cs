using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class QuoteContextSeedData
    {
        private QuoteContext _quoteContext;
        public QuoteContextSeedData(QuoteContext quoteContext)
        {
            _quoteContext = quoteContext;
        }
        public void EnsureSeedData()
        {
            if(!_quoteContext.Quotes.Any())
            {
                var quote = new Quote()
                {
                    FirstName = "Ashwini",
                    LastName = "Laxminarayana",
                    PostCode = "NR13NS",
                    Email = "dgsdg@sdgs.com"
                };
                _quoteContext.Quotes.Add(quote);
                _quoteContext.SaveChanges();
            }

            
        }
    }
}
