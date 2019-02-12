using Microsoft.AspNetCore.Mvc;
using Lib.Net.Http.WebPush;
using Demo.AspNetCore.Angular.PushNotifications.Services;

namespace Demo.AspNetCore.Angular.PushNotifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushSubscriptionsController : ControllerBase
    {
        private readonly IPushSubscriptionsService _pushSubscriptionsService;

        public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService)
        {
            _pushSubscriptionsService = pushSubscriptionsService;
        }

        [HttpPost]
        public void Post([FromBody] PushSubscription subscription)
        {
            _pushSubscriptionsService.Insert(subscription);
        }

        [HttpDelete("{endpoint}")]
        public void Delete(string endpoint)
        {
            _pushSubscriptionsService.Delete(endpoint);
        }
    }
}
