namespace FeedTN.Models.OrderViewModels
{
    public class OrderLineItem
    {
        public MenuItem MenuItem { get; set; }
        public int Units { get; set; }
        public double Cost { get; set; }
    }
}