namespace Rocky_Models.Models
{
    public class Like
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
