using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    class GameHandler
    {
        //-----------------Varibels--------------------
        private string nameP1;
        private string nameP2;

        /// <summary>
        /// Constructor for gamehandler
        /// </summary>
        /// <param name="nameInP1"></param>
        /// <param name="nameInP2"></param>
        public GameHandler(string nameInP1, string nameInP2)
        {
            nameP1 = nameInP1;
            nameP2 = nameInP2;
        }
    }
}
