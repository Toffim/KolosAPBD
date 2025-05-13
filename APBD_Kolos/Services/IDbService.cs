using APBD_Kolos.Model;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolos.Services;

public interface IDbService
{
    Task<Boolean> DoesDeliveryExist(int deliveryId);
    Task<DeliveryDTO> GetDelivery(int deliveryId);
    Task<List<ProductDTO>> GetDeliveryProducts(int deliveryId);
    Task<Customer> GetCustomerInfo(int customerId);
    Task<Driver> GetDriverInfo(int driverId);
}