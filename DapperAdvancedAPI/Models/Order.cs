namespace DapperAdvancedAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
