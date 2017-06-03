using AutoMapper;
using BotDetect.Web.Mvc;
using Common;
using Model.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Infrastructure.Extensions;
using Web.Models;

namespace Web.Controllers
{
    public class ContactDetailController : Controller
    {
        IContactDetailService _contactDetailService;
        IFeedbackService _feedbackService;

        public ContactDetailController(IContactDetailService contactDetailService, IFeedbackService feedbackService)
        {
            _contactDetailService = contactDetailService;
            _feedbackService = feedbackService;
        }

        public ActionResult Index()
        {
            var feedbackViewModel = new FeedbackViewModel();

            feedbackViewModel.ContactDetail = GetDetail();

            return View(feedbackViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Mã xác nhận không đúng")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                var newFeedback = new Feedback();

                newFeedback.UpdateFeedback(feedbackViewModel);

                _feedbackService.Create(newFeedback);
                _feedbackService.Save();

                TempData["MessageSuccess"] = "Gửi phản hổi thành công";

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/contact_template.html"));
                content = content.Replace("{{Name}}", feedbackViewModel.Name);
                content = content.Replace("{{Email}}", feedbackViewModel.Email);
                content = content.Replace("{{Message}}", feedbackViewModel.Message);
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

                feedbackViewModel.Name = string.Empty;
                feedbackViewModel.Email = string.Empty;
                feedbackViewModel.Message = string.Empty;
            }

            feedbackViewModel.ContactDetail = GetDetail();

            return View("Index", feedbackViewModel);
        }

        private ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();

            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);

            return contactViewModel;
        }
    }
}