using BookStore.Application.Dtos;
using BookStore.Application.Shared;

namespace BookStore.Application.IServices
{
    public interface IBookService
    {
        Task<ResponseResult> GetAllAsync();
        Task<ResponseResult> GetDetailsAsync(int bookId);
        Task<ResponseResult> ReserveAsync(int bookId, string customerName);
        Task<ResponseResult> ReturnAsync(int bookId);
        Task<ResponseResult> CreateAsync(CreateBookDto dto);
        Task<ResponseResult> UpdateAsync(UpdateBookDto dto);
        Task<ResponseResult> DeleteAsync(int bookId);
    }
}
