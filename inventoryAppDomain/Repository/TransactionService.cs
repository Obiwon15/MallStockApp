using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;

namespace inventoryAppDomain.Repository
{
    public class TransactionService : ITransactionService
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _dbContext;

        public TransactionService(IOrderService orderService)
        {
            _orderService = orderService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        public async Task<Transaction> CreateTransaction(string txRef, string generatedReference, int orderId)
        {
            var order = _orderService.GetOrderById(orderId);

            if (order != null)
            {
                var transaction = new Transaction
                {
                    Amount = order.Price.ToString(CultureInfo.InvariantCulture),
                    Order = order,
                    OrderId = orderId,
                    ReferenceNumber = txRef,
                    GeneratedReferenceNumber = generatedReference,
                };
                _dbContext.Transactions.Add(transaction);
                await _dbContext.SaveChangesAsync();
                return transaction;
            }

            throw new Exception("Order Not Found");
        }

        public async Task<Transaction> GetTransactionByTxRef(string txRef)
        {
            var transactions = await GetAllTransactions();
            return transactions.FirstOrDefault(transaction => transaction.ReferenceNumber == txRef);
        }

        public async Task<Transaction> GetTransactionByGeneratedRef(string generatedRef)
        {
            var transactions = await GetAllTransactions();
            return transactions.FirstOrDefault(transaction => transaction.GeneratedReferenceNumber == generatedRef);
        }

        public async Task<Transaction> GetTransactionById(int id)
        {
            var transactions = await GetAllTransactions();
            return transactions.FirstOrDefault(transaction => transaction.Id == id);
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _dbContext.Transactions.ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByStatus(TransactionStatus transactionStatus)
        {
            var transactions = await GetAllTransactions();
            return transactions.Where(transaction => transaction.TransactionStatus == transactionStatus).ToList();
        }

        public async Task<Transaction> GetLastTransaction()
        {
            return (await GetAllTransactions()).Last();
        }
    }
}