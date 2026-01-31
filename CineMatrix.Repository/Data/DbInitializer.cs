using CineMatrix.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMatrix.Repository.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.Genres.Any())
            {
                return; 
            }

            var genres = new List<Genre>
            {
                new Genre { Name = "Action", TmdbId = 28 },
                new Genre { Name = "Adventure", TmdbId = 12 },
                new Genre { Name = "Animation", TmdbId = 16 },
                new Genre { Name = "Comedy", TmdbId = 35 },
                new Genre { Name = "Crime", TmdbId = 80 },
                new Genre { Name = "Documentary", TmdbId = 99 },
                new Genre { Name = "Drama", TmdbId = 18 },
                new Genre { Name = "Family", TmdbId = 10751 },
                new Genre { Name = "Fantasy", TmdbId = 14 },
                new Genre { Name = "History", TmdbId = 36 },
                new Genre { Name = "Horror", TmdbId = 27 },
                new Genre { Name = "Music", TmdbId = 10402 },
                new Genre { Name = "Mystery", TmdbId = 9648 },
                new Genre { Name = "Romance", TmdbId = 10749 },
                new Genre { Name = "Science Fiction", TmdbId = 878 },
                new Genre { Name = "TV Movie", TmdbId = 10770 },
                new Genre { Name = "Thriller", TmdbId = 53 },
                new Genre { Name = "War", TmdbId = 10752 },
                new Genre { Name = "Western", TmdbId = 37 }
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();

            var actionGenre = genres.First(g => g.Name == "Action");
            var dramaGenre = genres.First(g => g.Name == "Drama");
            var sciFiGenre = genres.First(g => g.Name == "Science Fiction");
            var crimeGenre = genres.First(g => g.Name == "Crime");
            var thrillerGenre = genres.First(g => g.Name == "Thriller");
            var adventureGenre = genres.First(g => g.Name == "Adventure");

            var user = new User
            {
                Username = "MyMovieTracker",
                Email = "user@movietracker.com",
                Bio = "Personal movie collection and watchlist tracker",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            context.SaveChanges();

            var movies = new List<Movie>
            {
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    Overview = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ReleaseDate = new DateTime(1994, 9, 23),
                    RuntimeMinutes = 142,
                    Director = "Frank Darabont",
                    PosterPath = "/9cqNxx0GxF0bflZmeSMuL5tnGzr.jpg",
                    IsWatched = true,
                    WatchedDate = DateTime.UtcNow.AddDays(-30),
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Godfather",
                    Overview = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    ReleaseDate = new DateTime(1972, 3, 24),
                    RuntimeMinutes = 175,
                    Director = "Francis Ford Coppola",
                    PosterPath = "/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
                    IsWatched = true,
                    WatchedDate = DateTime.UtcNow.AddDays(-25),
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    Overview = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests.",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    RuntimeMinutes = 152,
                    Director = "Christopher Nolan",
                    PosterPath = "/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                    IsWatched = true,
                    WatchedDate = DateTime.UtcNow.AddDays(-20),
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    Overview = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    RuntimeMinutes = 154,
                    Director = "Quentin Tarantino",
                    PosterPath = "/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
                    IsInWatchlist = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Inception",
                    Overview = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea.",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    RuntimeMinutes = 148,
                    Director = "Christopher Nolan",
                    PosterPath = "/ljsZTbVsrQSqZgWeep2B1QiDKuh.jpg",
                    IsWatched = true,
                    WatchedDate = DateTime.UtcNow.AddDays(-15),
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Fight Club",
                    Overview = "An insomniac office worker and a devil-may-care soap maker form an underground fight club that evolves into much more.",
                    ReleaseDate = new DateTime(1999, 10, 15),
                    RuntimeMinutes = 139,
                    Director = "David Fincher",
                    PosterPath = "/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg",
                    IsInWatchlist = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Matrix",
                    Overview = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    ReleaseDate = new DateTime(1999, 3, 31),
                    RuntimeMinutes = 136,
                    Director = "Lana Wachowski",
                    PosterPath = "/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                    IsWatched = true,
                    WatchedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Interstellar",
                    Overview = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                    ReleaseDate = new DateTime(2014, 11, 7),
                    RuntimeMinutes = 169,
                    Director = "Christopher Nolan",
                    PosterPath = "/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                    IsInWatchlist = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            movies[0].Genres.Add(dramaGenre);
            movies[0].Genres.Add(crimeGenre);

            movies[1].Genres.Add(dramaGenre);
            movies[1].Genres.Add(crimeGenre);

            movies[2].Genres.Add(actionGenre);
            movies[2].Genres.Add(crimeGenre);
            movies[2].Genres.Add(dramaGenre);

            movies[3].Genres.Add(crimeGenre);
            movies[3].Genres.Add(thrillerGenre);

            movies[4].Genres.Add(actionGenre);
            movies[4].Genres.Add(sciFiGenre);
            movies[4].Genres.Add(thrillerGenre);

            movies[5].Genres.Add(dramaGenre);

            movies[6].Genres.Add(actionGenre);
            movies[6].Genres.Add(sciFiGenre);

            movies[7].Genres.Add(adventureGenre);
            movies[7].Genres.Add(sciFiGenre);
            movies[7].Genres.Add(dramaGenre);

            context.Movies.AddRange(movies);
            context.SaveChanges();


            var ratings = new List<Rating>
            {
                new Rating
                {
                    MovieId = movies[0].Id, // Shawshank
                    Score = 10,
                    Review = "An absolute masterpiece! One of the greatest films of all time.",
                    WatchedDate = DateTime.UtcNow.AddDays(-30),
                    CreatedAt = DateTime.UtcNow
                },
                new Rating
                {
                    MovieId = movies[1].Id, // Godfather
                    Score = 8,
                    Review = "The pinnacle of crime cinema. Perfection.",
                    WatchedDate = DateTime.UtcNow.AddDays(-25),
                    CreatedAt = DateTime.UtcNow
                },
                new Rating
                {
                    MovieId = movies[2].Id, // Dark Knight
                    Score = 10,
                    Review = "Heath Ledger's Joker is unforgettable. Best superhero movie ever.",
                    WatchedDate = DateTime.UtcNow.AddDays(-20),
                    CreatedAt = DateTime.UtcNow
                },
                new Rating
                {
                    MovieId = movies[4].Id, // Inception
                    Score = 9,
                    Review = "Mind-bending and brilliant. Nolan at his best!",
                    WatchedDate = DateTime.UtcNow.AddDays(-15),
                    CreatedAt = DateTime.UtcNow
                },
                new Rating
                {
                    MovieId = movies[6].Id, // The Matrix
                    Score = 5,
                    Review = "Revolutionary film that changed cinema forever.",
                    WatchedDate = DateTime.UtcNow.AddDays(-10),
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Ratings.AddRange(ratings);
            context.SaveChanges();
        }
    }
}
