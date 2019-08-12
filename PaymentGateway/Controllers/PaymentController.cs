using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private IPaymentCollection _paymentCollection;

        public PaymentController(IPaymentCollection collection) => _paymentCollection = collection;

        [HttpGet]
        public IEnumerable<IBankResponse> Get() => _paymentCollection.GetAll();

        [HttpGet("{id}")]
        public IBankResponse Get(int id) => _paymentCollection.Get(id);

        [HttpGet("guid/{guid}")]
        public IBankResponse Get(Guid guid) => _paymentCollection.GetByGUID(guid);

        [HttpPost]
        public IBankResponse Post([FromBody] IPaymentRequest paymentRequest)
        {
            IBankResponse response = null;
            if (Regex.IsMatch(paymentRequest.CardNumber, "^[0-9]{16,16}$") && Regex.IsMatch(paymentRequest.CardCVV, "^[0-9]{3,3}$"))
            {
                response = _paymentCollection.Create(new Payment
                {
                    CardNumber = paymentRequest.CardNumber,
                    CardCVV = paymentRequest.CardCVV,
                    CardExpiryDate = paymentRequest.CardExpiryDate,
                    Amount = paymentRequest.Amount,
                    Currency = paymentRequest.Currency,
                    Description = paymentRequest.Description
                });
            }
            else
                response = new Payment();
            return response;
        }

        [HttpDelete("{id}")]
        public void Delete(int id) => _paymentCollection.Delete(id);
    }
}
