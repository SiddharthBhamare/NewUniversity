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
    
    public partial class STUDENT
    {
        public int StudentID { get; set; }
        public Nullable<int> RegistrationID { get; set; }
        public Nullable<int> PersonID { get; set; }
    
        public virtual AUTHENTICATE_USER AUTHENTICATE_USER { get; set; }
        public virtual Person Person { get; set; }
    }
}