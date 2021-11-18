﻿using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShikkhanobishStudentApp.Model
{
    public class ShoeNotification
    {
        public async Task ShowNotification(string msg)
        {
            await NotificationCenter.Current.Show((notification) => notification
                    .WithScheduleOptions((schedule) => schedule
                    .Build())
                    .WithAndroidOptions((android) => android
                         .WithAutoCancel(true)
                         .WithChannelId("General")
                         .WithOngoing(true)
                         .WithTimeout(TimeSpan.FromSeconds(30))
                         .WithPriority(NotificationPriority.Max)
                         .WithVisibilityType(Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public)
                         .Build())
                    .WithiOSOptions((ios) => ios
                        .Build())
                    .WithReturningData("Dummy Data")
                    .WithTitle("Shikkhanobish")
                    .WithDescription(msg)
                    .WithSound("ringtone.mp3")
                    .WithNotificationId(100)
                    .Create());
        }
    }
}
