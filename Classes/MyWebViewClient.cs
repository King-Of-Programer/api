using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using InstApp.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstApp.Classes
{
    public class MyWebViewClient:WebViewClient
    {
        private AuthenticationDialog mDialog;
        private string redirect_url;
        IAuthenticationListener mListener;

        public MyWebViewClient(AuthenticationDialog mDialog, string redirect_url, IAuthenticationListener mListener)
        {
            this.mDialog = mDialog;
            this.redirect_url = redirect_url;
            this.mListener = mListener;
        }
        [Obsolete]
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (url.StartsWith(redirect_url))
            {
                mDialog.Dismiss();
                return true;
            }
            else if (url.Contains("?message="))
            {
                mDialog.Dismiss();
                return true;
            }
            return false;
        }
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            if (url.Contains("access_token="))
            {
                var uri = Android.Net.Uri.Parse(url);
                String access_token = uri.EncodedFragment;
                access_token = access_token.Substring(access_token.LastIndexOf("=") + 1);
                mListener.onTokenReceived(access_token);
            }
            if (url.Contains("message="))
            {
                var uri = url.Substring(url.LastIndexOf("?"));
                if (!string.IsNullOrEmpty(uri))
                {
                    mListener.onTokenReceived(uri);
                }
            }
        }
    }
}
