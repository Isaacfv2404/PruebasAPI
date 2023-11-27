namespace MachoBateriasAPI.Models
{
    public class Warranty
    {
        public int id { get; set; }
        public string description { get; set; }
        public bool? state { get; set; }
        public int? productId { get; set; }
        public Product? product { get; set; }
        public int? vehicleId { get; set; }
        public Vehicle? vehicle { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        
       


    }
}
