using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using APBD_Kolos.Model;

namespace APBD_Kolos.Services;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;
    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Boolean> DoesDeliveryExist(int deliveryId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        command.CommandText = "SELECT COUNT(1) FROM [Delivery] WHERE delivery_id = @IdDelivery";
        command.Parameters.AddWithValue("@IdDelivery", deliveryId);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
    
    public async Task<DeliveryDTO> GetDelivery(int deliveryId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();

        command.CommandText = @"SELECT Delivery.date as dateDelivery, customer_id, driver_id FROM [Delivery] WHERE delivery_id = @IdDelivery";
        command.Parameters.AddWithValue("@IdDelivery", deliveryId);

        using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                var delivery = new DeliveryDTO()
                {
                    DeliveryDate = reader.GetDateTime(reader.GetOrdinal("dateDelivery")),
                    Customer = await GetCustomerInfo(reader.GetInt32(reader.GetOrdinal("customer_id"))),
                    Driver = await GetDriverInfo(reader.GetInt32(reader.GetOrdinal("driver_id"))),
                    Products = await GetDeliveryProducts(deliveryId),
                };

                return delivery;
            }
        }

        return null;
    }
    
    public async Task<Customer> GetCustomerInfo(int customerId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();

        command.CommandText = @"SELECT first_name, last_name, date_of_birth FROM [Customer] WHERE customer_id = @IdCustomer";
        command.Parameters.AddWithValue("@IdCustomer", customerId);

        using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                var customer = new Customer()
                {
                   FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                   LastName = reader.GetString(reader.GetOrdinal("last_name")),
                   DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                };

                return customer;
            }
        }

        return null;
    }
    
    public async Task<Driver> GetDriverInfo(int driverId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();

        command.CommandText = @"SELECT first_name, last_name, licence_number FROM [Driver] WHERE driver_id = @IdDriver";
        command.Parameters.AddWithValue("@IdDriver", driverId);

        using (var reader = await command.ExecuteReaderAsync())
        {
            if (await reader.ReadAsync())
            {
                var driver = new Driver()
                {
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                    LicenceNumber = reader.GetString(reader.GetOrdinal("licence_number")),
                };

                return driver;
            }
        }

        return null;
    }

    public async Task<List<ProductDTO>> GetDeliveryProducts(int deliveryId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        /*
        command.CommandText = @"
            SELECT product_id, amount FROM Product_Delivery WHERE delivery_id = @IdDelivery
                ";
        */
        command.CommandText = @"
            SELECT pd.product_id, pd.amount, pr.name, pr.price
            FROM Product_Delivery AS pd
            JOIN Product AS pr ON pr.product_id = pd.product_id
            WHERE pd.delivery_id = @IdDelivery;
                ";
        command.Parameters.AddWithValue("@IdDelivery", deliveryId);

        var products = new List<ProductDTO>();

        using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                int idProduct = reader.GetOrdinal("product_id");
                int amountProduct = reader.GetInt32(reader.GetOrdinal("amount"));

                var productDTO = new ProductDTO()
                {
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Amount = reader.GetInt32(reader.GetOrdinal("amount")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price"))
                };

                products.Add(productDTO);
            }
        }

        return products;
    }
}