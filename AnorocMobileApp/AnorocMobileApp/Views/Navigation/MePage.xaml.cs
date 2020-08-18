using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using NUnit.Framework;
using SQLite;
using System.Collections.Generic;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    /// <summary>
    /// Page to show the Me page.
    /// </summary>
    [SQLite.Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {



        /// <summary>
        /// Initializes a new instance of the <see cref="MePage" /> class.
        /// </summary>
        /// 
        private string title = "";
        private string body = "";
        public MePage()
        {
            InitializeComponent();                        
        }

        public void OnAppearing()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<NotificationDB>();
                var notificationList = new List<NotificationDB>();
                var notificaitons = conn.Table<NotificationDB>().ToList();
                foreach(var notificationRow in notificaitons)
                {
                    notificationList.Add(notificationRow);
                }
            }            
        }
        
        /*
        public Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }
        */

    }
}