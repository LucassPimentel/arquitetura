namespace Social.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
        public int Likes { get; private set; }
        public bool Deleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Post(Guid userId, string message)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Message = message;
            Likes = 0;
            Deleted = false;
            CreatedAt = DateTime.Now;
        }

        public static Post PostMessage(Guid userId, string message)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("É necessário ter um usuário criador.");

            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("É necessário escrever uma mensagem");

            if (message.Count() > 140)
                throw new ArgumentException("A mensagem não pode conter mais de 140 caracteres");

            var newPost = new Post(userId, message);
            return newPost;
        }
    }
}
