using CoffeeShopApp.Data;
using CoffeeShopApp.Models;
using CoffeeShopApp.Patterns;
using Microsoft.Data.SqlClient;

namespace CoffeeShopApp;

class Program
{
    static void Main(string[] args)
    {
        //Factory
        ICoffee myCoffee = CoffeeFactory.CreateCoffee("latte");
        Console.WriteLine(myCoffee.GetDescription());
        Console.WriteLine(myCoffee.GetCost());

        //Decorator
        //adding milk and sugar
        myCoffee = new MilkDecorator(myCoffee);
        myCoffee = new SugarDecorator(myCoffee);
        Console.WriteLine("Order: " + myCoffee.GetDescription());
        Console.WriteLine("Cost: " + myCoffee.GetCost());

        //Strategy
        IDiscountStrategy discountProvider = new LuckyCustomerDiscount();
        var discountedAmount = discountProvider.GetDiscountedAmount(myCoffee.GetCost());
        Console.WriteLine("After Discount: " + discountedAmount.ToString("0.00") + " Taka");

        //Adapter
        InvoiceAdapter invoiceAdapter = new InvoiceAdapter(new LegacySystem());
        invoiceAdapter.generateInvoice("Feroj Miah", discountedAmount);



        //ado.net practice
        try
        {
            using var connection = SqlDbConnection.instance.CreateConnection();
            connection.Open();

            //insert query
            string insertQuery = "INSERT INTO Orders(CustomerId, OrderDate, TotalAmount) OUTPUT INSERTED.Id VALUES (@customerId, @orderDate, @totalAmount)";
            using var sqlInsertCommand = new SqlCommand(insertQuery, connection);
            sqlInsertCommand.Parameters.AddWithValue("@customerId", 1);
            sqlInsertCommand.Parameters.AddWithValue("@orderDate", DateTime.UtcNow);
            sqlInsertCommand.Parameters.AddWithValue("@totalAmount", discountedAmount); 
            var newOrderId = sqlInsertCommand.ExecuteScalar();
            Console.WriteLine($"Order has been added at OrderId: {newOrderId}");

            //update query
            string updateQuery = "UPDATE Orders SET TotalAmount = TotalAmount + 10 WHERE Id = @id;";
            using var sqlUpdateCommand = new SqlCommand(updateQuery, connection);
            sqlUpdateCommand.Parameters.AddWithValue("@id", newOrderId);
            sqlUpdateCommand.ExecuteNonQuery();
            Console.WriteLine($"Updated successfully at OrderId: {newOrderId}");

            //select query
            string selectQuery = "SELECT Id, CustomerId, OrderDate, TotalAmount FROM Orders;";
            using var selectCommand = new SqlCommand(selectQuery, connection);
            using var reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"Id: {reader["Id"]} | Date: {reader["OrderDate"]} | Amount: {reader["TotalAmount"]} Taka");
            } 
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Database operation failed: {ex.Message}");
        }
    }
}