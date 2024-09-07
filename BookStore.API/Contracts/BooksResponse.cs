namespace BookStore.API.Contracts;

public record class BooksResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price
);