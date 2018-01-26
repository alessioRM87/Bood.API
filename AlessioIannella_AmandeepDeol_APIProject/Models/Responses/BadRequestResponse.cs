using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Responses
{
    public class BadRequestResponse
    {
        public BadRequestResponse(string message)
        {
            error = message;
        }

        public string error { get; set; }
    }
}
