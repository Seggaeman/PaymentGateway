using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    #region PaymentCollection
    public class PaymentCollection : IPaymentCollection
    {
        private EFDatabaseContext _context;
        private IBank _bank = new FakeBank();
        public PaymentCollection(EFDatabaseContext ctx) => _context = ctx;


        public IBankResponse Create(IPaymentRequest paymentRequest)
        {
            _bank.ProcessPayment(paymentRequest);
            var obj = _context.Add((Payment)paymentRequest);
            _context.SaveChanges();

            return obj.Entity;
        }

        public IBankResponse Delete(long id)
        {
            IBankResponse bankResponse = Get(id);
            if (bankResponse != null)
            {
                _context.Remove(bankResponse);
                _context.SaveChanges();
            }
            return bankResponse;
        }

        public IBankResponse Get(long id)
        {
            return _context.Payments.Find(id);
        }

        public IEnumerable<IBankResponse> GetAll()
        {
            return _context.Payments;
        }

        public IBankResponse GetByGUID(Guid guid)
        {
            IEnumerable<Payment> payments = _context.Payments.Where(payment => payment.TransactionId == guid);
            if (payments.Count() > 0)
                return payments.First();
            else
                return null;
        }
    }
    #endregion

    #region FakeBank
    class FakeBank : IBank
    {
        public IBankResponse ProcessPayment(IPaymentRequest paymentRequest)
        {
            // Generate a random number.
            Random random = new Random();
            int randomNumber = random.Next(1, 1000);
            Payment payment = (Payment)paymentRequest;
            payment.Result = randomNumber % 2 == 0 ? StatusCode.OK : StatusCode.FAILED;
            payment.PaymentDate = DateTime.Now;
            payment.TransactionId = Guid.NewGuid();
            payment.MaskedCardNumber = "############" + payment.CardNumber.Substring(12, 4);
            return payment;
        }
    }
    #endregion
}
