using EcommerceAPI.Domain.Enums;

namespace EcommerceAPI.Domain.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal Total { get; set; }
    public string? StripePaymentIntentId { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
