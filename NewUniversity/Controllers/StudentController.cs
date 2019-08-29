using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NewUniversity.Models;

namespace NewUniversity.Controllers
{
    public class StudentController : Controller
    {
        private SchoolEntitiesDBContext db = new SchoolEntitiesDBContext();


        [HttpGet]
        public ActionResult RegisterToCourse(string astrConfirmed)
        {
            RegisterToCourse objRegisterToCourse = new RegisterToCourse { istListItems = new List<SelectListItem>() };
            foreach (var item in db.Departments.ToList())
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Value = item.DepartmentID.ToString();
                selectListItem.Text = item.Name;
                objRegisterToCourse.istListItems.Add(selectListItem);
            }

            return View(objRegisterToCourse);
        }
        [HttpPost]
        [Authorize(Roles = "LOCAL")]
        public ActionResult RegisterToCourse(RegisterToCourse ModelRegisterCourse)
        {
            if (ModelState.IsValid)
            {
                SessionData sessionData = Session["UserData"] as SessionData;
                PersonCours personCours = new PersonCours();
                personCours.PersonID = (sessionData.PersonID);
                personCours.CourseID = ModelRegisterCourse.CourseID;
                Course course = db.Courses.Find(ModelRegisterCourse.CourseID);
                personCours.Course_Name = course.Title;
                db.PersonCourses.Add(personCours);
                db.SaveChanges();
            }
            return RedirectToAction("PersonCourses", "Student");
        }
        [Authorize(Roles = "LOCAL")]
        public ActionResult PersonCourses()
        {
            using (SchoolEntitiesDBContext db = new SchoolEntitiesDBContext())
            {

                List<OnlineCourse> onlineCourses = new List<OnlineCourse>();
                List<OnsiteCourse> onsiteCourses = new List<OnsiteCourse>();
                List<PersonCours> allPersonCourses = new List<PersonCours>();

                onsiteCourses = db.OnsiteCourses.ToList();
                onlineCourses = db.OnlineCourses.ToList();
                allPersonCourses = db.PersonCourses.ToList();
                SessionData objSession = Session["UserData"] as SessionData;
                var lstAllCoursesOfPerson = from objOnlineCourse in onlineCourses
                                            join objOnsiteCourse in onsiteCourses
                                            on objOnlineCourse.CourseID equals objOnsiteCourse.CourseID into Table1
                                            from objOnsiteCourse in Table1.ToList()
                                            join objPersonCourses in allPersonCourses
                                            on objOnsiteCourse.CourseID equals objPersonCourses.CourseID into Table2
                                            from objPersonCourses in Table2.ToList()
                                            where objPersonCourses.PersonID == objSession.PersonID
                                            select new PersonCourses
                                            {
                                                busOnlineCourses = objOnlineCourse,
                                                busOnsiteCourses = objOnsiteCourse,
                                                busPersonCourses = objPersonCourses
                                            };
                return View(lstAllCoursesOfPerson);
            }
        }
        public JsonResult GetCascadeData(string astrDepartmentID)
        {

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            int DepartMentID = Convert.ToInt32(astrDepartmentID);
            Department department = db.Departments.Find(DepartMentID);
            List<Course> coursess = db.Courses.Where(c => c.DepartmentID == DepartMentID).ToList();
            foreach (var item in coursess)
            {
                selectListItems.Add(new SelectListItem { Text = item.Title, Value = item.CourseID.ToString() });
            }

            return Json(new SelectList(selectListItems, "Value", "Text"));
        }
        // GET: Student
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewData["UserName"] = User.Identity.Name;
            var people = db.People.Include(p => p.OfficeAssignment);
            return View(people.ToList());
        }
        [Authorize]
        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "PersonID,LastName,FirstName,HireDate,EnrollmentDate,EmailAddress")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                try
                {
                    if (db.People.Any(bus => bus.EmailAddress == person.EmailAddress))
                    {
                        //Show message email address already associate with another user account
                        throw new Exception("This Email is already Registered.");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return View(person);
                }

                db.SaveChanges();
                STUDENT lbusStudent = new STUDENT();
                AUTHENTICATE_USER DbUser = new AUTHENTICATE_USER();
                int personId = person.PersonID;
                int RegId = person.PersonID + 100;
                lbusStudent.PersonID = personId;
                lbusStudent.RegistrationID = RegId;

                db.STUDENTs.Add(lbusStudent);
                db.SaveChanges();
                RegId = person.PersonID + 100;
                DbUser.USER_SERIAL_ID = RegId;
                DbUser.USERNAME = person.FirstName + person.LastName + RegId;
                DbUser.PASSWORD = person.PersonID + person.LastName;
                DbUser.PERSONID = personId;
                db.AUTHENTICATE_USER.Add(DbUser);
                db.SaveChanges();
                sendEmail(person.EmailAddress, DbUser);
            }
            return View(person);
        }

        private void sendEmail(string astrEmail, AUTHENTICATE_USER user)
        {

            //var senderEmail = new MailAddress("myaccoun0493.siddharth1595@gmail.com", "Sid");
            //var receiverEmail = new MailAddress("sidbhamare85@gmail.com", "Receiver");
            var sub = "Registration details are sent!";
            var body = "Please do not share this id . Do not reply to this email, this is auto generated mail. Serial Id = " + user.USER_SERIAL_ID + " " +
                "UserName = " + user.USERNAME + " Password = " + user.PASSWORD;
            MailAddress from = new MailAddress("ben@contoso.com", "Ben Miller");
            MailAddress to = new MailAddress(astrEmail, "Jane Clayton");
            MailMessage mail = new MailMessage(from, to);
            //MailMessage From = MailAddress("myaccoun0493.siddharth1595@gmail.com","Test");
            mail.IsBodyHtml = true;
            mail.Subject = sub;
            mail.Body = body;

            SmtpClient smtp = new SmtpClient();

            smtp.Send(mail);
        }
        // GET: Student/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", person.PersonID);
            return View(person);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "PersonID,LastName,FirstName,HireDate,EnrollmentDate")] Person person)
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", person.PersonID);
            return View(person);
        }

        // GET: Student/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }
        [Authorize]
        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SessionData sessionObj = new SessionData();
            sessionObj = this.Session["UserData"] as SessionData;
            if (sessionObj != null)
                ViewBag.UserName = sessionObj.UserName;
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
