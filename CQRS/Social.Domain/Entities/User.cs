namespace Social.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
