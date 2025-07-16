using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface INotificationChannelService
    {
        NotificationChannel SelectById(Guid id);

        List<NotificationChannel> SelectBySupportNotification(Guid id);

        List<User> SelectUsersForNotificationChannel(Guid id);

        List<Queue> SelectQueuesForNotificationChannel(Guid id);

        List<Contact> SelectContactsForNotificationChannel(Guid id);
    }
}
