using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MNPOSTWEBSITE.Models;
using MNPOSTWEBSITE.Controllers;
using ASPSnippets.FaceBookAPI;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;

namespace MNPOSTWEBSITE.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            HomeController home = new HomeController();
            Session["token"] = home.getToken().Result;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                string username = model.UserName;
                string pass = model.Password;
                if (user != null)
                {
                        if(db.AspNetUsers.Where(s=>s.UserName == model.UserName).FirstOrDefault().IsActive == true)
                        {
                            Session["ID"] = db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().Id;
                            Session["Username"] = user.FullName;
                            Session["Email"] = username;
                            string idrole = db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().IDRole;
                            Session["RoleID"] = db.AspNetRoles.Where(s => s.Id == idrole).FirstOrDefault().Name;
                            Session["Authentication"] = "True";
                            await SignInAsync(user, model.RememberMe);
                            return RedirectToAction("Index","Manage");
                        }else
                        {
                            ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                        }
                  
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại hoặc nhập sai mật khẩu");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public EmptyResult LoginFacebook()
        {
            FaceBookConnect.Authorize("user_photos,email", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, "Home/Index/"));
            return new EmptyResult();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            HomeController home = new HomeController();
            Session["token"] = home.getToken().Result;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string Fullname, string Phone)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);                
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("mnpostvn@gmail.com", "Đăng ký tài khoản"),
                    new System.Net.Mail.MailAddress(user.UserName));
                    m.Subject = "Đăng ký tài khoản";
                    m.Body = string.Format("Kính gửi {0} <br/> Cảm ơn bạn đã đăng ký dịch vụ MNPOST, xin nhấp vào đường dẫn dưới đây để kích hoạt tài khoản: <a href =\"{1}\"title =\"User Email Confirm\">{1}</a>",
                    user.UserName, Url.Action("ConfirmEmail", "Account",
                    new { Token = user.Id, Email = user.UserName }, Request.Url.Scheme)) ;
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    smtp.Credentials = new System.Net.NetworkCredential("mnpostvn@gmail.com", "Mnpost_2018");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    string cusid = db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().Id;
                    db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().FullName = Fullname;
                    db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().Phone = Phone;
                    db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().IsActive = false;
                    db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().IDRole = "2";
                    db.AspNetUsers.Where(s => s.UserName == model.UserName).FirstOrDefault().CreatedDate =DateTime.Now;
                    db.SaveChanges();
                    await AddCustomer(cusid,Fullname, Phone, false, model.UserName);
                    return RedirectToAction("SuccessfulRegister", "Account");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //confirm email
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string UserName)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.UserName == UserName)
                {
                    await UserManager.UpdateAsync(user);
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    db.AspNetUsers.Where(s => s.UserName == user.UserName).FirstOrDefault().IsActive = true;
                    db.SaveChanges();
                    var cusid = db.AspNetUsers.Where(s => s.UserName == user.UserName).FirstOrDefault().IDClient;
                    await UpdateCustomer(cusid, true);
                    return RedirectToAction("Confirm", "Account", new { Email = user.UserName });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }
        }

        //confirm email view
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }

        public ActionResult SuccessfulRegister()
        {
            return View();
        }

        public async Task<ActionResult> AddCustomer(string custid, string Fullname ="", string Phone = "", bool IsActive = false, string Email = "")
        {
            CustomerInfo cus = new CustomerInfo
            {
                CustomerName = Fullname,
                Phone = Phone,
                Email = Email,
                IsActive = IsActive,
                CreateDate = DateTime.Now
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            string api = "http://noiboapi.miennampost.vn/api/customer/addcustomer";
            var response = await client.PostAsJsonAsync(api, new { customer = cus }).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(content);
                string msg = (string)obj["msg"];
                var rs = db.AspNetUsers.Find(custid);
                rs.IDClient = msg;
                db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new ResultInfo() { error = 0, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultInfo() { error = 1, msg = "Lỗi data" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateCustomer(string custid, bool IsActive = true)
        {
            CustomerInfo cus = new CustomerInfo
            {
                CustomerID = custid,
                IsActive = IsActive
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            string api = "http://noiboapi.miennampost.vn/api/customer/updatecustomer";
            var response = await client.PostAsJsonAsync(api, new { customer = cus }).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                return Json(new ResultInfo() { error = 0, msg = custid, data = cus }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultInfo() { error = 1, msg = "Lỗi data" }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["Username"] = null;
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public ActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(string email)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            using (MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities dc = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities())
            {
                var account = dc.AspNetUsers.Where(a => a.UserName == email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.UserName, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return RedirectToAction("Index","Home");
        }

        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("hoanganh27897@gmail.com", "MNPOST");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "pokemonblackwhite2";

            string subject = "";
            string body = "";
            if (emailFor == "ResetPassword")
            {
                subject = "Reset Mật khẩu";
                body = "Xin chào,<br/><br/>Chúng tôi nhận được yêu cầu reset lại mật khẩu của bạn. Xin hãy click vào đường link dưới đây để thay đổi mật khẩu mới" +
                    "<br/><br/><a href=" + link + ">Link Reset Mật khẩu</a>";
            }
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities dc = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities())
            {
                var user = dc.AspNetUsers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities dc = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities())
                {
                    var user = dc.AspNetUsers.Where(s => s.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.PasswordHash = null;
                        //user.PasswordHash = Crypto.Hash(model.NewPassword);  
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        var rs = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return RedirectToAction("Login","Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}