using System;
using System.Net;
using System.Data.SQLite;
using Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}
