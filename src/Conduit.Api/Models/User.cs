using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Conduit.Api.Models
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }

        public string Bio { get; set; }

        public IList<Article> Articles { get; set; } = new List<Article>();

        public IList<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();

        public IList<UserSubscription> Subscribers { get; set; } = new List<UserSubscription>();

        public bool HasSubscription(User user)
        {
            UserSubscription subscription = new UserSubscription { SubscriberId = Id, SubscriptionId = user.Id };

            return Subscriptions.Contains(subscription);
        }

        public void AddSubscription(User user)
        {
            UserSubscription subscription = new UserSubscription { SubscriberId = Id, SubscriptionId = user.Id };

            Subscriptions.Add(subscription);
        }

        public override bool Equals(object obj)
        {
            return obj is User user && Id == user.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
