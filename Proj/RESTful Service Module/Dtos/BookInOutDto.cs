using DBScaffold.Models;

namespace RESTful_Service_Module.Dtos
{
    public class BookInOutDto
    {
        public int? Idbook { get; set; }

        public string? Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? GenreId { get; set; }

        public static BookInOutDto GetBookDtoFromBook(Book book)
        {
            BookInOutDto dto = new();

            dto.Idbook = book.Idbook;
            dto.Name = book.Name;
            dto.Description = book.Description;
            dto.GenreId = book.GenreId;

            return dto;
        }

        public static List<BookInOutDto> GetBooksFromEnumerable(IEnumerable<Book> books)
        {
            List<BookInOutDto> booksOut = new();

            foreach (var book in books)
                booksOut.Add(GetBookDtoFromBook(book));

            return booksOut;
        }
    }
}
