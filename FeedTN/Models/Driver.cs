using System.ComponentModel.DataAnnotations;

namespace FeedTN.Models
{
    public class Driver : ApplicationUser
    {
        [Key]
        public int DriverId { get; set; }
    }
}