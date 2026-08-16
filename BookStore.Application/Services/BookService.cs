using AutoMapper;
using BookStore.Application.Dtos;
using BookStore.Application.IServices;
using BookStore.Application.Shared;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;

namespace BookStore.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly FluentValidation.IValidator<CreateBookDto> _createValidator;
        private readonly FluentValidation.IValidator<UpdateBookDto> _updateValidator;

        public BookService(IUnitOfWork uow, IMapper mapper,
            FluentValidation.IValidator<CreateBookDto> createValidator,
            FluentValidation.IValidator<UpdateBookDto> updateValidator)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<ResponseResult> CreateAsync(CreateBookDto dto)
        {
            try
            {
                var validation = await _createValidator.ValidateAsync(dto);
                if (!validation.IsValid)
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Code = 400,
                        MessageEn = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)),
                        MessageAr = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage))
                    };
                }

                // ensure unique title
                var existing = await _uow.Repository<Book>().GetAsync(b => b.Title.ToLower() == dto.Title.ToLower());
                if (existing != null)
                {
                    return new ResponseResult 
                    { 
                        Result = Result.Exist, 
                        Code = 400, 
                        MessageEn = "A book with the same title already exists",
                        MessageAr = "يوجد كتاب بنفس العنوان بالفعل"
                    };
                }

                var book = new Book { Title = dto.Title, TotalCopies = dto.TotalCopies, AvailableCopies = dto.TotalCopies };
                await _uow.Repository<Book>().CreateAsync(book);
                await _uow.SaveAsync();

                var createdDto = _mapper.Map<BookDto>(book);
                return new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 201, 
                    Data = createdDto, 
                    DataCount = 1, 
                    MessageEn = "Book created successfully",
                    MessageAr = "تم إنشاء الكتاب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message 
                };
            }
        }

        public async Task<ResponseResult> UpdateAsync(UpdateBookDto dto)
        {
            try
            {
                var validation = await _updateValidator.ValidateAsync(dto);
                if (!validation.IsValid)
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Code = 400,
                        MessageEn = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)),
                        MessageAr = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage))
                    };
                }

                var book = await _uow.Repository<Book>().GetAsync(b => b.Id == dto.Id);
                if (book == null) return new ResponseResult 
                { 
                    Result = Result.NoDataFound, 
                    Code = 404, 
                    MessageEn = "Book not found",
                    MessageAr = "الكتاب غير موجود"
                };

                // check unique title
                var existing = await _uow.Repository<Book>().GetAsync(b => b.Title.ToLower() == dto.Title.ToLower() && b.Id != dto.Id);
                if (existing != null) 
                    return new ResponseResult 
                    { 
                        Result = Result.Exist, 
                        Code = 400, 
                        MessageEn = "A book with the same title already exists",
                        MessageAr = "يوجد كتاب بنفس العنوان بالفعل"
                    };

                var diff = dto.TotalCopies - book.TotalCopies;
                book.Title = dto.Title;
                book.TotalCopies = dto.TotalCopies;
                book.AvailableCopies = Math.Max(0, book.AvailableCopies + diff);

                _uow.Repository<Book>().Update(book);
                await _uow.SaveAsync();

                var updated = _mapper.Map<BookDto>(book);
                return new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 200, 
                    Data = updated, 
                    DataCount = 1,
                    MessageEn = "Book updated successfully",
                    MessageAr = "تم تحديث الكتاب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message 
                };
            }
        }

        public async Task<ResponseResult> DeleteAsync(int bookId)
        {
            try
            {
                var book = await _uow.Repository<Book>().GetAsync(b => b.Id == bookId);
                if (book == null) 
                    return new ResponseResult 
                    { 
                        Result = Result.NoDataFound, 
                        Code = 404, 
                        MessageEn = "Book not found",
                        MessageAr = "الكتاب غير موجود"
                    };

                await _uow.Repository<Book>().SoftDeleteAsync(bookId);
                await _uow.SaveAsync();
                return new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 200, 
                    MessageEn = "Deleted",
                    MessageAr = "تم الحذف"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message
                };
            }
        }
        public async Task<ResponseResult> GetAllAsync()
        {
            try
            {
                var books = (await _uow.Repository<Book>().GetAllAsync()).ToList();
                var dtos = _mapper.Map<List<BookDto>>(books);
                // populate waiting list counts
                foreach (var dto in dtos)
                {
                    var count = (await _uow.Repository<WaitingListEntry>().GetAllAsync(w => w.BookId == dto.Id)).Count();
                    dto.WaitingListCount = count;
                }

                return new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 200,
                    Data = dtos, 
                    DataCount = dtos.Count, 
                    TotalCount = dtos.Count,
                    MessageEn = "Books retrieved successfully",
                    MessageAr = "تم استرجاع الكتب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message 
                };
            }
        }

        public async Task<ResponseResult> GetDetailsAsync(int bookId)
        {
            try
            {
                var book = await _uow.Repository<Book>().GetAsync(b => b.Id == bookId);
                if (book == null) 
                    return new ResponseResult 
                    { 
                        Result = Result.NoDataFound, 
                        Code = 404, 
                        MessageEn = "Book not found",
                        MessageAr = "الكتاب غير موجود"
                    };

                var dto = _mapper.Map<BookDto>(book);
                dto.WaitingListCount = (await _uow.Repository<WaitingListEntry>().GetAllAsync(w => w.BookId == dto.Id)).Count();
               
                return new ResponseResult 
                { 
                    Result = Result.Success,
                    Code = 200, 
                    Data = dto, 
                    DataCount = 1, 
                    TotalCount = 1,
                    MessageEn = "Book details retrieved successfully",
                    MessageAr = "تم استرجاع تفاصيل الكتاب بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message 
                };
            }
        }

        public async Task<ResponseResult> ReserveAsync(int bookId, string customerName)
        {
            try
            {
                var book = await _uow.Repository<Book>().GetAsync(b => b.Id == bookId);
                if (book == null)
                    return new ResponseResult 
                    { 
                        Result = Result.NoDataFound, 
                        Code = 404, 
                        MessageEn = "Book not found",
                        MessageAr = "الكتاب غير موجود"
                    };

                var existingReservation = (await _uow.Repository<Reservation>()
                    .GetAllAsync(r => r.BookId == bookId && r.CustomerName == customerName)).FirstOrDefault();

                if (existingReservation != null) 
                    return new ResponseResult { 
                        Result = Result.Exist, 
                        Code = 400, 
                        MessageEn = "Customer already has a reservation for this book",
                        MessageAr = "العميل لديه حجز بالفعل لهذا الكتاب"
                    };

                var existingWaiting = (await _uow.Repository<WaitingListEntry>()
                    .GetAllAsync(w => w.BookId == bookId && w.CustomerName == customerName)).FirstOrDefault();

                if (existingWaiting != null) return new ResponseResult { 
                    Result = Result.Exist, 
                    Code = 400, 
                    MessageEn = "Customer is already in the waiting list for this book",
                    MessageAr = "العميل موجود بالفعل في قائمة الانتظار لهذا الكتاب"
                };

                if (book.AvailableCopies > 0)
                {
                    var reservation = new Reservation 
                    { BookId = bookId, CustomerName = customerName };

                    await _uow.Repository<Reservation>().CreateAsync(reservation);

                    book.AvailableCopies = Math.Max(0, book.AvailableCopies - 1);
                    _uow.Repository<Book>().Update(book);
                    var saved = await _uow.SaveAsync();

                    return saved ? new ResponseResult 
                    { 
                        Result = Result.Success, 
                        Code = 201, 
                        MessageEn = "Reserved",
                        MessageAr = "تم الحجز"
                    } : new ResponseResult 
                    { 
                        Result = Result.Failed, 
                        Code = 500, 
                        MessageEn = "Failed to reserve",
                        MessageAr = "فشل في الحجز"
                    };
                }

                var entry = new WaitingListEntry 
                { BookId = bookId, CustomerName = customerName };

                await _uow.Repository<WaitingListEntry>().CreateAsync(entry);
                var savedWaiting = await _uow.SaveAsync();

                return savedWaiting ? new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 201, 
                    MessageEn = "Added to waiting list",
                    MessageAr = "تمت الإضافة إلى قائمة الانتظار"
                } : new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = "Failed to join waiting list",
                    MessageAr = "فشل في الانضمام إلى قائمة الانتظار"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message 
                };
            }
        }

        public async Task<ResponseResult> ReturnAsync(int bookId)
        {
            try
            {
                var book = await _uow.Repository<Book>().GetAsync(b => b.Id == bookId);
                if (book == null) 
                    return new ResponseResult 
                    { 
                        Result = Result.NoDataFound, 
                        Code = 404, 
                        MessageEn = "Book not found" 
                    };

                var waiting = (await _uow.Repository<WaitingListEntry>()
                    .GetAllAsync(w => w.BookId == bookId, asNoTracking: false))
                              .OrderBy(w => w.JoinedAt)
                              .FirstOrDefault();

                if (waiting != null)
                {
                    var reservation = new Reservation 
                    { BookId = bookId, CustomerName = waiting.CustomerName };
                    await _uow.Repository<Reservation>().CreateAsync(reservation);
                    await _uow.Repository<WaitingListEntry>().SoftDeleteAsync(waiting.Id);
                    var saved = await _uow.SaveAsync();

                    return saved ? new ResponseResult 
                    { 
                        Result = Result.Success, 
                        Code = 200, 
                        MessageEn = "Assigned to waiting customer",
                        MessageAr = "تم التعيين للعميل في قائمة الانتظار"
                    } : new ResponseResult 
                    { 
                        Result = Result.Failed, 
                        Code = 500, 
                        MessageEn = "Failed to assign to waiting customer",
                        MessageAr = "فشل في التعيين للعميل في قائمة الانتظار"
                    };
                }

                // Do not increase available copies beyond total copies
                if (book.AvailableCopies >= book.TotalCopies)
                {
                    return new ResponseResult
                    {
                        Result = Result.Success,
                        Code = 200,
                        MessageEn = "All copies are already available",
                        MessageAr = "جميع النسخ متوفرة بالفعل"
                    };
                }

                book.AvailableCopies = book.AvailableCopies + 1;
                _uow.Repository<Book>().Update(book);
                var result = await _uow.SaveAsync();
                return result ? new ResponseResult 
                { 
                    Result = Result.Success, 
                    Code = 200, 
                    MessageEn = "Returned",
                    MessageAr = "تمت الإرجاع"
                } : new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = "Failed to return book",
                    MessageAr = "فشل في إرجاع الكتاب"
                };
            }
            catch (Exception ex)
            {
                return new ResponseResult 
                { 
                    Result = Result.Failed, 
                    Code = 500, 
                    MessageEn = ex.Message,
                    MessageAr = "فشل في إرجاع الكتاب"
                };
            }
        }
    }
}
