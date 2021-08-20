using System.Collections.Generic;
using System.Threading.Tasks;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;

namespace inventoryAppDomain.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(string txRef, string generatedReference, int orderId);
        Task<Transaction> GetTransactionByTxRef(string txRef);
        Task<Transaction> GetTransactionByGeneratedRef(string generatedRef);
        Task<Transaction> GetTransactionById(int id);
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetTransactionsByStatus(TransactionStatus transactionStatus);

        Task<Transaction> GetLastTransaction();
    }
}