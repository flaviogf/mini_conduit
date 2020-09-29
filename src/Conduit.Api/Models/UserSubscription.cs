using System;

namespace Conduit.Api.Models
{
    public class UserSubscription
    {
        public User Subscriber { get; set; }

        public string SubscriberId { get; set; }

        public User Subscription { get; set; }

        public string SubscriptionId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserSubscription subscription && SubscriberId == subscription.SubscriberId && SubscriptionId == subscription.SubscriptionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SubscriberId, SubscriptionId);
        }
    }
}
