using BackendForFrontend.Models.Dtos;

namespace BackendForFrontend.Models.Interfaces
{
    public interface IUsedBookISBN
    {
        Task<UsedBookDto> GetBookByISBNAsync(string isbn);
    }
}
