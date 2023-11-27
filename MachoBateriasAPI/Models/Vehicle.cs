namespace MachoBateriasAPI.Models
{
    public class Vehicle
    {
        public int id { get; set; }
        public string plate { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public int? clientId { get; set; }
        public Client? client { get; set; }
    }
}
