using Data.Entities.Persons.Discounts;
using Services.DTOs.AdminPanel.UserDiscount;
using Services.DTOs.Home.Order;
using System.Linq.Expressions;

namespace Services.Contracts.Interfaces;

public interface IDiscountServices
{
    Task<FilteredUserDiscountDto> GetUserDiscountListAsync(Expression<Func<UDiscount, object>> orderByExpression, int pageId = 1, int take = 20, CancellationToken cancellationToken = default, bool? isDeleted = null);
    Task<bool> IsExistsUDiscountCodeAsync(string uDicountCode, CancellationToken cancellationToken = default);
    Task<UDiscount> GetUDiscountByIdAsync(int uDiscountId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<UDiscount> GetUDiscountByCodeAsync(string discountCode, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null);
    Task<UserDiscount> GetUserDiscountByOrderIdAsync(int orderId, CancellationToken cancellationToken = default, bool withTracking = true);
    Task<DiscountUseType> CheckDiscountForApplyToOrderAsync(UDiscount discount, int userId, CancellationToken cancellationToken = default);

    Task AddUDiscountAsync(UDiscount uDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task AddUserDiscountAsync(UserDiscount userDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
    Task RemoveUserDiscountAsync(UserDiscount userDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false);
}
