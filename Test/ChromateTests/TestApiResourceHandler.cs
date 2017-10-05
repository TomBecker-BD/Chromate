using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CefSharp;
using Chromate;
using NUnit.Framework;
using Rhino.Mocks;

namespace BD.Resolve.Chromate.Tests
{
    [TestFixture]
    public class TestApiResourceHandler
    {
        [Test]
        public void TestGetOK()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var webAPI = MockRepository.GenerateStub<IWebAPI>();
            webAPI.Stub(x => x.HandleRequestAsync(Arg<HttpRequestMessage>.Is.Anything)).Return(Task.FromResult(responseMessage));
            var request = MockRepository.GenerateStub<IRequest>();
            request.Method = "GET";
            request.Url = "http://local/api/test";
            request.Stub(x => x.ReferrerUrl).Return("http://local/index.html");
            var callback = MockRepository.GenerateStub<ICallback>();
            var handler = new ApiResourceHandler(webAPI);
            Assert.That(handler.ProcessRequest(request, callback));
            callback.AssertWasCalled(x => x.Continue());
            var response = MockRepository.GenerateStub<IResponse>();
            long responseLength;
            string redirectUrl;
            handler.GetResponseHeaders(response, out responseLength, out redirectUrl);
            Assert.That(responseLength, Is.EqualTo(0));
            Assert.That(redirectUrl, Is.Null);
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.StatusText, Is.EqualTo("OK"));
            using (MemoryStream dataOut = new MemoryStream())
            {
                int bytesRead;
                Assert.That(handler.ReadResponse(dataOut, out bytesRead, callback), Is.False);
                Assert.That(bytesRead, Is.EqualTo(0));
                Assert.That(dataOut.Length, Is.EqualTo(0));
                callback.AssertWasCalled(x => x.Dispose());
            }
        }
    }
}
