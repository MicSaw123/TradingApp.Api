using System.Data;

namespace TradingApp.Application.Repositories.DbTransactionRepository
{
    public interface IDbTransactionRepository
    {
        IDbTransaction BeginTransaction();
    }
}
