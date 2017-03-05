using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoEntityFrameworkandAJAX.Models;

namespace DemoEntityFrameworkandAJAX.Controllers
{
    public class AccountController : Controller
    {
        // access to account table
        private AccountModel _accountModel = new AccountModel();

        
        public ActionResult Index()
        {
            return View();
        }
 
        public ActionResult Demo1()
        {
            return View("CheckUsername");
        }
        public ActionResult Demo2()
        {
            return View("AutoComplete");
        }
        public ActionResult Demo3()
        {
            return View("AddUser");
        }
        public ActionResult Demo4()
        {
            return View("DeleteUser");
        }


        public ActionResult DeleteUserConfirmed(string username)
        {
            var user = _accountModel.Accounts
                                    .Where(acc => acc.username == username)
                                    .FirstOrDefault();
            _accountModel.Accounts.Remove(user);
            _accountModel.SaveChanges();
            return Content("Success");
        }

        public ActionResult DeleteUser(string username)
        {
            var result = _accountModel.Accounts
                                .Where(acc => acc.username.StartsWith(username))
                                .ToList();
            // check if no such user
            if (result.Count == 0)
            {
                return PartialView("_UserRecordForDeletionEmpty");
            }
            var model = new DemoEntityFrameworkandAJAX.Models.AddUserModel()
            {
                UserName = result[0].username,
                FullName = result[0].fullname,
                Password = null,
                ConfirmPassword = null
            };
            // everything is ok
            return PartialView("_UserRecordForDeletion", model);
        }

        public ActionResult CheckUsername(string username)
        {
            var result = _accountModel.Accounts
                              .Where(acc => acc.username == username)
                              .ToList()
                              .Count==0 ? "Username is available.": "Username is not available.";
            return Content(result, "text/plain");
        }

        public ActionResult Dump()
        {
            var result = string.Join(", ", _accountModel.Accounts.Select(s=>s.username + "/"+ s.fullname ));
            return Content(result, "text/plan");
        }

        public ActionResult Search(string term)
        {
            var result = _accountModel.Accounts
                .Where(acc=>acc.fullname.StartsWith(term))
                .Select(acc=>acc.fullname)
                .ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult AddUser(AddUserModel addUserModel)
        {
            // check password confirmation
            if (addUserModel.ConfirmPassword != addUserModel.Password)
            {
                return PartialView("_AddedAccountPasswordMismatch");
            }

            // check for account
            var checkUsers = _accountModel.Accounts
                                          .Where(u => u.username == addUserModel.UserName)
                                          .ToList();
            if (checkUsers.Count > 0)
            {
                // user exists
                return PartialView("_AddedAccountUserExists");
            }

            // try to create account
            Account user = new Account()
            {
                username = addUserModel.UserName,
                fullname = addUserModel.FullName,
                password = addUserModel.Password
            };

            _accountModel.Accounts.Add(user);
            _accountModel.SaveChanges();

            return PartialView("_AddedAccountSuccess");
        }
    }
}