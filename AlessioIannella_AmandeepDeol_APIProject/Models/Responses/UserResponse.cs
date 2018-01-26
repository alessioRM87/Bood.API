using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Responses
{
    public class UserResponse
    {
        public long userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
