using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FeedTN.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
