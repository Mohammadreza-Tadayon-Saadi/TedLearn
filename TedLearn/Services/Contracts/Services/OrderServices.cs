using Data.Context;
using Data.Entities.Persons.Transactions;
using Data.Entities.Products.Courses;
using Data.Entities.Sales;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.Home.Order;
using Services.DTOs.UserPanel.Order;

namespace Services.Contracts.Services;

public class OrderServices : BaseServices<Order>, IOrderServices
{
    #region ConstructorInjection

    private readonly ICourseServices _courseServices;
    private readonly ITransactionDbContextServices _transactions;
    private readonly DbSet<OrderDetail> _orderDetails;
    private readonly IUserPanelServices _userPanelServices;
    public OrderServices(TedLearnContext context, ICourseServices courseServices, ITransactionDbContextServices transactions, IUserPanelServices userPanelServices) : base(context)
    {
        _courseServices = courseServices;
        _transactions = transactions;
        _orderDetails = _context.Set<OrderDetail>();
        _userPanelServices = userPanelServices;
    }

    #endregion ConstructorInjection



    public async Task<bool> IsOrderExistAsync(int orderId, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(o => o.OrderId == orderId).AnyAsync(cancellationToken);

    public async Task<ShowBasketForUserDto> GetBasketForUserAsync(int orderId, int userId, CancellationToken cancellationToken = default)
        => await ShowBasketForUserDto.ProjectTo(TableNoTracking
                                        .Include(o => o.OrderDetails).ThenInclude(od => od.Course).ThenInclude(c => c.User)
                                        .Where(o => o.OrderId == orderId && o.UserId == userId))
                    .SingleOrDefaultAsync(cancellationToken);

    public async Task<Order> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(c => c.OrderId == orderId)
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(c => c.OrderId == orderId)
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<OrderDetail> GetOrderDetailByIdAsync(int orderDetialId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await _orderDetails.Where(c => c.DetailId == orderDetialId)
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await _orderDetails.AsNoTracking()
                .Where(c => c.DetailId == orderDetialId)
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ShowOrdersInUserPanelDto>> GetOrdersForUserAsync(int userId, CancellationToken cancellationToken = default)
        => await ShowOrdersInUserPanelDto.ProjectTo(TableNoTracking
                                            .Where(o => o.UserId == userId)
                                            .Include(o => o.OrderDetails).ThenInclude(od => od.Course))
                    .ToListAsync(cancellationToken);

    public async Task<bool> FinalyOrderAsync(int userId, int orderId, CancellationToken cancellationToken = default)
    {
        var order = await Table.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                            .Where(o => o.UserId == userId && o.OrderId == orderId)
                            .SingleOrDefaultAsync(cancellationToken);

        if (order == null || order.IsFinaly)
            return false;

        var totalPrice = order.OrderDetails.Sum(od => od.Price); //* (1 - od.Discount));

        if ( (await _userPanelServices.GetStockForUserAsync(userId)) >= (totalPrice * (1 - order.Discount)))
        {
            try
            {
                order.IsFinaly = true;

                var transaction = new Transaction
                {
                    Amount = (totalPrice * (1 - order.Discount)),
                    IsPay = true,
                    TypeId = 2,
                    UserId = userId,
                    TransactionDate = DateTime.Now,
                    Description = "فاکتور شماره #" + order.OrderId,
                };

                await _userPanelServices.AddTransactionAsync(transaction, cancellationToken , withSaveChanges: false);

                foreach (var detail in order.OrderDetails)
                {
                    await _courseServices.AddUserCourseAsync(new UserCourse()
                    {
                        UserId = userId,
                        CourseId = detail.CourseId
                    } , cancellationToken , withSaveChanges: false);
                }

                await _transactions.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }



    public async Task<int> AddOrderAsync(int courseId, int userId, CancellationToken cancellationToken = default)
    {
        decimal coursePrice = await _courseServices.GetCoursePriceByIdAsync(courseId, cancellationToken);
        Order order = await Table.Where(o => o.UserId == userId && !o.IsFinaly).FirstOrDefaultAsync(cancellationToken);

        if (order == null)
        {
            order = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                IsFinaly = false,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail()
                    {
                        CourseId = courseId,
                        Price = coursePrice,
                    }
                }
            };
            await Entity.AddAsync(order , cancellationToken);
        }
        else
        {
            OrderDetail orderDetail = await _orderDetails
                                                .Where(od => od.OrderId == order.OrderId && od.CourseId == courseId)
                                                .FirstOrDefaultAsync(cancellationToken);

            if (orderDetail == null)
            {
                orderDetail = new OrderDetail()
                {
                    CourseId = courseId,
                    Price = coursePrice,
                    OrderId = order.OrderId,
                };
                await _orderDetails.AddAsync(orderDetail , cancellationToken);
            }
        }

        await _transactions.SaveChangesAsync(cancellationToken);

        return order.OrderId;
    }

    public async Task RemoveOrderDetailAsync(OrderDetail orderDetail, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        _orderDetails.Remove(orderDetail);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
