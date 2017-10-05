using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using Ninject;
using Toaster.Device;
using Toaster.WebAPI;

namespace Toaster.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += UnhandledExceptionHandler;
            try
            {
                Logging.Configure(NLog.LogLevel.Info, "toaster.log");
                GetLogger().Info(System.Reflection.Assembly.GetEntryAssembly().FullName);

                //For Windows 7 and above, best to include relevant app.manifest entries as well
                Cef.EnableHighDPISupport();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var kernel = new StandardKernel(new ToasterDeviceModule(), new ToasterWebAPIModule());
                var form = kernel.Get<ToasterForm>();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                GetLogger().Fatal(ex, "Error starting the application");
                MessageBox.Show(string.Format("Error starting the application: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Logger for Unhandled Exceptions
        /// </summary>
        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            if (ex != null)
            {
                GetLogger().Log(NLog.LogLevel.Error, ex, "Unhandled Exception", args);
            }
        }

        /// <summary>
        /// Get a logger instance. 
        /// </summary>
        /// <returns>A logger.</returns>
        static NLog.ILogger GetLogger()
        {
            return NLog.LogManager.GetLogger(typeof(Program).FullName);
        }
    }
}
