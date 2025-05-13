using System.ComponentModel.DataAnnotations;

namespace APBD_Kolos.Model;

public class Customer
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}