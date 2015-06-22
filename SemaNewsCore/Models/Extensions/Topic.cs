using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class Topic
    {
        public static Topic Search(SemaNewsDBContext context, string name)
        {
            return context.Topics.SingleOrDefault(m => m.Name.ToLower() == name.ToLower().Trim());
        }
    }
}
