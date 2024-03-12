using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace InstApp.Classes
{

    public class AppPreferences
    {
        public static readonly string APP_PREFERENCES_FILE_NAME = "userdata";
        public static readonly string USER_ID = "userID";
        public static readonly string TOKEN = "token";
        public static readonly string PROFILE_PIC = "profile_pic";
        public static readonly string USER = "username";

        public string GetString(string key)
        {
            return Preferences.Get(key, null);
        }

        public void PutString(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void Clear()
        {
            Preferences.Clear();
        }
    }

}