using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ninject.Extensions.Logging;
using NUnit.Framework;
using Rhino.Mocks;
using TestingUtilities;
using Toaster.Interfaces;

namespace Toaster.WebAPI.Test
{
    [TestFixture]
    public class WebAPITest
    {
        [Test]
        public void TestGetStatus()
        {
            IToaster toaster = MockRepository.GenerateStub<IToaster>();
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            statusMonitor.Stub(x => x.GetToasterStatusAsync(Arg<int>.Is.Anything)).Return(Task.FromResult(new ToasterStatus()
            {
                setting = 3,
                content = "Wonder",
                toasting = false,
                color = "White"
            }));
            var logger = new ConsoleLogger();
            using (WebAPI webAPI = new WebAPI(new MockDependencyResolver(toaster, statusMonitor, logger), logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:80/api/toaster/status");
                HttpResponseMessage response = webAPI.HandleRequestAsync(request).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                Assert.That(content, Is.EqualTo("{\"setting\":3.0,\"content\":\"Wonder\",\"toasting\":false,\"color\":\"White\"}"));
            }
        }

        [Test]
        public void TestGetStatusWithTimeout()
        {
            IToaster toaster = MockRepository.GenerateStub<IToaster>();
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            statusMonitor.Stub(x => x.GetToasterStatusAsync(Arg<int>.Is.Anything)).Return(Task.FromResult(new ToasterStatus()
            {
                setting = 3,
                content = "Wonder",
                toasting = false,
                color = "White"
            }));
            var logger = new ConsoleLogger();
            using (WebAPI webAPI = new WebAPI(new MockDependencyResolver(toaster, statusMonitor, logger), logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:80/api/toaster/status");
                request.Headers.Add("timeout", "22");
                HttpResponseMessage response = webAPI.HandleRequestAsync(request).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                Assert.That(content, Is.EqualTo("{\"setting\":3.0,\"content\":\"Wonder\",\"toasting\":false,\"color\":\"White\"}"));
            }
            var args = statusMonitor.GetArgumentsForCallsMadeOn(x => x.GetToasterStatusAsync(Arg<int>.Is.Anything));
            Assert.That(args[0][0], Is.EqualTo(22 * 1000));
        }

        [Test]
        public void TestGetStatusException()
        {
            IToaster toaster = MockRepository.GenerateStub<IToaster>();
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            statusMonitor.Stub(x => x.GetToasterStatusAsync(Arg<int>.Is.Anything)).Return(Task.FromResult(new ToasterStatus()
            {
                setting = 3,
                content = "Wonder",
                toasting = false,
                color = "White"
            }))
            .WhenCalled(call =>
            {
                call.ReturnValue = Task.Run(() => Error());
            });
            var logger = MockRepository.GenerateStub<ILogger>();
            using (WebAPI webAPI = new WebAPI(new MockDependencyResolver(toaster, statusMonitor, logger), logger))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:80/api/toaster/status");
                HttpResponseMessage response = webAPI.HandleRequestAsync(request).Result;
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                Assert.That(content, Is.EqualTo("{\"Message\":\"An error has occurred.\"}"));
            }
            logger.AssertWasCalled(x => x.WarnException(Arg<string>.Is.Anything, Arg<Exception>.Is.Anything));
            var args = logger.GetArgumentsForCallsMadeOn(x => x.WarnException(Arg<string>.Is.Anything, Arg<Exception>.Is.Anything));
            Assert.That(args[0][1].ToString(), Does.Contain("InvalidOperationException: Testing"));
        }

        static ToasterStatus Error()
        {
            throw new InvalidOperationException("Testing");
        }
    }
}
