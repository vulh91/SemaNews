using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class CategoryVM
    {
        public CategoryVM()
        {
            GGRelationsIn = new List<GGRelationInstance>();
            GGRelationsOut = new List<GGRelationInstance>();
            Fields = new List<Field>();
        }
        public CategoryVM(int id)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                GField = db.GFields.Find(id);
                GGRelationsIn = this.GField.GGRelationInstancesIn.ToList();
                GGRelationsOut = this.GField.GGRelationInstancesOut.ToList();
                Fields = this.GField.Fields.ToList();
                Fields.Select(m => m.Newspaper).ToList();
                AllGFields = db.GFields.ToList();
                AllRelations = db.GGRelations.ToList();
            }
        }

        public GField GField { get; set; }
        public List<GGRelationInstance> GGRelationsIn { get; set; }
        public List<GGRelationInstance> GGRelationsOut { get; set; }
        public List<Field> Fields { get; set; }
        public List<GField> AllGFields { get; set; }
        public List<GGRelation> AllRelations { get; set; }
    }
}