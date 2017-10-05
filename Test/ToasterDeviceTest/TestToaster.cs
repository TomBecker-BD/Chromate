using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Toaster.Device.Test
{
    [TestFixture]
    public class TestToaster
    {
        [Test]
        public void TestSetting()
        {
            List<string> names = new List<string>();
            Toaster t = new Toaster();
            t.PropertyChanged += delegate (object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                names.Add(e.PropertyName);
            };
            t.Setting = 7;
            Assert.That(t.Setting, Is.EqualTo(7));
            Assert.That(names.ToArray(), Is.EqualTo(new string[] { "Setting" }));
        }

        [Test]
        public void TestContent()
        {
            List<string> names = new List<string>();
            Toaster t = new Toaster();
            t.PropertyChanged += delegate (object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                names.Add(e.PropertyName);
            };
            t.Content = "Sourdough";
            Assert.That(t.Content, Is.EqualTo("Sourdough"));
            Assert.That(names.ToArray(), Is.EqualTo(new string[] { "Content" }));
        }

        [Test]
        public void TestToasting()
        {
            using (AutoResetEvent done = new AutoResetEvent(false))
            {
                List<string> names = new List<string>();
                Toaster t = new Toaster();
                t.PropertyChanged += delegate (object sender, System.ComponentModel.PropertyChangedEventArgs e)
                {
                    names.Add(e.PropertyName);
                    if (e.PropertyName == "Toasting" && !t.Toasting)
                    {
                        done.Set();
                    }
                };
                t.Setting = 1;
                t.Content = "Sourdough";
                t.Toasting = true;
                Assert.That(done.WaitOne(5000), Is.True, "Timed out");
                Assert.That(t.Color, Is.Not.Null);
                Assert.That(names, Does.Contain("Color"));
            }
        }
    }
}
