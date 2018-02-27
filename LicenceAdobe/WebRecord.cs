using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenceAdobe
{
    public class WebRecord
    {
        public string Name { get; set; }
        public string Product { get; set; }
        public string Status { get; set; }
        public string Expiration { get; set; }

        public WebRecord()
        {
            Name = "";
            Product = "";
            Status = "";
            Expiration = "";
        }
    }
}
