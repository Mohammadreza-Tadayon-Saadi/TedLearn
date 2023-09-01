using Data.Entities.Sales;
using Services.DTOs.Home.Order;
using Services.DTOs.UserPanel.Order;

namespace Services.Contracts.Interfaces;

public interface IOrderServices
{
    Task<bool> IsOrderExistAsync(int orderId, CancellationToken cancellationToken = default);
    Task<ShowBasketForUserDto> GetBasketForUserAsync(int orderId, int userId, CancellationToken cancellationToken = default);
    Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetialId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<IEnumerable<ShowOrdersInUserPanelDto>> GetOrdersForUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> FinalyOrderAsync(int userId, int orderId, CancellationToken cancellationToken = default);

    Task<int> AddOrderAsync(int courseId, int userId, CancellationToken cancellationToken = default);
    Task RemoveOrderDetailAsync(OrderDetail orderDetail, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
