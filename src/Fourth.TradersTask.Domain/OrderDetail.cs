namespace Fourth.TradersTask.Domain;

/// <summary>
/// Represents an order detail (line item) in the Northwind database.
/// </summary>
public class OrderDetail
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }

    // Navigation properties
    public virtual Order? Order { get; set; }
}
