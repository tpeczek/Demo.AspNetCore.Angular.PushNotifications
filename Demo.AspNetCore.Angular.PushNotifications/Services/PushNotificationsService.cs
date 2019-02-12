using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Demo.AspNetCore.Angular.PushNotifications.Services
{
    public class PushNotificationsService : BackgroundService
    {
        private const int NOTIFICATION_FREQUENCY = 60000;

        private readonly Random _random = new Random();

        private readonly IPushSubscriptionsService _pushSubscriptionsService;
        private readonly PushServiceClient _pushClient;

        public PushNotificationsService(IOptions<PushNotificationsOptions> options, IPushSubscriptionsService pushSubscriptionsService, PushServiceClient pushClient)
        {
            _pushSubscriptionsService = pushSubscriptionsService;

            _pushClient = pushClient;
            _pushClient.DefaultAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                Subject = "https://angular-pushnotifications.demo.io"
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(NOTIFICATION_FREQUENCY, stoppingToken);

                SendNotifications(_random.Next(-20, 55), stoppingToken);
            }
        }

        private void SendNotifications(int temperatureC, CancellationToken stoppingToken)
        {
            AngularPushNotification notification = new AngularPushNotification
            {
                Title = "New Weather Forecast",
                Body = $"Temp. (C): {temperatureC} | Temp. (F): {32 + (int)(temperatureC / 0.5556)}",
                Icon = "assets/icons/icon-96x96.png"
            };

            PushMessage message = notification.ToPushMessage();

            foreach (PushSubscription subscription in _pushSubscriptionsService.GetAll())
            {
                // Fire-and-forget 
                _pushClient.RequestPushMessageDeliveryAsync(subscription, message, stoppingToken);
            }
        }
    }
}
