using Dapper;
using DapperAdvancedAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperAdvancedAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _config;
        public OrderRepository(IConfiguration config)
        {
            _config = config;
        }
        //4.
        public async Task CreateOrder(IEnumerable<OrderDetail> OrderDetails)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            connection.Open();
            using IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                // Generating Order
                var orderParam = new { OrderDate = DateTime.Now };
                string orderQuery = @"insert into dbo.[order](OrderDate) values (@OrderDate); 
                                  select SCOPE_IDENTITY();";
                var orderId = await connection.ExecuteScalarAsync<int>(orderQuery, orderParam, transaction, commandType: CommandType.Text);
                //throw new Exception("Could not saved ordre details");

                // saving order details
                foreach (var orderDetail in OrderDetails)
                {
                    var orderDetailParm = new
                    {
                        OrderId = orderId,
                        ProductId = orderDetail.ProductId,
                        Price = orderDetail.Price,
                        Quantity = orderDetail.Quantity
                    };
                    string orderDetailQuery = @"insert into OrderDetail 
                          (OrderId, ProductId, Price, Quantity) values
                           (@OrderId, @ProductId, @Price, @Quantity)";

                    await connection.ExecuteAsync(orderDetailQuery, orderDetailParm, transaction, commandType: CommandType.Text);
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }

        }
        //5.
        public async Task<IEnumerable<OrderDetail>> OrderDetails()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var sql = @"select o.Id, o.CustomerName, o.PhoneNumber,
                            b.Id,b.Title,b.Author,b.[Year],
                            od.Id,od.OrderId,od.ProductId,od.Price,od.Quantity
                            from OrderDetail od join [Order] o
                            on od.OrderId = o.id
                            join book b on od.ProductId=b.Id";

            var orderDetail = await connection.QueryAsync<Order, Book, OrderDetail, OrderDetail>
                (sql, (order, book, orderDetail) =>
                {
                    orderDetail.Order = order;
                    orderDetail.Book = book;
                    return orderDetail;
                }, splitOn: "Id");
            return orderDetail;
        }
    }
}
