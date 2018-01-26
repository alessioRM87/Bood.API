using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models
{
    public class BookUserMood
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public int MoodID { get; set; }
    }
}
