using System;

namespace Auth.Domain.Entities
{
    public class UserToken
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool IsValid { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Email { get; set; }
    }
}
