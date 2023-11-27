using System.ComponentModel.DataAnnotations;

namespace MachoBateriasAPI.Models
{
    public class SaleProduct
    {
        public int id { get; set; }
        public int saleId { get; set; }
        public int productId { get; set; }
    }
}
