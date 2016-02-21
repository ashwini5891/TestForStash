using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithGit.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PostCode { get; set; }
        public DateTime Created { get; set; }

        public string QuoteNumber { get; set; }
    }
}
