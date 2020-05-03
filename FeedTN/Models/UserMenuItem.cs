using System.ComponentModel.DataAnnotations;

namespace FeedTN.Models
{
    public class UserMenuItem
    {
        [Key]
        public int UserMenuItemId { get; set; }

        public string MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}