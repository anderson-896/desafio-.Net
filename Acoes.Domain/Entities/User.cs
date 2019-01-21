using System;
using System.Collections.Generic;

namespace Actions.Domain.Entities
{
    public class User: Model
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Phone> Phones { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Last_login { get; set; }
    }

}
