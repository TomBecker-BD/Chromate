using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace Toaster.WebAPI
{
    /// <summary>
    /// HTTP header helper. 
    /// </summary>
    public static class HeaderHelper
    {
        /// <summary>
        /// Get a string header value. 
        /// </summary>
        /// <param name="headers">HTTP headers. </param>
        /// <param name="name">Name of the header value to get. </param>
        /// <returns>Header value, or null if not found. </returns>
        public static string Get(this HttpHeaders headers, string name)
        {
            string text = null;
            IEnumerable<string> values;
            if (headers.TryGetValues(name, out values))
            {
                text = values.FirstOrDefault();
            }
            return text;
        }

        /// <summary>
        /// Get an integer header value. 
        /// </summary>
        /// <param name="headers">HTTP headers. </param>
        /// <param name="name">Name of the header value to get. </param>
        /// <returns>Header value, or null if not found. </returns>
        public static int? GetInt(this HttpHeaders headers, string name)
        {
            string text = headers.Get(name);
            if (text != null)
            {
                int value;
                if (int.TryParse(text, out value))
                {
                    return value;
                }
            }
            return null;
        }
    }
}