//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewUniversity.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PersonCours
    {
        public int PersonCoursesID { get; set; }
        public int PersonID { get; set; }
        public int CourseID { get; set; }
        public string Course_Name { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Person Person { get; set; }
    }
}