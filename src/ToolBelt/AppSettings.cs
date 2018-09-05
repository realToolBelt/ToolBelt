﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ToolBelt
{
    public static class AppSettings
    {
        private const string DefaultAppCenterAndroid = "a54ecf68-25e1-4870-9193-5f65305a2c33";
        private const string DefaultAppCenteriOS = "c06cf109-e217-4550-a73b-5c55ccb7aabc";

        public static string AppCenterAnalyticsAndroid
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsAndroid), DefaultAppCenterAndroid);
            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsAndroid), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Settings.GetValueOrDefault(nameof(AppCenterAnalyticsIos), DefaultAppCenteriOS);
            set => Settings.AddOrUpdateValue(nameof(AppCenterAnalyticsIos), value);
        }

        private static ISettings Settings => CrossSettings.Current;
    }
}
