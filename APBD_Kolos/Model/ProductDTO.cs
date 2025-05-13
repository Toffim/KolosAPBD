using System.ComponentModel.DataAnnotations;

namespace APBD_Kolos.Model;

public class ProductDTO
{
    public String Name { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
}