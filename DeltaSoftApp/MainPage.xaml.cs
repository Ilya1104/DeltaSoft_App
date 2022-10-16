using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Security.Permissions;//глянуть разрешения
using System.Threading.Tasks;
using Xamarin.Forms;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Firestore;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Android.Telephony;
using Android.Content;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Content.Res;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace DeltaSoftApp
{
    public partial class MainPage : ContentPage
    {
        FirestoreDb db;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            ConnectFirebase();
            await GetWEbView();
            base.OnAppearing();
        }
        public const string JSON_SDK_NAME = "deltasoftappfb-firebase-adminsdk-nlg98-df3196c5ea.json";
        private void ConnectFirebase()
        {
            string jsonSDKpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), JSON_SDK_NAME);
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"DeltaSoftApp.{JSON_SDK_NAME}"))
            {
                using (FileStream fs = new FileStream(jsonSDKpath, FileMode.OpenOrCreate))
                {
                    stream.CopyTo(fs);
                    fs.Flush();
                }
            }
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonSDKpath);
            db = FirestoreDb.Create("deltasoftappfb");
        }
        private string path;
        public async Task GetWEbView()
        {
            if (path is null)
            {
                await LoadFire();
            }
            else
            {
                await LoadFire();
            }
        }
        private string getUrl;
        private string deviceBrand;
        private string hasSim;
        private async Task GetDrownView()
        {
            await App.NewsDatabase.CreateTable();
            newsList.ItemsSource = await App.NewsDatabase.GetItemsAsync();
        }
        private async Task LoadFire()
        {
            if (path is null)
            {
                //getUrl = await GetFirebaseData();
                  getUrl = getJSON_Url();
                deviceBrand = GetManufacturer();
                hasSim = IsSIMExists();
            }

            if (string.IsNullOrEmpty(getUrl) || deviceBrand.ToLower() == "google" || hasSim.Contains("Absent"))
            {
                await GetDrownView();
                appName.Text += "\n News View";
                drownView.IsVisible = true;
            }
            else
            {
                path = getUrl;
                vw.Source = path;
                appName.Text += "\n Web View";
                viewWeb.IsVisible = true;
            }
        }
        private string GetManufacturer()
        {
            return DeviceInfo.Manufacturer;
        }
        private async Task<string> GetFirebaseData()//Grpc.Core.RpcException: 'Status(StatusCode="Internal", Detail="Bad gRPC response. Response protocol downgraded to HTTP/1.1.")'
        {
            DocumentReference documentReference = db.Collection("webView").Document("URLs");
            DocumentSnapshot snapshot = await documentReference.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                WebViewModel wv = snapshot.ConvertTo<WebViewModel>();
                path = wv.url;
            }
            return path;
        }
        private string IsSIMExists()
        {
            TelephonyManager mgr = Android.App.Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            return mgr.SimState.ToString();//absent - SIM отсутствует
        }
        public const string JSON_NAME = "DeltaSoftApp.firebase_remote_config.json";
        private string getJSON_Url()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(JSON_NAME);

            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(json);
                string url = jsonObject["url"].ToString();
                reader.Close();
                return url;
            }
        }
    }
}
