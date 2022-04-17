using MovieProDemo.Models;
using MovieProDemo.ViewModels.TmdbModel;
using System.Threading.Tasks;

namespace MovieProDemo.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);
    }
}
