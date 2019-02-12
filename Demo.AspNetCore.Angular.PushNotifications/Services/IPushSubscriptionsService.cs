using System.Collections.Generic;
using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.Angular.PushNotifications.Services
{
    public interface IPushSubscriptionsService
    {
        IEnumerable<PushSubscription> GetAll();

        void Insert(PushSubscription subscription);

        void Delete(string endpoint);
    }
}
