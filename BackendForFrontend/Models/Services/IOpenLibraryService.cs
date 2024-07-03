using BackendForFrontend.Models.Dtos;
using BackendForFrontend.Models.Interfaces;
using System.Net.Http;

namespace BackendForFrontend.Models.Services
{
    public interface IOpenLibraryService : IUsedBookISBN
    {
        // 已經在IBookService中定義了GetBookByISBNAsync
    }

   
}
