using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    public class HomeController : Controller
    {
        private IPaymentCollection _paymentCollection;
        public HomeController(IPaymentCollection collection) => _paymentCollection = collection;

        public IActionResult Index()
        {
            return View(_paymentCollection.GetAll());
        }

        public IActionResult CreatePayment(Payment payment)
        {
            return View();
        }
 
        [HttpPost]
        public IActionResult ConfirmPayment(Payment payment)
        {
            if (payment.CardNumber == null || !Regex.IsMatch(payment.CardNumber,"^[0-9]{16,16}$"))
            {
                ModelState.AddModelError(nameof(payment.CardNumber), "Please enter a 16 digit card number");
            }
            if (payment.CardCVV == null || !Regex.IsMatch(payment.CardCVV, "^[0-9]{3,3}$"))
            {
                ModelState.AddModelError(nameof(payment.CardCVV), "Please enter a 3 digit card CVV");
            }
            /*if (ModelState.GetValidationState("CardExpiryDate") != ModelValidationState.Valid)
                ModelState.AddModelError(nameof(payment.CardExpiryDate), "Please enter a valid card expiry date");

            if (ModelState.GetValidationState("Amount") != ModelValidationState.Valid)
                ModelState.AddModelError(nameof(payment.Amount), "Please enter a valid amount");*/

            if (ModelState.IsValid)
            {
                _paymentCollection.Create(payment);
                return View("PaymentResult", payment);
            }
            else
                return View("CreatePayment");
            //return RedirectToAction(nameof(Index));
        }
    }
}
