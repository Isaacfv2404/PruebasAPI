
using System.ComponentModel.DataAnnotations;

namespace MachoBateriasAPI.Models
{
    public class Sale
    {
        public int id { get; set; }
        public int code { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        public int employeeId { get; set; }
        public int clientId { get; set; }
        public double discount { get; set; }
        public double subTotal { get; set; }
        public double total { get; set; }



    }
}
