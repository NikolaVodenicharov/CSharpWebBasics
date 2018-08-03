namespace WebServer.GameStoreApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using WebServer.GameStoreApplication.Common;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.VideoIdLength)]
        [MaxLength(ValidationConstants.Game.VideoIdLength)]
        public string TrailerId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public double SizeGigabytes { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<UserGame> UsersGames { get; set; } = new List<UserGame>();
    }
}