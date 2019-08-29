using NewUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewUniversity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if(sessionObj !=null)
                ViewBag.UserName = sessionObj.UserName;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            return View();
        }

        [Authorize(Roles ="LOCAL")]
        public ActionResult DepartmentCourses()
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            using (SchoolEntitiesDBContext db = new SchoolEntitiesDBContext())
            {
                List<Course> lstCourses = db.Courses.ToList();
                List<Department> lstDepartments = db.Departments.ToList();
                var  lstDepartmentCources = from c in lstCourses
                                     join d in lstDepartments on c.DepartmentID equals d.DepartmentID into table1
                                     from d in table1.ToList()
                                     select new DepartmentCourses
                                     {
                                         ObjCourse =c,
                                         ObjDepartment=d
                                     };
                return View(lstDepartmentCources);
            }
        }
    }
}