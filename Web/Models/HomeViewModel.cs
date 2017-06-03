using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> NewProducts { get; set; }

        public IEnumerable<ProductViewModel> LaptopProducts { get; set; }

        public IEnumerable<ProductViewModel> TabletProducts { get; set; }

        public IEnumerable<ProductViewModel> MobileProducts { get; set; }

        public IEnumerable<ProductViewModel> AccessoryProducts { get; set; }
    }
}