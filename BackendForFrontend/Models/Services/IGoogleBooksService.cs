using BackendForFrontend.Models.Dtos;

namespace BackendForFrontend.Models.Services
{
    public interface IGoogleBooksService
    {
        Task<UsedBookDto> GetBookByISBNAsync(string ISBN);
    }
}
