using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewUniversity.Models
{
    public class RegisterToCourse : PersonCours
    {
        public List<SelectListItem> istListItems { get; set; }
    }
}