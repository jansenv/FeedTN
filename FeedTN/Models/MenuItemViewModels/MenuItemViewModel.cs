using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedTN.Models.MenuItemViewModels
{
    public class MenuItemViewModel
    {
        public MenuItem MenuItem { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool UserHasFavorited { get; set; }
    }
}
