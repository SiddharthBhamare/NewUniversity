using NewUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace NewUniversity.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private SchoolEntitiesDBContext db = new SchoolEntitiesDBContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserModel userModel, string astrReturnUrl)
         {
            if(IsValidUser(userModel))
            {
                
                if (userModel.UserName == "Administrator" )
                {
                    if(!Roles.RoleExists("Administrator"))
                    {
                        Roles.CreateRole("Administrator");
                    }
                    if (!Roles.IsUserInRole(userModel.UserName, "Administrator"))
                    {
                        Roles.AddUserToRole("Administrator", "Administrator");
                    }
                    return RedirectToAction("Admin", "Login");
                } 
                else 
                {
                    if(!Roles.RoleExists("LOCAL"))
                    {
                        Roles.CreateRole("LOCAL");
                    }
                    if(!Roles.IsUserInRole(userModel.UserName,"LOCAL"))
                    {
                        Roles.AddUserToRole(userModel.UserName, "LOCAL");
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                userModel.iblnErrorMessage = true;
                userModel.UserName = string.Empty;
                userModel.Password = string.Empty;
            }
            return View(userModel);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Admin()
        {
            Person lbusPerson = new Person();
            SessionData session = Session["UserData"] as SessionData;
            lbusPerson = db.People.Find(session.PersonID);
            return View(lbusPerson);
        }
        private bool IsValidUser(UserModel user)
        {
            AUTHENTICATE_USER DBuser = db.AUTHENTICATE_USER.FirstOrDefault(lbus => lbus.USERNAME == user.UserName);
            if(DBuser != null &&
                DBuser.USERNAME.Equals(user.UserName,StringComparison.CurrentCultureIgnoreCase) && user.Password==DBuser.PASSWORD)
            {
                //To authenticate in [Authorized] pages need to set cokkies and session for these we used below code
                FormsAuthentication.SetAuthCookie(DBuser.USERNAME, false);

                var profileData = new SessionData
                {
                    UserId = DBuser.USER_SERIAL_ID,
                    UserName = DBuser.USERNAME,
                    PersonID = DBuser.PERSONID
                };
                this.Session["UserData"] = profileData;
                ViewBag.UserName = DBuser.USERNAME;
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SignUp(int aIntRegistration,int aIntPersonID)
        {
            AUTHENTICATE_USER UserModel = new AUTHENTICATE_USER();
            UserModel.PERSONID = aIntPersonID;
            return View();
        }
        public ActionResult SignUp(AUTHENTICATE_USER userModel)
        {
            Person person = db.People.Find(userModel.PERSONID);
            if(person != null)
            {
                
            }
            return View();
        }
    }
}