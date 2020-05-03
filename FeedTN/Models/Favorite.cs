using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedTN.Models
{
    public class Favorite
    {
        [Key]
        public int FavoriteId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}
