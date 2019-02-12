using System;
using System.Collections.Generic;
using LiteDB;
using Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.Angular.PushNotifications.Services
{
    internal class PushSubscriptionsService : IPushSubscriptionsService, IDisposable
    {
        private class LitePushSubscription : PushSubscription
        {
            public int Id { get; set; }

            public LitePushSubscription()
            { }

            public LitePushSubscription(PushSubscription subscription)
            {
                Endpoint = subscription.Endpoint;
                Keys = subscription.Keys;
            }
        }

        private readonly LiteDatabase _db;
        private readonly LiteCollection<LitePushSubscription> _collection;

        public PushSubscriptionsService()
        {
            _db = new LiteDatabase("PushSubscriptionsStore.db");
            _collection = _db.GetCollection<LitePushSubscription>("subscriptions");
        }

        public IEnumerable<PushSubscription> GetAll()
        {
            return _collection.FindAll();
        }

        public void Insert(PushSubscription subscription)
        {
            _collection.Insert(new LitePushSubscription(subscription));
        }

        public void Delete(string endpoint)
        {
            _collection.Delete(subscription => subscription.Endpoint == endpoint);
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
