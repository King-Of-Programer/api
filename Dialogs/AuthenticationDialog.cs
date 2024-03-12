using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using InstApp.Classes;


namespace InstApp.Dialogs
{
    public class AuthenticationDialog : Dialog
    {
        private readonly string redirect_url;
        private readonly string request_url;
        private IAuthenticationListener listener;
        private MyWebViewClient webViewClient;

        public AuthenticationDialog(Context context, IAuthenticationListener listener) : base(context)
        {
            this.listener = listener;
            this.redirect_url = context.Resources.GetString(Resource.String.redirect_url);
            this.request_url = context.Resources.GetString(Resource.String.base_url) +
                "oauth/authorize/?client_id=" +
                context.Resources.GetString(Resource.String.client_id) +
                "&redirect_uri=" + redirect_url +
                "&scope=user_profile,user_media" +
                "&response_type=code";
            webViewClient = new MyWebViewClient(this, redirect_url, listener); 

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.auth_dialog);
            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl(request_url);
            webView.SetWebViewClient(webViewClient);
        }
    }
}