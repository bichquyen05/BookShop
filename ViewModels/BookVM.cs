namespace BookShop.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Dimensions { get; set; }
        public int Pages { get; set; }
        public double UnitPrice { get; set; }
        public int Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Supplier { get; set; }
        public int Quantity { get; set; }        
        public string Review { get; set; }

    }
}
