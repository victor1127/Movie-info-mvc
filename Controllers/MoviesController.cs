using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieProDemo.Data;
using MovieProDemo.Models;
using MovieProDemo.Services.Interfaces;
using MovieProDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IRemoteMovieService _remoteMovieService;
        private readonly IImageService _imageService;
        private readonly IDataMappingService _dataMappingService;


        public MoviesController(ApplicationDbContext context, IOptions<AppSettings> appSettings,
                                IRemoteMovieService remoteMovieService, IImageService imageService, IDataMappingService dataMappingService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _remoteMovieService = remoteMovieService;
            _imageService = imageService;
            _dataMappingService = dataMappingService;
        }


        public async Task<IActionResult> Import()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int? id)
        {
            if (id == null) return NotFound();

            if (_context.Movies.Any(m => m.Id == id))
            {
                return RedirectToAction("Details", new { id });
            }

            var movieDetails = await _remoteMovieService.GetMovieDetailAsync((int)id);
            var movie = await _dataMappingService.MapMovieDetailAsync(movieDetails);
            
            _context.Add(movie);
            await _context.SaveChangesAsync();

            //Add movie to default collection (All)
            await AddToDefaultMovieCollection(movie.Id, _appSettings.MovieProDemo.DefaultCollection.Name);

            return RedirectToAction("Import");

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.Include(m=>m.MovieCasts)
                                             .Include(m=>m.MovieCrews)
                                             .FirstOrDefaultAsync(m => m.Id == id);          
            if(movie is null)
            {
                var movieDetail = await _remoteMovieService.GetMovieDetailAsync((int)id);
                movie = await _dataMappingService.MapMovieDetailAsync(movieDetail);
            }

            if(movie is null)
            {
                return NotFound();
            }

            return View(movie);
        }
        public async Task<IActionResult> Library()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
            
        }



        //Methods
        private async Task AddToDefaultMovieCollection(int movieId, string collectionName)
        {
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Name == collectionName);
            _context.Add(new MovieCollection
            {
                MovieId = movieId,
                CollectionId = collection.Id
            });

            await _context.SaveChangesAsync();
        }
        private async Task AddToMovieCollection(int movieId, int collectionId)
        {
            _context.Add(new MovieCollection()
            {
                MovieId = movieId,
                CollectionId = collectionId
            });

            await _context.SaveChangesAsync();
        }
    }
}
