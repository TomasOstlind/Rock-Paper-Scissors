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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace RockPaperScissors
{
    public sealed partial class Game : Page
    {
        //------------Varibles-------------------
        private int vs = 0; //To check if user have choosen PvP or PvC
        private BitmapImage iBeforeSelection = new BitmapImage();
        private BitmapImage iRock = new BitmapImage();
        private BitmapImage iPaper = new BitmapImage();
        private BitmapImage iScissors = new BitmapImage();
        private GameHandler game;

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
            AssaignImages();
            EnterNameP2.Visibility = Visibility.Collapsed;
            GridButtonsP1.Visibility = Visibility.Collapsed;
            GridButtonsP2.Visibility = Visibility.Collapsed;
            imageP1.Source = iBeforeSelection;
            imageP2.Source = iBeforeSelection;
            textBlockWinner.Text = "";
            textBlockP1.Text = "";
            textBlockP2.Text = "Computer";
        }
        /// <summary>
        /// Assagin images that the game uses
        /// </summary>
        private void AssaignImages()
        {
            Uri image_before_selection = new Uri("ms-appx:/Assets/fingers.png", UriKind.RelativeOrAbsolute);
            Uri paper = new Uri("ms-appx:/Assets/paper.png", UriKind.RelativeOrAbsolute);
            Uri rock = new Uri("ms-appx:/Assets/rock.png", UriKind.RelativeOrAbsolute);
            Uri scissors = new Uri("ms-appx:/Assets/scissors.png", UriKind.RelativeOrAbsolute);

            iBeforeSelection.UriSource = image_before_selection;
            iRock.UriSource = rock;
            iScissors.UriSource = scissors;
            iPaper.UriSource = paper;
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
                if (String.IsNullOrEmpty(textBlockP2.Text))
                {
                    textBlockP2.Text = "";
                }
                else
                {                
                    textBlockP2.Text = ValueTextBoxP2.Text;
                }
            }
            else 
            {
                vs = 0;
                EnterNameP2.Visibility = Visibility.Collapsed;
                btnChoosePMode.Content = "PvP";
                textBlockP2.Text = "Computer";
            }
        }

        /// <summary>
        /// Assaigns the value from the textbox to P1 name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_P1(object sender, RoutedEventArgs e)
        {
            if (CheckNameLenght(ValueTextBoxP1.Text))
            {
                textBlockP1.Text = ValueTextBoxP1.Text;
                EnterNameP1.Flyout.Hide();
            }
        }

        /// <summary>
        /// Assaigns the value from the textbox to P2 name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_P2(object sender, RoutedEventArgs e)
        {
            if (CheckNameLenght(ValueTextBoxP2.Text))
            {
                textBlockP2.Text = ValueTextBoxP2.Text;
                EnterNameP2.Flyout.Hide();
            }
        }
        /// <summary>
        /// User has start the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Checks how long the name is
        /// </summary>
        /// <param name="nameIn"></param>
        private bool CheckNameLenght(string nameIn)
        {
            if (nameIn.Length > 6)
            {
                var msg = new Windows.UI.Popups.MessageDialog(
               "Sorry! The name cant be longer than six letters.");

               msg.ShowAsync(); //Im aware of the async but in this case i dont need async

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
