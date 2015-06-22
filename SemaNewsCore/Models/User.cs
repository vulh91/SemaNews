using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class User
    {
        public User()
        {
            this.UserQueries = new List<UserQuery>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Nullable<int> RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<UserQuery> UserQueries { get; set; }
    }
}
