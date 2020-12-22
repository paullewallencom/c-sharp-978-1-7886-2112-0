using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeCSharp
{
    public partial class frmMain : Form
    {
        PictureBox[] allCells;

        enum PlayerTurn {  None, Player1, Player2};
        enum Winner {  None, Player1, Player2, Draw};

        PlayerTurn turn;
        Winner     winner;
        

        /*
           +---+---+---+
           | 0 | 1 | 2 |
           +---+---+---+
           | 3 | 4 | 5 |
           +---+---+---+
           | 6 | 7 | 8 |
           +---+---+---+
        */
        Winner GetWinner()
        {
            PictureBox [] allWinningMoves =  {
                                               //Check rows
                                               pictureBox0, pictureBox1, pictureBox2,
                                               pictureBox3, pictureBox4, pictureBox5,
                                               pictureBox6, pictureBox7, pictureBox8,
                                               //check Columns
                                               pictureBox0, pictureBox3, pictureBox6,
                                               pictureBox1, pictureBox4, pictureBox7,
                                               pictureBox2, pictureBox5, pictureBox8,
                                               //Diagonal
                                               pictureBox0, pictureBox4, pictureBox8,
                                               pictureBox2, pictureBox4, pictureBox6,
                                              };

            for (int i = 0; i < allWinningMoves.Length; i += 3)
            {
                //Check for winner
                if (allWinningMoves[i].Image != null)
                {
                    if (allWinningMoves[i].Image == allWinningMoves[i + 1].Image && allWinningMoves[i].Image == allWinningMoves[i + 2].Image)
                    {
                        //We have a winner
                        if (allWinningMoves[i].Image == player1.Image)
                            return Winner.Player1;
                        else
                            return Winner.Player2;
                    }
                }
            }
            //Now check if we have a draw or continue to play
            for (int i = 0; i < allCells.Length; ++i)
                if (allCells[i].Image == null)
                    return Winner.None;
            
            //No empty cells, so it is a draw!
            return Winner.Draw;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;

            if (pic.Image != null)
                return;
            if (turn == PlayerTurn.None)
                return;

            //Make move
            if (turn == PlayerTurn.Player1)
                pic.Image = player1.Image;
            else
                pic.Image = player2.Image;

            //Check winner
            winner = GetWinner();
            if (winner == Winner.None)
                turn = (turn == PlayerTurn.Player1) ? PlayerTurn.Player2 : PlayerTurn.Player1;
            else
                turn = PlayerTurn.None;
            
            ShowStatus();
        }

        void OnNewGame()
        {
            foreach (var p in allCells)
            {
                p.Image = null;
            }
            turn = PlayerTurn.Player1;
            winner = Winner.None;
            ShowStatus();
        }

        void ShowStatus()
        {
            string status="";
            string msg = "";

            switch (winner)
            {
                case Winner.None:
                    if (turn == PlayerTurn.Player1)
                        status = "Turn: Player1";
                    else if (turn == PlayerTurn.Player2)
                        status = "Turn: Player2";
                    break;
                case Winner.Player1:
                    msg = status = "Player 1 Wins!";
                    break;
                case Winner.Player2:
                    msg = status = "Player 2 Wins!";
                    break;
                case Winner.Draw:
                    msg = status = "It's a draw!";
                    break;
            }

            lblStatus.Text = status;
            if (msg != "")
                MessageBox.Show(msg);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
             allCells = new PictureBox[] { pictureBox0,
                                           pictureBox1,
                                           pictureBox2,
                                           pictureBox3,
                                           pictureBox4,
                                           pictureBox5,
                                           pictureBox6,
                                           pictureBox7,
                                           pictureBox8 };
            OnNewGame();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to start a new game?", "New Game",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

            // If the yes button was pressed ...
            if (result == DialogResult.Yes)
            {
                OnNewGame();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure that you would like to exit the game?", 
                                         "Exit Game",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
        }
    }
}
