using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Toaster.Interfaces;
using Toaster.WebAPI.Controllers;

namespace Toaster.WebAPI.Test
{
    [TestFixture]
    public class TestToasterController
    {
        [Test]
        public void TestGetStatus()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            statusMonitor.Stub(x => x.GetToasterStatusAsync(Arg<int>.Is.Anything)).Return(Task.FromResult(new ToasterStatus()
            {
                setting = toaster.Setting,
                content = toaster.Content,
                toasting = toaster.Toasting,
                color = toaster.Color
            }));
            var controller = new ToasterController(toaster, statusMonitor)
            {
                Request = new HttpRequestMessage()
            };
            var result = controller.GetStatus();
            var response = result.ExecuteAsync(new CancellationToken()).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.EqualTo("{\"setting\":3.0,\"content\":\"Wonder\",\"toasting\":false,\"color\":\"White\"}"));
        }

        [Test]
        public void TestPutSetting()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            var controller = new ToasterController(toaster, statusMonitor)
            {
                Request = new HttpRequestMessage()
            };
            var result = controller.PutSetting(new ToasterStatus() { setting = 6 });
            var response = result.ExecuteAsync(new CancellationToken()).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.EqualTo("{\"setting\":6.0,\"content\":\"Wonder\",\"toasting\":false,\"color\":\"White\"}"));
        }

        [Test]
        public void TestPutContent()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            var controller = new ToasterController(toaster, statusMonitor)
            {
                Request = new HttpRequestMessage()
            };
            var result = controller.PutContent(new ToasterStatus() { content = "Wheat", color = "Brown" });
            var response = result.ExecuteAsync(new CancellationToken()).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.EqualTo("{\"setting\":3.0,\"content\":\"Wheat\",\"toasting\":false,\"color\":\"Brown\"}"));
        }

        [Test]
        public void TestPutToasting()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = MockRepository.GenerateStub<IToasterStatusMonitor>();
            var controller = new ToasterController(toaster, statusMonitor)
            {
                Request = new HttpRequestMessage()
            };
            var result = controller.PutToasting(new ToasterStatus() { toasting = true });
            var response = result.ExecuteAsync(new CancellationToken()).Result;
            string content = response.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.EqualTo("{\"setting\":3.0,\"content\":\"Wonder\",\"toasting\":true,\"color\":\"White\"}"));
        }
    }
}
