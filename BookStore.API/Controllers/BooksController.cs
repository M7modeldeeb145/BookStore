using BookStore.Application.IServices;
using BookStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return StatusCode(result.Code, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _bookService.GetDetailsAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> Reserve(int id, [FromQuery] string customer)
        {
            if (string.IsNullOrWhiteSpace(customer)) return BadRequest("Customer name is required");
            var result = await _bookService.ReserveAsync(id, customer);
            return StatusCode(result.Code, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            var result = await _bookService.CreateAsync(dto);
            return StatusCode(result.Code, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBookDto dto)
        {
            var result = await _bookService.UpdateAsync(dto);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(int id)
        {
            var result = await _bookService.ReturnAsync(id);
            return StatusCode(result.Code, result);
        }
    }
}
