using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedTN.Data;
using FeedTN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public SmsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> SendSms()
        {
            const string accountSid = "AC1c26a9a684895b4cf0a1104847f0b837";
            const string authToken = "b8733362f602a40fc5ba9c8a959fe184";

            var loggedInUser = await GetCurrentUserAsync();

            if (loggedInUser.PhoneNumber != null)
            {
            var phone = loggedInUser.PhoneNumber.ToString();

            TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                   body: "Your order is confirmed! Be on the lookout for your driver.",
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