using System;
using System.Net;
using System.Net.Http;
using System.Data.SQLite;
using Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway.Models;

namespace PaymentGateway.Tests
{
    public class PaymentControllerTests : IDisposable
    {
        private SQLiteConnection _connection;

        public PaymentControllerTests()
        {
            _connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\PaymentGateway\\PaymentGateway.sqlite");
            _connection.Open();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        [Fact]
        public void TestPaymentControllerGetAll()
        {
            SQLiteCommand command = new SQLiteCommand("SELECT COUNT(*) FROM PAYMENTS", _connection);
            int numEntries = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();

            // Get JSON content
            String jsonString = new WebClient().DownloadString("http://localhost:5000/api/payment");
            JArray objectArray = (JArray)JsonConvert.DeserializeObject(jsonString);
            Assert.Equal(objectArray.Count, numEntries);
        }

        [Fact]
        public void TestPaymentControllerGetById()
        {
            String jsonString = new WebClient().DownloadString("http://localhost:5000/api/payment/1");
            JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonString);
            int value = jObject.Value<int>("id");
            Assert.Equal(1, value);
        }

        [Fact]
        public void TestPaymentControllerGetByTransactionId()
        {
            String jsonString = new WebClient().DownloadString("http://localhost:5000/api/payment/guid/f3b8450d-4008-4127-82fd-07e60fc89f75");
            JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonString);
            string value = jObject.Value<string>("transactionId");
            Assert.Equal("f3b8450d-4008-4127-82fd-07e60fc89f75", value);
        }

        [Fact]
        public async void TestPaymentControllerPostPaymentRequest()
        {
            // Payment object not created explicitly because the CardNumber field is ignored by the json serializer.
            var payload = new
            {
                CardNumber = "7777777777777777",
                CardCVV = "555",
                CardExpiryDate = new DateTime(2021, 5, 1),
                Amount = 10000,
                Currency = "MUR",
                Description = "Furniture"
            };

            var stringPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(stringPayload, System.Text.Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync("http://localhost:5000/api/payment", httpContent);

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Payment bankResponse = JsonConvert.DeserializeObject<Payment>(responseContent);
                    Assert.Equal("############7777",bankResponse.MaskedCardNumber);
                }
            }
        }

        [Fact]
        public async void TestPaymentControllerDelete()
        {
            String uri = "http://localhost:5000/api/payment/15";
            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.DeleteAsync(uri);

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Payment bankResponse = JsonConvert.DeserializeObject<Payment>(responseContent);
                    Assert.Equal(15, bankResponse.Id);
                }
            }
        }
    }
}
