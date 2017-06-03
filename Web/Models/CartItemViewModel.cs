using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    [Serializable]
    public class CartItemViewModel
    {
        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }
    }
}