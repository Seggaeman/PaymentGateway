using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models
{
    public enum StatusCode
    {
        NOENTRY,
        OK,
        FAILED
    }

    public interface IPaymentRequest
    {
        long Id { get; set; }
        string CardNumber { get; set; }
        string CardCVV { get; set; }
        DateTime CardExpiryDate { get; set; }
        string Description { get; }
        double Amount { get; }
        string Currency { get; }
    }

    public interface IBankResponse
    {
        long Id { get; set; }
        string CardNumber { get; set; }
        string CardCVV { get; set; }
        DateTime CardExpiryDate { get; set; }
        StatusCode Result { get; set; }
        DateTime PaymentDate { get; }
        Guid TransactionId { get; }
        string MaskedCardNumber { get; }
        string Description { get; }
        double Amount { get; }
        string Currency { get; }
    }

    /*public interface ICreditCardCollection
    {
        CreditCard Get(long id);
        IEnumerable<CreditCard> GetAll();
        CreditCard Create(CreditCard newDataObject);
        CreditCard Update(CreditCard changedDataObject);
        void Delete(long id);
    }

    public interface IMerchantCollection
    {
        Merchant Get(long id);
        IEnumerable<Merchant> GetAll();
        Merchant Create(Merchant newDataObject);
        Merchant Update(Merchant changedDataObject);
        void Delete(long id);
    }*/


    public interface IPaymentCollection
    {
        IBankResponse Get(long id);
        IBankResponse GetByGUID(Guid guid);
        IEnumerable<IBankResponse> GetAll();
        IBankResponse Create(IPaymentRequest paymentRequest);
        void Delete(long id);
    }

    public interface IBank
    {
        IBankResponse ProcessPayment(IPaymentRequest paymentRequest);
    }
}
