using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Payments.CreatePaymentIntent;

public class CreatePaymentIntentHandler : IRequestHandler<CreatePaymentIntentCommand, CreatePaymentIntentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IPaymentService _paymentService;

    public CreatePaymentIntentHandler(IApplicationDbContext context, IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    public async Task<CreatePaymentIntentResponse> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == request.UserId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException("Order not found");

        var (clientSecret, paymentIntentId) = await _paymentService.CreatePaymentIntentAsync(order.Total);

        order.StripePaymentIntentId = paymentIntentId;
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return new CreatePaymentIntentResponse(clientSecret, paymentIntentId);
    }
}
