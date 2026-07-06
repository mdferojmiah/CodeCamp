namespace session04.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Item { get; set; } = string.Empty;
    }
}
