using SemaNewsCore;
using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SemaNewsWeb.ViewModels
{
    public class NewspaperVM
    {
        public Newspaper Newspaper { get; set; }

        [Display(Name="Trang báo điện tử cha")]
        public int? ParentNewspaperId { get; set; }

        public IEnumerable<Field> Fields { get; set; }

        public IEnumerable<FieldStructure> FieldStructures { get; set; }

        public IEnumerable<ArticleStructure> ArticleStructures { get; set; }

    }
}