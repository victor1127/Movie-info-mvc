using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.ViewModels
{
    public class AppSettings
    {
        public MovieProDemo MovieProDemo { get; set; }
        public TmDbSettings TmDbSettings { get; set; }
    }
    public class MovieProDemo
    {
        public string TmDbApiKey { get; set; }
        public string DefaultBackDropSize { get; set; }
        public string DefaultPosterSize { get; set; }
        public string DefaultYoutubeKey { get; set; }
        public string DefaultCastImage { get; set; }
        public DefaultCollection DefaultCollection { get; set; }
        public DefaultCredentials DefaultCredentials { get; set; }


    }
    public class TmDbSettings
    {
        public string BaseUrl { get; set; }
        public string BaseImagePath { get; set; }
        public string BaseYoutubePath { get; set; }
        public QueryOptions QueryOptions { get; set; }

    }
    public class DefaultCollection
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class DefaultCredentials
    {
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


    }

    public class QueryOptions
    {
        public string Language { get; set; }
        public string AppendToResponse { get; set; }
        public string Page { get; set; }
    }
}
