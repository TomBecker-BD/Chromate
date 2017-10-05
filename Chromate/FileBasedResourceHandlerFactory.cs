using System;
using System.Collections.Generic;
using System.IO;
using CefSharp;

namespace Chromate
{
    /// <summary>
    /// FileBasedResourceHandlerFactory is a CefSharp resource handler factory. 
    /// It makes files in a directory hierarchy available as if they were served 
    /// on the web, and it supports a web API. 
    /// </summary>
    public class FileBasedResourceHandlerFactory : IResourceHandlerFactory
    {
        /// <summary>
        /// Base HTTP URI. 
        /// </summary>
        Uri _baseWebUri;

        /// <summary>
        /// Base file URI. 
        /// </summary>
        Uri _baseDirectoryUri;

        /// <summary>
        /// Default resource handler factory. For requests not handled by this factory. 
        /// </summary>
        IResourceHandlerFactory _defaultResourceHandlerFactory;

        /// <summary>
        /// Cache of resource handlers. 
        /// </summary>
        Dictionary<string, IResourceHandler> _handlers = new Dictionary<string, IResourceHandler>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Web API. 
        /// </summary>
        IWebAPI _webAPI;

        /// <summary>
        /// Remap URLs. 
        /// </summary>
        IDictionary<string, string> _remap;
        
        /// <summary>
        /// Whether this factory has handlers. It had better. 
        /// </summary>
        public bool HasHandlers
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initialize the file based resource handler factory. 
        /// </summary>
        /// <param name="baseWeb">Base HTTP URL. </param>
        /// <param name="baseDirectory">Base directory path. </param>
        /// <param name="defaultResourceHandlerFactory">Default resource handler factory. </param>
        /// <param name="webAPI">Web API. </param>
        /// <param name="remap">Remap URLs. </param>
        public FileBasedResourceHandlerFactory(string baseWeb, string baseDirectory, IResourceHandlerFactory defaultResourceHandlerFactory, 
            IWebAPI webAPI, IDictionary<string, string> remap)
        {
            _baseWebUri = new Uri(baseWeb);
            _baseDirectoryUri = new Uri(Path.Combine(baseDirectory, "placeholder.html"));
            _defaultResourceHandlerFactory = defaultResourceHandlerFactory;
            _webAPI = webAPI;
            _remap = remap;
        }

        /// <summary>
        /// Get a resource handler for a request. 
        /// </summary>
        /// <param name="browserControl">Web browser control. </param>
        /// <param name="browser">Browser. </param>
        /// <param name="frame">Frame. </param>
        /// <param name="request">Request. </param>
        /// <returns>Resource handler for this request. </returns>
        public IResourceHandler GetResourceHandler(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
        {
            IResourceHandler handler = null;
            if (!_handlers.TryGetValue(request.Url, out handler))
            {
                Uri requestUri = new Uri(request.Url);
                if ((requestUri.Segments.Length > 1) && string.Equals(requestUri.Segments[1], "api/", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResourceHandler(_webAPI);
                }
                else
                {
                    if (_remap != null)
                    {
                        if (requestUri.Segments.Length > 1)
                        {
                            string remapUrl;
                            if (_remap.TryGetValue(requestUri.Segments[1], out remapUrl))
                            {
                                UriBuilder builder = new UriBuilder(requestUri);
                                builder.Path = remapUrl;
                                requestUri = builder.Uri;
                            }
                        }
                    }
                    handler = GetFileResourceHandler(requestUri);
                    if (handler != null)
                    {
                        _handlers[request.Url] = handler;
                    }
                    else if (_defaultResourceHandlerFactory != null)
                    {
                        handler = _defaultResourceHandlerFactory.GetResourceHandler(browserControl, browser, frame, request);
                    }
                }
            }
            return handler;
        }

        /// <summary>
        /// Get a file resource handler. 
        /// </summary>
        /// <param name="requestUri">Request URL. </param>
        /// <returns>Resource handler, if the request has the base URI as a base and it is for a file that exists, otherwise false. </returns>
        IResourceHandler GetFileResourceHandler(Uri requestUri)
        {
            IResourceHandler handler = null;
            if (_baseWebUri.IsBaseOf(requestUri))
            {
                Uri relativeUri = _baseWebUri.MakeRelativeUri(requestUri);
                Uri fileUri = new Uri(_baseDirectoryUri, relativeUri);
                string filePath = Uri.UnescapeDataString(fileUri.AbsolutePath);
                if (File.Exists(filePath))
                {
                    string mimeType = GetMimeType(filePath);
                    handler = ResourceHandler.FromFilePath(filePath, mimeType);
                }
            }
            return handler;
        }

        /// <summary>
        /// Get the MIME type for a file. 
        /// </summary>
        /// <param name="filePath">File name or path. </param>
        /// <returns>MIME type. </returns>
        string GetMimeType(string filePath)
        {
            string mimeType = null;
            string extension = Path.GetExtension(filePath);
            switch (extension)
            {
                case ".css":
                    mimeType = "text/css";
                    break;
                case ".html":
                    mimeType = "text/html";
                    break;
                case ".js":
                    mimeType = "text/javascript";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".svg":
                    mimeType = "image/svg+xml";
                    break;
            }
            return mimeType;
        }
    }
}
