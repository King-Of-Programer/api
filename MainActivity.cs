using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using FFImageLoading;
using InstApp.Classes;
using InstApp.Dialogs;
using System;

namespace InstApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IAuthenticationListener
    {
        private String token = null;
        private AppPreferences appPreferences;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            appPreferences = new AppPreferences();
            ImageService.Instance.Initialize();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Button but = FindViewById<Button>(Resource.Id.btn_login);
            but.Click += onClick;
           
        }
        public void onClick(object sender, EventArgs e)
        {
            if (token != null)
            {
                Logout();
            }
            else
            {
                var authenticationDialog = new AuthenticationDialog(this, this);
                authenticationDialog.SetCancelable(true);
                authenticationDialog.Show();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void onTokenReceived(string auth_token)
        {
            if (auth_token == null)
                return;
            if (auth_token.Contains("message"))
            {
                View info = FindViewById<LinearLayout>(Resource.Id.info);
                info.Visibility = ViewStates.Visible;
                TextView id = FindViewById<TextView>(Resource.Id.id);
                id.Text = auth_token.Substring(auth_token.LastIndexOf("=") + 1).Replace("+"," ");
                return;
            }
            appPreferences.PutString(AppPreferences.TOKEN, auth_token);
            token = auth_token;
            getUserInfoByAccessToken(token);
        }
        private void getUserInfoByAccessToken(String token)
        {
            var req = new RequestInstagramAPI();
            req.token = token;
            req.activity = this;
            req.Execute();
        }
        public void Login()
        {
            Button button = FindViewById<Button>(Resource.Id.btn_login);
            View info = FindViewById<LinearLayout>(Resource.Id.info);
            ImageView pic = FindViewById<ImageView>(Resource.Id.pic);
            TextView id = FindViewById<TextView>(Resource.Id.id);
            TextView name = FindViewById<TextView>(Resource.Id.name);
            info.Visibility = ViewStates.Visible;
            button.Text="LOGOUT";
            name.Text=appPreferences.GetString(AppPreferences.USER);
            id.Text=appPreferences.GetString(AppPreferences.USER_ID);
            ImageService.Instance.LoadUrl(appPreferences.GetString(AppPreferences.PROFILE_PIC)).Into(pic);
        }
        public void Logout()
        {
            Button button = FindViewById<Button>(Resource.Id.btn_login);
            View info = FindViewById<LinearLayout>(Resource.Id.info);
            button.Text="INSTAGRAM LOGIN";
            token = null;
            info.Visibility=ViewStates.Gone;
            appPreferences.Clear();
        }
    }
}