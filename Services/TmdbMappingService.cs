using Microsoft.Extensions.Options;
using MovieProDemo.Enums;
using MovieProDemo.Models;
using MovieProDemo.Services.Interfaces;
using MovieProDemo.ViewModels;
using MovieProDemo.ViewModels.TmdbModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class TmdbMappingService : IDataMappingService
    {
        private readonly AppSettings _appSetting;
        private readonly IImageService _imageService;
        
        public TmdbMappingService(IOptions<AppSettings> appSetting, IImageService imageService)
        {
            _appSetting = appSetting.Value;
            _imageService = imageService;
        }


        public ActorDetail MapActorDetail(ActorDetail actor)
        {
            actor.profile_path = BuildCastImage(actor.profile_path);
            
            if (string.IsNullOrEmpty(actor.biography)) actor.biography = "Not available";
            if (string.IsNullOrEmpty(actor.place_of_birth)) actor.place_of_birth = "Not available";
            if (string.IsNullOrEmpty(actor.birthday)) actor.birthday = "Not available";
            else actor.birthday = DateTime.Parse(actor.birthday).ToString("MMM dd, yyyy");

            return actor;
        }
        public async Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            Movie movieDb = null;

            try
            {
                movieDb = new Movie()
                {
                    TmDbMovieId = movie.id,
                    Title = movie.title,
                    TagLine = movie.tagline,
                    Overview = movie.overview,
                    RunTime = movie.runtime,
                    VoteAverage = movie.vote_average,
                    ReleaseDate = DateTime.Parse(movie.release_date),
                    TrailerUrl = BuildTrailerPath(movie.videos),
                    BackDropImage = await EncodeImagePathAsync(movie.backdrop_path, _appSetting.MovieProDemo.DefaultBackDropSize),
                    BackDropImageType = BuildImageType(movie.backdrop_path),
                    PosterImage = await EncodeImagePathAsync(movie.poster_path, _appSetting.MovieProDemo.DefaultPosterSize),
                    PostImageType = BuildImageType(movie.poster_path),
                    Rating = GetRating(movie.release_dates)

                };

                var movieCasts = movie.credits.cast.OrderByDescending(c => c.popularity)
                                       .GroupBy(c => c.cast_id).Select(c => c.FirstOrDefault())
                                       .Take(20).ToList();

                foreach(var cast in movieCasts)
                {
                    movieDb.MovieCasts.Add(
                        new MovieCast
                        {
                            //MovieId = movieDb.Id,
                            CastId = cast.cast_id,
                            Department = cast.known_for_department,
                            Name = cast.name,
                            Character = cast.character,
                            ImageUrl = BuildCastImage(cast.profile_path)
                        });
                }

                var movieCrew = movie.credits.crew.OrderByDescending(c => c.popularity)
                                      .GroupBy(c => c.id).Select(c => c.First())
                                      .Take(20).ToList();

                foreach (var crewMember in movieCrew)
                {
                    movieDb.MovieCrews.Add(
                        new MovieCrew
                        {
                            //MovieId = movieDb.Id,
                            CrewId = crewMember.id,
                            Department = crewMember.department,
                            Name = crewMember.name,
                            Job = crewMember.job,
                            ImageUrl = BuildCastImage(crewMember.profile_path)
                        });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
            }

            return movieDb;
        }
        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(v => v.type.ToLower().Trim() == "trailer" && v.key != "")?.key;
            return string.IsNullOrEmpty(videoKey) ? videoKey : $"{_appSetting.TmDbSettings.BaseYoutubePath}{videoKey}";

        }
        private async Task<byte[]> EncodeImagePathAsync(string path, string size)
        {
            var backDropPath = $"{_appSetting.TmDbSettings.BaseImagePath}/{size}/{path}";
            return await _imageService.ConvertImagePathToByteArray(backDropPath);
        }
        private string BuildImageType(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            return $"image/{Path.GetExtension(path).TrimStart('.')}";
        }
        private MovieRating GetRating(Release_Dates dates)
        {
            var movieRating = MovieRating.NR;
            var certification = dates.results.FirstOrDefault(r => r.iso_3166_1 == "US");
            if (certification is not null)
            {
                var apiRating = certification.release_dates.FirstOrDefault(c => c.certification != "")?.certification.Replace("-", "");
                if (!string.IsNullOrEmpty(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }
            return movieRating;
        }
        private string BuildCastImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return _appSetting.MovieProDemo.DefaultCastImage;

            return $"{_appSetting.TmDbSettings.BaseImagePath}/{_appSetting.MovieProDemo.DefaultPosterSize}/{path}";
        }

    }
}
