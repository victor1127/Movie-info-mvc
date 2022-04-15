using MovieProDemo.Enums;
using MovieProDemo.ViewModels.TmdbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services.Interfaces
{
    public interface IRemoteMovieService
    {
        Task<MovieDetail> GetMovieDetailAsync(int id);
        Task<MovieSearch> GetMoviesSearchAsync(MovieCategory category);
        Task<ActorDetail> GetActorDetailAsync(int id);


    }
}
