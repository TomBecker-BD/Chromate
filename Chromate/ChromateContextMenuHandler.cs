using System;
using CefSharp;

namespace Chromate
{
    public class ChromateContextMenuHandler : IContextMenuHandler
    {
        IWebBrowser _browserControl;

        public ChromateContextMenuHandler(IWebBrowser browserControl)
        {
            _browserControl = browserControl;
        }

        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            if (browserControl == _browserControl)
            {
                // Disable the context menu. 
                model.Clear();
            }
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
