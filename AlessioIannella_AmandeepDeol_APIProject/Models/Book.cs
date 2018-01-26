using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Requests
{
    public class Book
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageURL { get; set; }
        public string DownloadURL { get; set; }
        public string PublishedDate { get; set; }
        public string UploadDate { get; set; }
    }
}
