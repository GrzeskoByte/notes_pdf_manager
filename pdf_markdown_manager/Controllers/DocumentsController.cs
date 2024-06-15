using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pdf_markdown_manager.Models;
using pdf_markdown_manager.PDFGeneration;
using pdf_markdown_manager.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Microsoft.IdentityModel.Tokens;





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

        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            if (searchTerm.IsNullOrEmpty())
            {
                return RedirectToAction(nameof(Index));
            }
            var notes = _context.Documents.Where(x => x.title.ToLower().Contains(searchTerm.ToLower()));
            return View("Index", notes);
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

            if (!float.TryParse(documents.font_size, out float parsedFontSize))
            {
                ViewData["ErrorMessage"] = "Czcionka musi być wartością liczbową";
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(documents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(documents);
        }

        public async Task<IActionResult> PrintDocument(int id)
        {
            Documents doc = await _context.Documents.FindAsync(id);

            if (doc == null) return NotFound();

            PdfDocument pdf = new PdfDocument();

            pdf.Info.Title = doc.title;

            PdfPage page = pdf.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", double.Parse(doc.font_size), XFontStyleEx.Bold);

            double x = 50;
            double y = 50;

            string[] words = doc.content.Split(' ');

            foreach (string word in words)
            {
                XSize wordSize = gfx.MeasureString(word, font);

                if (x + wordSize.Width > page.Width)
                {
                    x = 50;
                    y += font.Height;
                }

                gfx.DrawString(word, font, XBrushes.Black, x, y);

                x += wordSize.Width + 1;
            }

            MemoryStream stream = new MemoryStream();

            pdf.Save(stream, false);

            byte[] fileContent = stream.ToArray();

            Files file = new Files();

            file.title = doc.title;
            file.content = fileContent;
            file.users_id = doc.users_id;

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Files");
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
        public async Task<IActionResult> Edit(int id, [Bind("title,content,font_size")] Documents documents)
        {

            Documents existingDoc = await _context.Documents.FindAsync(id);

            if (existingDoc == null) return NotFound();

             if (!float.TryParse(documents.font_size, out float parsedFontSize))
            {
                ViewData["ErrorMessage"] = "Czcionka musi być wartością liczbową";
                return View();
            }


            documents.id = existingDoc.id;
            documents.created_at = existingDoc.created_at;
            documents.users_id = existingDoc.users_id;

            _context.Entry(existingDoc).State = EntityState.Detached;

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
