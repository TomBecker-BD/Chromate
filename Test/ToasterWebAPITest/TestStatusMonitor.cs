using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Toaster.Interfaces;

namespace Toaster.WebAPI.Test
{
    [TestFixture]
    public class TestStatusMonitor
    {
        [Test]
        public void TestChangedBefore()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = new ToasterStatusMonitor(toaster);
            toaster.Setting = 4;
            toaster.Raise(x => x.PropertyChanged += null, toaster, new PropertyChangedEventArgs("Setting"));
            var getStatus = statusMonitor.GetToasterStatusAsync(5000);
            //Assert.That(getStatus.IsCompleted, Is.True);
            var status = getStatus.Result;
            Assert.That(status.setting, Is.EqualTo(4));
            Assert.That(status.content, Is.EqualTo("Wonder"));
            Assert.That(status.toasting, Is.False);
            Assert.That(status.color, Is.EqualTo("White"));
        }

        [Test]
        public void TestWaitForChange()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = new ToasterStatusMonitor(toaster);
            var getStatus = statusMonitor.GetToasterStatusAsync(5000);
            Assert.That(getStatus.IsCompleted, Is.False);
            toaster.Setting = 4;
            toaster.Raise(x => x.PropertyChanged += null, toaster, new PropertyChangedEventArgs("Setting"));
            var status = getStatus.Result;
            Assert.That(status.setting, Is.EqualTo(4));
            Assert.That(status.content, Is.EqualTo("Wonder"));
            Assert.That(status.toasting, Is.False);
            Assert.That(status.color, Is.EqualTo("White"));
        }

        [Test]
        public void TestMultipleChanges()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = new ToasterStatusMonitor(toaster);
            var getStatus = statusMonitor.GetToasterStatusAsync(5000);
            Assert.That(getStatus.IsCompleted, Is.False);
            toaster.Content = "Whole Wheat";
            toaster.Raise(x => x.PropertyChanged += null, toaster, new PropertyChangedEventArgs("Content"));
            toaster.Color = "Brown";
            toaster.Raise(x => x.PropertyChanged += null, toaster, new PropertyChangedEventArgs("Color"));
            var status = getStatus.Result;
            Assert.That(status.setting, Is.EqualTo(3));
            Assert.That(status.content, Is.EqualTo("Whole Wheat"));
            Assert.That(status.toasting, Is.False);
            Assert.That(status.color, Is.EqualTo("Brown"));
        }

        [Test]
        public void TestTimeout()
        {
            var toaster = MockRepository.GenerateStub<IToaster>();
            toaster.Setting = 3;
            toaster.Content = "Wonder";
            toaster.Color = "White";
            var statusMonitor = new ToasterStatusMonitor(toaster);
            var getStatus = statusMonitor.GetToasterStatusAsync(100);
            Assert.That(getStatus.IsCompleted, Is.False);
            var status = getStatus.Result;
            Assert.That(status.setting, Is.EqualTo(3));
            Assert.That(status.content, Is.EqualTo("Wonder"));
            Assert.That(status.toasting, Is.False);
            Assert.That(status.color, Is.EqualTo("White"));
        }
    }
}
