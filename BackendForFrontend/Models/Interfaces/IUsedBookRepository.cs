using BackendForFrontend.Models.Dtos;
using System.Collections.Generic;

namespace BackendForFrontend.Models.Interfaces
{
    public interface IUsedBookRepository
    {
        Task<List<UsedBookDto>> SearchAsync(string email, string ISBN, string bookName);
        Task<UsedBookDto> GetAsync(int id);
        Task UpdateProductStatusAsync(int id, bool status);
        Task EditAsync(UsedBookDto dto);
        Task DeleteAsync(int id);
        Task AddAsync(UsedBookDto dto);
        Task<List<UsedBookDto>> GetAllAsync();
        List<UsedBookDto> Search(string email, string ISBN, string bookName);
        void Edit(UsedBookDto dto);
        void UpdateProductStatus(int id, bool status);
        UsedBookDto Get(int id);
        void Delete(int id);
        void Add(UsedBookDto dto); 
        List<UsedBookDto> GetAll(); 
    }
}
