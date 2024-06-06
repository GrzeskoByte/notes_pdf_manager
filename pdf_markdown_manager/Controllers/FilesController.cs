using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pdf_markdown_manager.Models;
using Microsoft.AspNetCore.Identity;

namespace pdf_markdown_manager.Controllers
{
    public class FilesController : Controller
    {
        private readonly DocumentsManagerContext _context;
         private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;


        public FilesController(DocumentsManagerContext context, UserManager<AuthUser> userMan, SignInManager<AuthUser> signMan)
        {
            _context = context;
            _userManager = userMan;
            _signInManager = signMan;

        }

        // GET: Files
        public async Task<IActionResult> Index()
        {

             if (!_signInManager.IsSignedIn(User)) return View(await _context.Files.Where(x => false).ToListAsync());

            string userId = _userManager.GetUserId(User);

            return View(await _context.Files.Where(x => x.users_id == userId).ToListAsync());

        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files
                .FirstOrDefaultAsync(m => m.id == id);
            if (files == null)
            {
                return NotFound();
            }

            return View(files);
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            Files file = await _context.Files.FindAsync(id);

            byte[] fileBytes = file.content;

            string fileName = file.title + ".pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,content")] Files files)
        {
            if (ModelState.IsValid)
            {
                _context.Add(files);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(files);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files.FindAsync(id);
            if (files == null)
            {
                return NotFound();
            }
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,content")] Files files)
        {
            if (id != files.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(files);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilesExists(files.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(files);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await _context.Files
                .FirstOrDefaultAsync(m => m.id == id);
            if (files == null)
            {
                return NotFound();
            }

            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var files = await _context.Files.FindAsync(id);
            if (files != null)
            {
                _context.Files.Remove(files);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilesExists(int id)
        {
            return _context.Files.Any(e => e.id == id);
        }
    }
}
