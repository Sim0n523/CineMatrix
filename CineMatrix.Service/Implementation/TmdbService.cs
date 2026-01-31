using CineMatrix.Service.DTO;
using CineMatrix.Service.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Service.Implementation
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            _apiKey = configuration["TMDb:ApiKey"] ?? "";
        }

        public List<TmdbMovieDto> GetPopularMovies()
        {
            try
            {
                var response = _httpClient.GetAsync($"movie/popular?api_key={_apiKey}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<TmdbMovieDto>();
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TmdbSearchResponse>(content);


                return result?.Results ?? new List<TmdbMovieDto>();
            }
            catch (Exception ex)
            {
                return new List<TmdbMovieDto>();
            }
        }

        public List<TmdbMovieDto> GetTrendingMovies()
        {
            try
            {
                var response = _httpClient.GetAsync($"trending/movie/week?api_key={_apiKey}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<TmdbMovieDto>();
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TmdbSearchResponse>(content);


                return result?.Results ?? new List<TmdbMovieDto>();
            }
            catch (Exception ex)
            {
                return new List<TmdbMovieDto>();
            }
        }

        public List<TmdbMovieDto> SearchMovies(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return new List<TmdbMovieDto>();

                var encodedQuery = Uri.EscapeDataString(query);
                var response = _httpClient.GetAsync($"search/movie?api_key={_apiKey}&query={encodedQuery}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    return new List<TmdbMovieDto>();
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TmdbSearchResponse>(content);

                return result?.Results ?? new List<TmdbMovieDto>();
            }
            catch (Exception ex)
            {
                return new List<TmdbMovieDto>();
            }
        }

        public TmdbMovieDto? GetMovieDetails(int tmdbId)
        {
            try
            {

                var response = _httpClient.GetAsync($"movie/{tmdbId}?api_key={_apiKey}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var movie = JsonConvert.DeserializeObject<TmdbMovieDto>(content);


                return movie;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
