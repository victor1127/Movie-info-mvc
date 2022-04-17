using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MovieProDemo.Enums;
using MovieProDemo.Services.Interfaces;
using MovieProDemo.ViewModels;
using MovieProDemo.ViewModels.TmdbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class TmdbMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        public TmdbMovieService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            _appSettings = appSettings.Value;
            _httpClientFactory = httpClientFactory;
        }
        
        
        public async Task<MovieSearch> GetMoviesSearchAsync(MovieCategory category)
        {
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{category}";
            
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key",_appSettings.MovieProDemo.TmDbApiKey },
                {"language",_appSettings.TmDbSettings.QueryOptions.Language },
                {"page",_appSettings.TmDbSettings.QueryOptions.Page }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);

            MovieSearch movieSearch = new();
            if (response.IsSuccessStatusCode)
            {
                var jsonDeserializer = new DataContractJsonSerializer(typeof(MovieSearch));
                using var contentFromResponse = await response.Content.ReadAsStreamAsync();
                movieSearch = (MovieSearch)jsonDeserializer.ReadObject(contentFromResponse);
                movieSearch.results.ToList().ForEach(r => r.poster_path = $"{_appSettings.TmDbSettings.BaseImagePath}/{_appSettings.MovieProDemo.DefaultPosterSize}/{r.poster_path}");
            }

            return movieSearch;
        }
        public async Task<MovieDetail> GetMovieDetailAsync(int id)
        {
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key",_appSettings.MovieProDemo.TmDbApiKey},
                {"language",_appSettings.TmDbSettings.QueryOptions.Language},
                {"append_to_response",_appSettings.TmDbSettings.QueryOptions.AppendToResponse},
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);

            MovieDetail movieDetail = new();
            if (response.IsSuccessStatusCode)
            {
                var jsonDeserializer = new DataContractJsonSerializer(typeof(MovieDetail));
                using var contentResponse = await response.Content.ReadAsStreamAsync();
                movieDetail = (MovieDetail)jsonDeserializer.ReadObject(contentResponse);
            }

            return movieDetail;
        }
        public async Task<ActorDetail> GetActorDetailAsync(int id)
        {
            var query = $"{_appSettings.TmDbSettings.BaseUrl}/person/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                {"api_key",_appSettings.MovieProDemo.TmDbApiKey},
                {"language",_appSettings.TmDbSettings.QueryOptions.Language}
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage);

            ActorDetail actorDetail = new();
            if (response.IsSuccessStatusCode)
            {
                var jsonDeserializer = new DataContractJsonSerializer(typeof(ActorDetail));
                using var contentResponse = await response.Content.ReadAsStreamAsync();
                actorDetail = (ActorDetail)jsonDeserializer.ReadObject(contentResponse);
            }

            return actorDetail;
        }

    

    }
}
