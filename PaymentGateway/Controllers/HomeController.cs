using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult MakePayment(Payment payment)
        {
            _paymentCollection.Create(payment);
            return RedirectToAction(nameof(Index));
        }
    }
}
