using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol_APIProject.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}
