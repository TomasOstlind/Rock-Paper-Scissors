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
    public sealed partial class Game : Page
    {
        //------------Varibles-------------------
        private int vs = 0; //To check if user have choosen PvP or PvC

        public Game()
        {
            this.InitializeComponent();
            InitializeGui();
        }
        /// <summary>
        /// Handles the pre-GUI
        /// </summary>
        private void InitializeGui()
        {
            EnterNameP2.Visibility = Visibility.Collapsed;
            GridButtonsP1.Visibility = Visibility.Collapsed;
            GridButtonsP2.Visibility = Visibility.Collapsed;

        }

        private void SplitView(object sender, RoutedEventArgs e)
        {
            MyPane.SplitView.IsPaneOpen = !MyPane.SplitView.IsPaneOpen;
        }
        /// <summary>
        /// Switch between PvP and PvC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoosePMode_Click(object sender, RoutedEventArgs e)
        {
            if (vs == 0)
            {
                vs = 1;
                EnterNameP2.Visibility = Visibility.Visible;
                btnChoosePMode.Content = "PvC";
            }
            else 
            {
                vs = 0;
                EnterNameP2.Visibility = Visibility.Collapsed;
                btnChoosePMode.Content = "PvP";
            }
        }
    }
}
