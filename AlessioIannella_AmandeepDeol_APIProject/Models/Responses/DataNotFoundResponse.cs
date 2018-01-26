using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Responses
{
    public class DataNotFoundResponse
    {
        public DataNotFoundResponse(string message)
        {
            error = message;
        }

        public string error { get; set; }
    }
}
