using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Infrastructure.DataAccess
{
    public class RequiredProductOfDiscount
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
    }

    public class MicroDiscount
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
    }
}
