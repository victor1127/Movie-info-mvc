using MovieProDemo.Models;
using MovieProDemo.ViewModels.TmdbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.ViewModels
{
    public class LandingPageVM
    {
        public List<Collection> CustomCollectionss { get; set; }
        public MovieSearch NowPlaying { get; set; }
        public MovieSearch Popular { get; set; }
        public MovieSearch TopRated { get; set; }
        public MovieSearch Upcoming { get; set; }

    }
}
