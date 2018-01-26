using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(string message) : base(message)
        {

        }
    }
}
