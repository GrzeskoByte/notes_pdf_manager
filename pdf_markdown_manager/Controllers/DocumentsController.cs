using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pdf_markdown_manager.Models;



namespace pdf_markdown_manager.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly DocumentsManagerContext _context;
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;

        private readonly string userId;

        public DocumentsController(DocumentsManagerContext context, UserManager<AuthUser> userMan, SignInManager<AuthUser> signMan)
        {
            _context = context;
            _userManager = userMan;
            _signInManager = signMan;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User)) return View(await _context.Documents.Where(x => false).ToListAsync());

            string userId = _userManager.GetUserId(User);

            return View(await _context.Documents.Where(x => x.users_id == userId).ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documents = await _context.Documents
                .FirstOrDefaultAsync(m => m.id == id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,content,font_size")] Documents documents)
        {

            string userId = _userManager.GetUserId(User);
            if (userId == null) return View();

            documents.created_at = DateTime.Now;
            documents.users_id = userId;

         
            if (ModelState.IsValid)
            {
                _context.Add(documents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(documents);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documents = await _context.Documents.FindAsync(id);
            if (documents == null)
            {
                return NotFound();
            }
            return View(documents);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,content,font_size,users_id,created_at")] Documents documents)
        {
            if (id != documents.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentsExists(documents.id))
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
            return View(documents);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documents = await _context.Documents
                .FirstOrDefaultAsync(m => m.id == id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documents = await _context.Documents.FindAsync(id);
            if (documents != null)
            {
                _context.Documents.Remove(documents);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentsExists(int id)
        {
            return _context.Documents.Any(e => e.id == id);
        }
    }
}
