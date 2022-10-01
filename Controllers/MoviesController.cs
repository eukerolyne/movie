using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using movie.Models;

namespace movie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Contexto _context;

        public MoviesController(Contexto context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.movies == null)
            {
                return NotFound();
            }

            var movie = await _context.movies
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,Autor,Titulo,LocalGravacao,TipoVideo,Extensao,Duracao,Assunto,Descricao")] Movie movie, IFormFile arquivo)
        {
            var fileName = arquivo.FileName;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/video/", fileName);

            if (movie.Descricao != null)
            {
                movie.Titulo = fileName;
                movie.Extensao = VerificaExtensao(fileName);

                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = arquivo.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public string VerificaExtensao(string nomeArquivo)
        {
            string extensaoArquivo = System.IO.Path.GetExtension(nomeArquivo).ToLower();
            string[] validacaoLista = { ".mp4", ".m4v", ".mov" };

            foreach (string extensao in validacaoLista)
            {
                if (extensao == extensaoArquivo)
                    return extensao;
            }
            return "none";
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.movies == null)
            {
                return NotFound();
            }

            var movie = await _context.movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,Autor,Titulo,LocalGravacao,TipoVideo,Extensao,Duracao,Assunto,Descricao")] Movie movie)
        {
            if (id != movie.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.VideoId))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.movies == null)
            {
                return NotFound();
            }

            var movie = await _context.movies
                .FirstOrDefaultAsync(m => m.VideoId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.movies == null)
            {
                return Problem("Entity set 'Contexto.movies'  is null.");
            }
            var movie = await _context.movies.FindAsync(id);
            if (movie != null)
            {
                _context.movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.movies.Any(e => e.VideoId == id);
        }
    }
}
