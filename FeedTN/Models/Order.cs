﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedTN.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateCompleted { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public ApplicationUser User { get; set; }

        public string DriverId { get; set; }

        public Driver Driver { get; set; }

        public virtual ICollection<UserMenuItem> UserMenuItems { get; set; }
    }
}