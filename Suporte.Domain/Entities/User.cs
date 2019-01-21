using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suporte.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserForgottenToken> ForgottenTokens { get; set; }
    }

    //public class UserForgottenToken
    //{
    //    public int ForgottenTokenId { get; set; }
    //    public string Token { get; set; }
    //    public DateTime CreateDate { get; set; }
    //    public DateTime? ModifyDate { get; set; }
    //}
}
