using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Model;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.ExternalLoginModel;

namespace SparkAuto.Areas.Identity.Pages.Account
{
    public class VerifyEmailModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public VerifyEmailModel(UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _db = db;
        }

        [BindProperty]
        public string Email { get; set; }

        public IActionResult OnGet(string id)
        {
            Email = id;
            return Page(); 
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = new ApplicationUser
            {                
                UserName = id,
                Email = id,
            };

            //var results = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Email == id);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToPage();
        }
    }
}