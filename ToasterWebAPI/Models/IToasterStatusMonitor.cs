using System.Threading.Tasks;

namespace Toaster.WebAPI
{
    public interface IToasterStatusMonitor
    {
        /// <summary>
        /// Get a task to wait for the toaster status. 
        /// </summary>
        /// <param name="timeout">Timeout in milliseconds. </param>
        /// <returns>Task to wait for the toaster status. </returns>
        Task<ToasterStatus> GetToasterStatusAsync(int timeout);
    }
}
