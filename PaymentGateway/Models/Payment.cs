using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace PaymentGateway.Models
{
    public class Payment : IPaymentRequest, IBankResponse
    {
        public long Id { get; set; }
        [JsonIgnore]
        public string CardNumber { get; set; }
        public string CardCVV { get; set; }
        public DateTime CardExpiryDate { get; set; }
        public StatusCode Result { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid TransactionId { get; set; }
        public string MaskedCardNumber { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
