using Lab3Movie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.ViewModels
{
    public class MoviePostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public int YearOfRelease { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        [Range(1, 10)]
        public int Rating { get; set; }
        public Watched Watched { get; set; }
        
        public List<Comment> Comments { get; set; }

        public static Movie ToMovie(MoviePostModel movie)
        {
            Genre genre = Models.Genre.action;

            if (movie.Genre == "comedy")
            {
                genre = Models.Genre.comedy;
            }
            else if (movie.Genre == "horror")
            {
                genre = Models.Genre.horror;
            }
            else
            {
                genre = Models.Genre.thriller;
            }

            return new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Genre = genre,
                DurationInMinutes = movie.DurationInMinutes,
                YearOfRelease = movie.YearOfRelease,
                DateAdded = movie.DateAdded,
                Director = movie.Director,
                Rating = movie.Rating,
                Watched = movie.Watched,
                Comments = movie.Comments
            };

        }
    }
}