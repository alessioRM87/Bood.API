using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlessioIannella_AmandeepDeol.API.Models.Requests
{
    public class Upload
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishedDate { get; set; }
        public string UploadDate { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile Pdf { get; set; }
    }
}
