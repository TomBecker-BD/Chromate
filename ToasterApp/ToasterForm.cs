using Chromate;
using Ninject;
using Ninject.Extensions.Logging;

namespace Toaster.App
{
    public class ToasterForm : ChromateForm
    {
        [Inject]
        public new IWebAPI WebAPI
        {
            get { return base.WebAPI; }
            set { base.WebAPI = value; }
        }

        [Inject]
        public ILogger Logger
        {
            get;
            set;
        }

        public ToasterForm()
            : base()
        {
            Text = "Toaster";
        }
    }
}
