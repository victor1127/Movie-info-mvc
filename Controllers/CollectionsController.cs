using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieProDemo.Data;
using MovieProDemo.Models;
using MovieProDemo.Services.Interfaces;
using MovieProDemo.ViewModels;

namespace MovieProDemo.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IRemoteMovieService _remoteMovieService;
        private readonly IDataMappingService _dataMappingService;

        public CollectionsController(ApplicationDbContext context, IOptions<AppSettings> appSettings,
                                     IRemoteMovieService remoteMovieService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _remoteMovieService = remoteMovieService;
        }

        // GET: Collections
        public async Task<IActionResult> Index()
        {
            var defaultMovieCollection = _appSettings.MovieProDemo.DefaultCollection.Name;
            return View(await _context.Collections.Where(c=>c.Name!= defaultMovieCollection).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Collection collection)
        {
            _context.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "MovieCollections", new { id = collection.Id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Collection collection)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //
                    _context.Update(collection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
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
            return View(collection);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }
    }
}
