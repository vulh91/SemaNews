using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class FieldVM
    {
        public Newspaper Newspaper { get; set; }
        public Field Field { get; set; }

        [Display(Name="Trang lĩnh vực cha")]
        public int? ParentFieldId { get; set; }

        [Display(Name="Lĩnh vực phân loại")]
        public int? GFieldId { get; set; }

        public virtual IEnumerable<Field> SiblingFields { get; set; }
        public virtual IEnumerable<GField> GFields { get; set; }
        public virtual IEnumerable<FieldStructure> FieldStructures { get; set; }
    }
}