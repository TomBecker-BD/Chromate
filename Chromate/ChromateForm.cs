using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace Chromate
{
    public partial class ChromateForm : Form
    {
        ChromiumWebBrowser _browser;

        public IWebAPI WebAPI
        {
            get;
            set;
        }

        protected ChromiumWebBrowser Browser
        {
            get { return _browser; }
        }

        protected IDictionary<string, string> Remap
        {
            get;
            set;
        }

        public ChromateForm()
        {
            InitializeComponent();
        }

        protected virtual void Form1_Load(object sender, EventArgs e)
        {
            Cef.Initialize();
            FormClosed += ChromateForm_FormClosed;
            _browser = new ChromiumWebBrowser("");
            _browser.LifeSpanHandler = new ChromateLifeSpanHandler();
            _browser.MenuHandler = new ChromateContextMenuHandler(_browser);
            string html = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "html");
            _browser.ResourceHandlerFactory = new FileBasedResourceHandlerFactory("http://local", html, _browser.ResourceHandlerFactory, WebAPI, Remap);
            this.Controls.Add(_browser);
            _browser.Dock = DockStyle.Fill;
            _browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
        }

        private void Browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized)
            {
#if DEBUG
                _browser.AddressChanged += Browser_AddressChanged;
                _browser.ShowDevTools();
#endif
                _browser.Load("http://local/index.html");
            }
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Console.WriteLine("Browser address: {0}", e.Address);
        }

        void ChromateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
