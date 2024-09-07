using BookStore.Core.Abstractions;
using BookStore.Core.Models;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class BooksReposytory : IBooksRepository
{
    private readonly BookStoreDbContext _context;

    public BooksReposytory(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> Get()
    {
        var booksEntities = await _context.Books
            .AsNoTracking()
            .ToListAsync();

        var books = booksEntities
            .Select(be => Book.Create(be.Id, be.Title, be.Description, be.Price).Book)
            .ToList();

        return books;
    }

    public async Task<Guid> Create(Book book)
    {
        var bookEntity = new BookEntity
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Price = book.Price,
        };

        await _context.Books.AddAsync(bookEntity);
        await _context.SaveChangesAsync();

        return bookEntity.Id;
    }

    public async Task<Guid> Update(Guid id, string title, string description, decimal price)
    {
        await _context.Books
            .Where(be => be.Id == id)
            .ExecuteUpdateAsync(
                q => q
                    .SetProperty(be => be.Title, be => title)
                    .SetProperty(be => be.Description, be => description)
                    .SetProperty(be => be.Price, be => price)
            );

        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Books
            .Where(be => be.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}
