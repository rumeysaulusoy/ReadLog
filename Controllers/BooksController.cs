using Microsoft.EntityFrameworkCore;
namespace ReadLog.Controllers;
using Microsoft.AspNetCore.Mvc;
using ReadLog.Data;
using ReadLog.Models;

public class BooksController : Controller {
    private readonly ApplicationDbContext _context;

    public BooksController(ApplicationDbContext context) {
        _context = context;
    }
    
    public IActionResult Index() {
        var books = _context.Books.ToList();
        return View(books);
    }
    
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book) {
        
        if (ModelState.IsValid) {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index)); 
        }
        return View(book);
    }

    
    public async Task<IActionResult> Edit(int id) {
        var book = await _context.Books.FindAsync(id);
        if (book == null) {
            return NotFound();
        }
        return View(book);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,PublishedDate")] Book book) {
        
        if (id != book.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!BookExists(book.Id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(book);
    }

    private bool BookExists(int id) {
        return _context.Books.Any(e => e.Id == id);
    }
    
    public async Task<IActionResult> Delete(int id) {
        var book = await _context.Books.FindAsync(id);
        if (book == null) {
            return NotFound();
        }
        return View(book);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) {
        
        var book = await _context.Books.FindAsync(id);
        if (book != null) {

            var readingProgresses = _context.ReadingProgresses.Where(rp => rp.BookId == id).ToList();
            
            if (readingProgresses.Any()) {
                _context.ReadingProgresses.RemoveRange(readingProgresses);
            }
            
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Details(int id) {

        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
    
        if (book == null) {
            return NotFound();
        }
        
        var readingProgress = await _context.ReadingProgresses.FirstOrDefaultAsync(rp => rp.BookId == id);
    
        if (readingProgress == null) {
            readingProgress = new ReadingProgress {
                BookId = book.Id,
                StartDate = DateTime.Now, 
                PagesRead = 0
            };

            _context.ReadingProgresses.Add(readingProgress); 
            await _context.SaveChangesAsync(); 
        }

        ViewBag.Book = book; 
        ViewBag.ReadingProgress = readingProgress; 

        return View(); 
    }
}
