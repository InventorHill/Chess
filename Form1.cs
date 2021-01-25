using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace Chess
{
    public partial class MainGame : Form
    {
        char moveCoordinate1, moveCoordinate2, moveCoordinate3, moveCoordinate4;
        string currentX, currentY, nextX, nextY;

        public string pieceToChangeWithPawn = "";
        public int pawnChange = 0;
        public string pawnChangePositionString = "";
        public int pawnChangePositionInt = 0;

        public int check = 0;
        public int checkIfLeft = 0;
        public int checkIfRight = 0;
        public int checkIfUp = 0;
        public int checkIfDown = 0;
        public int checkIfUpLeft = 0;
        public int checkIfUpRight = 0;
        public int checkIfDownRight = 0;
        public int checkIfDownLeft = 0;
        public int checkTotal = 0;

        public bool A2Check = false;
        public bool A7Check = false;

        public string rook = "";
        public int castle = 0;
        public int illegalAction = 0;
        public static bool weAreAlone = true;
        public static string name = "";
        public static string ipAddress = "";
        public static int portNumber = 0;
        public static string playerState = "";
        public static int moveAllowed = 0;
        public int moveInProgress = 0;
        public string moveCoordinates = "";
        public int testCorrect = 0;
        public string[] pieces;
        public string[] cur_Piece;
        public static Button[] buttonArray = null;
        public MainGame()
        {
            InitializeComponent();
        }

        private void C8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 38;
                test_move();
            }
            else
            {
                moveInProgress = 38;
            }
        }

        private void H1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 81;
                test_move();
            }
            else
            {
                moveInProgress = 81;
            }
        }

        private void H2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 82;
                test_move();
            }
            else
            {
                moveInProgress = 82;
            }
        }

        private void G2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 72;
                test_move();
            }
            else
            {
                moveInProgress = 72;
            }
        }

        private void G1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 71;
                test_move();
            }
            else
            {
                moveInProgress = 71;
            }
        }

        private void G4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 74;
                test_move();
            }
            else
            {
                moveInProgress = 74;
            }
        }

        private void G3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 73;
                test_move();
            }
            else
            {
                moveInProgress = 73;
            }
        }

        private void H4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 84;
                test_move();
            }
            else
            {
                moveInProgress = 84;
            }
        }

        private void H3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 83;
                test_move();
            }
            else
            {
                moveInProgress = 83;
            }
        }

        private void E4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 54;
                test_move();
            }
            else
            {
                moveInProgress = 54;
            }
        }

        private void E3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 53;
                test_move();
            }
            else
            {
                moveInProgress = 53;
            }
        }

        private void F4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 64;
                test_move();
            }
            else
            {
                moveInProgress = 64;
            }
        }

        private void F3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 63;
                test_move();
            }
            else
            {
                moveInProgress = 63;
            }
        }

        private void E2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 52;
                test_move();
            }
            else
            {
                moveInProgress = 52;
            }
        }

        private void E1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 51;
                test_move();
            }
            else
            {
                moveInProgress = 51;
            }
        }

        private void F2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 62;
                test_move();
            }
            else
            {
                moveInProgress = 62;
            }
        }

        private void F1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 61;
                test_move();
            }
            else
            {
                moveInProgress = 61;
            }
        }

        private void E8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 58;
                test_move();
            }
            else
            {
                moveInProgress = 58;
            }
        }

        private void E7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 57;
                test_move();
            }
            else
            {
                moveInProgress = 57;
            }
        }

        private void F8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 68;
                test_move();
            }
            else
            {
                moveInProgress = 68;
            }
        }

        private void F7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 67;
                test_move();
            }
            else
            {
                moveInProgress = 67;
            }
        }

        private void E6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 56;
                test_move();
            }
            else
            {
                moveInProgress = 56;
            }
        }

        private void E5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 55;
                test_move();
            }
            else
            {
                moveInProgress = 55;
            }
        }

        private void F6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 66;
                test_move();
            }
            else
            {
                moveInProgress = 66;
            }
        }

        private void F5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 65;
                test_move();
            }
            else
            {
                moveInProgress = 65;
            }
        }

        private void G8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 78;
                test_move();
            }
            else
            {
                moveInProgress = 78;
            }
        }

        private void G7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 77;
                test_move();
            }
            else
            {
                moveInProgress = 77;
            }
        }

        private void H8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 88;
                test_move();
            }
            else
            {
                moveInProgress = 88;
            }
        }

        private void H7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 87;
                test_move();
            }
            else
            {
                moveInProgress = 87;
            }
        }

        private void G6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 76;
                test_move();
            }
            else
            {
                moveInProgress = 76;
            }
        }

        private void G5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 75;
                test_move();
            }
            else
            {
                moveInProgress = 75;
            }
        }

        private void H6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 86;
                test_move();
            }
            else
            {
                moveInProgress = 86;
            }
        }

        private void H5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 85;
                test_move();
            }
            else
            {
                moveInProgress = 85;
            }
        }

        private void A4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 14;
                test_move();
            }
            else
            {
                moveInProgress = 14;
            }
        }

        private void A3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 13;
                test_move();
            }
            else
            {
                moveInProgress = 13;
            }
        }

        private void B5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 25;
                test_move();
            }
            else
            {
                moveInProgress = 25;
            }
        }

        private void B4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 24;
                test_move();
            }
            else
            {
                moveInProgress = 24;
            }
        }

        private void B3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 23;
                test_move();
            }
            else
            {
                moveInProgress = 23;
            }
        }

        private void A2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 12;
                test_move();
            }
            else
            {
                moveInProgress = 12;
            }
        }

        private void A1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 11;
                test_move();
            }
            else
            {
                moveInProgress = 11;
            }
        }

        private void B2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 22;
                test_move();
            }
            else
            {
                moveInProgress = 22;
            }
        }

        private void B1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 21;
                test_move();
            }
            else
            {
                moveInProgress = 21;
            }
        }

        private void C4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 34;
                test_move();
            }
            else
            {
                moveInProgress = 34;
            }
        }

        private void C3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 33;
                test_move();
            }
            else
            {
                moveInProgress = 33;
            }
        }

        private void D4_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 44;
                test_move();
            }
            else
            {
                moveInProgress = 44;
            }
        }

        private void D3_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 43;
                test_move();
            }
            else
            {
                moveInProgress = 43;
            }
        }

        private void C2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 32;
                test_move();
            }
            else
            {
                moveInProgress = 32;
            }
        }

        private void C1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 31;
                test_move();
            }
            else
            {
                moveInProgress = 31;
            }
        }

        private void D2_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 42;
                test_move();
            }
            else
            {
                moveInProgress = 42;
            }
        }

        private void D1_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 41;
                test_move();
            }
            else
            {
                moveInProgress = 41;
            }
        }

        private void A8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 18;
                test_move();
            }
            else
            {
                moveInProgress = 18;
            }
        }

        private void A7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 17;
                test_move();
            }
            else
            {
                moveInProgress = 17;
            }
        }

        private void B8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 28;
                test_move();
            }
            else
            {
                moveInProgress = 28;
            }
        }

        private void B7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 27;
                test_move();
            }
            else
            {
                moveInProgress = 27;
            }
        }

        private void A6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 16;
                test_move();
            }
            else
            {
                moveInProgress = 16;
            }
        }

        private void A5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 15;
                test_move();
            }
            else
            {
                moveInProgress = 15;
            }
        }

        private void B6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 26;
                test_move();
            }
            else
            {
                moveInProgress = 26;
            }
        }

        private void WK_Click(object sender, EventArgs e)
        {

        }

        private void C7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 37;
                test_move();
            }
            else
            {
                moveInProgress = 37;
            }
        }

        private void D8_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 48;
                test_move();
            }
            else
            {
                moveInProgress = 48;
            }
        }

        private void D7_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 47;
                test_move();
            }
            else
            {
                moveInProgress = 47;
            }
        }

        private void C6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 36;
                test_move();
            }
            else
            {
                moveInProgress = 36;
            }
        }

        private void C5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 35;
                test_move();
            }
            else
            {
                moveInProgress = 35;
            }
        }

        private void D6_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 46;
                test_move();
            }
            else
            {
                moveInProgress = 46;
            }
        }

        private void D5_Click(object sender, EventArgs e)
        {
            if (moveInProgress != 0)
            {
                moveInProgress *= 100;
                moveInProgress += 45;
                test_move();
            }
            else
            {
                moveInProgress = 45;
            }
        }

        private void BRR_Click(object sender, EventArgs e)
        {

        }

        private void BKR_Click(object sender, EventArgs e)
        {

        }

        private void BP8_Click(object sender, EventArgs e)
        {

        }

        private void BP7_Click(object sender, EventArgs e)
        {

        }

        private void BBR_Click(object sender, EventArgs e)
        {

        }

        private void BQ_Click(object sender, EventArgs e)
        {

        }

        private void BP6_Click(object sender, EventArgs e)
        {

        }

        private void BP5_Click(object sender, EventArgs e)
        {

        }

        private void BK_Click(object sender, EventArgs e)
        {

        }

        private void BBL_Click(object sender, EventArgs e)
        {

        }

        private void BP4_Click(object sender, EventArgs e)
        {

        }

        private void BP3_Click(object sender, EventArgs e)
        {

        }

        private void BKL_Click(object sender, EventArgs e)
        {

        }

        private void BRL_Click(object sender, EventArgs e)
        {

        }

        private void BP2_Click(object sender, EventArgs e)
        {

        }

        private void BP1_Click(object sender, EventArgs e)
        {

        }

        private void WP8_Click(object sender, EventArgs e)
        {

        }

        private void WP7_Click(object sender, EventArgs e)
        {

        }

        private void WRR_Click(object sender, EventArgs e)
        {

        }

        private void WKR_Click(object sender, EventArgs e)
        {

        }

        private void WP6_Click(object sender, EventArgs e)
        {

        }

        private void WP5_Click(object sender, EventArgs e)
        {

        }

        private void WBR_Click(object sender, EventArgs e)
        {

        }

        private void WP4_Click(object sender, EventArgs e)
        {

        }

        private void WP3_Click(object sender, EventArgs e)
        {

        }

        private void WQ_Click(object sender, EventArgs e)
        {

        }

        private void WBL_Click(object sender, EventArgs e)
        {

        }

        private void WP2_Click(object sender, EventArgs e)
        {

        }

        private void WP1_Click(object sender, EventArgs e)
        {

        }

        private void WKL_Click(object sender, EventArgs e)
        {

        }

        private void WRL_Click(object sender, EventArgs e)
        {

        }

        private void piece_button_Click(object sender, EventArgs e)
        {

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            moveInProgress = 0;
        }

        private void disconnect_button_Click(object sender, EventArgs e)
        {

        }

        private void chat_button_Click(object sender, EventArgs e)
        {

        }
        private int PawnCheck()
        {
            return 1;
        }

        private int RookCheck()
        {
            return 1;
        }

        private int KnightCheck()
        {
            return 1;
        }

        private int BishopCheck()
        {
            return 1;
        }

        private int QueenCheck()
        {
            return 1;
        }

        private int KingCheck()
        {
            return 1;
        }

        private int OthersUpDown()
        {
            return 1;
        }

        private int OthersLeftRight()
        {
            return 1;
        }

        private int OthersLeftUpDownRight()
        {
            return 1;
        }

        private int OthersRightUpDownLeft()
        {
            return 1;
        }

        private int TestMoveLegality()
        {
            if (moveAllowed == 1)
            {
                int legal = 0;
                string testingMove = Convert.ToString(moveInProgress);
                string testingMoveCur = Convert.ToString(testingMove[0] + testingMove[1]);
                string testingMoveNext = Convert.ToString(testingMove[2] + testingMove[3]);
                string[] next_Piece;

                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                string[] opponentPieces = File.ReadLines("Player2_Current.txt").ToArray();
                
                for(int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    if (cur_Piece[3] == testingMoveCur)
                        break;
                }

                for (int i = 0; i < 16; i++)
                {
                    next_Piece = pieces[i].Split(':');
                    if (next_Piece[3] == testingMoveNext)
                        break;
                }

                switch (cur_Piece[0])
                {
                    case "1Pa": legal = PawnCheck(); break;
                    case "2Pa": legal = PawnCheck(); break;
                    case "3Pa": legal = PawnCheck(); break;
                    case "4Pa": legal = PawnCheck(); break;
                    case "5Pa": legal = PawnCheck(); break;
                    case "6Pa": legal = PawnCheck(); break;
                    case "7Pa": legal = PawnCheck(); break;
                    case "8Pa": legal = PawnCheck(); break;
                    case "1Ro": legal = RookCheck(); break;
                    case "1Kn": legal = KnightCheck(); break;
                    case "1Bi": legal = BishopCheck(); break;
                    case "Kin": legal = KingCheck(); break;
                    case "Que": legal = QueenCheck(); break;
                    case "2Bi": legal = BishopCheck(); break;
                    case "2Kn": legal = KnightCheck(); break;
                    case "2Ro": legal = RookCheck(); break;
                }

                return legal;
            }
            else
                return 2;
        }

        private void test_move()
        {

            testCorrect = TestMoveLegality();
            //testCorrect = 0;

            if (testCorrect == 2)
            {
                MessageBox.Show("Illegal Action", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                illegalAction = 0;
                moveCoordinates = "";
                moveInProgress = 0;
                return;
            }
            else if (testCorrect == 0)
            {
                MessageBox.Show("Illegal Move", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                illegalAction = 0;
                moveCoordinates = "";
                moveInProgress = 0;
                return;
            }
            else
            {
                moveCoordinates = Convert.ToString(moveInProgress);
                moveCoordinate1 = moveCoordinates[0];
                moveCoordinate2 = moveCoordinates[1];
                moveCoordinate3 = moveCoordinates[2];
                moveCoordinate4 = moveCoordinates[3];

                currentX = Convert.ToString(moveCoordinate1);
                currentY = Convert.ToString(moveCoordinate2);
                nextX = Convert.ToString(moveCoordinate3);
                nextY = Convert.ToString(moveCoordinate4);

                switch (currentX)
                {
                    case "1": currentX = "A"; break;
                    case "2": currentX = "B"; break;
                    case "3": currentX = "C"; break;
                    case "4": currentX = "D"; break;
                    case "5": currentX = "E"; break;
                    case "6": currentX = "F"; break;
                    case "7": currentX = "G"; break;
                    case "8": currentX = "H"; break;
                    default: break;
                }

                switch (nextX)
                {
                    case "1": nextX = "A"; break;
                    case "2": nextX = "B"; break;
                    case "3": nextX = "C"; break;
                    case "4": nextX = "D"; break;
                    case "5": nextX = "E"; break;
                    case "6": nextX = "F"; break;
                    case "7": nextX = "G"; break;
                    case "8": nextX = "H"; break;
                    default: break;
                }

                string currentPlace = currentX + currentY;
                string nextPlace = nextX + nextY;
                if (currentPlace == nextPlace)
                {
                    MessageBox.Show("You may not move a piece to the same position on the board", "SystemError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    moveInProgress = 0;
                    return;
                }

                int cur_position = -1;
                int nex_position = -1;

                for (int i = 0; i < 102; i++)
                {
                    if (buttonArray[i].Name == currentPlace)
                    {
                        cur_position = i;
                    }
                    else if (buttonArray[i].Name == nextPlace)
                    {
                        nex_position = i;                    
                    }

                    if ((cur_position != -1) && (nex_position != -1))
                    {
                        break;
                    }
                }

                if ((((currentPlace == "A4") && (playerState == "black")) || ((currentPlace == "A5") && (playerState == "white"))) && ((nextPlace == "A1") || (nextPlace == "A8")) && (A1.Image != null) && (A8.Image != null) && (((playerState == "white") && (A5.Image != null)) || ((playerState == "black") && (A4.Image != null))))
                {
                    pieces = File.ReadLines("Player1_Current.txt").ToArray();
                    string[] otherPieces = File.ReadLines("Player2_Current.txt").ToArray();
                    string[] cur_Others;
                    string piecesPositions;

                    if (nextPlace == "A1")
                    {
                        int pieceInPlace = 0;
                        for (int i = 0; i < 16; i++)
                        {
                            cur_Piece = pieces[i].Split(':');
                            cur_Others = otherPieces[i].Split(':');
                            piecesPositions = cur_Piece[2] + cur_Others[2];

                            if (((cur_Piece[0] == "1Ro") && (cur_Piece[4] != "1")) || ((cur_Piece[0] == "Kin") && (cur_Piece[4] != "1")))
                                pieceInPlace++;
                            else if (((cur_Piece[0] == "1Ro") && (cur_Piece[4] == "1")) || ((cur_Piece[0] == "Kin") && (cur_Piece[4] == "1")))
                            {
                                pieceInPlace = 0;
                                illegalAction = 1;
                                MessageBox.Show("Either your Rook or your King has been moved, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                moveInProgress = 0;
                                return;
                            }
                            else if ((((piecesPositions[0] == 'A') && (piecesPositions[1] < currentPlace[1])) || ((piecesPositions[2] == 'A') && (piecesPositions[3] < currentPlace[1]))) && ((cur_Piece[0] != "1Ro") && (cur_Piece[0] != "Kin")))
                            {
                                pieceInPlace = 0;
                                illegalAction = 1;
                                MessageBox.Show("Other pieces are between your Rook and your King, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                moveInProgress = 0;
                                return;
                            }
                        }
                        if (pieceInPlace == 2)
                        {
                            StartOfCastle("left");
                            moveInProgress = 0;
                            SendMoveToOtherPlayer();
                            return;
                        }
                    }
                    else if (nextPlace == "A8")
                    {
                        int pieceInPlace = 0;
                        for (int i = 0; i < 16; i++)
                        {
                            cur_Piece = pieces[i].Split(':');
                            cur_Others = otherPieces[i].Split(':');
                            piecesPositions = cur_Piece[2] + cur_Others[2];

                            if (((cur_Piece[0] == "2Ro") && (cur_Piece[4] != "1")) || ((cur_Piece[0] == "Kin") && (cur_Piece[4] != "1")))
                                pieceInPlace++;
                            else if (((cur_Piece[0] == "2Ro") && (cur_Piece[4] == "1")) || ((cur_Piece[0] == "Kin") && (cur_Piece[4] == "1")))
                            {
                                pieceInPlace = 0;
                                illegalAction = 1;
                                MessageBox.Show("Either your Rook or your King has been moved, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                moveInProgress = 0;
                                return;
                            }
                            else if ((((piecesPositions[0] == 'A') && (piecesPositions[1] > currentPlace[1])) || ((piecesPositions[2] == 'A') && (piecesPositions[3] > currentPlace[1]))) && ((cur_Piece[0] != "2Ro") && (cur_Piece[0] != "Kin")))
                            {
                                pieceInPlace = 0;
                                illegalAction = 1;
                                MessageBox.Show("Other pieces are between your Rook and your King, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                moveInProgress = 0;
                                return;
                            }
                        }
                        if (pieceInPlace == 2)
                        {
                            StartOfCastle("right");
                            moveInProgress = 0;
                            SendMoveToOtherPlayer();
                            return;
                        }
                    }
                }

                pieces = File.ReadLines("Player2_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    if (cur_Piece[2] == buttonArray[cur_position].Name)
                    {
                        MessageBox.Show("Illegal move. You may not move an opponent's piece", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        illegalAction = 1;
                        break;
                    }
                }

                if (buttonArray[cur_position].Image == null)
                {
                    MessageBox.Show("Illegal move. You may not move a blank square", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    illegalAction = 1;
                    cur_position = nex_position;
                }

                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    if ((cur_Piece[2] == buttonArray[nex_position].Name) && (illegalAction == 0))
                    {
                        MessageBox.Show("Illegal move. You may not capture one of your own pieces", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        illegalAction = 1;
                        break;
                    }
                }

                if (illegalAction != 0)
                {
                    illegalAction = 0;
                    moveInProgress = 0;
                    return;
                }

                transitionBox.Image = buttonArray[nex_position].Image;
                buttonArray[nex_position].Image = buttonArray[cur_position].Image;
                buttonArray[nex_position].ImageAlign = ContentAlignment.MiddleCenter;
                buttonArray[nex_position].FlatStyle = FlatStyle.Flat;

                if (transitionBox.Image == null)
                {
                    buttonArray[cur_position].Image = transitionBox.Image;
                    buttonArray[cur_position].ImageAlign = ContentAlignment.MiddleCenter;
                    buttonArray[cur_position].FlatStyle = FlatStyle.Flat;
                }
                else
                {
                    pieces = File.ReadLines("Player2_Current.txt").ToArray();

                    for (int i = 0; i < 16; i++)
                    {
                        cur_Piece = pieces[i].Split(':');
                        if (buttonArray[nex_position].Name == cur_Piece[2])
                        {
                            switch (cur_Piece[0])
                            {
                                case "1Pa": BP1.Image = transitionBox.Image; break;
                                case "2Pa": BP2.Image = transitionBox.Image; break;
                                case "3Pa": BP3.Image = transitionBox.Image; break;
                                case "4Pa": BP4.Image = transitionBox.Image; break;
                                case "5Pa": BP5.Image = transitionBox.Image; break;
                                case "6Pa": BP6.Image = transitionBox.Image; break;
                                case "7Pa": BP7.Image = transitionBox.Image; break;
                                case "8Pa": BP8.Image = transitionBox.Image; break;
                                case "1Ro": BRL.Image = transitionBox.Image; break;
                                case "1Kn": BKL.Image = transitionBox.Image; break;
                                case "1Bi": BBL.Image = transitionBox.Image; break;
                                case "Que": BK.Image = transitionBox.Image; break;
                                case "Kin": BQ.Image = transitionBox.Image; break;
                                case "2Bi": BBR.Image = transitionBox.Image; break;
                                case "2Kn": BKR.Image = transitionBox.Image; break;
                                case "2Ro": BRR.Image = transitionBox.Image; break;
                            }

                            cur_Piece[1] = "A";
                            cur_Piece[2] = "I9";
                            cur_Piece[3] = "99";
                            cur_Piece[4] = "1";
                            pieces[i] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4];

                            FileStream aFile = new FileStream("Player2_Current.txt", FileMode.Create, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(aFile);

                            foreach (string element in pieces)
                            {
                                sw.WriteLine(element);
                            }
                            sw.Close();
                            aFile.Close();

                            break;
                        }
                    }

                    transitionBox.Image = null;
                    buttonArray[cur_position].Image = transitionBox.Image;
                    buttonArray[cur_position].ImageAlign = ContentAlignment.MiddleCenter;
                    buttonArray[cur_position].FlatStyle = FlatStyle.Flat;
                }

                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    if (cur_Piece[2] == buttonArray[cur_position].Name)
                    {
                        cur_Piece[1] = "A";
                        cur_Piece[2] = buttonArray[nex_position].Name;
                        cur_Piece[3] = nextPlace;
                        cur_Piece[4] = "1";
                        pieces[i] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4];

                        FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(aFile);

                        foreach (string element in pieces)
                        {
                            sw.WriteLine(element);
                        }
                        sw.Close();
                        aFile.Close();

                        break;
                    }
                }

                SendMoveToOtherPlayer();
            }
        }

        private void SendMoveToOtherPlayer()
        {
            moveAllowed = 0;
            moveInProgress = 0;
        }

        private void StartOfCastle(string direction)
        {
            if (direction == "left")
            {
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                if ((check == 0) && (checkIfLeft == 0))
                {
                    if (playerState == "white")
                    {
                        transitionBox.Image = A4.Image;
                        A4.Image = A1.Image;
                        A1.Image = transitionBox.Image;
                        A3.Image = A5.Image;
                        A5.Image = transitionBox.Image;
                        int pos = Array.IndexOf(pieces, "Kin:A:A5:15:0");
                        pieces[pos] = "Kin:A:A3:13:1";
                        pos = Array.IndexOf(pieces, "1Ro:A:A1:11:0");
                        pieces[pos] = "1Ro:A:A4:14:1";

                        FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(aFile);

                        foreach (string element in pieces)
                            sw.WriteLine(element);

                        sw.Close();
                        aFile.Close();
                    }
                    else
                    {
                        if (A2Check == false)
                        {
                            transitionBox.Image = A3.Image;
                            A3.Image = A1.Image;
                            A1.Image = transitionBox.Image;
                            A2.Image = A4.Image;
                            A4.Image = transitionBox.Image;
                            int pos = Array.IndexOf(pieces, "Kin:A:A4:14:0");
                            pieces[pos] = "Kin:A:A2:12:1";
                            pos = Array.IndexOf(pieces, "1Ro:A:A1:11:0");
                            pieces[pos] = "1Ro:A:A3:13:1";

                            FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(aFile);

                            foreach (string element in pieces)
                                sw.WriteLine(element);

                            sw.Close();
                            aFile.Close();
                        }
                        else
                        {
                            MessageBox.Show("Your King will land on a space under attack, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if ((check == 0) && (checkIfLeft == 1))
                {
                    MessageBox.Show("Your King will move through a space under attack, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Your King is currently under attack, so castling is impossible", "Castling error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                moveInProgress = 0;
                return;
            }
            else
            {
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                if ((check == 0) && (checkIfRight == 0))
                {
                    if (playerState == "black")
                    {
                        transitionBox.Image = A5.Image;
                        A5.Image = A8.Image;
                        A8.Image = transitionBox.Image;
                        A6.Image = A4.Image;
                        A4.Image = transitionBox.Image;
                        int pos = Array.IndexOf(pieces, "Kin:A:A4:14:0");
                        pieces[pos] = "Kin:A:A6:16:1";
                        pos = Array.IndexOf(pieces, "2Ro:A:A8:18:0");
                        pieces[pos] = "2Ro:A:A5:15:1";

                        FileStream aFile;
                        StreamWriter sw;

                        aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                        sw = new StreamWriter(aFile);

                        foreach (string element in pieces)
                            sw.WriteLine(element);

                        sw.Close();
                        aFile.Close();
                    }
                    else
                    {
                        if (A7Check == false)
                        {
                            transitionBox.Image = A6.Image;
                            A6.Image = A8.Image;
                            A8.Image = transitionBox.Image;
                            A7.Image = A5.Image;
                            A5.Image = transitionBox.Image;
                            int pos = Array.IndexOf(pieces, "Kin:A:A5:15:0");
                            pieces[pos] = "Kin:A:A7:17:1";
                            pos = Array.IndexOf(pieces, "2Ro:A:A8:18:0");
                            pieces[pos] = "2Ro:A:A6:16:1";

                            FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(aFile);

                            foreach (string element in pieces)
                                sw.WriteLine(element);

                            sw.Close();
                            aFile.Close();
                        }
                        else
                        {
                            MessageBox.Show("Your King will land on a space under attack, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if ((check == 0) && (checkIfRight == 1))
                {
                    MessageBox.Show("Your King will move through a space under attack, so castling is impossible", "Castling Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Your King is currently under attack, so castling is impossible", "Castling error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    string pieceName = cur_Piece[0];
                    string pieceLocation = cur_Piece[2];

                    pieceName = Convert.ToString(pieceName[1] + pieceName[2]);

                    if ((pieceName == "Pa") && (pieceLocation[0] == 'H'))
                    {
                        pawnChangePositionString = Convert.ToString(pieceLocation[0] + pieceLocation[1]);
                        pawnChangePositionInt = Int32.Parse(cur_Piece[3]);
                        MessageBox.Show("Please select a piece with which to change your pawn");
                        moveInProgress = 0;
                        return;
                    }
                }
            }
        }

        private void MainGame_Load(object sender, EventArgs e)
        {
            buttonArray = this.Controls.OfType<Button>().ToArray();
        }

        public static void SetUpBoard(string playerState)
        {
            //if ipAddress == 0, set up server
            //if ipAddress != 0, set up client

            if (playerState == "white")
            {
                string data = Chess.Properties.Resources.White_Origin;
                string[] Origins = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                FileStream aFile;
                StreamWriter sw;

                string pl1Cur = @"Player1_Current.txt";
                string pl2Cur = @"Player2_Current.txt";
                aFile = new FileStream(pl1Cur, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(aFile);

                for (int i = 0; i < 16; i++)
                {
                    sw.WriteLine(Origins[i]);
                }

                sw.Close();
                aFile.Close();

                FileStream bFile = new FileStream(pl2Cur, FileMode.Create, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(bFile);

                for (int i = 16; i < 32; i++)
                {
                    sw1.WriteLine(Origins[i]);
                }

                sw1.Close();
                bFile.Close();

                char letterCoordinate = 'A';
                int numberCoordinate = 1;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 8)
                    {
                        letterCoordinate++;
                        numberCoordinate = 1;
                    }

                    string letrCoordinate = Convert.ToString(letterCoordinate);
                    string buttonCoordinates = Convert.ToString(letrCoordinate + numberCoordinate);

                    for (int j = 0; j < 100; j++)
                    {
                        if (buttonArray[j].Name == buttonCoordinates)
                        {
                            if ((buttonCoordinates == "A1") || (buttonCoordinates == "A8"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_rook_white_small;
                            }
                            else if ((buttonCoordinates == "A2") || (buttonCoordinates == "A7"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_knight_white_small;
                            }
                            else if ((buttonCoordinates == "A3") || (buttonCoordinates == "A6"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_bishop_white_small;
                            }
                            else if (buttonCoordinates == "A4")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_queen_white_small;
                            }
                            else if (buttonCoordinates == "A5")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_king_white_small;
                            }
                            else
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_pawn_white_small;
                            }

                            buttonArray[j].ImageAlign = ContentAlignment.MiddleCenter;
                            buttonArray[j].FlatStyle = FlatStyle.Flat;
                        }
                    }

                    numberCoordinate++;
                }

                letterCoordinate = 'H';
                numberCoordinate = 1;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 8)
                    {
                        letterCoordinate--;
                        numberCoordinate = 1;
                    }

                    string letrCoordinate = Convert.ToString(letterCoordinate);
                    string buttonCoordinates = Convert.ToString(letrCoordinate + numberCoordinate);

                    for (int j = 0; j < 100; j++)
                    {
                        if (buttonArray[j].Name == buttonCoordinates)
                        {
                            if ((buttonCoordinates == "H1") || (buttonCoordinates == "H8"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_rook_black_small;
                            }
                            else if ((buttonCoordinates == "H2") || (buttonCoordinates == "H7"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_knight_black_small;
                            }
                            else if ((buttonCoordinates == "H3") || (buttonCoordinates == "H6"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_bishop_black_small;
                            }
                            else if (buttonCoordinates == "H4")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_queen_black_small;
                            }
                            else if (buttonCoordinates == "H5")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_king_black_small;
                            }
                            else
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_pawn_black_small;
                            }

                            buttonArray[j].ImageAlign = ContentAlignment.MiddleCenter;
                            buttonArray[j].FlatStyle = FlatStyle.Flat;
                        }
                    }

                    numberCoordinate++;
                }
            }

            else
            {
                string data = Chess.Properties.Resources.Black_Origin;
                string[] Origins = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                FileStream aFile;
                StreamWriter sw;

                string pl1Cur = @"Player1_Current.txt";
                string pl2Cur = @"Player2_Current.txt";
                aFile = new FileStream(pl1Cur, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(aFile);

                for (int i = 0; i < 16; i++)
                {
                    sw.WriteLine(Origins[i]);
                }

                sw.Close();
                aFile.Close();

                FileStream bFile = new FileStream(pl2Cur, FileMode.Create, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(bFile);

                for (int i = 16; i < 32; i++)
                {
                    sw1.WriteLine(Origins[i]);
                }

                sw1.Close();
                bFile.Close();

                char letterCoordinate = 'A';
                int numberCoordinate = 1;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 8)
                    {
                        letterCoordinate++;
                        numberCoordinate = 1;
                    }

                    string letrCoordinate = Convert.ToString(letterCoordinate);
                    string buttonCoordinates = Convert.ToString(letrCoordinate + numberCoordinate);

                    for (int j = 0; j < 100; j++)
                    {
                        if (buttonArray[j].Name == buttonCoordinates)
                        {
                            if ((buttonCoordinates == "A1") || (buttonCoordinates == "A8"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_rook_black_small;
                            }
                            else if ((buttonCoordinates == "A2") || (buttonCoordinates == "A7"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_knight_black_small;
                            }
                            else if ((buttonCoordinates == "A3") || (buttonCoordinates == "A6"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_bishop_black_small;
                            }
                            else if (buttonCoordinates == "A5")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_queen_black_small;
                            }
                            else if (buttonCoordinates == "A4")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_king_black_small;
                            }
                            else
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_pawn_black_small;
                            }

                            buttonArray[j].ImageAlign = ContentAlignment.MiddleCenter;
                            buttonArray[j].FlatStyle = FlatStyle.Flat;
                        }
                    }

                    numberCoordinate++;
                }

                letterCoordinate = 'H';
                numberCoordinate = 1;

                for (int i = 0; i < 16; i++)
                {
                    if (i == 8)
                    {
                        letterCoordinate--;
                        numberCoordinate = 1;
                    }

                    string letrCoordinate = Convert.ToString(letterCoordinate);
                    string buttonCoordinates = Convert.ToString(letrCoordinate + numberCoordinate);

                    for (int j = 0; j < 100; j++)
                    {
                        if (buttonArray[j].Name == buttonCoordinates)
                        {
                            if ((buttonCoordinates == "H1") || (buttonCoordinates == "H8"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_rook_white_small;
                            }
                            else if ((buttonCoordinates == "H2") || (buttonCoordinates == "H7"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_knight_white_small;
                            }
                            else if ((buttonCoordinates == "H3") || (buttonCoordinates == "H6"))
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_bishop_white_small;
                            }
                            else if (buttonCoordinates == "H5")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_queen_white_small;
                            }
                            else if (buttonCoordinates == "H4")
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_king_white_small;
                            }
                            else
                            {
                                buttonArray[j].Image = Chess.Properties.Resources.chess_pawn_white_small;
                            }

                            buttonArray[j].ImageAlign = ContentAlignment.MiddleCenter;
                            buttonArray[j].FlatStyle = FlatStyle.Flat;
                        }
                    }

                    numberCoordinate++;
                }
            }
        }
    }
}
