using System.ComponentModel.DataAnnotations;

namespace APBD_Kolos.Model;

public class DeliveryDTO
{
    public DateTime DeliveryDate { get; set; }
    public Customer Customer { get; set; }
    public Driver Driver { get; set; }
    public List<ProductDTO> Products { get; set; }
}