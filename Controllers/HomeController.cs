using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieProDemo.Data;
using MovieProDemo.Models;
using MovieProDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MovieProDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using MovieProDemo.Enums;

namespace MovieProDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRemoteMovieService _remoteMovieService;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,
                              IRemoteMovieService remoteMovieService)
        {
            _logger = logger;
            _context = context;
            _remoteMovieService = remoteMovieService;
        }

        public async Task<IActionResult> Index()
        {
            var data = new LandingPageVM()
            {
                CustomCollections = await _context.Collections.Include(c => c.MovieCollections)
                                                              .ThenInclude(mc => mc.Movie)
                                                              .ToListAsync(),

                NowPlaying = await _remoteMovieService.GetMoviesSearchAsync(MovieCategory.now_playing),
                Popular = await _remoteMovieService.GetMoviesSearchAsync(MovieCategory.popular),
                TopRated = await _remoteMovieService.GetMoviesSearchAsync(MovieCategory.top_reted),
                Upcoming = await _remoteMovieService.GetMoviesSearchAsync(MovieCategory.upcoming),

            };
            return View(data);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
