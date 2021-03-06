﻿using System;
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
        private bool newGame = true;

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
        public GameHandler(string nameInP1, string nameInP2, bool playing)
        {
            nameP1 = nameInP1;
            nameP2 = nameInP2;
            if (playing)
            {
                AssaignExistingGame();
            }
        }
        //----------------Methods----------------------------------
        #region methods

        /// <summary>
        /// Assaigns the current values from an existing game
        /// </summary>
        private void AssaignExistingGame()
        {
            var games = (from g in App.connection.Table<GameHistory>()
                         select g).Last();

            nameP1 = games.NamePlayerOne;
            nameP2 = games.NamePlayerTwo;
            newGame = false;

            if (String.IsNullOrEmpty(games.RoundTwo) && !String.IsNullOrEmpty(games.RoundOne))
            {
                RoundCounter = 1;
                PointsP1 = games.PointsPlayerOne;
                PointsP2 = games.PointsPlayerTwo;
            }
            else if (String.IsNullOrEmpty(games.RoundThree) && !String.IsNullOrEmpty(games.RoundTwo))
            {
                RoundCounter = 2;
                PointsP1 = games.PointsPlayerOne;
                PointsP2 = games.PointsPlayerTwo;
            }
        }

        /// <summary>
        /// Returns the winner of the total game
        /// </summary>
        /// <returns></returns>
        public string CalcTotalWinner()
        {
            newGame = true;
            var games = (from g in App.connection.Table<GameHistory>()
                         select g).LastOrDefault();
            if (PointsP1 == PointsP2)
            {
                games.Winner = "No winner, tie!";
                App.connection.Update(games);
                return "No winner, tie!";
            }
            else if (PointsP1 > PointsP2)
            {
                games.Winner = nameP1;
                App.connection.Update(games);
                return nameP1;
            }
            else
            {
                games.Winner = nameP2;
                App.connection.Update(games);
                return nameP2;
            }
        }
        /// <summary>
        /// Creacte new game in the local SQLite database
        /// </summary>
        private void CreateGameInDB()
        {
            App.connection.Insert(new GameHistory { NamePlayerOne = nameP1, NamePlayerTwo = nameP2 });
        }
        /// <summary>
        /// Handles the gamelogic
        /// </summary>
        /// <param name="choiceP1"></param>
        /// <param name="choiceP2"></param>
        /// <returns></returns>
        public int Start(int choiceP1, int choiceP2)
        {
            if (newGame)
            {
                CreateGameInDB();
                newGame = false;
            }
            RoundCounter++;

            if (choiceP2 == 0)
            {
                choiceP2 = ComputerSelection();
                int winner = CalcWinner(choiceP1, choiceP2);
                SaveToDB(choiceP1, choiceP2);
                return winner;

            }
            else
            {
                int winner = CalcWinner(choiceP1, choiceP2);
                SaveToDB(choiceP1, choiceP2);
                return winner;
            }
        }
        /// <summary>
        /// Saves the played game
        /// </summary>
        private void SaveToDB(int choiceP1, int choiceP2)
        {
            var games = (from g in App.connection.Table<GameHistory>()
                         select g).Last();
            if (RoundCounter == 1)
            {
                games.RoundOne = assaignChoiceInText(choiceP1) + " | " + assaignChoiceInText(choiceP2);
                App.connection.Update(games);
            }
            else if (RoundCounter == 2)
            {
                games.RoundTwo = assaignChoiceInText(choiceP1) + " | " + assaignChoiceInText(choiceP2);
                App.connection.Update(games);
            }
            else if (RoundCounter == 3)
            {
                games.RoundThree = assaignChoiceInText(choiceP1) + " | " + assaignChoiceInText(choiceP2);
                games.PointsPlayerOne = PointsP1;
                games.PointsPlayerTwo = PointsP2;
                App.connection.Update(games);
            }
        }
        /// <summary>
        /// Translates the number to text to store in database
        /// </summary>
        /// <param name="choiceIn"></param>
        /// <returns></returns>
        private string assaignChoiceInText(int choiceIn)
        {
            if (choiceIn == 1)
            {
                return "Rock";
            }
            else if (choiceIn == 2)
            {
                return "Paper";
            }
            else
            {
                return "Scissors";
            }
        }
        /// <summary>
        /// Calculates the winner of a round
        /// </summary>
        private int CalcWinner(int choiceP1, int choiceP2)
        {
            var games = (from g in App.connection.Table<GameHistory>()
                         select g).Last();
            if (choiceP1 == choiceP2)
            {
                return 1;
            }
            else if (choiceP1 == 1 && choiceP2 == 3 || choiceP1 == 3 && choiceP2 == 2 || choiceP1 == 2
                && choiceP2 == 1)
            {
                PointsP1++;
                games.PointsPlayerOne = PointsP1;
                App.connection.Update(games);
                return 2;
            }
            else
            {
                PointsP2++;
                games.PointsPlayerTwo = PointsP2;
                App.connection.Update(games);
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
