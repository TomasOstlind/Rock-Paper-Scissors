using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    class GameHandler
    {
        //-----------------Private fields--------------------
        private string nameP1;
        private string nameP2;
        private Random rnd = new Random();
        private int[] hands = new int[] { 1, 2, 3 }; // 1 = Rock | 2 = Paper | 3 = Scissors
        private int computerChoice;
        private SQLite.Net.SQLiteConnection conn;

        //-----------------Properties-----------------------------
        #region props
        public int PointsP1 { get; set; }
        public int PointsP2 { get; set; }
        public int RoundCounter { get; set; }
        public int ComputersChoice { get { return computerChoice; } }

        #endregion

        /// <summary>
        /// Constructor for gamehandler
        /// </summary>
        /// <param name="nameInP1"></param>
        /// <param name="nameInP2"></param>
        public GameHandler(string nameInP1, string nameInP2)
        {
            nameP1 = nameInP1;
            nameP2 = nameInP2;
            ConnectToDB();
        }

        //----------------Methods----------------------------------
        #region methods
        /// <summary>
        /// Connects to the local SQLite database
        /// </summary>
        private void ConnectToDB()
        {
            var sqlpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "MoviesDb.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), sqlpath);
        }
        /// <summary>
        /// Handles the gamelogic
        /// </summary>
        /// <param name="choiceP1"></param>
        /// <param name="choiceP2"></param>
        /// <returns></returns>
        public int Start(int choiceP1, int choiceP2)
        {
            RoundCounter++;

            if (choiceP2 == 0)
            {
                choiceP2 = ComputerSelection();
                if (RoundCounter == 3)
                {
                    int winner = CalcWinner(choiceP1, choiceP2);
                    //TODO Save to DB
                    return winner;
                }
                else
                {
                    int winner = CalcWinner(choiceP1, choiceP2);
                    return winner;
                }
            }
            else
            {
                if (RoundCounter == 3)
                {
                    int winner = CalcWinner(choiceP1, choiceP2);
                    //TODO save to DB
                    return winner;
                }
                else
                {
                    int winner = CalcWinner(choiceP1, choiceP2);
                    return winner;
                }
            }
        }
        /// <summary>
        /// Calculates the winner of a round
        /// </summary>
        private int CalcWinner(int choiceP1, int choiceP2)
        {
            if (choiceP1 == choiceP2)
            {
                return 1;
            }
            else if (choiceP1 == 1 && choiceP2 == 3 || choiceP1 == 3 && choiceP2 == 2 || choiceP1 == 2
                && choiceP2 == 1)
            {
                PointsP1++;
                return 2;
            }
            else
            {
                PointsP2++;
                return 3;
            }
        }
        /// <summary>
        /// Randomize the computer selection
        /// </summary>
        private int ComputerSelection()
        {
            return computerChoice = hands[rnd.Next(hands.Length)];
        }

        #endregion methods
    }
}
