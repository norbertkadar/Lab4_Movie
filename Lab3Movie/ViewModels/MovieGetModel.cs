using Lab3Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.ViewModels
{
    public class MovieGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Genre Genre { get; set; }
        public DateTime DateAdded { get; set; }
        public int DurationInMinutes { get; set; }
        
        public int YearOfRelease { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }
        public Watched Watched { get; set; }
        public int NumberOfComments { get; set; }

        public static MovieGetModel FromMovie(Movie movie)

        {

            return new MovieGetModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                DateAdded = movie.DateAdded,
                Director = movie.Director,
                DurationInMinutes = movie.DurationInMinutes,
                Rating = movie.Rating,
                YearOfRelease = movie.YearOfRelease,
                Watched = movie.Watched,
                NumberOfComments = movie.Comments.Count
            };
        }
    }
}