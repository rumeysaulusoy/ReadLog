using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadLog.Data;
using ReadLog.Models;

namespace ReadLog.Controllers;

public class ReadingProgressController : Controller {
    private readonly ApplicationDbContext _context;

    public ReadingProgressController(ApplicationDbContext context) {
        _context = context;
    }
    
    public async Task<IActionResult> Index() {
        var readingProgressList = await _context.ReadingProgresses.ToListAsync();
        return View(readingProgressList);
    }
    
    
    public async Task<IActionResult> Edit(int id) {
        var readingProgress = await _context.ReadingProgresses.FindAsync(id);
        if (readingProgress == null) {
            return NotFound();
        }
        return View(readingProgress);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReadingProgress readingProgress) {
        if (id != readingProgress.Id) {
            return NotFound();
        }

        if (ModelState.IsValid) {
            try {
                _context.Update(readingProgress);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ReadingProgressExists(readingProgress.Id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return RedirectToAction("Details", "Books", new { id = readingProgress.BookId });
        }
        return View(readingProgress);
    }

    private bool ReadingProgressExists(int id) {
        return _context.ReadingProgresses.Any(e => e.Id == id);
    }
}
