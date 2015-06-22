using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class UserProfile
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Signature { get; set; }
        public virtual User User { get; set; }
    }
}
