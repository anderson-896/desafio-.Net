namespace Auth.Domain.Entities
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
    }
}
