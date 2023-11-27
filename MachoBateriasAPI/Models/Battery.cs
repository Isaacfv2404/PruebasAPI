namespace MachoBateriasAPI.Models
{
    public class Battery:Product
    {
        public string model { get; set; }
        public double capacity{ get; set; }
        public double voltage { get; set; }
        public string type { get; set; }
        public double weight { get; set; }
        public double large { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public DateTime expiration { get; set; }
    }
}
