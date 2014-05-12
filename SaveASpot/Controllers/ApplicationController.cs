using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SaveASpot.Models;
using SaveASpot.Data.Models;
using System.Net.Mail;

namespace SaveASpot.Controllers
{
    public class ApplicationController : Controller
    {
        public IRoleService RoleService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (RoleService == null) { RoleService = new AccountRoleService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.IsCreator = IsCreator;
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Constructor for the application wide controller that configures default values for the entire site.
        /// </summary>
        public ApplicationController()
        {
            Configuration = new ConfigurationManagerWrapper();

            // retrieve the values
            Build = Configuration.AppSettings["Build"];

            // attempt to retrieve the build date, if fail, just use the current date
            string date = Configuration.AppSettings["BuildDate"];
            try { BuildDate = Convert.ToDateTime(date); }
            catch { BuildDate = DateTime.MinValue; }
        }

        /// <summary>
        /// Build version for the current release of the application.  Used to tell us which version we are looking at.
        /// </summary>
        public string Build { get; set; }

        /// <summary>
        /// Date of the current build
        /// </summary>
        public DateTime BuildDate { get; set; }


        /// <summary>
        /// String that summarizes the build and build date for the current application.
        /// </summary>
        public string CurrentBuild
        {
            get
            {
                return ViewData["CurrentBuild"].ToString() ?? "";
            }
        }

        private SaveASpot.Data.DataContext _context;

        /// <summary>
        /// Data context that is used to communicate with and retrieve the data from the database.
        /// </summary>
        public SaveASpot.Data.DataContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SaveASpot.Data.DataContext(Configuration.ConnectionStrings("MongoConnection"));
                }
                return _context;
            }
        }

        #region permissions names

        public const string kAdministratorRoles = "administrator, developer";
        
        #endregion

        #region application settings

        /// <summary>
        /// Config field that states if the application is currently in production or not.
        /// </summary>
        public bool IsProduction
        {
            get
            {
                return Convert.ToBoolean(Configuration.AppSettings["Production"]);
            }
        }

        public string StripeSecretKey
        {
            get
            {
                return Configuration.AppSettings["StripeSecretKey"];
            }
        }

        public string StripePublicKey
        {
            get
            {
                return Configuration.AppSettings["StripePublicKey"];
            }
        }

        public IConfigurationManager Configuration { get; set; }

        #endregion

        #region permissions

        /// <summary>
        /// returns whether the current user is an administrator
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                return IsInRole("administrator");
            }
        }

        public bool IsCreator
        {
            get
            {
                return IsInRole("creator");
            }
        }

        /// <summary>
        /// checks to see if the current logged in user is in the specified role.
        /// </summary>
        /// <param name="role">role to check with</param>
        /// <returns>returns true if logged in user is in the given role.</returns>
        private bool IsInRole(string role)
        {
            return RoleService.IsUserInRole(User.Identity.Name, role);
        }

        #endregion

        #region site management

        public MembershipUser SiteUser
        {
            get
            {
                return MembershipService.GetUser();
            }
        }

        public ObjectId SiteUserId
        {
            get
            {
                if (Context.Author == null || Context.Author.Name != SiteUser.UserName)
                {
                    Context.Author.AuthorID = new ObjectId(SiteUser.ProviderUserKey.ToString());
                    Context.Author.Name = SiteUser.UserName;
                }
                return Context.Author.AuthorID;
            }
        }

        private User _currentUser = null;
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null) { _currentUser = Context.Users.GetUser(this.SiteUserId); }
                return _currentUser;
            }
        }

        public Author CurrentAuthor
        {
            get
            {
                return new Author { AuthorID = SiteUserId, Name = SiteUser.UserName };
            }
        }
        #endregion

        #region email

        /// <summary>
        /// Used to send email messages out
        /// </summary>
        /// <param name="tos">The list of users to send the message to.</param>
        /// <param name="ccs">List of users to carbon copy</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="body">The body of the message</param>
        public void SendMail(List<string> tos, List<string> ccs, string subject, string body)
        {
            SendMail(tos, ccs, subject, body, "support@squarehook.com");
        }

        /// <summary>
        /// Used to send email messages out
        /// </summary>
        /// <param name="tos">The list of users to send the message to.</param>
        /// <param name="ccs">List of users to carbon copy</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="body">The body of the message</param>
        /// <param name="from">The sender of the message</param>
        public void SendMail(List<string> tos, List<string> ccs, string subject, string body, string from)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                MailMessage message = new MailMessage();

                if (tos != null)
                {
                    foreach (string email in tos)
                    {
                        message.To.Add(email);
                    }
                }

                if (ccs != null)
                {
                    foreach (string email in ccs)
                    {
                        message.Bcc.Add(email);
                    }
                }

                message.From = new MailAddress(from);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                client.Send(message);
            }
        }

        public string ToRelativeDate(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} seconds ago", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format("{0} minutes ago", timeSpan.Minutes) : "a minute ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format("{0} hours ago", timeSpan.Hours) : "an hour ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format("{0} days ago", timeSpan.Days) : "yesterday";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format("{0} months ago", timeSpan.Days / 30) : "a month ago";

            return timeSpan.Days > 365 ? String.Format("{0} years ago", timeSpan.Days / 365) : "a year ago";
        }

        #endregion

        public Cart CurrentCart
        {
            get
            {
                var cart = Context.Carts.GetCart(ObjectId.Empty, ClientIP) ??
                new Cart { CartID = ObjectId.GenerateNewId(), ClientIP = ClientIP, Items = new List<CartItem>() };
                return cart;
            }
        }

        public void ClearCart()
        {
            Context.Carts.DeleteCart(ObjectId.Empty, ClientIP);
        }

        public string ClientIP
        {
            get
            {
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? Request.UserHostAddress;
            }
        }

    }
}
