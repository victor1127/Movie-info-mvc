using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieProDemo.Enums;

namespace MovieProDemo.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int TmDbMovieId { get; set; }
        public string Title { get; set; }
        public string TagLine { get; set; }
        public string Overview { get; set; }
        public int RunTime { get; set; }
        public float VoteAverage { get; set; }
        public MovieRating Rating { get; set; }
        public string TrailerUrl { get; set; }


        [DataType(DataType.Date)]
        [Display(Name="Release date")]
        public DateTime ReleaseDate { get; set; }


        public byte[] PosterImage { get; set; }
        public byte[] BackDropImage { get; set; }
        public string PostImageType { get; set; }
        public string BackDropImageType { get; set; }


        [NotMapped]
        public IFormFile PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile BackDropImageFile { get; set; }


        public ICollection<MovieCast> MovieCasts { get; set; } = new HashSet<MovieCast>();
        public ICollection<MovieCrew> MovieCrews { get; set; } = new HashSet<MovieCrew>();
        public ICollection<MovieCollection> MovieCollections { get; set; } = new HashSet<MovieCollection>();

    }
}
