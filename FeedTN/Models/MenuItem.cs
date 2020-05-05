using System.ComponentModel.DataAnnotations;

namespace FeedTN.Models
{
    public class MenuItem
    {
        [Key]

        public int MenuItemId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int FavoriteCount { get; set; }

        public bool Active { get; set; }

        public bool GlutenFree { get; set; }

        public bool Vegetarian { get; set; }

        public bool Vegan { get; set; }
    }
}