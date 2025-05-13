namespace APBD_Kolos.Model;

public class DeliveryCreate
{
    int DeliveryId { get; set; }
    int CustomerId { get; set; }
    String LicenceNumber { get; set; }
    List<ProductDTO> Products { get; set; }
}