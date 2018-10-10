namespace WebShop.Discounts
{
    public class Discount
    {
        public int Id { get; set; }
        public int ForProductId { get; set; }
        public int RequiredMinimalQuantity { get; set; }
        public decimal Value { get; set; }
        public int MaxNumberOfItemsToApplyTo { get; set; }

        /// <summary>  
        ///  Can be null.
        /// </summary>
        public Product ForProduct { get; set; }
    }
}
