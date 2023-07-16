﻿using AutoMapper.QueryableExtensions;
using Data.Context;
using Data.Entities.Persons.Transactions;
using Data.Entities.Persons.Users;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Interfaces;
using Services.DTOs.UserPanel;
using TedLearn_Core.ViewModels.UserPanel;

namespace Services.Contracts.Services;

public class UserPanelServices : BaseServices<User> , IUserPanelServices
{
    #region ConstructorInjection

    public UserPanelServices(TedLearnContext context) : base(context)
    {
    }

    #endregion

    public Task<EditUserProfileDto> GetUserForEdit(int userId , CancellationToken cancellationToken)
        => EditUserProfileDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);

    public Task<UserInformationDto> GetUserInformation(int userId, CancellationToken cancellationToken)
        => UserInformationDto.ProjectTo(TableNoTracking.Where(u => u.UserId == userId)).SingleOrDefaultAsync(cancellationToken);

    public async Task<decimal> GetStockForUserAsync(int userId, CancellationToken cancellationToken)
    {
        var userTransactions = await _context.Set<Transaction>().AsNoTracking()
                                        .Where(t => t.UserId == userId).Select(t => new
                                        {
                                            t.Amount,
                                            t.IsPay,
                                            t.TypeId,
                                        }).ToListAsync(cancellationToken);

        if (userTransactions.Count > 0)
        {
            var quits = userTransactions.Where(t => t.TypeId == 1 && t.IsPay)
                                             .Select(t => t.Amount).ToList();
            var spent = userTransactions.Where(t => t.TypeId == 2)
                                             .Select(t => t.Amount).ToList();

            return (quits.Sum() - spent.Sum());
        }
        else return 0;
    }

    public async Task<int> AddTransactionAsync(Transaction transaction , CancellationToken cancellationToken, 
        bool configureAwait = false)
    {
        await _context.AddAsync(transaction);
        await SaveChangesAsync(cancellationToken , configureAwait);
        return transaction.TransactionId;
    }

    public async Task<int> ChargeWalletAsync(int userId, decimal amount, string description, 
        CancellationToken cancellationToken, bool isPay = false, bool configureAwait = false)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            TypeId = 1,
            Amount = amount,
            Description = description,
            IsPay = isPay,
            TransactionDate = DateTime.Now
        };

        return await AddTransactionAsync(transaction , cancellationToken , configureAwait);
    }

    public async Task<Transaction> GetTransactionByTransactionIdAsync(int transactionId, 
        CancellationToken cancellationToken, bool withTracking = true)
    {
        if (withTracking)
            return await _context.Set<Transaction>()
                            .Where(t => t.TransactionId == transactionId)
                            .SingleOrDefaultAsync(cancellationToken);

        return await _context.Set<Transaction>().AsNoTracking()
                        .Where(t => t.TransactionId == transactionId)
                        .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool configureAwait = false)
    {
        _context.Update(transaction);
        await SaveChangesAsync(cancellationToken , configureAwait);
    }

    public async Task RemoveTransactionAsync(Transaction transaction, CancellationToken cancellationToken, bool configureAwait = false)
    {
        _context.Remove(transaction);
        await SaveChangesAsync(cancellationToken, configureAwait);
    }

    public async Task<TransactionReportsWithPaginationDto> GetTransationsForUser(int userId , CancellationToken cancellationToken, int pageId = 1)
    {
        var transactionReports = new TransactionReportsWithPaginationDto();

        IQueryable<Transaction> transactions = _context.Set<Transaction>().AsNoTracking()
                                                    .Where(t => t.UserId == userId && t.IsPay);
        
        int take = 15;
        int skip = (pageId - 1) * take;
        transactionReports.CurrentPage = pageId;

        var allRecordCount = await transactions.CountAsync();
        transactionReports.PageCount = allRecordCount / take;
        if (allRecordCount % take != 0) transactionReports.PageCount++;

        transactionReports.TransactionReports = await TransactionReportDto.ProjectTo(transactions.Skip(skip).Take(take)).ToListAsync(cancellationToken);
        
        return transactionReports;
    }
}