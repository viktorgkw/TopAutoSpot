namespace TopAutoSpot.Data.Entities
{
    public class Notification
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}