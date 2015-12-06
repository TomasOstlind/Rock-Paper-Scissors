using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RockPaperScissors
{
    public sealed partial class HistoryGames : Page
    {
        public HistoryGames()
        {
            this.InitializeComponent();
            InitializeGUI();
        }
        private void SplitView(object sender, RoutedEventArgs e)
        {
            MyPane.SplitView.IsPaneOpen = !MyPane.SplitView.IsPaneOpen;
        }
        private void InitializeGUI()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "GameHistoryDB.sqlite");
            SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath);

            var games = from g in conn.Table<GameHistory>()
                        select g;

            gridView.ItemsSource = games.ToList();
            //TODO fix this
        }
    }
}
