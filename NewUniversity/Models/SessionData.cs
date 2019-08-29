using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewUniversity.Models
{
    [Serializable]
    public class SessionData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PersonID { get; set; }
    }
}