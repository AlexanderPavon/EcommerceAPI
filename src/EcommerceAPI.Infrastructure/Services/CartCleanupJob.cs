using EcommerceAPI.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EcommerceAPI.Infrastructure.Services;

public class CartCleanupJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CartCleanupJob> _logger;

    public CartCleanupJob(IApplicationDbContext context, ILogger<CartCleanupJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CleanAbandonedCartsAsync()
    {
        var cutoff = DateTime.UtcNow.AddDays(-7);

        var abandonedCarts = await _context.Carts
            .Include(c => c.Items)
            .Where(c => c.CreatedAt < cutoff)
            .ToListAsync();

        if (!abandonedCarts.Any())
        {
            _logger.LogInformation("No abandoned carts found.");
            return;
        }

        foreach (var cart in abandonedCarts)
            _context.Carts.Remove(cart);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Removed {Count} abandoned carts.", abandonedCarts.Count);
    }
}
