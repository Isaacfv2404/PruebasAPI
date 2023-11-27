namespace MachoBateriasAPI.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string brand { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public string productType { get; set; }
    }

}
