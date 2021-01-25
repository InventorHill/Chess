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
using System.Net.Sockets;
using System.Net;

namespace Chess
{
    public partial class MainGame : Form
    {
        private int checkingIfInCheck = 0;

        public static int serverState = 2;

//        private const int portNum = 0;
        delegate void SetTextCallback(string text);
        TcpListener listener;
        TcpClient client;
        NetworkStream ns;
        Thread t = null;
//        private const string hostName = "";


        public static int gameStart = 0;

        public static int pawnChange = 0;
        public static string KingPosition = "";

        int enPassant = 0;
        int enPassantImpossible = 0;

        int oppCurX, oppNextX;

        char moveCoordinate1, moveCoordinate2, moveCoordinate3, moveCoordinate4;
        string currentX, currentY, nextX, nextY;

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
        public static string changePieceAllowed = "";
        public int moveInProgress = 0;
        public string moveCoordinates = "";
        public int testCorrect = 0;
        public static string[] pieces;
        public static string[] cur_Piece;
        public static Button[] buttonArray = null;
        string[] oppPieces;
        string opponentName = "";
        public MainGame()
        {
            InitializeComponent();

            buttonArray = this.Controls.OfType<Button>().ToArray();

            if (serverState == 0)
            {
                client = new TcpClient(ipAddress, portNumber);
                ns = client.GetStream();
                SendData("cct~");
                t = new Thread(DoWork);
                t.Start();
                SendData("nme~" + name + "~" + playerState);
            }
            else if (serverState == 1)
            {
                listener = new TcpListener(IPAddress.Any, portNumber);
                listener.Start();
                client = listener.AcceptTcpClient();
                ns = client.GetStream();
                t = new Thread(DoWork);
                t.Start();
            }
            else
                MessageBox.Show("You can only see this if something has gone terribly wrong");
        }

        private void C8_Click(object sender, EventArgs e)
        {
            if (pawnChange == 1)
                return_Piece("C8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("E5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("F5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("G5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("H5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D4");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D3");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D2");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D1");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("A5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("B6");
            else if (moveInProgress != 0)
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

        private void C7_Click(object sender, EventArgs e)
        {
            if (pawnChange == 1)
                return_Piece("C7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D8");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D7");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("C5");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D6");
            else if (moveInProgress != 0)
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
            if (pawnChange == 1)
                return_Piece("D5");
            else if (moveInProgress != 0)
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

        private void WP8_Click(object sender, EventArgs e)
        {
            return_Piece("8Pa");
        }

        private void WP7_Click(object sender, EventArgs e)
        {
            return_Piece("7Pa");
        }

        private void WRR_Click(object sender, EventArgs e)
        {
            return_Piece("2Ro");
        }

        private void WKR_Click(object sender, EventArgs e)
        {
            return_Piece("2Kn");
        }

        private void WP6_Click(object sender, EventArgs e)
        {
            return_Piece("6Pa");
        }

        private void WP5_Click(object sender, EventArgs e)
        {
            return_Piece("5Pa");
        }

        private void WBR_Click(object sender, EventArgs e)
        {
            return_Piece("2Bi");
        }

        private void WP4_Click(object sender, EventArgs e)
        {
            return_Piece("4Pa");
        }

        private void WP3_Click(object sender, EventArgs e)
        {
            return_Piece("3Pa");
        }

        private void WQ_Click(object sender, EventArgs e)
        {
            return_Piece("Que");
        }

        private void WBL_Click(object sender, EventArgs e)
        {
            return_Piece("1Bi");
        }

        private void WP2_Click(object sender, EventArgs e)
        {
            return_Piece("2Pa");
        }

        private void WP1_Click(object sender, EventArgs e)
        {
            return_Piece("1Pa");
        }

        private void WKL_Click(object sender, EventArgs e)
        {
            return_Piece("1Kn");
        }

        private void WRL_Click(object sender, EventArgs e)
        {
            return_Piece("1Ro");
        }

        private void BK_Click(object sender, EventArgs e)
        {
            return_Piece("Kin");
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            moveInProgress = 0;
        }

        private void disconnect_button_Click(object sender, EventArgs e)
        {
            if (gameStart == 0)
                return;

            DialogResult result = MessageBox.Show("Are you sure you want to disconnect? The game cannot be started again from this current position.", "Disconnect Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (Button element in buttonArray)
                {
                    element.Enabled = false;
                }
                SendData("dct~");
            }
            return;
        }

        private void chat_button_Click(object sender, EventArgs e)
        {
            if (gameStart == 0)
                return;

            ChatForm chat = new ChatForm();
            chat.Show();
        }

        private void return_Piece(string pieceToReturn)
        {
            if (gameStart == 0)
                return;

            if (pawnChange == 1)
            {
                pieces = null;
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                string pawnPosition1 = "";
                string pawnPosition2 = "";
                int forPoint = 0;
                int errorCheck = 1;

                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    string r = cur_Piece[3];
                    string st = cur_Piece[0];
                    st = Convert.ToString(st[1]) + Convert.ToString(st[2]);
                    if ((r[0] == '8') && (st == "Pa"))
                    {
                        pawnPosition1 = cur_Piece[2];
                        pawnPosition2 = cur_Piece[3];

                        string s = cur_Piece[0];
                        s = Convert.ToString(s[0]);
                        cur_Piece[2] = "WP" + s;
                        cur_Piece[3] = "99";

                        pieces[i] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];
                    }
                    else if ((cur_Piece[0] == pieceToReturn) || (cur_Piece[2] == pieceToReturn))
                    {
                        forPoint = i;
                        errorCheck = 0;
                    }
                }

                if (errorCheck == 0)
                {
                    cur_Piece = pieces[forPoint].Split(':');
                    cur_Piece[2] = pawnPosition1;
                    cur_Piece[3] = pawnPosition2;

                    pieces[forPoint] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                    FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(aFile);

                    foreach (string element in pieces)
                    {
                        sw.WriteLine(element);
                    }

                    sw.Close();
                    aFile.Close();

                    pawnChange = 0;

                    SetUpBoard(playerState, false);
                }
                else
                {
                    MessageBox.Show("That is not an available piece", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pawn Change Not Allowed", "Illegal Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                moveInProgress = 0;
            }
        }


        private int PawnCheck()
        {
            int legal = 0;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));
            string moveNextCoordinates = Convert.ToString((moveNextX * 10) + moveNextY);

            string[] opp_Pieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = null;
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            for (int i = 0; i < 16; i++)
            {
                cur_Piece = pieces[i].Split(':');
                if (cur_Piece[3] == Convert.ToString((moveCurX * 10) + moveCurY))
                    break;
            }

            string s = cur_Piece[3];
            if (((Int32.Parse(Convert.ToString(s[0])) + 3) > moveNextX) && (cur_Piece[4] == "0") && (moveNextX > moveCurX) && (moveCurY == moveNextY))
                legal = 1;
            else if (((Int32.Parse(Convert.ToString(s[0])) + 2) > moveNextX) && (moveNextX > moveCurX) && (moveCurY == moveNextY))
                legal = 1;

            if ((moveNextX == (moveCurX + 1)) && ((moveNextY == (moveCurY + 1)) || (moveNextY == (moveCurY - 1))))
            {
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = opp_Pieces[i].Split(':');
                    if (cur_Piece[3] == moveNextCoordinates)
                        legal = 1;
                    if ((legal == 0) && (oppNextX == (oppCurX - 2)) && (s[0] == '5') && (enPassantImpossible == 0) && ((cur_Piece[0] == "1Pa") || (cur_Piece[0] == "2Pa")))
                    {
                        enPassant = 1;
                        legal = 1;
                    }
                    if (cur_Piece[3] == moveNextCoordinates)
                        enPassantImpossible = 1;
                }
            }

            string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

            if (legal == 1)
            {
                for (int i = 0; i < 32; i++)
                {
                    cur_Piece = bothTeamsPieces[i].Split(':');
                    if ((Int32.Parse(cur_Piece[3]) == Int32.Parse(moveNextCoordinates)) || (Int32.Parse(cur_Piece[3]) == (Int32.Parse(moveNextCoordinates) - 1)))
                    {
                        legal = 0;
                    }
                }
            }

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("ours");
            if (check == 1)
                legal = 0;

            return legal;
        }

        private int RookCheck()
        {
            int legal = 0;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));

            if ((moveCurX == moveNextX) || (moveCurY == moveNextY))
            {
                if (moveCurY == moveNextY)
                {
                    if (moveNextX > moveCurX)
                        legal = OthersStraight("up");
                    else
                        legal = OthersStraight("down");
                }
                else
                {
                    if (moveNextY > moveCurY)
                        legal = OthersStraight("right");
                    else
                        legal = OthersStraight("left");
                }
            }

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("ours");
            if (check == 1)
                legal = 0;

            return legal;
        }

        private int KnightCheck()
        {
            string moveProgress = Convert.ToString(moveInProgress);
            int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));

            int xp2 = moveCurX + 2;
            int xm2 = moveCurX - 2;
            int xp1 = moveCurX + 1;
            int xm1 = moveCurX - 1;

            int yp2 = moveCurY + 2;
            int ym2 = moveCurY - 2;
            int yp1 = moveCurY + 1;
            int ym1 = moveCurY - 1;

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("ours");

            if ((((moveNextX == xp2) && (moveNextY == yp1)) || ((moveNextX == xp1) && (moveNextY == yp2)) || ((moveNextX == xm1) && (moveNextY == yp2)) || ((moveNextX == xm2) && (moveNextY == yp1)) || ((moveNextX == xm2) && (moveNextY == ym1)) || ((moveNextX == xm1) && (moveNextY == ym2)) || ((moveNextX == xp1) && (moveNextY == ym2)) || ((moveNextX == xp2) && (moveNextY == ym1))) && (check == 0))
                return 1;
            else
                return 0;
        }

        private int BishopCheck()
        {
            int legal = 0;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));
            int DiffX = moveCurX - moveNextX;
            int DiffY = moveCurY - moveNextY;

            if (Math.Abs(DiffX) == Math.Abs(DiffY))
            {
                if ((Math.Sign(DiffX) == -1) && (Math.Sign(DiffY) == -1))
                {
                    legal = OthersDiagonal("upright");
                }
                else if ((Math.Sign(DiffX) == 1) && (Math.Sign(DiffY) == 1))
                {
                    legal = OthersDiagonal("downleft");
                }
                else if ((Math.Sign(DiffX) == -1) && (Math.Sign(DiffY) == 1))
                {
                    legal = OthersDiagonal("upleft");
                }
                else if ((Math.Sign(DiffX) == 1) && (Math.Sign(DiffY) == -1))
                {
                    legal = OthersDiagonal("downright");
                }
            }

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("ours");
            if (check == 1)
                legal = 0;

            return legal;
        }

        private int QueenCheck()
        {
            int legal = 0;
            if ((BishopCheck() == 1) || (RookCheck() == 1))
                legal = 1;

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("ours");
            if (check == 1)
                legal = 0;
            return legal;
        }

        private int KingCheck()
        {
            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string unusedString = CheckIfInCheck("king");
            int legal = 0;
            checkTotal = checkIfUpLeft + checkIfUp + checkIfUpRight + checkIfRight + checkIfDownRight + checkIfDown + checkIfDownLeft + checkIfLeft;
            string moveProgress = Convert.ToString(moveInProgress);
            int curX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int curY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int nexX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int nexY = Int32.Parse(Convert.ToString(moveProgress[3]));

            if (((nexX == 1) && (nexY == 1)) || ((nexX == 1) && (nexY == 8)))
                legal = 1;

            if ((checkTotal == 0) && (legal == 0))
            {
                if (((nexX <= (curX + 1)) && (nexX >= (curX - 1)) && (nexY <= (curY + 1)) && (nexY >= (curY - 1))))
                    legal = 1;
            }
            else if ((checkTotal >= 1) && (legal == 0))
            {
                if (((nexX <= (curX + 1)) && (nexX >= (curX - 1)) && (nexY <= (curY + 1)) && (nexY >= (curY - 1))))
                    legal = 1;

                if ((checkIfUpLeft == 1) && (nexX == (curX + 1)) && (nexY == (curY - 1)))
                    legal = 0;
                else if ((checkIfUp == 1) && (nexX == (curX + 1)))
                    legal = 0;
                else if ((checkIfUpRight == 1) && (nexX == (curX + 1)) && (nexY == (curY + 1)))
                    legal = 0;
                else if ((checkIfRight == 1) && (nexY == (curY + 1)))
                    legal = 0;
                else if ((checkIfDownRight == 1) && (nexX == (curX - 1)) && (nexY == (curY + 1)))
                    legal = 0;
                else if ((checkIfDown == 1) && (nexX == (curX - 1)))
                    legal = 0;
                else if ((checkIfDownLeft == 1) && (nexX == (curX - 1)) && (nexY == (curY - 1)))
                    legal = 0;
                else if ((checkIfLeft == 1) && (nexY == (curY - 1)))
                    legal = 0;
            }

            if(legal == 1)
            {
                oppPieces = null;
                pieces = null;
                oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
                pieces = File.ReadLines("Player1_Current.txt").ToArray();

                unusedString = CheckIfInCheck("ours");
                if (check == 1)
                    legal = 0;
            }

            return legal;
        }

        private int OthersStraight(string direction)
        {
            int legal = 1;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));

            pieces = null;
            string[] opp_Pieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();
            string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = bothTeamsPieces[i].Split(':');
                string s = cur_Piece[3];
                if (direction == "up")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) < moveNextX) && (Int32.Parse(Convert.ToString(s[1])) == moveNextY) && (cur_Piece[0] != "1Ro") && (cur_Piece[0] != "2Ro"))
                    {
                        legal = 0;
                        //pieceHitList.Add(s);
                        break;
                    }
                }
                else if (direction == "down")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) > moveNextX) && (Int32.Parse(Convert.ToString(s[1])) == moveNextY) && ((cur_Piece[0] != "1Ro") && (cur_Piece[0] != "2Ro")))
                    {
                        legal = 0;
                        //pieceHitList.Add(s);
                        break;
                    }
                }
                else if (direction == "left")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) == moveNextX) && (Int32.Parse(Convert.ToString(s[1])) > moveNextY) && ((cur_Piece[0] != "1Ro") && (cur_Piece[0] != "2Ro")))
                    {
                        legal = 0;
                        //pieceHitList.Add(s);
                        break;
                    }
                }
                else if (direction == "right")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) == moveNextX) && (Int32.Parse(Convert.ToString(s[1])) < moveNextY) && ((cur_Piece[0] != "1Ro") && (cur_Piece[0] != "2Ro")))
                    {
                        legal = 0;
                        //pieceHitList.Add(s);
                        break;
                    }
                }
            }

            return legal;
        }

        private int OthersDiagonal(string direction)
        {
            int legal = 1;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));

            pieces = null;
            string[] opp_Pieces = File.ReadLines("Player2_Current.txt").ToArray();
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = bothTeamsPieces[i].Split(':');
                string s = cur_Piece[3];
                int diffX = moveNextX - Int32.Parse(Convert.ToString(s[0]));
                int diffY = moveNextY - Int32.Parse(Convert.ToString(s[1]));
                int absDiffX = Math.Abs(diffX);
                int absDiffY = Math.Abs(diffY);

                if (absDiffX == absDiffY)
                {
                    if (direction == "upleft")
                    {
                        if ((Math.Sign(diffX) == 1) && (Math.Sign(diffY) == -1) && (cur_Piece[0] != "1Bi") && (cur_Piece[0] != "2Bi"))
                        {
                            legal = 0;
                            //pieceHitList.Add(s);
                            break;
                        }
                    }
                    else if (direction == "upright")
                    {
                        if ((Math.Sign(diffX) == 1) && (Math.Sign(diffY) == 1) && (cur_Piece[0] != "1Bi") && (cur_Piece[0] != "2Bi"))
                        {
                            legal = 0;
                            //pieceHitList.Add(s);
                            break;
                        }
                    }
                    else if (direction == "downleft")
                    {
                        if ((Math.Sign(diffX) == -1) && (Math.Sign(diffY) == -1) && (cur_Piece[0] != "1Bi") && (cur_Piece[0] != "2Bi"))
                        {
                            legal = 0;
                            //pieceHitList.Add(s);
                            break;
                        }
                    }
                    else if (direction == "downright")
                    {
                        if ((Math.Sign(diffX) == -1) && (Math.Sign(diffY) == 1) && (cur_Piece[0] != "1Bi") && (cur_Piece[0] != "2Bi"))
                        {
                            legal = 0;
                            //pieceHitList.Add(s);
                            break;
                        }
                    }
                }
            }

            return legal;
        }

        private string CheckIfInCheck(string oursOrTheirs)
        {
            int moveInProgressBackup = moveInProgress;
            /*
            if ((oursOrTheirs == "ours") || (oursOrTheirs == "king"))
            {
                oppPieces = File.ReadLines("Player2_Current.txt").ToArray();
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
            }
            else if (oursOrTheirs == "theirs")
            {
                oppPieces = File.ReadLines("Player1_Current.txt").ToArray();
                pieces = File.ReadLines("Player2_Current.txt").ToArray();
            }

            */

            string[] cur_Opp_Piece = pieces[12].Split(':').ToArray();
            string opp_Piece_Pos = cur_Opp_Piece[3];
            if (oursOrTheirs == "theirs")
                moveInProgress = Int32.Parse(opp_Piece_Pos);
            else if (oursOrTheirs == "ours")
                moveInProgress = moveInProgress - ((moveInProgress / 100) * 100);
            else
                moveInProgress = (moveInProgress / 100);
            int safety = 0;
            int checkTotalPrivate = 0;

            for (int i = 0; i < 9; i++)
            {
                if ((i == 1) && (moveInProgress < 80)) 
                {
                    moveInProgress += 10;
                    safety = 1;
                }
                else if ((i == 2) && ((moveInProgress - ((moveInProgress / 10) * 10)) < 8) && (moveInProgress < 80)) 
                {
                    moveInProgress += 11;
                    safety = 2;
                }
                else if ((i == 3) && ((moveInProgress - ((moveInProgress / 10) * 10)) < 8)) 
                { 
                    moveInProgress += 1;
                    safety = 3;
                } 
                else if ((i == 4) && ((moveInProgress - ((moveInProgress / 10) * 10)) < 8) && (moveInProgress > 20)) 
                { 
                    moveInProgress -= 9;
                    safety = 4; 
                }
                else if ((i == 5) && (moveInProgress > 20)) 
                { 
                    moveInProgress -= 10; 
                    safety = 5; 
                }
                else if ((i == 6) && ((moveInProgress - ((moveInProgress / 10) * 10)) > 1) && (moveInProgress > 20)) 
                { 
                    moveInProgress -= 11; 
                    safety = 6; 
                } 
                else if ((i == 7) && ((moveInProgress - ((moveInProgress / 10) * 10)) > 1)) 
                { 
                    moveInProgress -= 1; 
                    safety = 7; 
                } 
                else if ((i == 8) && ((moveInProgress - ((moveInProgress / 10) * 10)) > 1) && (moveInProgress < 80)) 
                { 
                    moveInProgress += 9;
                    safety = 8; 
                }
                    

                foreach (string element in pieces)
                {
                    cur_Piece = element.Split(':').ToArray();
                    string s = cur_Piece[3];
                    if (s != "99")
                    {
                        moveInProgress += Int32.Parse(s) * 100;
                        checkingIfInCheck = 1;

                        if (safety == 0)
                            check = TestMoveLegality();
                        else if (safety == 1)
                            checkIfUp = TestMoveLegality();
                        else if (safety == 2)
                            checkIfUpRight = TestMoveLegality();
                        else if (safety == 3)
                            checkIfRight = TestMoveLegality();
                        else if (safety == 4)
                            checkIfDownRight = TestMoveLegality();
                        else if (safety == 5)
                            checkIfDown = TestMoveLegality();
                        else if (safety == 6)
                            checkIfDownLeft = TestMoveLegality();
                        else if (safety == 7)
                            checkIfLeft = TestMoveLegality();
                        else if (safety == 8)
                            checkIfUpLeft = TestMoveLegality();

                        checkingIfInCheck = 0;
                        moveInProgress = Int32.Parse(opp_Piece_Pos);
                    }
                }
            }

            checkTotalPrivate = check + checkIfUp + checkIfUpRight + checkIfRight + checkIfDownRight + checkIfDown + checkIfDownLeft + checkIfLeft + checkIfUpLeft;

            string returnValue = "";
            if ((checkTotalPrivate == 8) && (check == 0))
                returnValue = "stm~";
            else if (checkTotalPrivate == 9)
                returnValue = "chm~";
            else if (((checkTotalPrivate < 8) && (checkTotalPrivate > 0)) || ((checkTotalPrivate == 8) && (check == 1)))
                returnValue = "chk~" + checkIfUpLeft.ToString() + ":" + checkIfUp.ToString() + ":" + checkIfUpRight.ToString() + ":" + checkIfRight.ToString() + ":" + checkIfDownRight.ToString() + ":" + checkIfDown.ToString() + ":" + checkIfDownLeft.ToString() + ":" + checkIfLeft.ToString() + ":" + check.ToString() + "~";
            else
                returnValue = "mve~";

            moveInProgress = moveInProgressBackup;

            return returnValue;
        }

        private int TestMoveLegality()
        {
            if ((moveAllowed == 1) || (checkingIfInCheck == 1))
            {
                int legal = 0;
                string testingMove = Convert.ToString(moveInProgress);
                string testingMoveCurX = Convert.ToString(testingMove[0]);
                string testingMoveCurY = Convert.ToString(testingMove[1]);
                string testingMoveCur = testingMoveCurX + testingMoveCurY;

                string testingMoveNextX = Convert.ToString(testingMove[2]);
                string testingMoveNextY = Convert.ToString(testingMove[3]);
                string testingMoveNext = testingMoveNextX + testingMoveNextY;
                string[] next_Piece;

                pieces = null;
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
            if (gameStart == 0)
                return;

            testCorrect = TestMoveLegality();
            //testCorrect = 1;

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

                for (int i = 0; i < 100; i++)
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
                    pieces = null;
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

                pieces = null;
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

                pieces = null;
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

                if (enPassant == 1)
                {
                    int nextCoordinates = moveInProgress - ((moveInProgress / 100) * 100) - 10;

                    pieces = null;
                    pieces = File.ReadLines("Player2_Current.txt").ToArray();
                    for (int i = 0; i < 16; i++)
                    {
                        cur_Piece = pieces[i].Split(':');
                        if (cur_Piece[3] == Convert.ToString(nextCoordinates))
                            break;
                    }

                    for (int i = 0; i < 100; i++)
                    {
                        if (buttonArray[i].Name == cur_Piece[2])
                        {
                            nex_position = i;
                            break;
                        }
                    }
                }

                if (transitionBox.Image != null)
                {
                    pieces = null;
                    pieces = File.ReadLines("Player2_Current.txt").ToArray();

                    for (int i = 0; i < 16; i++)
                    {
                        cur_Piece = pieces[i].Split(':');
                        if (buttonArray[nex_position].Name == cur_Piece[2])
                        {
                            switch (cur_Piece[0])
                            {
                                case "1Pa": cur_Piece[2] = "BP1"; break;
                                case "2Pa": cur_Piece[2] = "BP2"; break;
                                case "3Pa": cur_Piece[2] = "BP3"; break;
                                case "4Pa": cur_Piece[2] = "BP4"; break;
                                case "5Pa": cur_Piece[2] = "BP5"; break;
                                case "6Pa": cur_Piece[2] = "BP6"; break;
                                case "7Pa": cur_Piece[2] = "BP7"; break;
                                case "8Pa": cur_Piece[2] = "BP8"; break;
                                case "1Ro": cur_Piece[2] = "BRL"; break;
                                case "1Kn": cur_Piece[2] = "BKL"; break;
                                case "1Bi": cur_Piece[2] = "BBL"; break;
                                case "Que":
                                    if (playerState == "white")
                                    {
                                        cur_Piece[2] = "BK";
                                        break;
                                    }
                                    else
                                    {
                                        cur_Piece[2] = "BQ";
                                        break;
                                    }
                                case "Kin":
                                    if (playerState == "white")
                                    {
                                        cur_Piece[2] = "BQ";
                                        break;
                                    }
                                    else
                                    {
                                        cur_Piece[2] = "BK";
                                        break;
                                    }
                                case "2Bi": cur_Piece[2] = "BBR"; break;
                                case "2Kn": cur_Piece[2] = "BKR"; break;
                                case "2Ro": cur_Piece[2] = "BRR"; break;
                            }

                            cur_Piece[1] = "A";
                            cur_Piece[3] = "99";
                            cur_Piece[4] = "1";
                            pieces[i] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

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
                }

                pieces = null;
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    if (cur_Piece[2] == buttonArray[cur_position].Name)
                    {
                        cur_Piece[1] = "A";
                        cur_Piece[2] = buttonArray[nex_position].Name;
                        string next = Convert.ToString(moveInProgress - ((moveInProgress / 100) * 100));
                        cur_Piece[3] = next;
                        cur_Piece[4] = "1";
                        pieces[i] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

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

                pieces = null;
                pieces = File.ReadLines("Player1_Current.txt").ToArray();
                for (int i = 0; i < 16; i++)
                {
                    cur_Piece = pieces[i].Split(':');
                    string pieceName = cur_Piece[0];
                    string pieceLocation = cur_Piece[2];

                    if (((Convert.ToString(pieceName[1]) + Convert.ToString(pieceName[2])) == "Pa") && (pieceLocation[0] == 'H'))
                    {
                        pawnChangePositionString = Convert.ToString(pieceLocation[0] + pieceLocation[1]);
                        pawnChangePositionInt = Convert.ToInt32(cur_Piece[4]);
                        SetUpBoard(playerState, false);
                        MessageBox.Show("Please select a piece with which to change your pawn");
                        pawnChange = 1;
                        moveInProgress = 0;
                        changePieceAllowed = pieceName + ":" + buttonArray[nex_position].Name + ":" + nextPlace;
                        moveAllowed = 0;
                        return;
                    }
                }

                transitionBox.Image = null;

                SetUpBoard(playerState, false);

                moveAllowed = 0;
                moveInProgress = 0;

                SendMoveToOtherPlayer();
            }
        }

        private void SendMoveToOtherPlayer()
        {
            if (gameStart == 0)
                return;

            string messageBoxInformation = "";

            oppPieces = null;
            pieces = null;
            oppPieces = File.ReadLines("Player1_Current.txt").ToArray();
            pieces = File.ReadLines("Player2_Current.txt").ToArray();

            string dataToSendToOpponent = CheckIfInCheck("theirs");
            if (dataToSendToOpponent == "stm~")
                messageBoxInformation = "A stalemate has been declared. Congratulations " + name + ", for winning!";
            else if (dataToSendToOpponent == "chm~")
                messageBoxInformation = "A checkmate has been declared. Congratulations " + name + ", for winning!";
            else if ((dataToSendToOpponent[0].ToString() + dataToSendToOpponent[1].ToString() + dataToSendToOpponent[2].ToString() + dataToSendToOpponent[3].ToString()) == "chk~")
                messageBoxInformation = opponentName + "is in check.";

            moveAllowed = 0;
            moveInProgress = 0;

            pieces = null;
            oppPieces = null;
            pieces = File.ReadLines("Player1_Current.txt").ToArray();
            oppPieces = File.ReadLines("Player2_Current.txt").ToArray();

            string[] work = pieces.Concat(oppPieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = work[i].Split(':').ToArray();

                switch (cur_Piece[2])
                {
                    case "WP1": cur_Piece[2] = "BP8"; break;
                    case "WP2": cur_Piece[2] = "BP7"; break;
                    case "WP3": cur_Piece[2] = "BP6"; break;
                    case "WP4": cur_Piece[2] = "BP5"; break;
                    case "WP5": cur_Piece[2] = "BP4"; break;
                    case "WP6": cur_Piece[2] = "BP3"; break;
                    case "WP7": cur_Piece[2] = "BP2"; break;
                    case "WP8": cur_Piece[2] = "BP1"; break;
                    case "WRL": cur_Piece[2] = "BRR"; break;
                    case "WRR": cur_Piece[2] = "BRL"; break;
                    case "WKL": cur_Piece[2] = "BKR"; break;
                    case "WKR": cur_Piece[2] = "BKL"; break;
                    case "WBL": cur_Piece[2] = "BBR"; break;
                    case "WBR": cur_Piece[2] = "BBL"; break;
                    case "WK": cur_Piece[2] = "BK"; break;
                    case "WQ": cur_Piece[2] = "BQ"; break;
                    case "BP1": cur_Piece[2] = "WP8"; break;
                    case "BP2": cur_Piece[2] = "WP7"; break;
                    case "BP3": cur_Piece[2] = "WP6"; break;
                    case "BP4": cur_Piece[2] = "WP5"; break;
                    case "BP5": cur_Piece[2] = "WP4"; break;
                    case "BP6": cur_Piece[2] = "WP3"; break;
                    case "BP7": cur_Piece[2] = "WP2"; break;
                    case "BP8": cur_Piece[2] = "WP1"; break;
                    case "BRL": cur_Piece[2] = "WRR"; break;
                    case "BRR": cur_Piece[2] = "WRL"; break;
                    case "BKL": cur_Piece[2] = "WKR"; break;
                    case "BKR": cur_Piece[2] = "WKL"; break;
                    case "BBL": cur_Piece[2] = "WBR"; break;
                    case "BBR": cur_Piece[2] = "WBL"; break;
                    case "BK": cur_Piece[2] = "WK"; break;
                    case "BQ": cur_Piece[2] = "WQ"; break;
                    default:
                        ReverseFile(2);
                        break;
                }

                ReverseFile(5);
                string writer = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                if (i == 15)
                    writer += "~";
                else
                    writer += ";";

                dataToSendToOpponent += writer;
            }

            dataToSendToOpponent += "~" + moveInProgress;
            SendData(dataToSendToOpponent);

            if (messageBoxInformation != "")
            {
                MessageBox.Show(messageBoxInformation);
                messageBoxInformation = "";
            }
        }

        private void ReverseFile(int place)
        {
            string cur_Piece_Halves = cur_Piece[place];
            char cur_Piece_Half_1 = cur_Piece_Halves[0];
            char cur_Piece_Half_2 = cur_Piece_Halves[1];

            switch (cur_Piece_Halves[0])
            {
                case 'A': cur_Piece_Half_1 = 'H'; break;
                case 'B': cur_Piece_Half_1 = 'G'; break;
                case 'C': cur_Piece_Half_1 = 'F'; break;
                case 'D': cur_Piece_Half_1 = 'E'; break;
                case 'E': cur_Piece_Half_1 = 'D'; break;
                case 'F': cur_Piece_Half_1 = 'C'; break;
                case 'G': cur_Piece_Half_1 = 'B'; break;
                case 'H': cur_Piece_Half_1 = 'A'; break;
            }

            switch (cur_Piece_Halves[1])
            {
                case '1': cur_Piece_Half_2 = '8'; break;
                case '2': cur_Piece_Half_2 = '7'; break;
                case '3': cur_Piece_Half_2 = '6'; break;
                case '4': cur_Piece_Half_2 = '5'; break;
                case '5': cur_Piece_Half_2 = '4'; break;
                case '6': cur_Piece_Half_2 = '3'; break;
                case '7': cur_Piece_Half_2 = '2'; break;
                case '8': cur_Piece_Half_2 = '1'; break;
            }

            cur_Piece[place] = cur_Piece_Half_1.ToString() + cur_Piece_Half_2.ToString();
        }

        private void StartOfCastle(string direction)
        {
            if (gameStart == 0)
                return;

            if (direction == "left")
            {
                pieces = null;
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
                        int pos = Array.IndexOf(pieces, "Kin:A:A5:15:0:A5");
                        pieces[pos] = "Kin:A:A3:13:1:A5";
                        pos = Array.IndexOf(pieces, "1Ro:A:A1:11:0:A1");
                        pieces[pos] = "1Ro:A:A4:14:1:A1";

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
                            int pos = Array.IndexOf(pieces, "Kin:A:A4:14:0:A4");
                            pieces[pos] = "Kin:A:A2:12:1:A4";
                            pos = Array.IndexOf(pieces, "1Ro:A:A1:11:0:A1");
                            pieces[pos] = "1Ro:A:A3:13:1:A1";

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
                pieces = null;
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
                        int pos = Array.IndexOf(pieces, "Kin:A:A4:14:0:A4");
                        pieces[pos] = "Kin:A:A6:16:1:A4";
                        pos = Array.IndexOf(pieces, "2Ro:A:A8:18:0:A8");
                        pieces[pos] = "2Ro:A:A5:15:1:A8";

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
                            int pos = Array.IndexOf(pieces, "Kin:A:A5:15:0:A5");
                            pieces[pos] = "Kin:A:A7:17:1:A5";
                            pos = Array.IndexOf(pieces, "2Ro:A:A8:18:0:A8");
                            pieces[pos] = "2Ro:A:A6:16:1:A8";

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
            }
        }

        private static void CreatePlayerCurrentTXTs()
        {
            string pl1Cur = @"Player1_Current.txt";
            string pl2Cur = @"Player2_Current.txt";
            string data = "";

            if (playerState == "white")
            {
                data = Chess.Properties.Resources.White_Origin;
            }
            else
            {
                data = Chess.Properties.Resources.Black_Origin;
            }

            string[] Origins = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            FileStream aFile;
            StreamWriter sw;

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
        }

        private void concedeButton_Click(object sender, EventArgs e)
        {
            SendData("drw~");
        }

        public void SendData(string data)
        {
            String s = data;
            byte[] byteTime = Encoding.ASCII.GetBytes(s);
            ns.Write(byteTime, 0, byteTime.Length);
            System.Threading.Thread.Sleep(3000);
        }

        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                int bytesRead = ns.Read(bytes, 0, bytes.Length);
                this.SetText(Encoding.ASCII.GetString(bytes, 0, bytesRead));
            }
        }

        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                ProcessOpponentData(text);
            }
        }

        private void ProcessOpponentData(string oppData)
        {
            string testString = Convert.ToString(oppData[0]) + Convert.ToString(oppData[1]) + Convert.ToString(oppData[2]) + Convert.ToString(oppData[3]);
            string[] bothPieces;

            switch (testString)
            {
                case "lve~":
                    Login l = new Login();
                    l.Close();
                    return;
                case "dct~":
                    if (serverState == 1)
                    {
                        listener.Stop();
                    }
                    client.Close();
                    MessageBox.Show(opponentName + " has disconnected", "Disconnect Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SendData("lve~");

                    foreach (Button element in buttonArray)
                    {
                        element.Enabled = false;
                    }

                    t.Abort();

                    return;
                case "cct~": return;
                case "clr~":
                    gameStart = 1;
                    bothPieces = oppData.Split('~').ToArray();
                    playerState = bothPieces[1];

                    if (playerState == "white")
                        moveAllowed = 1;
                    else
                        moveAllowed = 0;

                    SetUpBoard(playerState, true);

                    return;
                case "nme~":
                    bothPieces = oppData.Split('~').ToArray();
                    opponentName = bothPieces[1];
                    DialogResult acceptCall = MessageBox.Show("Would you like to accept " + opponentName + " to your game?", "Player Joining", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (acceptCall == DialogResult.No)
                    {
                        listener.Stop();
                        System.Threading.Thread.Sleep(100);
                        listener.Start();
                        return;
                    }

                    gameStart = 1;

                    if (bothPieces[2] == playerState)
                    {
                        Random rnd = new Random();
                        int rndNum = rnd.Next();
                        if ((rndNum % 2) == 0)
                        {
                            SendData("clr~white");
                            playerState = "black";
                            moveAllowed = 0;
                        }
                        else
                        {
                            SendData("clr~black");
                            playerState = "white";
                            moveAllowed = 1;
                        }
                    }
                    else
                        SendData("clr~" + bothPieces[2]);

                    SetUpBoard(playerState, true);

                    return;
                case "msg~":
                    bothPieces = oppData.Split('~').ToArray();
                    ChatForm chat = new ChatForm();
                    chat.Show();
                    chat.SetTextBox(bothPieces[1]);
                    return;
                case "mve~":
                    bothPieces = oppData.Split('~').ToArray();
                    pieces = bothPieces[2].Split(';').ToArray();
                    oppPieces = bothPieces[1].Split(';').ToArray();
                    string otherXs = bothPieces[3];
                    oppCurX = otherXs[0];
                    oppNextX = otherXs[2];
                    break;
                case "chk~":
                    bothPieces = oppData.Split('~').ToArray();
                    MessageBox.Show("Check");
                    ChatForm chat2 = new ChatForm();
                    chat2.Show();
                    chat2.SetTextBox(opponentName + ": Check");
                    string[] bothPieces1 = bothPieces[1].Split(':').ToArray();
                    checkIfUpLeft = Convert.ToInt32(bothPieces1[0]);
                    checkIfUp = Convert.ToInt32(bothPieces1[1]);
                    checkIfUpRight = Convert.ToInt32(bothPieces1[2]);
                    checkIfRight = Convert.ToInt32(bothPieces1[3]);
                    checkIfDownRight = Convert.ToInt32(bothPieces1[4]);
                    checkIfDown = Convert.ToInt32(bothPieces1[5]);
                    checkIfDownLeft = Convert.ToInt32(bothPieces1[6]);
                    checkIfLeft = Convert.ToInt32(bothPieces1[7]);
                    check = Convert.ToInt32(bothPieces1[8]);

                    bothPieces = testString.Split('~').ToArray();
                    pieces = bothPieces[3].Split(';').ToArray();
                    oppPieces = bothPieces[2].Split(';').ToArray();
                    string otherX = bothPieces[3];
                    oppCurX = otherX[0];
                    oppNextX = otherX[2];

                    break;
                case "stm~": MessageBox.Show("A stalemate has been declared, with " + opponentName + " the victor.");  return;
                case "chm~": MessageBox.Show("Checkmate has been declared, with " + opponentName + "the victor"); return;
                case "drw~": MessageBox.Show(opponentName + " has conceded. The game has ended."); return;
                default: listener.Stop(); System.Threading.Thread.Sleep(100); listener.Start(); return;
            }

            FileStream aFile = new FileStream("Player1_Current.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(aFile);

            FileStream bFile = new FileStream("Player2_Current.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(bFile);

            foreach (string element in pieces)
                sw.WriteLine(element);

            foreach (string element in oppPieces)
                sw1.WriteLine(element);

            sw.Close();
            sw1.Close();
            aFile.Close();
            bFile.Close();
            moveAllowed = 1;
            SetUpBoard(playerState, false);
        }

        public static void SetUpBoard(string stateOfPlayer, bool createTXTs)
        {
            if (gameStart == 0)
                return;

            Login.HideLogin();

            foreach (Button element in buttonArray)
            {
                element.Image = null;
            }

            if (createTXTs)
                CreatePlayerCurrentTXTs();

            int curPosition = -1;

            pieces = null;
            pieces = File.ReadLines("Player1_Current.txt").ToArray();

            for (int i = 0; i < 32; i++)
            {
                curPosition++;
                if (curPosition > 15)
                {
                    pieces = null;
                    pieces = File.ReadLines("Player2_Current.txt").ToArray();
                    curPosition = 0;
                }

                cur_Piece = pieces[curPosition].Split(':');
                string s = cur_Piece[5];
                string oppPlayerState = "";

                if (playerState == "white")
                    oppPlayerState = "black";
                else
                    oppPlayerState = "white";

                for (int j = 0; j < 100; j++)
                {
                    if (buttonArray[j].Name == cur_Piece[2])
                    {
                        if ((cur_Piece[5] == "A1") || (cur_Piece[5] == "A8"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_rook_" + playerState + "_small");
                        }
                        else if ((cur_Piece[5] == "A2") || (cur_Piece[5] == "A7"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_knight_" + playerState + "_small");
                        }
                        else if ((cur_Piece[5] == "A3") || (cur_Piece[5] == "A6"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_bishop_" + playerState + "_small");
                        }
                        else if (cur_Piece[5] == "A5")
                        {
                            if (stateOfPlayer == "black")
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_king_" + playerState + "_small");
                            else
                            {
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_queen_" + playerState + "_small");
                                KingPosition = cur_Piece[3];
                            }
                        }
                        else if (cur_Piece[5] == "A4")
                        {
                            if (stateOfPlayer == "black")
                            {
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_queen_" + playerState + "_small");
                                KingPosition = cur_Piece[3];
                            }
                            else
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_king_" + playerState + "_small");
                        }
                        else if (s[0] == 'B')
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_pawn_" + playerState + "_small");
                        }
                        else if ((cur_Piece[5] == "H1") || (cur_Piece[5] == "H8"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_rook_" + oppPlayerState + "_small");
                        }
                        else if ((cur_Piece[5] == "H2") || (cur_Piece[5] == "H7"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_knight_" + oppPlayerState + "_small");
                        }
                        else if ((cur_Piece[5] == "H3") || (cur_Piece[5] == "H6"))
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_bishop_" + oppPlayerState + "_small");
                        }
                        else if (cur_Piece[5] == "H5")
                        {
                            if (stateOfPlayer == "black")
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_queen_" + oppPlayerState + "_small");
                            else
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_king_" + oppPlayerState + "_small");
                        }
                        else if (cur_Piece[5] == "H4")
                        {
                            if (stateOfPlayer == "black")
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_king_" + oppPlayerState + "_small");
                            else
                                buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_queen_" + oppPlayerState + "_small");
                        }
                        else
                        {
                            buttonArray[j].Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject("chess_pawn_" + oppPlayerState + "_small");
                        }

                        buttonArray[j].ImageAlign = ContentAlignment.MiddleCenter;
                        buttonArray[j].FlatStyle = FlatStyle.Flat;
                    }
                }
            }
        }
    }
}
