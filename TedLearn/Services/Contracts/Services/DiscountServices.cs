using Data.Context;
using Data.Entities.Persons.Discounts;
using Data.Entities.Products.Courses;
using Data.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Services.DTOs.AdminPanel.UserDiscount;
using Services.DTOs.Home.Order;
using System.Linq.Expressions;

namespace Services.Contracts.Services;

public class DiscountServices : BaseServices<UDiscount>, IDiscountServices
{
    #region ConstructorInjection

    private readonly ITransactionDbContextServices _transactions;
    private readonly DbSet<UserDiscount> _userDiscounts;
    public DiscountServices(TedLearnContext context, ITransactionDbContextServices transactions) : base(context)
    {
        _userDiscounts = _context.Set<UserDiscount>();
        _transactions = transactions;
    }

    #endregion ConstructorInjection

    public async Task<FilteredUserDiscountDto> GetUserDiscountListAsync(Expression<Func<UDiscount, object>> orderByExpression, int pageId = 1, int take = 20, CancellationToken cancellationToken = default, bool? isDeleted = null)
    {
        FilteredUserDiscountDto filteredUserDiscount = new FilteredUserDiscountDto();

        var skip = (pageId - 1) * take;

        IQueryable<UDiscount> query = TableNoTracking.Where(ud => (isDeleted.HasValue) ? ud.IsDelete == isDeleted : true);

        var queryCount = query.Count();
        var pageCount = queryCount / take;
        if (queryCount % take != 0) pageCount++;

        filteredUserDiscount.UserDiscounts = await UserDiscountDto.ProjectTo(query.Skip(skip)
                                                                            .Take(take)
                                                                            .OrderByDescending(orderByExpression))
                                                    .ToListAsync(cancellationToken);
        filteredUserDiscount.Paginantion = new PaginantionDto
        {
            Currentpage = pageId,
            ItemsCount = queryCount,
            ItemsPerPage = take,
            PageCount = pageCount
        };

        return filteredUserDiscount;
    }

    public async Task<bool> IsExistsUDiscountCodeAsync(string uDicountCode, CancellationToken cancellationToken = default)
        => await TableNoTracking.Where(ud => ud.DiscountCode == uDicountCode).AnyAsync(cancellationToken);

    public async Task<UDiscount> GetUDiscountByIdAsync(int uDiscountId, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(ud => ud.DiscountId == uDiscountId && (getActive.HasValue ? ud.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(ud => ud.DiscountId == uDiscountId && (getActive.HasValue ? ud.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<UDiscount> GetUDiscountByCodeAsync(string discountCode, CancellationToken cancellationToken = default, bool withTracking = true, bool? getActive = null)
    {
        if (withTracking)
            return await Table.Where(ud => ud.DiscountCode == discountCode && (getActive.HasValue ? ud.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await TableNoTracking.Where(ud => ud.DiscountCode == discountCode && (getActive.HasValue ? ud.IsDelete == !getActive : true))
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDiscount> GetUserDiscountByOrderIdAsync(int orderId, CancellationToken cancellationToken = default, bool withTracking = true)
    {
        if (withTracking)
            return await _userDiscounts
                .Where(ud => ud.OrderId == orderId)
                .SingleOrDefaultAsync(cancellationToken);
        else
            return await _userDiscounts.AsNoTracking()
                .Where(ud => ud.OrderId == orderId)
                .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsUsedDiscountByUserAsync(int userId, int discountId, CancellationToken cancellationToken = default)
        => await _userDiscounts.Where(ud => ud.DiscountId == discountId && ud.UserId == userId).AnyAsync(cancellationToken);

    public async Task<DiscountUseType> CheckDiscountForApplyToOrderAsync(UDiscount discount, int userId, CancellationToken cancellationToken = default)
    {
        if (discount == null) return DiscountUseType.NotFound;

        if (await IsUsedDiscountByUserAsync(userId, discount.DiscountId , cancellationToken))
            return DiscountUseType.IsUsedByUser;

        if (discount.StartDate > DateTime.Now)
            return DiscountUseType.NotStartedDate;

        if (discount.EndDate < DateTime.Now)
            return DiscountUseType.ExpireDate;

        if (discount.UsableCount == 0)
            return DiscountUseType.IsFinished;

        return DiscountUseType.Successed;
    }




    public async Task AddUDiscountAsync(UDiscount uDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await Entity.AddAsync(uDiscount, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task AddUserDiscountAsync(UserDiscount userDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        await _userDiscounts.AddAsync(userDiscount, cancellationToken);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task RemoveUserDiscountAsync(UserDiscount userDiscount, CancellationToken cancellationToken = default, bool withSaveChanges = true, bool configureAwait = false)
    {
        _userDiscounts.Remove(userDiscount);

        if (withSaveChanges)
            await _transactions.SaveChangesAsync(cancellationToken, configureAwait);
    }
}
