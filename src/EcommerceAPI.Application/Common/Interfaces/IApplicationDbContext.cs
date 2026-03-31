using EcommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
