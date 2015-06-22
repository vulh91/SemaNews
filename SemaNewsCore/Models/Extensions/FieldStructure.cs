using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public class FieldStructure
    {
        public int GroupIdentity { get; set; }
        public Newspaper Newspaper { get; set; }
        public IEnumerable<FieldWebElement> ArticleListElements { get; set; }
        public FieldWebElement PaginationElement { get; set; }

        public FieldStructure()
        {
        }

        public static FieldStructure GetFieldStructure(int newspaperId, int group)
        {
            FieldStructure fieldStructure = new FieldStructure();
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var newspaper = db.Newspapers.Find(newspaperId);
                if (newspaper == null)
                    return fieldStructure;

                var elements = newspaper.FieldWebElements.Where(e => e.Group == group);
                if (elements == null || elements.Count() == 0)
                    return fieldStructure;

                fieldStructure = BuildStructure(elements);
                fieldStructure.Newspaper = newspaper;
                fieldStructure.GroupIdentity = group;

                return fieldStructure;
            }
        }

        public static List<FieldStructure> GetAllFieldStructures(SemaNewsDBContext db, int newspaperId)
        {
            List<FieldStructure> structures = new List<FieldStructure>();
            var newspaper = db.Newspapers.Find(newspaperId);
            if (newspaper == null)
                return structures;

            var groups = newspaper.FieldWebElements.GroupBy(m => m.Group);
            foreach (var group in groups)
            {
                var structure = FieldStructure.BuildStructure(group);
                structure.GroupIdentity = group.Key.HasValue ? group.Key.Value : 0;
                structure.Newspaper = newspaper;
                structures.Add(structure);
            }
            return structures;
        }

        public static FieldStructure BuildStructure(IEnumerable<FieldWebElement> webElements)
        {
            return new FieldStructure()
            {
                ArticleListElements = webElements.Where(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.LIST.ToString().ToUpper()),
                PaginationElement = webElements.FirstOrDefault(m => m.WebElementType.WENotation.ToUpper() == WebElementNotation.PAGINATION.ToString().ToUpper()),
            };
        }
    }
}
