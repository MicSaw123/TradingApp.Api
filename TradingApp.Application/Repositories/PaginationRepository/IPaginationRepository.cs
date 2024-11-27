using TradingApp.Application.DataTransferObjects.PaginationDto;

namespace TradingApp.Application.Repositories.PaginationRepository
{
    public interface IPaginationRepository
    {
        Task<PaginationDto> GetPaginationDto(string connectionId);
    }
}
