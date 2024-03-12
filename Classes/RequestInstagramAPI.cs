using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstApp.Classes
{
    public class RequestInstagramAPI : AsyncTask<Java.Lang.Void, String, String>
    {
        public string token { get; set; }
        private AppPreferences appPreferences = new AppPreferences();
        public MainActivity activity{get; set;}
        
        protected override string RunInBackground(params Java.Lang.Void[] @params)
        {
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response;
            //string url = Android.App.Application.Context.Resources.GetString(Resource.String.get_user_info_url) + token;
            //try
            //{
            //    response = await httpClient.GetAsync(url);
            //    return await response.Content.ReadAsStringAsync();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
            //return null;
            return RunAsync().Result;
        }
        private async Task<string> RunAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            string url = Android.App.Application.Context.Resources.GetString(Resource.String.get_user_info_url) + token;
            try
            {
                response = await httpClient.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
        protected override void OnPostExecute([AllowNull] string result)
        {
            base.OnPostExecute(result);
            if (result != null)
            {
                try
                {
                    JObject jsonObject = JObject.Parse(result);
                    JObject jsonData = (JObject)jsonObject["data"];
                    if (jsonData.ContainsKey("id"))
                    {
                        appPreferences.PutString(AppPreferences.USER_ID, jsonData["id"].ToString());
                        appPreferences.PutString(AppPreferences.USER, jsonData["username"].ToString());
                        appPreferences.PutString(AppPreferences.PROFILE_PIC, jsonData["profile_picture"].ToString());
                        activity.Login();
                    }
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Toast.MakeText(Application.Context, "Login error!", ToastLength.Long).Show();
            }
        }
    }
}