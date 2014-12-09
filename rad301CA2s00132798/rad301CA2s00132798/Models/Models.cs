using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace rad301CA2s00132798.Models
{
    class MovieDbInitialiser : DropCreateDatabaseAlways<MoviesDB>
    {
        protected override void Seed(MoviesDB context)
        {
            // seed Director
            var directors = new List<Director>()
            {
                new Director() {Name="Peter Jackson"},
                new Director() {Name="James Cameron"},
                new Director() {Name="Ridley Scot"},
                new Director() {Name="Brett Ratner"},
                new Director() {Name="Jonathan Demme"}
            };
            directors.ForEach(dir => context.Directors.Add(dir));
            context.SaveChanges();

            // seed Camps
            var movies = new List<Movie>()
            {
                new Movie() {Title = "Avatar", ReleaseDate = DateTime.Parse("8/12/2009"), 
                    Director = context.Directors.SingleOrDefault(l=>l.Name=="James Cameron"),
                    MovieActors = new List<Actor>()
                            {
                                new Actor()
                                {   ScreenName = "Sam Worthington",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   ScreenName = "Zoe Saldana",
                                    Sex = Sex.Female
                                },
                                new Actor()
                                {   ScreenName = "Sigourney Weaver",
                                    Sex = Sex.Female
                                }

                    }},
                new Movie() {Title = "Alien", ReleaseDate = DateTime.Parse("22/6/1979"),
                    Director = context.Directors.Where(l=>l.Name=="Ridley Scot").FirstOrDefault(),
                    MovieActors = new List<Actor>()
                            {
                                new Actor()
                                {   ScreenName = "Tom Skerritt",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   ScreenName = "Sigourney Weaver",
                                    Sex = Sex.Female
                                },
                                new Actor()
                                {   ScreenName = "John Hurt",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   ScreenName = "Ian Holm",
                                    Sex = Sex.Female
                                }
                    }},
                    new Movie() {Title = "Red Dragon", ReleaseDate = DateTime.Parse("22/6/1979"),
                    Director = context.Directors.Where(l=>l.Name=="Brett Ratner").FirstOrDefault(),
                    MovieActors = new List<Actor>()
                            {
                                new Actor()
                                {   ScreenName = "Anthony Hopkins",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   ScreenName = "Edward Norton",
                                    Sex = Sex.Female
                                },
                                new Actor()
                                {   ScreenName = "Ralph Fiennes",
                                    Sex = Sex.Male
                                },
                    }},
                    new Movie() {Title = "The Silence of the Lambs", ReleaseDate = DateTime.Parse("14/2/1991"),
                    Director = context.Directors.Where(l=>l.Name=="Jonathan Demme").FirstOrDefault(),
                    MovieActors = new List<Actor>()
                            {
                                new Actor()
                                {   ScreenName = "Anthony Hopkins",
                                    Sex = Sex.Male
                                },
                                new Actor()
                                {   ScreenName = "Jodie Foster",
                                    Sex = Sex.Female
                                },
                    }},
                new Movie() { Title = "The Lord of the Rings: The Fellowship of the Ring", ReleaseDate = DateTime.Parse("19/12/2001"),
                    Director = context.Directors.FirstOrDefault(l=>l.Name=="Peter Jackson")}
            };
            movies.ForEach(mov => context.Movies.Add(mov));
            context.SaveChanges();

            base.Seed(context);
        }   // end seed()

    }
    public class MoviesDB : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public MoviesDB()
            : base("MoviesDB")
        { }
    }
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Display(Name = "Movie Name"), Required]
        [StringLength(30, ErrorMessage = "Must be between " +
                       "{2} and {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }
        [DisplayName("Release Date"), DataType(DataType.Date),
                    DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
        public virtual List<Actor> MovieActors { get; set; }
    }

    public enum Sex
    {
        Male, Female
    }

    public class Actor
    {
        public int ActorId { get; set; }
        public string ScreenName { get; set; }
        public Sex Sex { get; set; }
        public int MovieId { get; set; }
        public virtual List<Movie> Movies { get; set; }
    }

    public class Director
    {
        public int DirectorId { get; set; }
        public string Name { get; set; }
    }
}