using EcommerceAPI.Domain.Enums;
using MediatR;

namespace EcommerceAPI.Application.Features.Orders.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest<bool>;
