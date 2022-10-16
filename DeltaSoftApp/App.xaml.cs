using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DeltaSoftApp
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "newsDb.db";
        public static NewsRepository newsDatabase;
        public static NewsRepository NewsDatabase
        {
            get
            {
                if (newsDatabase == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
                    if (!File.Exists(dbPath))
                    {
                        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                        using (Stream stream = assembly.GetManifestResourceStream($"DeltaSoftApp.{DATABASE_NAME}"))
                        {
                            using (FileStream fs = new FileStream(dbPath, FileMode.OpenOrCreate))
                            {
                                stream.CopyTo(fs); 
                                fs.Flush();
                            }
                        }
                    }
                    newsDatabase = new NewsRepository(dbPath);
                }
                return newsDatabase;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
    }
}
