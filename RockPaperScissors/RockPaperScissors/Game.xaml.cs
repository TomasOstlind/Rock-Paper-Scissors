using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        //------------Private fields-------------------
        private int vs = 0; //To check if user have choosen PvP or PvC | 0 = PvC | 1 = PvP
        private BitmapImage iBeforeSelection = new BitmapImage();
        private BitmapImage iRock = new BitmapImage();
        private BitmapImage iPaper = new BitmapImage();
        private BitmapImage iScissors = new BitmapImage();
        private GameHandler game;
        private int choiceP1 = 0;
        private int choiceP2 = 0;

        public Game()
        {
            this.InitializeComponent();

            if (!CheckGamePlaying())
            {
                InitializeGui();
            }
            else
            {
                InitializePlayingGame();
            }
        }
        /// <summary>
        /// Assagin all the values from the game that is currently active
        /// </summary>
        private void InitializePlayingGame()
        {
            AssaignImages();

            var games = (from g in App.connection.Table<GameHistory>()
                         select g).Last();

            game = new GameHandler(games.NamePlayerOne, games.NamePlayerTwo, true);

            if (games.NamePlayerTwo == "Computer")
            {
                GridButtonsP1.Visibility = Visibility.Visible;
                GridStartButtons.Visibility = Visibility.Collapsed;
            }
            else
            {
                GridButtonsP1.Visibility = Visibility.Visible;
                GridButtonsP2.Visibility = Visibility.Visible;
                GridStartButtons.Visibility = Visibility.Collapsed;
            }

            textBlockP1.Text = games.NamePlayerOne;
            textBlockP2.Text = games.NamePlayerTwo;
            textBlockPointsP1.Text = game.PointsP1.ToString();
            textBlockPointsP2.Text = game.PointsP2.ToString();
        }

        /// <summary>
        /// Checks if a game is playing or not
        /// </summary>
        /// <returns></returns>
        private bool CheckGamePlaying()
        {
            var games = (from g in App.connection.Table<GameHistory>()
                         select g).Last();

            if (String.IsNullOrEmpty(games.Winner) && !String.IsNullOrEmpty(games.NamePlayerOne))
            {
                return false;
            }
            else
            {
                return true;
            }
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
        /// <summary>
        /// Handles the manu pane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// User start the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            if (vs == 0 && !String.IsNullOrEmpty(textBlockP1.Text))
            {
                GridButtonsP1.Visibility = Visibility.Visible;
                GridStartButtons.Visibility = Visibility.Collapsed;

                game = new GameHandler(textBlockP1.Text, "Computer", false);
            }
            else if (vs == 1 && !String.IsNullOrEmpty(textBlockP1.Text) && !String.IsNullOrEmpty(textBlockP2.Text))
            {
                GridButtonsP1.Visibility = Visibility.Visible;
                GridButtonsP2.Visibility = Visibility.Visible;
                GridStartButtons.Visibility = Visibility.Collapsed;

                game = new GameHandler(textBlockP1.Text, textBlockP2.Text, false);
            }
            else
            {
                var msg = new Windows.UI.Popups.MessageDialog("You must enter name(s) to play!", "Name(s) needed!");

                msg.ShowAsync(); //Im aware of the async but in this case i dont need async and I think this do not affect the game
            }
        }
        /// <summary>
        /// Handles the player 1 choices
        /// </summary>
        private void Player1Selection(int selectionIn)
        {
            if (choiceP2 == 0)
            {
                GridButtonsP1.Visibility = Visibility.Collapsed;
                choiceP1 = selectionIn;
            }
            else
            {
                GridButtonsP1.Visibility = Visibility.Collapsed;
                choiceP1 = selectionIn;

                ShowPlayersChoice();
                Play();
            }
            //If someone playing against the computer
            if (vs == 0)
            {
                GridButtonsP1.Visibility = Visibility.Collapsed;
                choiceP1 = selectionIn;

                ShowPlayersChoice();
                Play();
            }
        }
        /// <summary>
        /// Handles the player 2 choices
        /// </summary>
        private void Player2Selection(int selectionIn)
        {
            if (choiceP1 == 0)
            {
                GridButtonsP2.Visibility = Visibility.Collapsed;
                choiceP2 = selectionIn;
            }
            else
            {
                GridButtonsP2.Visibility = Visibility.Collapsed;
                choiceP2 = selectionIn;

                ShowPlayersChoice();
                Play();
            }
        }

        private void Play()
        {
            int winner = game.Start(choiceP1, choiceP2);
            if (vs == 0)
            {
                PlayerVsComputer(winner);
            }
            else
            {
                PlayerVsPlayer(winner);
            }

        }
        /// <summary>
        /// Handles the PvP
        /// </summary>
        /// <param name="winner"></param>
        private async void PlayerVsPlayer(int winnerIn)
        {
            textBlockPointsP1.Text = game.PointsP1.ToString();
            textBlockPointsP2.Text = game.PointsP2.ToString();
            if (game.RoundCounter == 3)
            {
                DisplayWinner();
                await Task.Delay(TimeSpan.FromSeconds(3));
                textBlockPointsP1.Text = "0";
                textBlockPointsP2.Text = "0";
                Reset();
            }
            else if (winnerIn == 1)
            {
                textBlockWinner.Text = "It was a tie...";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }
            else if (winnerIn == 2)
            {
                textBlockWinner.Text = textBlockP1.Text + " was the winner!";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }
            else
            {
                textBlockWinner.Text = textBlockP2.Text + " was the winner!";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }

        }
        /// <summary>
        /// Handles PvC
        /// </summary>
        /// <param name="winner"></param>
        private async void PlayerVsComputer(int winnerIn)
        {
            ShowComputerChoice();
            textBlockPointsP1.Text = game.PointsP1.ToString();
            textBlockPointsP2.Text = game.PointsP2.ToString();
            if (game.RoundCounter == 3)
            {
                DisplayWinner();
                await Task.Delay(TimeSpan.FromSeconds(3));
                textBlockPointsP1.Text = "0";
                textBlockPointsP2.Text = "0";
                Reset();
            }
            else if (winnerIn == 1)
            {
                textBlockWinner.Text = "It was a tie...";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }
            else if (winnerIn == 2)
            {
                textBlockWinner.Text = textBlockP1.Text + " was the winner!";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }
            else
            {
                textBlockWinner.Text = textBlockP2.Text + " was the winner!";
                await Task.Delay(TimeSpan.FromSeconds(3));
                Reset();
            }

        }
        /// <summary>
        /// Displayes the total winner
        /// </summary>
        private void DisplayWinner()
        {
            if (game.PointsP1 == game.PointsP2)
            {
                textBlockWinner.Text = game.CalcTotalWinner();
            }
            else
            {
                textBlockWinner.Text = game.CalcTotalWinner() + " won the game!";
            }
        }
        /// <summary>
        /// Resets the buttons, choice and text after a round
        /// </summary>
        private void Reset()
        {
            choiceP1 = 0;
            choiceP2 = 0;

            if (game.RoundCounter != 3)
            {
                GridButtonsP1.Visibility = Visibility.Visible;
                if (vs != 0)
                {
                    GridButtonsP2.Visibility = Visibility.Visible;
                }
                imageP1.Source = iBeforeSelection;
                imageP2.Source = iBeforeSelection;
                textBlockWinner.Text = "";
            }
            else
            {
                game.RoundCounter = 0;
                game.PointsP1 = 0;
                game.PointsP2 = 0;
                GridButtonsP1.Visibility = Visibility.Visible;
                if (vs != 0)
                {
                    GridButtonsP2.Visibility = Visibility.Visible;
                }
                imageP1.Source = iBeforeSelection;
                imageP2.Source = iBeforeSelection;
                textBlockWinner.Text = "";
            }


        }
        /// <summary>
        /// Shows player one choice
        /// </summary>
        private void ShowPlayersChoice()
        {
            //-----Player 1------------
            if (choiceP1 == 1)
            {
                imageP1.Source = iRock;
            }
            else if (choiceP1 == 2)
            {
                imageP1.Source = iPaper;
            }
            else if (choiceP1 == 3)
            {
                imageP1.Source = iScissors;
            }
            //-----Player 2------------
            if (choiceP2 == 1)
            {
                imageP2.Source = iRock;
            }
            else if (choiceP2 == 2)
            {
                imageP2.Source = iPaper;
            }
            else if (choiceP2 == 3)
            {
                imageP2.Source = iScissors;
            }
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
               "Sorry! The name can´t be longer than six letters.", "Name to long");

                msg.ShowAsync(); //Im aware of the async but in this case i dont need async and I think this do not affect the game

                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Handles when to show what choice the computer did
        /// </summary>
        private void ShowComputerChoice()
        {
            if (game.ComputersChoice == 1)
            {
                imageP2.Source = iRock;
            }
            else if (game.ComputersChoice == 2)
            {
                imageP2.Source = iPaper;
            }
            else
            {
                imageP2.Source = iScissors;
            }
        }
        #region Players RPS-button click
        /// <summary>
        /// Player one choses Rock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1_RockButtonClick(object sender, RoutedEventArgs e)
        {
            Player1Selection(1);
        }
        /// <summary>
        /// Player one choses Paper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1_PaperButtonClick(object sender, RoutedEventArgs e)
        {
            Player1Selection(2);
        }
        /// <summary>
        /// Player one choses Scissors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P1_ScissorsButtonClick(object sender, RoutedEventArgs e)
        {
            Player1Selection(3);
        }
        /// <summary>
        /// Player two choses Rock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P2_RockButtonClick(object sender, RoutedEventArgs e)
        {
            Player2Selection(1);
        }
        /// <summary>
        /// Player two choses Paper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P2_PaperButtonClick(object sender, RoutedEventArgs e)
        {
            Player2Selection(2);
        }
        /// <summary>
        /// Player two choses Scissors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P2_ScissorsButtonClick(object sender, RoutedEventArgs e)
        {
            Player2Selection(3);
        }
        #endregion
    }
}
