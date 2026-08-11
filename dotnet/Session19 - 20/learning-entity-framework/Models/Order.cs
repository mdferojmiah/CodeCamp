namespace learning_entity_framework.Models;

public class Order
{
    public Guid Id { get; set; }
    public decimal ToTal { get; set; }

    public Guid PersonId { get; set; }
    public virtual Person? Person { get; set; }
}