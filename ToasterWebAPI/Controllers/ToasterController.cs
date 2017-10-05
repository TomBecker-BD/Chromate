using System.Threading.Tasks;
using System.Web.Http;
using Toaster.Interfaces;

namespace Toaster.WebAPI.Controllers
{
    [RoutePrefix("api/toaster")]
    public class ToasterController : ApiController
    {
        IToaster _toaster;
        IToasterStatusMonitor _statusMonitor;

        public ToasterController(IToaster toaster, IToasterStatusMonitor statusMonitor)
        {
            _toaster = toaster;
            _statusMonitor = statusMonitor;
        }

        [HttpGet, Route("status")]
        public IHttpActionResult GetStatus()
        {
            int timeout = (Request.Headers.GetInt("timeout") ?? 0) * 1000;
            return _statusMonitor.GetToasterStatusAsync(timeout).ToHttpContentResult(Request);
        }

        ToasterStatus GetToasterStatus()
        {
            ToasterStatus status = new ToasterStatus()
            {
                setting = _toaster.Setting,
                content = _toaster.Content,
                toasting = _toaster.Toasting,
                color = _toaster.Color
            };
            return status;
        }

        [HttpPut, Route("setting")]
        public IHttpActionResult PutSetting([FromBody]ToasterStatus body)
        {
            return Task.Run(() =>
            {
                _toaster.Setting = body.setting;
                return GetToasterStatus();
            }).ToHttpContentResult(Request);
        }

        [HttpPut, Route("content")]
        public IHttpActionResult PutContent([FromBody]ToasterStatus body)
        {
            return Task.Run(() =>
            {
                _toaster.Content = body.content;
                _toaster.Color = body.color;
                return GetToasterStatus();
            }).ToHttpContentResult(Request);
        }

        [HttpPut, Route("toasting")]
        public IHttpActionResult PutToasting([FromBody]ToasterStatus body)
        {
            return Task.Run(() =>
            {
                _toaster.Toasting = body.toasting;
                return GetToasterStatus();
            }).ToHttpContentResult(Request);
        }
    }
}
