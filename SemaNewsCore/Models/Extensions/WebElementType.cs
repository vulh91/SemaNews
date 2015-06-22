using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public partial class WebElementType
    {
        public static WebElementType Find(WebElementNotation elementNotation)
        {
            using(SemaNewsDBContext db =new SemaNewsDBContext())
            {
                return db.WebElementTypes.SingleOrDefault(m=>m.WENotation == elementNotation.ToString());
            }
        }
    }
}
