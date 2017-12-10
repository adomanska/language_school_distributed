using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageSchool.WebApi
{
    public class ClassesSearchingModel
    {
        public string ClassName { get; set; }
        public int LanguageId { get; set; }
        public int LanguageLevelId { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}