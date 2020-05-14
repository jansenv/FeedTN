using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;

namespace FeedTN.Controllers
{
    public class SmsController : TwilioController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration Configuration;

        public SmsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            Configuration = configuration;
        }

        public async Task<IActionResult> SendSms()
        {
            var twilioSecrets = new TwilioSecrets();

            Configuration.GetSection("Twilio").Bind(twilioSecrets);

            var loggedInUser = await GetCurrentUserAsync();

            if (loggedInUser.PhoneNumber != null)
            {
            var phone = loggedInUser.PhoneNumber.ToString();

            TwilioClient.Init(twilioSecrets.accountSid, twilioSecrets.authToken);

                var message = MessageResource.Create(
                   body: "Your order with Feed TN is confirmed! Be on the lookout for your driver.",
                   from: new PhoneNumber("+12055396681"),
                   to: new PhoneNumber($"+1{phone}"));

            return RedirectToAction("Index", "Orders");
            } 
            else
            {
                return RedirectToAction("Index", "Orders");
            };

        }

        [HttpPost]
        public TwiMLResult Index()
        {
            var requestBody = Request.Form["Body"];
            var response = new MessagingResponse();
            if (requestBody == "Order" || requestBody == "order")
            {
                response.Message("Welcome to Feed TN! Please log in with your user credentials to order:");
            }


            return TwiML(response);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}