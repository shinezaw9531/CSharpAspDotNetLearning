namespace DapperAdvancedAPI.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Order? Order { get; set; }
        public Book? Book { get; set; }
    }
}
