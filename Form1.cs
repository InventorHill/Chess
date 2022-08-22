using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Collections;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace Chess
{
    public partial class MainGame : Form
    {
        bool startReplay = true;

        bool replayClicked = false;
        bool replaySuspended = false;
        bool opponentReplay = false;

        public static bool replaying = false;

        bool waitForMoreData = false;
        bool saving = false;
        bool checkingCheck = false;
        bool allowPawnMethod = true;

        static Stream chessPieceMoveStream = Chess.Properties.Resources.Chess_Piece_Move;
        static SoundPlayer chessPieceMoveSound = new SoundPlayer(chessPieceMoveStream);
        string pieceMoving;

        static Stream chessWinStream = Chess.Properties.Resources.Chess_Win_Sound;
        static SoundPlayer chessWinSound = new SoundPlayer(chessWinStream);

        static Stream chessLoseStream = Chess.Properties.Resources.Chess_Lose_Sound;
        static SoundPlayer chessLoseSound = new SoundPlayer(chessLoseStream);

        static Stream chessDrawStream = Chess.Properties.Resources.Chess_Draw_Sound;
        static SoundPlayer chessDrawSound = new SoundPlayer(chessDrawStream);

        bool restoreStaticPieces = true;

        Thread waitForClient = null;
        bool isClientConnected = false;

        public ChatForm chat = new ChatForm();
        public static int checkInProgress = 0;

        public static string[] staticPieces = new string[16];
        public static string[] staticOpponentPieces = new string[16];

        private int checkingIfInCheck = 0;

        public static int serverState = 2;

        delegate void SetTextCallback(string text);
        static TcpListener listener;
        static TcpClient client;
        static NetworkStream ns;
        static Thread t = null;

        public static int gameStart = 0;

        public static int pawnChange = 0;

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

        public string rook = "";
        public int castle = 0;
        public int illegalAction = 0;
        public static string name = "";
        public static string ipAddress = "";
        public static int portNumber = 0;
        public static string playerState = "";
        public static int moveAllowed = 0;
        public static string changePieceAllowed = "";
        public int moveInProgress = 0;
        public string moveCoordinates = "";
        public int testCorrect = 0;
        public static string[] pieces = new string[16];
        public static string[] cur_Piece;
        public static Button[] buttonArray = null;
        string[] oppPieces = new string[16];
        public static string opponentName = "";
        public static int connectionStarted = 0;
        
        public MainGame()
        {
            InitializeComponent();

            buttonArray = this.Controls.OfType<Button>().ToArray();
            foreach(Button element in buttonArray)
            {
                element.Enabled = false;
            }

            if ((serverState == 0) && (connectionStarted == 0))
            {
                try
                {
                    connectionStarted = LoginVariables.connectionStarted = 1;
                    client = new TcpClient(ipAddress, portNumber);
                    ns = client.GetStream();
                    SendData("cct~", false);
                    t = new Thread(DoWork);
                    t.Start();
                    SendData("nme~" + name + "~" + playerState, false);
                    LoginVariables.exiting = true;
                }
                catch
                {
                    if (!LoginVariables.exiting)
                    {
                        MessageBox.Show("There is no host to which this program can connect. The game will now exit.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Login.ExitProgram();
                    }
                }
            }
            else if ((serverState == 1) && (connectionStarted == 0))
            {
                LoginVariables.exiting = true;
                waitForClient = new Thread(WaitForClient);
                waitForClient.Start();
            }
        }

        public void WaitForClient()
        {
            try
            {
                Thread checkIfClientConnected = new Thread(CheckIfClientConnected);
                connectionStarted = LoginVariables.connectionStarted = 1;
                listener = new TcpListener(IPAddress.Any, portNumber);
                checkIfClientConnected.Start();
                listener.Start();
                client = listener.AcceptTcpClient();
                ns = client.GetStream();
                t = new Thread(DoWork);
                t.Start();
                isClientConnected = true;
            }
            catch
            {
                LoginVariables.exiting = true;
                Login.ExitProgram();
            }
        }

        private void MainGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            disconnectGame(true);
        }

        private void CheckIfClientConnected()
        {
            Thread.Sleep(55000);
            if (!isClientConnected)
            {
                waitForClient.Abort();
                MessageBox.Show("The server has timed out. The program will now exit.", "Server timeout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoginVariables.exiting = true;
                Login.ExitProgram();
            }
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

        private void cancel_button_Click(object sender, EventArgs e)
        {
            moveInProgress = 0;
        }

        private void disconnectGame(bool hasFormClosingBeenCalled)
        {
            if (gameStart == 0)
                return;

            DialogResult result = MessageBox.Show("Are you sure you want to disconnect?", "Disconnect Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (Button element in buttonArray)
                {
                    element.Enabled = false;
                }
                SendData("dct~", false);
                if (hasFormClosingBeenCalled)
                    Login.ExitProgram();
            }
        }
        private void disconnect_button_Click(object sender, EventArgs e)
        {
            if (replaying)
                return;

            if (LoginVariables.enableDraw)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to declare a draw? The game cannot be started again from this current position", "Disconnect Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    SendData("ask~", false);
                }
            }
            else
                disconnectGame(false);
        }

        private void chat_button_Click(object sender, EventArgs e)
        {
            if (gameStart == 0 || replaying)
                return;

            if (chat.IsDisposed)
                chat = new ChatForm();
            chat.Show();

            chat.SetTextBox("", false);
        }

        private void return_Piece(string pieceToReturn)
        {
            if (gameStart == 0)
                return;

            if (pawnChange == 1)
            {
                int piecePositionInArray = -1;
                for (int i = 0; i < 16; i++)
                {
                    if(Int32.Parse(staticPieces[i].Split(':').ToArray()[3]) == (moveInProgress - ((moveInProgress / 100) * 100)))
                    {
                        piecePositionInArray = i;
                        break;
                    }
                }

                LoginVariables.oppNums[0]++;
                bool returnToGame = false;
                if (piecePositionInArray != -1)
                {
                    string[] currentPiece = staticPieces[piecePositionInArray].Split(':').ToArray();

                    switch (pieceToReturn)
                    {
                        case "knight":
                            currentPiece[0] = "2Kn";
                            returnToGame = true;
                            break;
                        case "rook":
                            currentPiece[0] = "2Ro";
                            returnToGame = true;
                            break;
                        case "bishop":
                            currentPiece[0] = "2Bi";
                            returnToGame = true;
                            break;
                        case "queen":
                            currentPiece[0] = "Que";
                            returnToGame = true;
                            break;
                        default: break;
                    }

                    staticPieces[piecePositionInArray] = string.Join(":", currentPiece);
                }

                if (returnToGame)
                {
                    staticPieces.CopyTo(LoginVariables.Player1_Current, 0);
                    pawnChange = 0;
                    foreach(Button element in buttonArray)
                    {
                        element.Enabled = true;
                    }

                    knightSelectButton.Enabled = false;
                    rookSelectButton.Enabled = false;
                    bishopSelectButton.Enabled = false;
                    queenSelectButton.Enabled = false;

                    SendMoveToOtherPlayer();
                    SetUpBoard(playerState, false);

                    pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                    knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                    rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                    bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                    queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                    oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                    oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                    oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                    oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                    oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();
                }
                else
                {
                    MessageBox.Show("Piece cannot be selected. Please try again", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pawn Change Not Allowed", "Illegal Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                moveInProgress = 0;
            }
        }

        private bool PiecesBetween()
        {
            bool between = true;

            string moveProgress = moveInProgress.ToString();
            int moveProgressFirst = moveInProgress / 100;
            int moveProgressSecond = moveInProgress - ((moveInProgress / 100) * 100);
            int moveProgress1 = Int32.Parse(moveProgress[0].ToString());
            int moveProgress2 = Int32.Parse(moveProgress[1].ToString());
            int moveProgress3 = Int32.Parse(moveProgress[2].ToString());
            int moveProgress4 = Int32.Parse(moveProgress[3].ToString());
            string bothTeamsPieces = string.Join(":", staticPieces.Concat(staticOpponentPieces).ToArray());
            int moveDirection = 0;
            string[] positionsBetween = new string[0];

            if ((moveProgress1 == moveProgress3) && (moveProgress2 != moveProgress4))
                moveDirection = 1;
            else if ((moveProgress1 != moveProgress3) && (moveProgress2 == moveProgress4))
                moveDirection = 2;
            else
                moveDirection = 3;

            switch (moveDirection)
            {
                case 1:
                    int lateralDifference = moveProgress4 - moveProgress2;
                    if (Math.Abs(lateralDifference) != 1)
                    {
                        int startPoint = moveProgress2;
                        int endPoint = moveProgress4;

                        if (Math.Sign(lateralDifference) == -1) 
                        {
                            startPoint = moveProgress4;
                            endPoint = moveProgress2;
                        }

                        for (int i = startPoint + 1; i != endPoint; i++)
                        {
                            Array.Resize(ref positionsBetween, positionsBetween.Length + 1);
                            positionsBetween[positionsBetween.Length - 1] = ((moveProgress1 * 10) + i).ToString();
                        }
                    }
                    else
                    {
                        between = false;
                    }
                    break;
                case 2:
                    int verticalDifference = moveProgress3 - moveProgress1;
                    if (Math.Abs(verticalDifference) != 1)
                    {
                        int startPoint = moveProgress1;
                        int endPoint = moveProgress3;

                        if (Math.Sign(verticalDifference) == -1)
                        {
                            startPoint = moveProgress3;
                            endPoint = moveProgress1;
                        }

                        for (int i = startPoint + 1; i != endPoint; i++)
                        {
                            Array.Resize(ref positionsBetween, positionsBetween.Length + 1);
                            positionsBetween[positionsBetween.Length - 1] = ((i * 10) + moveProgress2).ToString();
                        }
                    }
                    else
                    {
                        between = false;
                    }
                    break;
                case 3:
                    int change = 0;

                    if ((moveProgress1 > moveProgress3) && (moveProgress2 > moveProgress4))
                        change = -11;
                    else if ((moveProgress1 > moveProgress3) && (moveProgress2 < moveProgress4))
                        change = -9;
                    else if ((moveProgress1 < moveProgress3) && (moveProgress2 > moveProgress4))
                        change = 9;
                    else if ((moveProgress1 < moveProgress3) && (moveProgress2 < moveProgress4))
                        change = 11;

                    if ((moveProgressFirst + change) == moveProgressSecond)
                        between = false;
                    else
                    {
                        for(int i = moveProgressFirst + change; i != moveProgressSecond; i += change)
                        {
                            Array.Resize(ref positionsBetween, positionsBetween.Length + 1);
                            positionsBetween[positionsBetween.Length - 1] = i.ToString();
                        }
                    }
                    break;
            }

            if (between)
            {
                between = false;
                foreach(string element in positionsBetween)
                {
                    if(bothTeamsPieces.IndexOf(":" + element + ":") != -1)
                    {
                        between = true;
                        break;
                    }
                }
            }

            return between;
        }
        private int PawnCheck(int checkingOurs, int pawnMoveInProgress)
        {
            if ((checkInProgress == 0) || (checkingOurs == 0))
            {
                int legal = 0;
                string moveProgress = Convert.ToString(pawnMoveInProgress);
                int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
                int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
                int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
                int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));
                string moveNextCoordinates = Convert.ToString((moveNextX * 10) + moveNextY);

                string[] opp_Pieces = staticOpponentPieces;

                int piecesIndex = (string.Join(",", staticPieces).Replace(",", "").IndexOf(":" + ((moveCurX * 10) + moveCurY).ToString() + ":") + 2) / 16;

                cur_Piece = staticPieces[piecesIndex].Split(':');

                string s = cur_Piece[3];
                if (((Int32.Parse(Convert.ToString(s[0])) + 3) > moveNextX) && (cur_Piece[4] == "0") && (moveNextX > moveCurX) && (moveCurY == moveNextY))
                    legal = 1;
                else if ((((Int32.Parse(Convert.ToString(s[0])) + 2) == moveNextX) && (cur_Piece[4] != "0")))
                    legal = 0;
                else if (((Int32.Parse(Convert.ToString(s[0])) + 2) > moveNextX) && (moveNextX > moveCurX) && (moveCurY == moveNextY))
                    legal = 1;

                if ((moveNextX == (moveCurX + 1)) && ((moveNextY == (moveCurY + 1)) || (moveNextY == (moveCurY - 1))))
                {
                    for (int i = 0; i < 16; i++)
                    {
                        cur_Piece = opp_Pieces[i].Split(':');
                        string pieceName = cur_Piece[0][1].ToString() + cur_Piece[0][2].ToString();
                        if (cur_Piece[3] == moveNextCoordinates)
                        {
                            legal = 1;
                            enPassantImpossible = 1;
                        }
                        if ((legal == 0) && (oppNextX == (oppCurX + 2)) && (s[0] == '5') && (enPassantImpossible == 0) && (pieceName == "Pa"))
                        {
                            int nextCoordinates = pawnMoveInProgress - ((pawnMoveInProgress / 100) * 100) - 10;
                            int nextCoordinatesOpposite1 = 9 - (nextCoordinates / 10);
                            int nextCoordinatesOpposite2 = 9 - (nextCoordinates - ((nextCoordinates / 10) * 10));
                            int nextCoordinatesOpposite = (nextCoordinatesOpposite1 * 10) + nextCoordinatesOpposite2;

                            foreach (string element in staticOpponentPieces)
                            {
                                if ((Int32.Parse(element.Split(':').ToArray()[3]) == nextCoordinates) && (LoginVariables.oppCurrentPos == (LoginVariables.oppPreviousPos + 20)) && (LoginVariables.oppCurrentPos == nextCoordinatesOpposite))
                                {
                                    enPassant = 1;
                                    legal = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

                if (legal == 1)
                {
                    string moveNextCoordinatesBackup = moveNextCoordinates;
                    moveNextCoordinates = ((moveNextX * 10) + moveCurY).ToString();
                    int arePiecesInFrontOfOurPawn = string.Join(",", bothTeamsPieces).Replace(",", "").IndexOf(":" + moveNextCoordinates + ":");
                    moveNextCoordinates = moveNextCoordinatesBackup;

                    if ((arePiecesInFrontOfOurPawn == -1) && (moveCurY == moveNextY) && (moveNextX == (moveCurX + 2)))
                        arePiecesInFrontOfOurPawn = string.Join(",", bothTeamsPieces).Replace(",", "").IndexOf(":" + (Int32.Parse(moveNextCoordinates) - 10).ToString() + ":");

                    if ((arePiecesInFrontOfOurPawn != -1) && (moveCurY == moveNextY) && ((moveNextX == (moveCurX + 2)) || (moveNextX == (moveCurX + 1))))
                        legal = 0;
                }

                if (restoreStaticPieces)
                {
                    staticOpponentPieces.CopyTo(oppPieces, 0);
                    staticPieces.CopyTo(pieces, 0);
                }

                if (allowPawnMethod && checkingCheck && (checkInProgress == 1) && !LoginVariables.opponentCheckArrayLock && LoginVariables.setCheckValues)
                {
                    allowPawnMethod = false;
                    int pawnMoveInProgressBackup = moveInProgress;
                    Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                    LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = pawnMoveInProgress / 100;

                    int[] movePositions = 
                    {
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) + 10,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) + 20,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) + 9,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) + 11
                    };

                    foreach (int element in movePositions)
                    {
                        moveInProgress = element;
                        int testMove = TestMoveLegality(1, 0);
                        if(testMove == 1)
                        {
                            Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                            LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = element - ((element / 100) * 100);
                        }
                    }

                    moveInProgress = pawnMoveInProgressBackup;
                    allowPawnMethod = true;
                }

                checkInProgress = 1;
                return legal;
            }
            else
            {
                int legal = 0;
                string moveProgress = Convert.ToString(pawnMoveInProgress);
                int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
                int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
                int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
                int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));
                string moveNextCoordinates = Convert.ToString((moveNextX * 10) + moveNextY);

                string[] opp_Pieces = staticOpponentPieces;

                int piecesIndex = (string.Join(",", staticPieces).Replace(",", "").IndexOf(":" + ((moveCurX * 10) + moveCurY).ToString() + ":") + 2) / 16;

                cur_Piece = staticPieces[piecesIndex].Split(':');

                string s = cur_Piece[3];
                if (((Int32.Parse(Convert.ToString(s[0])) - 3) < moveNextX) && (cur_Piece[4] == "0") && (moveNextX < moveCurX) && (moveCurY == moveNextY))
                    legal = 1;
                else if ((((Int32.Parse(Convert.ToString(s[0])) - 2) == moveNextX) && (cur_Piece[4] != "0")))
                    legal = 0;
                else if (((Int32.Parse(Convert.ToString(s[0])) - 2) < moveNextX) && (moveNextX < moveCurX) && (moveCurY == moveNextY))
                    legal = 1;

                if ((moveNextX == (moveCurX - 1)) && ((moveNextY == (moveCurY + 1)) || (moveNextY == (moveCurY - 1))))
                {
                    for (int i = 0; i < 16; i++)
                    {
                        cur_Piece = opp_Pieces[i].Split(':');
                        if (cur_Piece[3] == moveNextCoordinates)
                            legal = 1;
                    }
                }

                string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

                if (legal == 1)
                {
                    string moveNextCoordinatesBackup = moveNextCoordinates;
                    moveNextCoordinates = ((moveNextX * 10) + moveCurY).ToString();
                    int arePiecesInFrontOfOurPawn = string.Join(",", bothTeamsPieces).Replace(",", "").IndexOf(":" + moveNextCoordinates + ":");
                    moveNextCoordinates = moveNextCoordinatesBackup;

                    if ((arePiecesInFrontOfOurPawn == -1) && (moveCurY == moveNextY))
                        arePiecesInFrontOfOurPawn = string.Join(",", bothTeamsPieces).Replace(",", "").IndexOf(":" + (Int32.Parse(moveNextCoordinates) + 1).ToString() + ":");

                    if ((arePiecesInFrontOfOurPawn != -1) && (moveCurY == moveNextY))
                        legal = 0;
                }

                if (restoreStaticPieces)
                {
                    staticOpponentPieces.CopyTo(oppPieces, 0);
                    staticPieces.CopyTo(pieces, 0);
                }

                if (allowPawnMethod && checkingCheck && (checkInProgress == 1) && !LoginVariables.opponentCheckArrayLock && LoginVariables.setCheckValues)
                {
                    allowPawnMethod = false;
                    int pawnMoveInProgressBackup = moveInProgress;
                    Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                    LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = pawnMoveInProgress / 100;

                    int[] movePositions =
                    {
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) - 10,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) - 20,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) - 9,
                        ((pawnMoveInProgress / 100) * 100) + (pawnMoveInProgress / 100) - 11
                    };

                    foreach (int element in movePositions)
                    {
                        moveInProgress = element;
                        int testMove = TestMoveLegality(1, 1);
                        if (testMove == 1)
                        {
                            Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                            LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = element - ((element / 100) * 100);
                        }
                    }

                    moveInProgress = pawnMoveInProgressBackup;
                    allowPawnMethod = true;
                }

                checkInProgress = 1;
                return legal;
            }
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

            if (restoreStaticPieces)
            {
                staticOpponentPieces.CopyTo(oppPieces, 0);
                staticPieces.CopyTo(pieces, 0);
            }

            if ((checkInProgress == 1) && checkingCheck && !LoginVariables.opponentCheckArrayLock && LoginVariables.setCheckValues)
            {
                int rookPos = moveInProgress / 100;
                int kingPos = moveInProgress - (rookPos * 100);

                int difference = rookPos - kingPos;
                int sign = Math.Sign(difference);
                int newRookPos;
                difference = Math.Abs(difference);
                Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = rookPos;

                if ((kingPos / 10) == (rookPos / 10))
                {
                    for (int i = difference; i > 1; i--)
                    {
                        newRookPos = rookPos;
                        if(sign == -1)
                            newRookPos += i - 1;
                        else
                            newRookPos -= i - 1;
                        Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                        LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = newRookPos;
                    }
                }
                else
                {
                    for (int i = difference; i > 10; i -= 10)
                    {
                        newRookPos = rookPos;
                        if (sign == -1)
                            newRookPos += i - 10;
                        else
                            newRookPos -= i - 10;
                        Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                        LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = newRookPos;
                    }
                }
            }

            if(legal == 1)
                if (PiecesBetween())
                    legal = 0;

            checkInProgress = 1;
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

            int legal = 0;

            if (restoreStaticPieces)
            {
                staticOpponentPieces.CopyTo(oppPieces, 0);
                staticPieces.CopyTo(pieces, 0);
            }

            checkInProgress = 1;

            if (((moveNextX == xp2) && (moveNextY == yp1)) || ((moveNextX == xp1) && (moveNextY == yp2)) || ((moveNextX == xm1) && (moveNextY == yp2)) || ((moveNextX == xm2) && (moveNextY == yp1)) || ((moveNextX == xm2) && (moveNextY == ym1)) || ((moveNextX == xm1) && (moveNextY == ym2)) || ((moveNextX == xp1) && (moveNextY == ym2)) || ((moveNextX == xp2) && (moveNextY == ym1)))
                legal = 1;
            else
                legal = 0;

            if ((checkInProgress == 1) && checkingCheck && !LoginVariables.opponentCheckArrayLock && LoginVariables.setCheckValues)
            {
                Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = moveInProgress / 100;

                int knightPos = moveInProgress / 100;

                int[] knightMoves = { knightPos + 21, knightPos + 12, knightPos - 8, knightPos - 19, knightPos - 21, knightPos - 12, knightPos + 8, knightPos + 19 };
                foreach(int element in knightMoves)
                {
                    if ((element >= 11) && (element <= 88))
                    {
                        Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                        LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = element;
                    }
                }
            }

                return legal;
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
            string diagonalDirection = "";

            if (Math.Abs(DiffX) == Math.Abs(DiffY))
            {
                if ((Math.Sign(DiffX) == -1) && (Math.Sign(DiffY) == -1))
                {
                    legal = OthersDiagonal("upright");
                    diagonalDirection = "upright";
                }
                else if ((Math.Sign(DiffX) == 1) && (Math.Sign(DiffY) == 1))
                {
                    legal = OthersDiagonal("downleft");
                    diagonalDirection = "downleft";
                }
                else if ((Math.Sign(DiffX) == -1) && (Math.Sign(DiffY) == 1))
                {
                    legal = OthersDiagonal("upleft");
                    diagonalDirection = "upleft";
                }
                else if ((Math.Sign(DiffX) == 1) && (Math.Sign(DiffY) == -1))
                {
                    legal = OthersDiagonal("downright");
                    diagonalDirection = "downright";
                }
                if (diagonalDirection != "")
                    legal = OthersDiagonal(diagonalDirection);
            }

            if (restoreStaticPieces)
            {
                staticOpponentPieces.CopyTo(oppPieces, 0);
                staticPieces.CopyTo(pieces, 0);
            }

            if ((checkInProgress == 1) && checkingCheck && !LoginVariables.opponentCheckArrayLock && (diagonalDirection != "") && LoginVariables.setCheckValues)
            {
                int bishopPos = moveInProgress / 100;
                int kingPos = moveInProgress - (bishopPos * 100);
                int kingBishopDiff = 0;

                switch (diagonalDirection)
                {
                    case "upright": kingBishopDiff = 11; break;
                    case "downright": kingBishopDiff = -9; break;
                    case "downleft": kingBishopDiff = -11; break;
                    case "upleft": kingBishopDiff = 9; break;
                }

                if (kingBishopDiff != 0)
                {
                    for (int i = bishopPos; i != kingPos; i += kingBishopDiff)
                    {
                        Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                        LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = i;
                    }
                }
            }

            if (legal == 1)
                if (PiecesBetween())
                    legal = 0;

            checkInProgress = 1;
            return legal;
        }

        private int QueenCheck()
        {
            int legal = 0;
            if ((BishopCheck() == 1) || (RookCheck() == 1))
                legal = 1;

            if (legal == 1)
                if (PiecesBetween())
                    legal = 0;

            return legal;
        }

        private int KingCheck()
        {
            if (restoreStaticPieces)
            {
                staticOpponentPieces.CopyTo(oppPieces, 0);
                staticPieces.CopyTo(pieces, 0);
            }

            if (checkInProgress == 0)
            {
                string unusedString = CheckIfInCheck("king", true);
            }
            int legal = 0;
            checkTotal = checkIfUpLeft + checkIfUp + checkIfUpRight + checkIfRight + checkIfDownRight + checkIfDown + checkIfDownLeft + checkIfLeft;
            string moveProgress = Convert.ToString(moveInProgress);
            int curX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int curY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int nexX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int nexY = Int32.Parse(Convert.ToString(moveProgress[3]));

            if ((moveInProgress == 1518) || (moveInProgress == 1511) || (moveInProgress == 1418) || (moveInProgress == 1411))
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

            if (restoreStaticPieces)
            {
                staticOpponentPieces.CopyTo(oppPieces, 0);
                staticPieces.CopyTo(pieces, 0);
            }

            if ((checkInProgress == 1) && checkingCheck && !LoginVariables.opponentCheckArrayLock && LoginVariables.setCheckValues)
            {
                Array.Resize(ref LoginVariables.opponentCheckArray, LoginVariables.opponentCheckArray.Length + 1);
                LoginVariables.opponentCheckArray[LoginVariables.opponentCheckArray.Length - 1] = moveInProgress / 100;
            }

            checkInProgress = 1;
            return legal;
        }

        private int OthersStraight(string direction)
        {
            int legal = 1;
            string moveProgress = Convert.ToString(moveInProgress);
            int moveCurX = Int32.Parse(Convert.ToString(moveProgress[0]));
            int moveCurY = Int32.Parse(Convert.ToString(moveProgress[1]));
            int moveNextX = Int32.Parse(Convert.ToString(moveProgress[2]));
            int moveNextY = Int32.Parse(Convert.ToString(moveProgress[3]));

            string[] opp_Pieces = staticOpponentPieces;
            staticPieces.CopyTo(pieces, 0);
            string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = bothTeamsPieces[i].Split(':');
                string s = cur_Piece[3];
                if (direction == "up")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) < moveNextX) && (Int32.Parse(Convert.ToString(s[0])) > moveCurX) && (Int32.Parse(Convert.ToString(s[1])) == moveNextY))
                    {
                        legal = 0;
                        break;
                    }
                }
                else if (direction == "down")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) > moveNextX) && (Int32.Parse(Convert.ToString(s[0])) < moveCurX) && (Int32.Parse(Convert.ToString(s[1])) == moveNextY))
                    {
                        legal = 0;
                        break;
                    }
                }
                else if (direction == "left")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) == moveNextX) && (Int32.Parse(Convert.ToString(s[1])) > moveNextY) && (Int32.Parse(Convert.ToString(s[1])) < moveCurY))
                    {
                        legal = 0;
                        break;
                    }
                }
                else if (direction == "right")
                {
                    if ((Int32.Parse(Convert.ToString(s[0])) == moveNextX) && (Int32.Parse(Convert.ToString(s[1])) < moveNextY) && (Int32.Parse(Convert.ToString(s[1])) > moveCurY))
                    {
                        legal = 0;
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

            string[] opp_Pieces = staticOpponentPieces;
            staticPieces.CopyTo(pieces, 0);

            string[] bothTeamsPieces = pieces.Concat(opp_Pieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = bothTeamsPieces[i].Split(':');
                string s = cur_Piece[3];
                int diffX = moveNextX - Int32.Parse(Convert.ToString(s[0]));
                int diffY = moveNextY - Int32.Parse(Convert.ToString(s[1]));
                int curX = moveInProgress / 1000;
                int curY = (moveInProgress / 100) - (curX * 10);
                int diffCurX = curX - Int32.Parse(Convert.ToString(s[0]));
                int diffCurY = curY - Int32.Parse(Convert.ToString(s[1]));

                int absDiffX = Math.Abs(diffX);
                int absDiffY = Math.Abs(diffY);
                

                if (absDiffX == absDiffY)
                {
                    if (direction == "upleft")
                    {
                        if ((Math.Sign(diffX) == 1) && (Math.Sign(diffY) == -1) && (Math.Sign(diffCurX) == -1) && (Math.Sign(diffCurY) == 1))
                        {
                            legal = 0;
                            break;
                        }
                    }
                    else if (direction == "upright")
                    {
                        if ((Math.Sign(diffX) == 1) && (Math.Sign(diffY) == 1) && (Math.Sign(diffCurX) == -1) && (Math.Sign(diffCurY) == -1))
                        {
                            legal = 0;
                            break;
                        }
                    }
                    else if (direction == "downleft")
                    {
                        if ((Math.Sign(diffX) == -1) && (Math.Sign(diffY) == -1) && (Math.Sign(diffCurX) == 1) && (Math.Sign(diffCurY) == 1))
                        {
                            legal = 0;
                            break;
                        }
                    }
                    else if (direction == "downright")
                    {
                        if ((Math.Sign(diffX) == -1) && (Math.Sign(diffY) == 1) && (Math.Sign(diffCurX) == 1) && (Math.Sign(diffCurY) == -1))
                        {
                            legal = 0;
                            break;
                        }
                    }
                }
            }

            return legal;
        }

        private void SwitchArrays()
        {
            staticOpponentPieces.CopyTo(oppPieces, 0);
            staticPieces.CopyTo(pieces, 0);
            pieces.CopyTo(staticOpponentPieces, 0);
            oppPieces.CopyTo(staticPieces, 0);
        }

        private string CheckIfInCheck(string oursOrTheirs, bool changeCheckVals)
        {
            string testPieces;
            int checkingOurs = 0;

            int checkBackup = check;
            int checkIfUpBackup = checkIfUp;
            int checkIfUpRightBackup = checkIfUpRight;
            int checkIfRightBackup = checkIfRight;
            int checkIfDownRightBackup = checkIfDownRight;
            int checkIfDownBackup = checkIfDown;
            int checkIfDownLeftBackup = checkIfDownLeft;
            int checkIfLeftBackup = checkIfLeft;
            int checkIfUpLeftBackup = checkIfUpLeft;

            check = 0;
            checkIfUp = 0;
            checkIfUpRight = 0;
            checkIfRight = 0;
            checkIfDownRight = 0;
            checkIfDown = 0;
            checkIfDownLeft = 0;
            checkIfLeft = 0;
            checkIfUpLeft = 0;

            int falseCheckTotal = 0;

            restoreStaticPieces = false;
            int moveInProgressBackup = moveInProgress;
            string[] cur_Opp_Piece = null;

            if ((oursOrTheirs == "ours") || (oursOrTheirs == "king"))
            {
                SwitchArrays();
                checkingOurs = 1;
                LoginVariables.setCheckValues = true;
            }
            else
            {
                staticPieces.CopyTo(oppPieces, 0);
                staticOpponentPieces.CopyTo(pieces, 0);
            }

            testPieces = string.Join("£", staticOpponentPieces);

            cur_Opp_Piece = staticOpponentPieces[12].Split(':').ToArray();

            string opp_Piece_Pos = cur_Opp_Piece[3];
            if (oursOrTheirs != "king")
                moveInProgress = Int32.Parse(opp_Piece_Pos);
            else
                moveInProgress -= (moveInProgress / 100) * 100;

            int constMoveInProgress = moveInProgress;
            int checkTotalPrivate = 0;
            int safety = 0;

            for (int i = 0; i < 9; i++)
            {
                int change = 1;
                switch (i)
                {
                    case 1:

                        if (moveInProgress < 80)
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress + 10).ToString() + ":"))
                            {
                                safety = 1;
                                change = 1;
                                moveInProgress = constMoveInProgress += 10;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfUp = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfUp = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 2:

                        if (((moveInProgress - ((moveInProgress / 10) * 10)) < 8) && (moveInProgress < 80))
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress + 11).ToString() + ":"))
                            {
                                safety = 2;
                                change = 1;
                                moveInProgress = constMoveInProgress += 11;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfUpRight = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfUpRight = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 3:

                        if ((moveInProgress - ((moveInProgress / 10) * 10)) < 8)
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress + 1).ToString() + ":"))
                            {
                                safety = 3;
                                change = 1;
                                moveInProgress = constMoveInProgress += 1;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfRight = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfRight = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 4:

                        if (((moveInProgress - ((moveInProgress / 10) * 10)) < 8) && (moveInProgress > 20))
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress - 9).ToString() + ":"))
                            {
                                safety = 4;
                                change = 1;
                                moveInProgress = constMoveInProgress -= 9;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfDownRight = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfDownRight = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 5:

                        if (moveInProgress > 20)
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress - 10).ToString() + ":"))
                            {
                                safety = 5;
                                change = 1;
                                moveInProgress = constMoveInProgress -= 10;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfDown = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfDown = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 6:

                        if (((moveInProgress - ((moveInProgress / 10) * 10)) > 1) && (moveInProgress > 20))
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress - 11).ToString() + ":"))
                            {
                                safety = 6;
                                change = 1;
                                moveInProgress = constMoveInProgress -= 11;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfDownLeft = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfDownLeft = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 7:

                        if ((moveInProgress - ((moveInProgress / 10) * 10)) > 1)
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress - 1).ToString() + ":"))
                            {
                                safety = 7;
                                change = 1;
                                moveInProgress = constMoveInProgress -= 1;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfLeft = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfLeft = 1;
                            falseCheckTotal++;
                        }

                        break;
                    case 8:

                        if (((moveInProgress - ((moveInProgress / 10) * 10)) > 1) && (moveInProgress < 80))
                        {
                            SwitchArrays();
                            if (!testPieces.Contains(":" + (moveInProgress + 9).ToString() + ":"))
                            {
                                safety = 8;
                                change = 1;
                                moveInProgress = constMoveInProgress += 9;
                            }
                            else
                            {
                                moveInProgress = constMoveInProgress;
                                checkIfUpLeft = 1;
                                falseCheckTotal++;
                            }
                            SwitchArrays();
                        }
                        else
                        {
                            checkIfUpLeft = 1;
                            falseCheckTotal++;
                        }

                        break;
                }

                for (int j = 0; j < 16; j++)
                {
                    cur_Piece = oppPieces[j].Split(':').ToArray();
                    string s = cur_Piece[3];
                    if ((s != "99") && (change == 1))
                    {
                        moveInProgress += Int32.Parse(s) * 100;
                        checkingIfInCheck = 1;
                        checkingCheck = false;

                        switch (safety)
                        {
                            case 0: checkingCheck = true; check += TestMoveLegality(1, checkingOurs); break;
                            case 1: checkIfUp += TestMoveLegality(1, checkingOurs); break;
                            case 2: checkIfUpRight += TestMoveLegality(1, checkingOurs); break;
                            case 3: checkIfRight += TestMoveLegality(1, checkingOurs); break;
                            case 4: checkIfDownRight += TestMoveLegality(1, checkingOurs); break;
                            case 5: checkIfDown += TestMoveLegality(1, checkingOurs); break;
                            case 6: checkIfDownLeft += TestMoveLegality(1, checkingOurs); break;
                            case 7: checkIfLeft += TestMoveLegality(1, checkingOurs); break;
                            case 8: checkIfUpLeft += TestMoveLegality(1, checkingOurs); break;
                        }

                        checkingIfInCheck = 0;
                        moveInProgress = constMoveInProgress;
                    }
                }
                change = 0;
            }

            restoreStaticPieces = true;

            LoginVariables.setCheckValues = false;
            if (check > 0)
                check = 1;
            if (checkIfUp > 0)
                checkIfUp = 1;
            if (checkIfUpRight > 0)
                checkIfUpRight = 1;
            if (checkIfRight > 0)
                checkIfRight = 1;
            if (checkIfDownRight > 0)
                checkIfDownRight = 1;
            if (checkIfDown > 0)
                checkIfDown = 1;
            if (checkIfDownLeft > 0)
                checkIfDownLeft = 1;
            if (checkIfLeft > 0)
                checkIfLeft = 1;
            if (checkIfUpLeft > 0)
                checkIfUpLeft = 1;

            checkTotalPrivate = check + checkIfUp + checkIfUpRight + checkIfRight + checkIfDownRight + checkIfDown + checkIfDownLeft + checkIfLeft + checkIfUpLeft;

            string returnValue = "";
            if ((checkTotalPrivate == 8) && (check == 0) && (falseCheckTotal != 8))
                returnValue = "stm~";
            else if (checkTotalPrivate == 9)
                returnValue = "chm~";
            else if (((checkTotalPrivate < 8) && (checkTotalPrivate > 0)) || ((checkTotalPrivate == 8) && (check == 1)))
                returnValue = "chk~" + checkIfUpLeft + ":" + checkIfUp + ":" + checkIfUpRight + ":" + checkIfRight + ":" + checkIfDownRight + ":" + checkIfDown + ":" + checkIfDownLeft + ":" + checkIfLeft + ":" + check + "~";
            else
                returnValue = "mve~";

            moveInProgress = moveInProgressBackup;

            if (!changeCheckVals)
            {
                check = checkBackup;
                checkIfUp = checkIfUpLeftBackup;
                checkIfUpRight = checkIfUpLeftBackup;
                checkIfRight = checkIfUpLeftBackup;
                checkIfDownRight = checkIfUpLeftBackup;
                checkIfDown = checkIfUpLeftBackup;
                checkIfDownLeft = checkIfUpLeftBackup;
                checkIfLeft = checkIfUpLeftBackup;
                checkIfUpLeft = checkIfUpLeftBackup;
            }
            else
            {
                checkIfUp = 0;
                checkIfUpRight = 0;
                checkIfRight = 0;
                checkIfDownRight = 0;
                checkIfDown = 0;
                checkIfDownLeft = 0;
                checkIfLeft = 0;
                checkIfUpLeft = 0;
            }

            LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
            LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);

            staticPieces.CopyTo(pieces, 0);
            staticOpponentPieces.CopyTo(oppPieces, 0);

            return returnValue;
        }

        private int TestMoveLegality(int isCheckInProgress, int checkingOurs)
        {
            if ((moveAllowed == 1) || (checkingIfInCheck == 1))
            {
                int legal = 0;
                string testingMove = Convert.ToString(moveInProgress);

                staticPieces.CopyTo(pieces, 0);

                int curPieceInt = ((string.Join(",", pieces).Replace(",", "").IndexOf(":" + testingMove[0].ToString() + testingMove[1].ToString() + ":") + 2) / 16);
                
                cur_Piece = pieces[curPieceInt].Split(':');

                checkInProgress = isCheckInProgress;
                switch (cur_Piece[0])
                {
                    case "1Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "2Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "3Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "4Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "5Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "6Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "7Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "8Pa": legal = PawnCheck(checkingOurs, moveInProgress); break;
                    case "1Ro": legal = RookCheck(); break;
                    case "1Kn": legal = KnightCheck(); break;
                    case "1Bi": legal = BishopCheck(); break;
                    case "Kin": legal = KingCheck(); break;
                    case "Que": legal = QueenCheck(); break;
                    case "2Bi": legal = BishopCheck(); break;
                    case "2Kn": legal = KnightCheck(); break;
                    case "2Ro": legal = RookCheck(); break;
                }
                checkInProgress = 0;

                return legal;
            }
            else
                return 2;
        }

        private void test_move()
        {
            if (replaying)
            {
                moveInProgress = 0;
                return;
            }

            if (saving)
            {
                MessageBox.Show("Cannot perform move whilst save is in progress", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                moveInProgress = 0;
                return;
            }

            LoginVariables.dataMoveInProgress = moveInProgress.ToString();
            testCorrect = 0;
            pieceMoving = null;
            if (gameStart == 0)
                return;

            testCorrect = TestMoveLegality(0, 0);

            int curStatPieceIndex = Array.FindIndex(staticPieces, element => element.Contains((moveInProgress / 100).ToString()));
            if ((curStatPieceIndex != -1) && (testCorrect == 1))
            {
                int moveInProgressBackup = moveInProgress;
                int oppCurStatPieceIndex = 0;
                string[] oppCurStatPiece = null;
                string[] curStatPiece = staticPieces[curStatPieceIndex].Split(':').ToArray();
                curStatPiece[3] = (moveInProgress - ((moveInProgress / 100) * 100)).ToString();
                staticPieces[curStatPieceIndex] = string.Join(":", curStatPiece);
                string backup = "";

                bool existsInArray = Array.Exists(staticOpponentPieces, element => element.Contains((moveInProgress - ((moveInProgress / 100) * 100)).ToString()));
                if (existsInArray)
                {
                    oppCurStatPieceIndex = Array.FindIndex(staticOpponentPieces, element => element.Contains((moveInProgress - ((moveInProgress / 100) * 100)).ToString()));
                    oppCurStatPiece = staticOpponentPieces[curStatPieceIndex].Split(':').ToArray();
                    oppCurStatPiece[3] = "99";
                    backup = staticOpponentPieces[oppCurStatPieceIndex];
                    staticOpponentPieces[oppCurStatPieceIndex] = string.Join(":", oppCurStatPiece);
                }

                bool rookGood = false;
                bool kingGood = false;

                string kingString = "";
                string rookString = "";
                foreach(string element in staticPieces)
                {
                    string[] elementArray = element.Split(':').ToArray();
                    if((moveInProgress == 1511) || (moveInProgress == 1411))
                    {
                        if ((elementArray[0] == "1Ro") && (elementArray[4] == "0"))
                        {
                            rookGood = true;
                            rookString = string.Join(":", elementArray);
                        }
                        else if ((elementArray[0] == "Kin") && (elementArray[4] == "0"))
                        {
                            kingGood = true;
                            kingString = string.Join(":", elementArray);
                        }
                    }
                    else if ((moveInProgress == 1518) || (moveInProgress == 1418))
                    {
                        if ((elementArray[0] == "2Ro") && (elementArray[4] == "0"))
                        { 
                            rookGood = true;
                            rookString = string.Join(":", elementArray);
                        }
                        else if ((elementArray[0] == "Kin") && (elementArray[4] == "0"))
                        { 
                            kingGood = true;
                            kingString = string.Join(":", elementArray);
                        }
                    }
                }

                if(rookGood && kingGood)
                {
                    string[] kingPiecesChange = staticPieces[Array.IndexOf(staticPieces, kingString)].Split(':').ToArray();
                    string[] rookPiecesChange = staticPieces[Array.IndexOf(staticPieces, rookString)].Split(':').ToArray();

                    switch (moveInProgress)
                    {
                        case 1418: 
                            moveInProgress = 1416;
                            kingPiecesChange[3] = "16";
                            rookPiecesChange[3] = "15";
                            break;
                        case 1411: 
                            moveInProgress = 1412;
                            kingPiecesChange[3] = "12";
                            rookPiecesChange[3] = "13";
                            break;
                        case 1518: 
                            moveInProgress = 1517;
                            kingPiecesChange[3] = "17";
                            rookPiecesChange[3] = "16";
                            break;
                        case 1511: 
                            moveInProgress = 1513;
                            kingPiecesChange[3] = "13";
                            rookPiecesChange[3] = "14";
                            break;
                    }

                    staticPieces[Array.IndexOf(staticPieces, kingString)] = string.Join(":", kingPiecesChange);
                    staticPieces[Array.IndexOf(staticPieces, rookString)] = string.Join(":", rookPiecesChange);
                }
                string unusedString = CheckIfInCheck("ours", true);

                moveInProgress = moveInProgressBackup;

                LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
                LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);

                if ((check == 1) && (testCorrect == 1))
                    testCorrect = 3;
            }
            else if (testCorrect == 1)
                testCorrect = 0;

            if (testCorrect == 3)
            {
                MessageBox.Show("That move would place or keep your King in check, and so is illegal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                illegalAction = 0;
                moveCoordinates = "";
                moveInProgress = 0;
                check = 0;
                
                return;
            }
            else if (testCorrect == 2)
            {
                MessageBox.Show("Illegal Action", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                illegalAction = 0;
                moveCoordinates = "";
                moveInProgress = 0;
                check = 0;
                
                return;
            }
            else if (testCorrect == 0)
            {
                MessageBox.Show("Illegal Move", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                illegalAction = 0;
                moveCoordinates = "";
                moveInProgress = 0;
                check = 0;
                
                return;
            }
            else
            {
                foreach (string element in LoginVariables.Player1_Current)
                {
                    string[] elementArray = element.Split(':').ToArray();
                    if (elementArray[3] == (moveInProgress / 100).ToString())
                    {
                        pieceMoving = elementArray[0][1].ToString() + elementArray[0][2].ToString();
                        break;
                    }
                }
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

                for (int i = 0; i < buttonArray.Length; i++)
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
                    staticPieces.CopyTo(pieces, 0);
                    string[] otherPieces = staticOpponentPieces;
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
                            SendMoveToOtherPlayer();
                            return;
                        }
                    }
                }

                if ((staticOpponentPieces).ToString().IndexOf(buttonArray[cur_position].Name) != -1)
                {
                    MessageBox.Show("Illegal move. You may not move an opponent's piece", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    illegalAction = 1;
                }

                if (buttonArray[cur_position].Image == null)
                {
                    MessageBox.Show("Illegal move. You may not move a blank square", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    illegalAction = 1;
                    cur_position = nex_position;
                }

                if((string.Join(",", staticPieces).Replace(",", "").IndexOf(":" + buttonArray[nex_position].Name + ":") != -1) && (illegalAction == 0))
                {
                    MessageBox.Show("Illegal move. You may not capture one of your own pieces", "Illegal Move", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    illegalAction = 1;
                }

                if (illegalAction != 0)
                {
                    illegalAction = 0;
                    moveInProgress = 0;
                    
                    return;
                }

                loadGameButton.Enabled = false;

                transitionBox.Image = buttonArray[nex_position].Image;
                int curPiecePosition = 0;
                int opponentPositionImage = 0;
                int nextCoordinates = 0;

                if (enPassant == 1)
                {
                    nextCoordinates = moveInProgress - ((moveInProgress / 100) * 100) - 10;

                    int piecesPosition = (string.Join(",", staticOpponentPieces).Replace(",", "").IndexOf(":" + nextCoordinates.ToString() + ":") + 2) / 16;

                    cur_Piece = staticOpponentPieces[piecesPosition].Split(':');


                    for (int i = 0; i < buttonArray.Length; i++)
                    {
                        if (buttonArray[i].Name == cur_Piece[2])
                        {
                            opponentPositionImage = i;
                            break;
                        }
                    }

                    transitionBox.Image = buttonArray[opponentPositionImage].Image;
                    nextCoordinates = opponentPositionImage;
                }

                if (transitionBox.Image != null)
                {
                    if (enPassant == 0)
                        curPiecePosition = (string.Join(",", staticOpponentPieces).Replace(",", "").IndexOf(":" + buttonArray[nex_position].Name + ":") + 2) / 16;
                    else
                        curPiecePosition = (string.Join(",", staticOpponentPieces).Replace(",", "").IndexOf(":" + buttonArray[nextCoordinates].Name + ":") + 2) / 16;

                    enPassant = 0;
                    enPassantImpossible = 0;

                    cur_Piece = staticOpponentPieces[curPiecePosition].Split(':');

                    switch (cur_Piece[0])
                    {
                        case "1Pa": cur_Piece[2] = "BP"; break;
                        case "2Pa": cur_Piece[2] = "BP"; break;
                        case "3Pa": cur_Piece[2] = "BP"; break;
                        case "4Pa": cur_Piece[2] = "BP"; break;
                        case "5Pa": cur_Piece[2] = "BP"; break;
                        case "6Pa": cur_Piece[2] = "BP"; break;
                        case "7Pa": cur_Piece[2] = "BP"; break;
                        case "8Pa": cur_Piece[2] = "BP"; break;
                        case "1Ro": cur_Piece[2] = "BR"; break;
                        case "1Kn": cur_Piece[2] = "BK"; break;
                        case "1Bi": cur_Piece[2] = "BB"; break;
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
                        case "2Bi": cur_Piece[2] = "BB"; break;
                        case "2Kn": cur_Piece[2] = "BK"; break;
                        case "2Ro": cur_Piece[2] = "BR"; break;
                    }                    

                    cur_Piece[1] = "A";
                    cur_Piece[3] = "99";
                    cur_Piece[4] = "1";
                    staticOpponentPieces[curPiecePosition] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                    staticOpponentPieces.CopyTo(LoginVariables.Player2_Current, 0);
                }

                staticPieces.CopyTo(pieces, 0);
                staticOpponentPieces.CopyTo(oppPieces, 0);

                curPiecePosition = (string.Join(",", staticPieces).Replace(",", "").IndexOf(":" + buttonArray[cur_position].Name + ":") + 2) / 16;
                cur_Piece = staticPieces[curPiecePosition].Split(':');

                cur_Piece[1] = "A";
                cur_Piece[2] = buttonArray[nex_position].Name;
                string next = Convert.ToString(moveInProgress - ((moveInProgress / 100) * 100));
                cur_Piece[3] = next;
                cur_Piece[4] = "1";
                pieces[curPiecePosition] = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                pieces.CopyTo(LoginVariables.Player1_Current, 0);

                staticPieces.CopyTo(pieces, 0);

                curPiecePosition = (string.Join(",", staticPieces).Replace(",", "").IndexOf("Pa") + 1) / 16;
                if (curPiecePosition != -1)
                    cur_Piece = staticPieces[curPiecePosition].Split(':');

                string pieceName = cur_Piece[0];
                string pieceLocation = cur_Piece[2];

                if ((pieceMoving == "Pa") && (((moveInProgress - ((moveInProgress / 100) * 100)) / 10) == 8))
                {
                    pawnChangePositionString = pieceLocation[0].ToString() + pieceLocation[1].ToString();
                    pawnChangePositionInt = Int32.Parse(cur_Piece[4]);
                    SetUpBoard(playerState, false);

                    pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                    knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                    rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                    bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                    queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                    oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                    oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                    oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                    oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                    oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();

                    foreach (Button element in buttonArray)
                    {
                        element.Enabled = false;
                    }

                    chat_button.Enabled = true;
                    concedeButton.Enabled = true;
                    disconnect_button.Enabled = true;
                    knightSelectButton.Enabled = true;
                    rookSelectButton.Enabled = true;
                    bishopSelectButton.Enabled = true;
                    queenSelectButton.Enabled = true;

                    MessageBox.Show("Please select the piece to which you wish to promote your pawn");
                    pawnChange = 1;
                    changePieceAllowed = pieceName + ":" + buttonArray[nex_position].Name + ":" + nextPlace;
                    moveAllowed = 0;
                    return;
                }

                transitionBox.Image = null;
                SendMoveToOtherPlayer();
                SetUpBoard(playerState, false);

                pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();
            }
        }


        private string CheckStalemateAndCheckmate(string dataToSendToOpponent, string oursOrTheirs)
        {
            if ((dataToSendToOpponent == "stm~") || (dataToSendToOpponent == "chm~"))
            {
                int[] dist = LoginVariables.opponentCheckArray.Distinct().ToArray();
                string dataString = "";
                LoginVariables.opponentCheckArrayLock = true;
                bool breakFromForeach = false;
                foreach (int arrayElement in dist)
                {
                    if (arrayElement != 0)
                    {
                        foreach (string pieceElement in staticOpponentPieces)
                        {
                            string[] elementComponents = pieceElement.Split(':').ToArray();
                            moveInProgress = Int32.Parse(elementComponents[3]) * 100 + arrayElement;
                            SwitchArrays();
                            checkingIfInCheck = 1;
                            if (TestMoveLegality(1, 0) == 1)
                            {
                                LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
                                LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);
                                staticPieces.CopyTo(pieces, 0);
                                staticOpponentPieces.CopyTo(oppPieces, 0);

                                elementComponents[3] = arrayElement.ToString();
                                staticOpponentPieces[Array.IndexOf(staticOpponentPieces, pieceElement)] = string.Join(":", elementComponents);
                                dataString = CheckIfInCheck(oursOrTheirs, true);
                                string dataStringFirstFour = dataString[0].ToString() + dataString[1].ToString() + dataString[2].ToString() + dataString[3].ToString();
                                if ((dataStringFirstFour == "chk~") || (dataStringFirstFour == "mve~"))
                                {
                                    string[] dataArray = dataString.Split(':').ToArray();
                                    string[] allPieces = dataString.Split('~').ToArray();
                                    string[] checkValues = allPieces[1].Split(':').ToArray();
                                    string checkValuesOld = allPieces[1];
                                    dataArray[0] = dataStringFirstFour;
                                    breakFromForeach = true;
                                    if (dataToSendToOpponent == "chm~")
                                    {
                                        check = 1;
                                        checkValues[8] = "1~";
                                    }
                                    else
                                    {
                                        check = 0;
                                        checkValues[8] = "0~";
                                    }

                                    dataString = dataArray[0] + string.Join(":", checkValues);
                                    break;
                                }
                                LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);
                                Array.Resize(ref staticOpponentPieces, 16);
                            }
                            else
                            {
                                LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
                                LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);
                                staticPieces.CopyTo(pieces, 0);
                                staticOpponentPieces.CopyTo(oppPieces, 0);
                            }

                            checkingIfInCheck = 0;
                        }
                    }
                    if (breakFromForeach)
                        break;
                }

                if (breakFromForeach)
                    dataToSendToOpponent = dataString;
            }

            return dataToSendToOpponent;
        }
        private void SendMoveToOtherPlayer()
        {
            if (gameStart == 0)
                return;

            AddToReplayFile(moveInProgress);

            string messageBoxInformation = "";
            int moveInProgressBackup = moveInProgress;
            moveInProgress = 0;

            staticOpponentPieces.CopyTo(oppPieces, 0);
            staticPieces.CopyTo(pieces, 0);

            string dataToSendToOpponent = CheckIfInCheck("theirs", true);
            LoginVariables.opponentCheckArray = new int[0];

            dataToSendToOpponent = CheckStalemateAndCheckmate(dataToSendToOpponent, "theirs");

            LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
            LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);

            Array.Resize(ref staticPieces, 16);
            Array.Resize(ref staticOpponentPieces, 16);

            LoginVariables.opponentCheckArray = new int[0];
            LoginVariables.opponentCheckArrayLock = false;
            
            moveAllowed = 0;
            moveInProgress = 0;

            staticOpponentPieces.CopyTo(oppPieces, 0);
            staticPieces.CopyTo(pieces, 0);

            string[] work = pieces.Concat(oppPieces).ToArray();

            for (int i = 0; i < 32; i++)
            {
                cur_Piece = work[i].Split(':').ToArray();

                ReverseFile(2);
                ReverseFile(3);
                ReverseFile(5);
                string writer = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                if (i == 15)
                    writer += "~";
                else
                    writer += ";";

                dataToSendToOpponent += writer;
            }

            moveInProgress = moveInProgressBackup;

            dataToSendToOpponent += "~" + moveInProgress;
            SendData(dataToSendToOpponent, true);
            saveGameButton.Enabled = true;
            replayGameButton.Enabled = true;
            moveInProgress = 0;

            string dataSent = dataToSendToOpponent[0].ToString() + dataToSendToOpponent[1].ToString() + dataToSendToOpponent[2].ToString() + dataToSendToOpponent[3].ToString();

            if (dataSent == "stm~")
                messageBoxInformation = "A stalemate has been declared. Congratulations " + name + "!";
            else if (dataSent == "chm~")
                messageBoxInformation = "A checkmate has been declared. Congratulations " + name + "!";
            else if (check == 1)
                messageBoxInformation = opponentName + " is in check.";
            check = 0;

            if (messageBoxInformation != "")
            {
                if ((dataSent == "stm~") || (dataSent == "chm~"))
                {
                    chessWinSound.Play();
                    foreach (Button element in buttonArray)
                    {
                        if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                            element.Enabled = false;
                        else
                            element.Enabled = true;
                    }
                    disconnect_button.Text = "Disconnect";
                    LoginVariables.enableDraw = false;
                    RenameReplayFile();
                }

                MessageBox.Show(messageBoxInformation);
                messageBoxInformation = "";
            }
        }

        private void ReverseFile(int place)
        {
            char cur_Piece_Half_1 = cur_Piece[place][0];
            char cur_Piece_Half_2 = cur_Piece[place].Last();
            char cur_Piece_Half_2_Orig = cur_Piece_Half_2;
            int triedParse = 0;

            switch (cur_Piece_Half_2)
            {
                case '1': cur_Piece_Half_2 = '8'; break;
                case '2': cur_Piece_Half_2 = '7'; break;
                case '3': cur_Piece_Half_2 = '6'; break;
                case '4': cur_Piece_Half_2 = '5'; break;
                case '5': cur_Piece_Half_2 = '4'; break;
                case '6': cur_Piece_Half_2 = '3'; break;
                case '7': cur_Piece_Half_2 = '2'; break;
                case '8': cur_Piece_Half_2 = '1'; break;
                case 'R': cur_Piece_Half_2 = 'L'; break;
                case 'L': cur_Piece_Half_2 = 'R'; break;
            }

            if (int.TryParse(cur_Piece_Half_2.ToString(), out triedParse) && (cur_Piece[place].Length == 2))
            {
                switch (cur_Piece_Half_1)
                {
                    case 'A': cur_Piece_Half_1 = 'H'; break;
                    case 'B': cur_Piece_Half_1 = 'G'; break;
                    case 'C': cur_Piece_Half_1 = 'F'; break;
                    case 'D': cur_Piece_Half_1 = 'E'; break;
                    case 'E': cur_Piece_Half_1 = 'D'; break;
                    case 'F': cur_Piece_Half_1 = 'C'; break;
                    case 'G': cur_Piece_Half_1 = 'B'; break;
                    case 'H': cur_Piece_Half_1 = 'A'; break;
                    case '1': cur_Piece_Half_1 = '8'; break;
                    case '2': cur_Piece_Half_1 = '7'; break;
                    case '3': cur_Piece_Half_1 = '6'; break;
                    case '4': cur_Piece_Half_1 = '5'; break;
                    case '5': cur_Piece_Half_1 = '4'; break;
                    case '6': cur_Piece_Half_1 = '3'; break;
                    case '7': cur_Piece_Half_1 = '2'; break;
                    case '8': cur_Piece_Half_1 = '1'; break;
                }

                cur_Piece[place] = cur_Piece_Half_1.ToString() + cur_Piece_Half_2.ToString();
            }
            else
            {
                if(cur_Piece[place].Length == 3)
                {
                    cur_Piece[place] = cur_Piece[place][0].ToString() + cur_Piece[place][1].ToString() + cur_Piece_Half_2.ToString();
                    if (cur_Piece[place][0] == 'B')
                        cur_Piece[place] = "W" + cur_Piece[place][1].ToString() + cur_Piece[place][2].ToString();
                    else
                        cur_Piece[place] = "B" + cur_Piece[place][1].ToString() + cur_Piece[place][2].ToString();
                }
                else
                {
                    if (cur_Piece[place][0] == 'B')
                        cur_Piece[place] = "W" + cur_Piece[place][1].ToString();
                    else
                        cur_Piece[place] = "B" + cur_Piece[place][1].ToString();
                }
            }

        }

        private void StartOfCastle(string direction)
        {
            if (gameStart == 0)
                return;

            if (direction == "left")
            {
                staticPieces.CopyTo(pieces, 0);
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

                        pieces.CopyTo(LoginVariables.Player1_Current, 0);
                    }
                    else
                    {
                        string[] staticPiecesBackup = new string[16];
                        staticPieces.CopyTo(staticPiecesBackup, 0);
                        staticPieces[Array.IndexOf(pieces, "Kin:A:A4:14:0:A4")] = "Kin:A:A2:12:1:A4";
                        string unusedString = CheckIfInCheck("ours", true);
                        staticPiecesBackup.CopyTo(staticPieces, 0);
                        if (check == 0)
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

                            pieces.CopyTo(LoginVariables.Player1_Current, 0);
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
                return;
            }
            else
            {
                staticPieces.CopyTo(pieces, 0);
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

                        pieces.CopyTo(LoginVariables.Player1_Current, 0);
                    }
                    else
                    {
                        string[] staticPiecesBackup = new string[16];
                        staticPieces.CopyTo(staticPiecesBackup, 0);
                        staticPieces[Array.IndexOf(pieces, "Kin:A:A5:15:0:A5")] = "Kin:A:A7:17:1:A5";
                        string unusedString = CheckIfInCheck("ours", true);
                        staticPiecesBackup.CopyTo(staticPieces, 0);
                        if (check == 0)
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

                            pieces.CopyTo(LoginVariables.Player1_Current, 0);
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

        private static void CreatePlayerCurrentTXTs(bool copyToLoginVariables)
        {
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
            string[] primOrigins = Origins;
            Array.Resize(ref primOrigins, 16);

            if (copyToLoginVariables)
                primOrigins.CopyTo(LoginVariables.Player1_Current, 0);
            else
                primOrigins.CopyTo(staticPieces, 0);

            string[] secOrigins = Origins.Reverse().ToArray();
            Array.Resize(ref secOrigins, 16);
            secOrigins = secOrigins.Reverse().ToArray();

            if (copyToLoginVariables)
                secOrigins.CopyTo(LoginVariables.Player2_Current, 0);
            else
                secOrigins.CopyTo(staticOpponentPieces, 0);
        }

        private void knightSelectButton_Click(object sender, EventArgs e)
        {
            if (!replaying)
                return_Piece("knight");
        }

        private void rookSelectButton_Click(object sender, EventArgs e)
        {
            if (!replaying)
                return_Piece("rook");
        }

        private void bishopSelectButton_Click(object sender, EventArgs e)
        {
            if (!replaying)
                return_Piece("bishop");
        }

        private void queenSelectButton_Click(object sender, EventArgs e)
        {
            if (!replaying)
                return_Piece("queen");
        }

        private void MainGame_Load(object sender, EventArgs e)
        {

        }

        private void saveGameButton_Click(object sender, EventArgs e)
        {
            if (replaying)
                return;

            if (moveInProgress == 0)
            {
                try
                {
                    saving = true;
                    string fullPath = "";
                    string fileName = "";
                    string directory = "";

                    saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    DialogResult result = saveFileDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        directory = Path.GetDirectoryName(saveFileDialog.FileName);
                        fileName = Path.GetFileName(saveFileDialog.FileName);
                        if (Path.GetExtension(saveFileDialog.FileName) != ".chess")
                            fileName += ".chess";
                        fullPath = directory + @"\" + fileName;
                        if (File.Exists(fullPath))
                        {
                            DialogResult messageResult = MessageBox.Show(fileName + " already exists.\r\nDo you want to replace it?                          ", "Confirm Save As", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (messageResult == DialogResult.No)
                                return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                        return;

                    string[] Player1_Current = new string[16];
                    string[] Player2_Current = new string[16];
                    LoginVariables.Player1_Current.CopyTo(Player1_Current, 0);
                    LoginVariables.Player2_Current.CopyTo(Player2_Current, 0);
                    string dataMoveInProgress = LoginVariables.dataMoveInProgress;
                    string dataOppMoveInProgress = LoginVariables.dataOppMoveInProgress;
                    string[] dataToSave = new string[10];
                    dataToSave[0] = playerState;

                    if (moveAllowed == 1)
                        dataToSave[1] = playerState;
                    else if (playerState == "white")
                        dataToSave[1] = "black";
                    else
                        dataToSave[1] = "white";

                    dataToSave[2] = string.Join(";", Player1_Current);
                    dataToSave[3] = string.Join(";", Player2_Current);

                    staticOpponentPieces.CopyTo(oppPieces, 0);
                    staticPieces.CopyTo(pieces, 0);

                    string[] work = pieces.Concat(oppPieces).ToArray();
                    int dataToSaveIndex = 5;

                    for (int i = 0; i < 32; i++)
                    {
                        cur_Piece = work[i].Split(':').ToArray();

                        ReverseFile(2);
                        ReverseFile(3);
                        ReverseFile(5);
                        string writer = cur_Piece[0] + ":" + cur_Piece[1] + ":" + cur_Piece[2] + ":" + cur_Piece[3] + ":" + cur_Piece[4] + ":" + cur_Piece[5];

                        if (i == 16)
                            dataToSaveIndex--;

                        writer += ";";

                        dataToSave[dataToSaveIndex] += writer;
                    }

                    moveInProgress = Int32.Parse(dataMoveInProgress);
                    dataToSave[6] = dataOppMoveInProgress;
                    dataToSave[7] = dataMoveInProgress;
                    string dataToSave8 = CheckIfInCheck("ours", false);
                    string dataToSave9 = CheckIfInCheck("theirs", false);

                    dataToSave8 = CheckStalemateAndCheckmate(dataToSave8, "ours");
                    dataToSave9 = CheckStalemateAndCheckmate(dataToSave9, "theirs");

                    for (int i = 0; i < dataToSave8.Length - 1; i++)
                        dataToSave[8] += dataToSave8[i].ToString();
                    for (int i = 0; i < dataToSave9.Length - 1; i++)
                        dataToSave[9] += dataToSave9[i].ToString();

                    moveInProgress = 0;

                    String dataToSaveString = string.Join("~", dataToSave) + "@";
                    byte[] saveStringBytes = Encoding.ASCII.GetBytes(dataToSaveString);
                    int rows = saveStringBytes.Length / 40;
                    int columns = saveStringBytes.Length - (rows * 40);

                    if (columns != 0)
                        rows++;

                    byte[] pixels = new byte[1600];
                    int pos = 0;
                    for (int i = 0; i < 1600; i++)
                    {
                        pos = i;
                        if (pos >= saveStringBytes.Length)
                            pos -= saveStringBytes.Length;
                        pixels[i] = saveStringBytes[pos];
                    }

                    Bitmap dataToBitmap = new Bitmap(40, 40, PixelFormat.Format8bppIndexed);
                    BitmapData data = dataToBitmap.LockBits(new Rectangle(0, 0, 40, 40), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    Int64 scan0 = data.Scan0.ToInt64();
                    for (Int32 y = 0; y < 40; y++)
                        Marshal.Copy(pixels, y * 40, new IntPtr(scan0 + y * data.Stride), 40);
                    dataToBitmap.UnlockBits(data);

                    ColorPalette palette = dataToBitmap.Palette;
                    for (int i = 0; i < 256; i++)
                        palette.Entries[i] = Color.FromArgb(LoginVariables.coloursArray[i, 0], LoginVariables.coloursArray[i, 1], LoginVariables.coloursArray[i, 2]);
                    dataToBitmap.Palette = palette;

                    dataToBitmap.Save(fullPath);

                    saving = false;
                }
                catch
                {
                    MessageBox.Show("Unknown Error", "Save File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Move is currently in progress. Save cannot be completed", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadGameFromFile(string dataToLoad)
        {
            try
            {
                int indexSubtract = 0;
                string[] loadedData = dataToLoad.Split('~');
                if ((loadedData[0] == "white") || (loadedData[0] == "black"))
                    indexSubtract = 1;
                string saverState = loadedData[1 - indexSubtract];
                string legalState = loadedData[2 - indexSubtract];
                string checkTest = "";
                string oppCheckTest = "";
                string otherXs = "";
                int checkTestNum = 0;
                int oppCheckTestNum = 0;
                if (saverState == playerState)
                {
                    string[] loadedData3 = loadedData[3 - indexSubtract].Split(';').ToArray();
                    string[] loadedData4 = loadedData[4 - indexSubtract].Split(';').ToArray();
                    Array.Resize(ref loadedData3, 16);
                    loadedData3.CopyTo(LoginVariables.Player1_Current, 0);
                    Array.Resize(ref loadedData4, 16);
                    loadedData4.CopyTo(LoginVariables.Player2_Current, 0);
                    oppCheckTest = loadedData[9 - indexSubtract];
                    oppCheckTestNum = 9;
                    if (oppCheckTest == "chk")
                    {
                        checkTest = loadedData[11 - indexSubtract];
                        checkTestNum = 11;
                    }
                    else
                    {
                        checkTest = loadedData[10 - indexSubtract];
                        checkTestNum = 10;
                    }

                    if (legalState == playerState)
                    {
                        otherXs = loadedData[7 - indexSubtract];
                        oppCurX = otherXs[0];
                        oppNextX = otherXs[2];
                        LoginVariables.dataOppMoveInProgress = otherXs;
                        LoginVariables.oppPreviousPos = Int32.Parse(otherXs[0].ToString() + otherXs[1].ToString());
                        LoginVariables.oppCurrentPos = Int32.Parse(otherXs[2].ToString() + otherXs[3].ToString());
                        moveAllowed = 1;
                    }
                    else
                    {
                        moveAllowed = 0;
                    }
                }
                else
                {
                    string[] loadedData5 = loadedData[6 - indexSubtract].Split(';').ToArray();
                    string[] loadedData6 = loadedData[5 - indexSubtract].Split(';').ToArray();
                    Array.Resize(ref loadedData5, 16);
                    loadedData5.CopyTo(LoginVariables.Player2_Current, 0);
                    Array.Resize(ref loadedData6, 16);
                    loadedData6.CopyTo(LoginVariables.Player1_Current, 0);
                    checkTest = loadedData[9 - indexSubtract];
                    checkTestNum = 9;
                    if (checkTest == "chk")
                    {
                        oppCheckTest = loadedData[11 - indexSubtract];
                        oppCheckTestNum = 11;
                    }
                    else
                    {
                        oppCheckTest = loadedData[10 - indexSubtract];
                        oppCheckTestNum = 10;
                    }

                    if (legalState == playerState)
                    {
                        otherXs = loadedData[8 - indexSubtract];
                        oppCurX = otherXs[0];
                        oppNextX = otherXs[2];
                        LoginVariables.dataOppMoveInProgress = otherXs;
                        LoginVariables.oppPreviousPos = Int32.Parse(otherXs[0].ToString() + otherXs[1].ToString());
                        LoginVariables.oppCurrentPos = Int32.Parse(otherXs[2].ToString() + otherXs[3].ToString());
                        moveAllowed = 1;
                    }
                    else
                    {
                        moveAllowed = 0;
                    }
                }

                if (checkTest == "chk")
                {
                    checkTestNum++;
                    string[] checkTestArray = loadedData[checkTestNum - indexSubtract].Split(':').ToArray();
                    checkIfUpLeft = Convert.ToInt32(checkTestArray[0]);
                    checkIfUp = Convert.ToInt32(checkTestArray[1]);
                    checkIfUpRight = Convert.ToInt32(checkTestArray[2]);
                    checkIfRight = Convert.ToInt32(checkTestArray[3]);
                    checkIfDownRight = Convert.ToInt32(checkTestArray[4]);
                    checkIfDown = Convert.ToInt32(checkTestArray[5]);
                    checkIfDownLeft = Convert.ToInt32(checkTestArray[6]);
                    checkIfLeft = Convert.ToInt32(checkTestArray[7]);
                    check = Convert.ToInt32(checkTestArray[8]);

                    if (check == 1)
                        MessageBox.Show("Check");
                }

                if (oppCheckTest == "chk")
                {
                    oppCheckTestNum++;
                    string[] checkTestArray = loadedData[oppCheckTestNum - indexSubtract].Split(':').ToArray();
                    int oppCheck = Convert.ToInt32(checkTestArray[8]);

                    if (oppCheck == 1)
                        MessageBox.Show(opponentName + " is in check");
                }

                Array.Resize(ref LoginVariables.Player1_Current, 16);
                Array.Resize(ref LoginVariables.Player2_Current, 16);
                LoginVariables.Player1_Current.CopyTo(pieces, 0);
                LoginVariables.Player2_Current.CopyTo(oppPieces, 0);

                SetUpBoard(playerState, false);

                pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();
            }
            catch
            {
                MessageBox.Show("Invalid or corrupt file", "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadGameButton_Click(object sender, EventArgs e)
        {
            if (replaying)
                return;
            try
            {
                string fullPath = "";
                string fileName = "";
                string directory = "";
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    directory = Path.GetDirectoryName(openFileDialog.FileName);
                    fileName = Path.GetFileName(openFileDialog.FileName);
                    fullPath = directory + @"\" + fileName;
                }
                else if (result == DialogResult.Cancel)
                    return;

                Bitmap image = new Bitmap(fullPath);

                ColorPalette palette = image.Palette;
                for (int i = 0; i < 256; i++)
                    palette.Entries[i] = Color.FromArgb(LoginVariables.coloursArray[i, 0], LoginVariables.coloursArray[i, 1], LoginVariables.coloursArray[i, 2]);
                image.Palette = palette;

                BitmapData imageData = image.LockBits(new Rectangle(0, 0, 40, 40), ImageLockMode.ReadOnly, image.PixelFormat);
                IntPtr ptr = imageData.Scan0;
                Int64 scan0 = imageData.Scan0.ToInt64();
                byte[] loadedData = new byte[40];
                byte[] allBytes = new byte[0];
                for (Int32 y = 0; y < 40; y++)
                {
                    ptr = new IntPtr(scan0 + y * imageData.Stride);
                    loadedData = new byte[40];
                    Marshal.Copy(ptr, loadedData, 0, 40);
                    allBytes = allBytes.Concat(loadedData).ToArray();
                }

                string loadedString = Encoding.ASCII.GetString(allBytes);
                loadedString = loadedString.Split('@').ToArray()[0];
                string[] loadedStringArray = new string[2];
                loadedStringArray[0] = loadedString.Substring(0, 1020);
                loadedStringArray[1] = loadedString.Substring(1020, loadedString.Length - 1020);
                SendData("~~~~", false);
                SendData("rqs~" + loadedStringArray[0], false);
                SendData(loadedStringArray[1], false);
                SendData("~~~~", false);

                LoginVariables.savedData = loadedString;
            }
            catch
            {
                MessageBox.Show("Invalid or corrupt file", "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void concedeButton_Click(object sender, EventArgs e)
        {
            if (replaying)
                return;

            DialogResult concedeResult = MessageBox.Show("Are you sure you want to resign? The game cannot be started again from its current position.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (concedeResult == DialogResult.Yes)
            {
                chessDrawSound.Play();
                SendData("drw~", false);
                foreach (Button element in buttonArray)
                {
                    if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                        element.Enabled = false;
                    else
                        element.Enabled = true;
                }
                disconnect_button.Text = "Disconnect";
                LoginVariables.enableDraw = false;
                RenameReplayFile();
            }
        }

        public void SendData(string data, bool playSound)
        {
            try
            {
                String s = data;
                byte[] byteTime = Encoding.ASCII.GetBytes(s);
                ns.Write(byteTime, 0, byteTime.Length);
                this.FindForm().Cursor = Cursors.WaitCursor;
                Thread.Sleep(1500);
                if (playSound)
                    chessPieceMoveSound.Play();
                this.FindForm().Cursor = Cursors.Default;
            }
            catch
            {
                LoginVariables.exiting = true;
                Login.ExitProgram();
            }
        }

        public void DoWork()
        {
            byte[] bytes = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = ns.Read(bytes, 0, bytes.Length);
                    this.SetText(Encoding.ASCII.GetString(bytes, 0, bytesRead));
                }
                catch
                {
                    connectionStarted = LoginVariables.connectionStarted = 0;
                    LoginVariables.exiting = true;
                    if (!LoginVariables.ignoreDoWork)
                    {
                        MessageBox.Show("The connection to your opponent has been lost. The program will now exit.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Login.ExitProgram();
                    }
                }
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
                if ((text[0].ToString() + text[1].ToString() + text[2].ToString() + text[3].ToString()) == "~~~~")
                {
                    if (waitForMoreData)
                        waitForMoreData = false;
                    else
                        waitForMoreData = true;
                }

                if (waitForMoreData)
                {
                    if (text != "~~~~")
                        LoginVariables.incompleteData += text;
                }
                else
                {
                    if (LoginVariables.incompleteData != "")
                        text = LoginVariables.incompleteData;
                    LoginVariables.incompleteData = "";
                    ProcessOpponentData(text);
                }

            }
        }

        private void ShowSearchMessage()
        {
            MessageBox.Show("Searching for different opponents...", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ProcessOpponentData(string oppData)
        {
            string testString = Convert.ToString(oppData[0]) + Convert.ToString(oppData[1]) + Convert.ToString(oppData[2]) + Convert.ToString(oppData[3]);
            string[] bothPieces;

            switch (testString)
            {
                case "rqs~":
                    DialogResult result = MessageBox.Show("Would you like to accept the offer of " + opponentName + " to start the game at a saved point?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        SendData("aqs~", false);
                        LoadGameFromFile(oppData);
                    }
                    else
                    {
                        SendData("dqs~", false);
                    }
                    saveGameButton.Enabled = true;
                    replayGameButton.Enabled = true;
                    return;
                case "dqs~":
                    MessageBox.Show("The request has been denied", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                case "aqs~":
                    LoadGameFromFile(LoginVariables.savedData);
                    saveGameButton.Enabled = true;
                    replayGameButton.Enabled = true;
                    return;
                case "lve~":
                    LoginVariables.exiting = true;
                    Login.ExitProgram();
                    return;
                case "dct~":
                    Login l = new Login();

                    MessageBox.Show(opponentName + " has disconnected. The game will now exit.", "Disconnect Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SendData("lve~", false);

                    LoginVariables.exiting = true;

                    Login.ExitProgram();

                    return;
                case "cct~": return;
                case "clr~":
                    System.Threading.Thread.Sleep(1500);

                    foreach (Button element in buttonArray)
                    {
                        element.Enabled = true;
                    }

                    knightSelectButton.Enabled = false;
                    rookSelectButton.Enabled = false;
                    bishopSelectButton.Enabled = false;
                    queenSelectButton.Enabled = false;
                    saveGameButton.Enabled = false;
                    replayGameButton.Enabled = false;
                    loadGameButton.Enabled = true;
                    gameStart = 1;
                    bothPieces = oppData.Split('~').ToArray();
                    playerState = bothPieces[1];
                    opponentName = bothPieces[2];
                    this.FindForm().Text = "Chess - " + name + " vs " + opponentName;

                    if (playerState == "white")
                    {
                        moveAllowed = 1;

                        pawnBox.Image = Chess.Properties.Resources.chess_pawn_white_large;
                        knightBox.Image = Chess.Properties.Resources.chess_knight_white_large;
                        rookBox.Image = Chess.Properties.Resources.chess_rook_white_large;
                        bishopBox.Image = Chess.Properties.Resources.chess_bishop_white_large;
                        queenBox.Image = Chess.Properties.Resources.chess_queen_white_large;

                        oppPawnBox.Image = Chess.Properties.Resources.chess_pawn_black_large;
                        oppKnightBox.Image = Chess.Properties.Resources.chess_knight_black_large;
                        oppRookBox.Image = Chess.Properties.Resources.chess_rook_black_large;
                        oppBishopBox.Image = Chess.Properties.Resources.chess_bishop_black_large;
                        oppQueenBox.Image = Chess.Properties.Resources.chess_queen_black_large;
                    }
                    else
                    {
                        moveAllowed = 0;

                        pawnBox.Image = Chess.Properties.Resources.chess_pawn_black_large;
                        knightBox.Image = Chess.Properties.Resources.chess_knight_black_large;
                        rookBox.Image = Chess.Properties.Resources.chess_rook_black_large;
                        bishopBox.Image = Chess.Properties.Resources.chess_bishop_black_large;
                        queenBox.Image = Chess.Properties.Resources.chess_queen_black_large;

                        oppPawnBox.Image = Chess.Properties.Resources.chess_pawn_white_large;
                        oppKnightBox.Image = Chess.Properties.Resources.chess_knight_white_large;
                        oppRookBox.Image = Chess.Properties.Resources.chess_rook_white_large;
                        oppBishopBox.Image = Chess.Properties.Resources.chess_bishop_white_large;
                        oppQueenBox.Image = Chess.Properties.Resources.chess_queen_white_large;
                    }

                    SetUpBoard(playerState, true);

                    pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                    knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                    rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                    bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                    queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                    oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                    oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                    oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                    oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                    oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();

                    return;
                case "dcl~": MessageBox.Show(name + " has denied you entry into the game. The program will now exit."); LoginVariables.exiting = true; Login.ExitProgram(); return;
                case "nme~":
                    System.Threading.Thread.Sleep(1500);
                    bothPieces = oppData.Split('~').ToArray();
                    opponentName = bothPieces[1];
                    DialogResult acceptCall = MessageBox.Show("Would you like to accept " + opponentName + " to your game?", "Player Joining", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (acceptCall == DialogResult.No)
                    {
                        LoginVariables.exiting = true;
                        LoginVariables.ignoreDoWork = true;
                        Thread messageThread = new Thread(ShowSearchMessage);
                        messageThread.Start();
                        connectionStarted = LoginVariables.connectionStarted = 0;
                        SendData("dcl~", false);
                        t.Abort();
                        listener.Stop();
                        client = null;
                        Thread.Sleep(1500);
                        Login.AlterMainGameState("hide");

                        waitForClient = new Thread(WaitForClient);
                        waitForClient.Start();
                        LoginVariables.exiting = false;
                        return;
                    }

                    foreach (Button element in buttonArray)
                    {
                        element.Enabled = true;
                    }

                    knightSelectButton.Enabled = false;
                    rookSelectButton.Enabled = false;
                    bishopSelectButton.Enabled = false;
                    queenSelectButton.Enabled = false;
                    saveGameButton.Enabled = false;
                    replayGameButton.Enabled = false;
                    loadGameButton.Enabled = true;
                    this.FindForm().Text = "Chess - " + name + " vs " + opponentName;
                    Login.AlterMainGameState("show");

                    gameStart = 1;

                    if (bothPieces[2] == playerState)
                    {
                        Random rnd = new Random();
                        int rndNum = rnd.Next();
                        if ((rndNum % 2) == 0)
                        {
                            SendData("clr~white~" + name, false);
                            playerState = "black";
                            moveAllowed = 0;
                        }
                        else
                        {
                            SendData("clr~black~" + name, false);
                            playerState = "white";
                            moveAllowed = 1;
                        }
                    }
                    else
                        SendData("clr~" + bothPieces[2] + "~" + name, false);

                    SetUpBoard(playerState, true);

                    pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
                    knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
                    rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
                    bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
                    queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

                    oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
                    oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
                    oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
                    oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
                    oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();

                    if (playerState == "white")
                    {
                        pawnBox.Image = Chess.Properties.Resources.chess_pawn_white_large;
                        knightBox.Image = Chess.Properties.Resources.chess_knight_white_large;
                        rookBox.Image = Chess.Properties.Resources.chess_rook_white_large;
                        bishopBox.Image = Chess.Properties.Resources.chess_bishop_white_large;
                        queenBox.Image = Chess.Properties.Resources.chess_queen_white_large;

                        oppPawnBox.Image = Chess.Properties.Resources.chess_pawn_black_large;
                        oppKnightBox.Image = Chess.Properties.Resources.chess_knight_black_large;
                        oppRookBox.Image = Chess.Properties.Resources.chess_rook_black_large;
                        oppBishopBox.Image = Chess.Properties.Resources.chess_bishop_black_large;
                        oppQueenBox.Image = Chess.Properties.Resources.chess_queen_black_large;
                    }
                    else
                    {
                        pawnBox.Image = Chess.Properties.Resources.chess_pawn_black_large;
                        knightBox.Image = Chess.Properties.Resources.chess_knight_black_large;
                        rookBox.Image = Chess.Properties.Resources.chess_rook_black_large;
                        bishopBox.Image = Chess.Properties.Resources.chess_bishop_black_large;
                        queenBox.Image = Chess.Properties.Resources.chess_queen_black_large;

                        oppPawnBox.Image = Chess.Properties.Resources.chess_pawn_white_large;
                        oppKnightBox.Image = Chess.Properties.Resources.chess_knight_white_large;
                        oppRookBox.Image = Chess.Properties.Resources.chess_rook_white_large;
                        oppBishopBox.Image = Chess.Properties.Resources.chess_bishop_white_large;
                        oppQueenBox.Image = Chess.Properties.Resources.chess_queen_white_large;
                    }

                    return;
                case "msg~":
                    bothPieces = oppData.Split('~').ToArray();

                    if (chat.IsDisposed)
                        chat = new ChatForm();
                    chat.Show();
                    chat.SetTextBox(bothPieces[1], true);

                    return;
                case "mve~":
                    System.Threading.Thread.Sleep(1500);
                    loadGameButton.Enabled = false;
                    saveGameButton.Enabled = true;
                    replayGameButton.Enabled = true;
                    replayGameButton.Enabled = true;
                    bothPieces = oppData.Split('~').ToArray();
                    pieces = bothPieces[2].Split(';').ToArray();
                    oppPieces = bothPieces[1].Split(';').ToArray();
                    Array.Resize(ref pieces, 16);
                    Array.Resize(ref pieces, 16);
                    string otherXs = bothPieces[3];
                    LoginVariables.dataOppMoveInProgress = otherXs;
                    AddToReplayFile(9999 - Int32.Parse(otherXs));
                    oppCurX = otherXs[0];
                    oppNextX = otherXs[2];
                    chessPieceMoveSound.Play();
                    moveAllowed = 1;
                    LoginVariables.oppPreviousPos = Int32.Parse(otherXs[0].ToString() + otherXs[1].ToString());
                    LoginVariables.oppCurrentPos = Int32.Parse(otherXs[2].ToString() + otherXs[3].ToString());
                    break;
                case "chk~":
                    System.Threading.Thread.Sleep(1500);
                    saveGameButton.Enabled = true;
                    replayGameButton.Enabled = true;
                    loadGameButton.Enabled = false;
                    bothPieces = oppData.Split('~').ToArray();
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

                    pieces = bothPieces[3].Split(';').ToArray();
                    oppPieces = bothPieces[2].Split(';').ToArray();
                    Array.Resize(ref pieces, 16);
                    Array.Resize(ref pieces, 16);
                    string otherX = bothPieces[4];

                    AddToReplayFile(9999 - Int32.Parse(otherX));

                    LoginVariables.dataOppMoveInProgress = otherX;
                    oppCurX = otherX[0];
                    oppNextX = otherX[2];

                    if (check == 1)
                        MessageBox.Show("Check");

                    chessPieceMoveSound.Play();
                    moveAllowed = 1;
                    LoginVariables.oppPreviousPos = Int32.Parse(otherX[0].ToString() + otherX[1].ToString());
                    LoginVariables.oppCurrentPos = Int32.Parse(otherX[2].ToString() + otherX[3].ToString());
                    break;
                case "stm~":
                    chessLoseSound.Play();
                    foreach (Button element in buttonArray)
                    {
                        if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                            element.Enabled = false;
                        else
                            element.Enabled = true;
                    }
                    disconnect_button.Text = "Disconnect";
                    MessageBox.Show("A stalemate has been declared, with " + opponentName + " the victor.");
                    LoginVariables.enableDraw = false;
                    RenameReplayFile();
                    return;
                case "chm~":
                    chessLoseSound.Play();
                    foreach (Button element in buttonArray)
                    {
                        if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                            element.Enabled = false;
                        else
                            element.Enabled = true;
                    }
                    disconnect_button.Text = "Disconnect";
                    MessageBox.Show("Checkmate has been declared, with " + opponentName + " the victor.");
                    LoginVariables.enableDraw = false;
                    RenameReplayFile();
                    return;
                case "drw~":
                    chessDrawSound.Play();
                    MessageBox.Show(opponentName + " has resigned. The game has now ended.");
                    foreach (Button element in buttonArray)
                    {
                        if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                            element.Enabled = false;
                        else
                            element.Enabled = true;
                    }
                    disconnect_button.Text = "Disconnect";
                    LoginVariables.enableDraw = false;
                    RenameReplayFile();
                    return;
                case "dra~":
                    chessDrawSound.Play();
                    MessageBox.Show(opponentName + " has agreed to the draw. The game has now ended");
                    disconnect_button.Text = "Disconnect";
                    LoginVariables.enableDraw = false;
                    foreach (Button element in buttonArray)
                    {
                        if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                            element.Enabled = false;
                        else
                            element.Enabled = true;
                    }
                    RenameReplayFile();
                    return;
                case "ask~":
                    DialogResult drawResult = MessageBox.Show("Do you wish to accept a draw from " + opponentName + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (drawResult == DialogResult.Yes)
                    {
                        chessDrawSound.Play();
                        foreach (Button element in buttonArray)
                        {
                            if ((element.Name != "disconnect_button") && (element.Name != "chat_button") && (element.Name != "replayGameButton"))
                                element.Enabled = false;
                            else
                                element.Enabled = true;
                        }

                        disconnect_button.Text = "Disconnect";
                        LoginVariables.enableDraw = false;
                        SendData("dra~", false);
                        RenameReplayFile();
                    }
                    else
                    {
                        SendData("ndr~", false);
                    }
                    return;
                case "ndr~":
                    MessageBox.Show(opponentName + " has denied your request for a draw. The game will continue.");
                    return;
                case "rpl~":
                    if (!replayClicked)
                        MessageBox.Show(opponentName + " has begun to replay their game. All buttons have thus been disabled, except for the replay game button.", "Game State Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    replaying = true;
                    opponentReplay = true;
                    return;
                case "drp~":
                    if (!replayClicked)
                    {
                        MessageBox.Show(opponentName + " has finished replaying their game. All eligible buttons have now been enabled.", "Game State Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    opponentReplay = false;
                    replaying = replayClicked || opponentReplay;
                    return;
                default:
                    MessageBox.Show("Warning! Unknown data entering program! Security compromised! Program now exiting!", "Program Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoginVariables.exiting = true;
                    Login.ExitProgram();
                    return;
            }

            pieces.CopyTo(LoginVariables.Player1_Current, 0);

            oppPieces.CopyTo(LoginVariables.Player2_Current, 0);

            string oppPlayerState = "";
            if (playerState == "black")
                oppPlayerState = "white";
            else
                oppPlayerState = "black";

            SetUpBoard(oppPlayerState, false);

            pawnLabel.Text = "x" + LoginVariables.nums[0].ToString();
            knightLabel.Text = "x" + LoginVariables.nums[1].ToString();
            rookLabel.Text = "x" + LoginVariables.nums[2].ToString();
            bishopLabel.Text = "x" + LoginVariables.nums[3].ToString();
            queenLabel.Text = "x" + LoginVariables.nums[4].ToString();

            oppPawnLabel.Text = "x" + LoginVariables.oppNums[0].ToString();
            oppKnightLabel.Text = "x" + LoginVariables.oppNums[1].ToString();
            oppRookLabel.Text = "x" + LoginVariables.oppNums[2].ToString();
            oppBishopLabel.Text = "x" + LoginVariables.oppNums[3].ToString();
            oppQueenLabel.Text = "x" + LoginVariables.oppNums[4].ToString();
        }

        void RenameReplayFile()
        {
            if (!LoginVariables.replayAvailable)
                return;

            DialogResult result = MessageBox.Show("Do you wish to save the replay file?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            DateTime currentDateTime = DateTime.Now;
            string newName = "Chess Replay - " + currentDateTime.ToLongDateString() + " @ " + currentDateTime.ToString("HH:mm:ss").Replace(':', ';');

            bool continueTesting = true;
            int num = 1;

            while (continueTesting)
            {
                if (File.Exists(newName + ".chessdat"))
                {
                    if (newName[newName.Length - 1] == ')')
                    {
                        newName = newName.Substring(0, (newName.IndexOf('(') + 1));

                        newName += num.ToString() + ")";
                    }
                    else
                        newName += " (1)";

                    num++;

                    if(num == 501)
                    {
                        while (File.Exists(newName + ".chessdat"))
                        {
                            newName = Interaction.InputBox("Please enter a name for the replay file. If you do not wish to save it, leave the field blank and press OK.", "Rename Tool", "");
                            if (newName == "")
                            {
                                File.Delete("chess.chessdat");
                                return;
                            }

                            if (File.Exists(newName + ".chessdat"))
                            {
                                DialogResult dlgResult = MessageBox.Show("A file with that name already exists. Do you wish to delete it and replace it with this one?", "File System Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                if (dlgResult == DialogResult.Yes)
                                {
                                    File.Delete(newName + ".chessdat");
                                    break;
                                }
                            }
                        }

                        File.Move("chess.chessdat", (newName + ".chessdat"));
                    }
                }
                else
                    continueTesting = false;
            }
        }

        void ReplayGame()
        {
            replaying = true;
            replayClicked = true;
            replayGameButton.Text = "Pause Replay";
            string previousText = "";

            if (startReplay)
            {
                SetUpBoard(playerState, false, false);
                chessPieceMoveSound.PlaySync();
                System.Threading.Thread.Sleep(500);
                startReplay = false;
            }

            /*using (StreamReader reader = File.OpenText("chess.chessdat"))
            {
                bool continueRead = true;
                bool first = true;
                while (continueRead)
                {
                    if (replaySuspended)
                    {
                        replayGameButton.Text = "Resume Replay";
                        while (replaySuspended)
                        {
                            System.Threading.Thread.Sleep(50);
                        }
                    }

                    replayGameButton.Text = "Pause Replay";

                    string readText = reader.ReadLine();
                    int tryParse;
                    if ((first && readText != "aef769c0-5207-4b29-b38a-f1209252e9bb") || (readText == "" && previousText.Length == 4) || readText == null)
                        continueRead = false;
                    else if (readText == "aef769c0-5207-4b29-b38a-f1209252e9bb" || readText == "" || !Int32.TryParse(readText, out tryParse) || readText.Length != 4)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        Button firstButton = null;
                        Button secondButton = null;
                        string firstPos = "";
                        string secondPos = "";
                        int convertToInt = Int32.Parse(readText);
                        string char0 = readText[0].ToString();
                        string char1 = readText[1].ToString();
                        string char2 = readText[2].ToString();
                        string char3 = readText[3].ToString();

                        for (int i = 0; i < 2; i++)
                        {
                            string charToReturn = "";
                            int charX = i == 0 ? Int32.Parse(char0) : Int32.Parse(char2);

                            switch (charX)
                            {
                                case 1: charToReturn = "A"; break;
                                case 2: charToReturn = "B"; break;
                                case 3: charToReturn = "C"; break;
                                case 4: charToReturn = "D"; break;
                                case 5: charToReturn = "E"; break;
                                case 6: charToReturn = "F"; break;
                                case 7: charToReturn = "G"; break;
                                case 8: charToReturn = "H"; break;
                            }

                            if (i == 0)
                                char0 = charToReturn;
                            else
                                char2 = charToReturn;
                        }

                        firstPos = char0 + char1;
                        secondPos = char2 + char3;

                        foreach (Button element in buttonArray)
                        {
                            if (element.Name == firstPos)
                                firstButton = element;
                            else if (element.Name == secondPos)
                                secondButton = element;

                            if ((firstButton != null) && (secondButton != null))
                                break;
                        }

                        foreach(Button element in buttonArray)
                        {
                            if (element.Name == secondButton.Name)
                            {
                                element.Image = firstButton.Image;
                                element.ImageAlign = ContentAlignment.MiddleCenter;
                            }
                            else if (element.Name == firstButton.Name)
                                element.Image = null;
                        }

                        chessPieceMoveSound.PlaySync();
                        System.Threading.Thread.Sleep(500);
                    }
                    first = false;
                    previousText = readText;
                }
            }
            
            SetUpBoard(playerState, false);
            chessPieceMoveSound.PlaySync();
            SendData("drp~", false);
            */
            replayGameButton.Text = "Replay Game";
            replaying = opponentReplay;
            replayClicked = false;
            startReplay = true;
        }

        private void replayGameButton_Click(object sender, EventArgs e)
        {
            if (!LoginVariables.replayAvailable)
            {
                MessageBox.Show("Game cannot be replayed", "File System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (replaying && replayClicked)
                replaySuspended = !replaySuspended;
            else
            {
                SendData("rpl~", false);
                ReplayGame();
            }
        }

        static void AddToReplayFile(int dataToSave)
        {
            if (!LoginVariables.replayAvailable)
            {
                return;
            }

            File.AppendAllText("chess.chessdat", Environment.NewLine + dataToSave.ToString());
        }

        public static void SetUpBoard(string stateOfPlayer, bool createTXTs, bool copy = true)
        {
            if (gameStart == 0)
                return;

            foreach (Button element in buttonArray)
            {
                element.Image = null;
                element.Tag = null;
            }

            if (createTXTs || !copy)
                CreatePlayerCurrentTXTs(copy);

            int curPosition = -1;

            if (copy)
            {
                LoginVariables.Player1_Current.CopyTo(staticPieces, 0);
                LoginVariables.Player2_Current.CopyTo(staticOpponentPieces, 0);
            }
            LoginVariables.nums = new int[] { 0, 0, 0, 0, 0, 0 };
            LoginVariables.oppNums = new int[] { 0, 0, 0, 0, 0, 0 };
            staticPieces.CopyTo(pieces, 0);

            for (int i = 0; i < 32; i++)
            {
                curPosition++;
                if (curPosition > 15)
                {
                    staticOpponentPieces.CopyTo(pieces, 0);
                    curPosition = 0;
                }

                cur_Piece = pieces[curPosition].Split(':');
                if (cur_Piece[3] != "99")
                {
                    string s = cur_Piece[5];
                    string oppPlayerState = "";

                    if (playerState == "white")
                        oppPlayerState = "black";
                    else
                        oppPlayerState = "white";

                    string pieceForPlacing = Convert.ToString(cur_Piece[0][1]) + Convert.ToString(cur_Piece[0][2]);
                    string imageName = "";
                    string tag = "";

                    if (pieces.SequenceEqual(staticPieces))
                    {
                        switch (pieceForPlacing)
                        {
                            case "Pa": imageName = "chess_pawn_" + playerState + "_small"; tag = playerState[0].ToString() + "pa"; break;
                            case "Ro": imageName = "chess_rook_" + playerState + "_small"; tag = playerState[0].ToString() + "ro"; break;
                            case "Kn": imageName = "chess_knight_" + playerState + "_small"; tag = playerState[0].ToString() + "kn"; break;
                            case "Bi": imageName = "chess_bishop_" + playerState + "_small"; tag = playerState[0].ToString() + "bi"; break;
                            case "ue": imageName = "chess_queen_" + playerState + "_small"; tag = playerState[0].ToString() + "qu"; break;
                            case "in": imageName = "chess_king_" + playerState + "_small"; tag = playerState[0].ToString() + "ki"; break;
                        }
                    }
                    else
                    {
                        switch (pieceForPlacing)
                        {
                            case "Pa": imageName = "chess_pawn_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "pa"; break;
                            case "Ro": imageName = "chess_rook_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "ro"; break;
                            case "Kn": imageName = "chess_knight_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "kn"; break;
                            case "Bi": imageName = "chess_bishop_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "bi"; break;
                            case "ue": imageName = "chess_queen_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "qu"; break;
                            case "in": imageName = "chess_king_" + oppPlayerState + "_small"; tag = oppPlayerState[0].ToString() + "ki"; break;
                        }
                    }

                    foreach (Button buttonElement in buttonArray)
                    {
                        if (buttonElement.Name == cur_Piece[2])
                        {
                            buttonElement.Tag = tag;
                            buttonElement.Image = (System.Drawing.Image)Chess.Properties.Resources.ResourceManager.GetObject(imageName);
                            buttonElement.ImageAlign = ContentAlignment.MiddleCenter;
                            buttonElement.FlatStyle = FlatStyle.Flat;
                        }
                    }
                }
                else
                {
                    string pieceName = cur_Piece[0][1].ToString() + cur_Piece[0][2].ToString();
                    if (pieces.SequenceEqual(staticPieces))
                    {
                        switch (pieceName)
                        {
                            case "Pa": LoginVariables.nums[0]++; break;
                            case "Kn": LoginVariables.nums[1]++; break;
                            case "Ro": LoginVariables.nums[2]++; break;
                            case "Bi": LoginVariables.nums[3]++; break;
                            case "ue": LoginVariables.nums[4]++; break;
                        }
                    }
                    else
                    {
                        switch (pieceName)
                        {
                            case "Pa": LoginVariables.oppNums[0]++; break;
                            case "Kn": LoginVariables.oppNums[1]++; break;
                            case "Ro": LoginVariables.oppNums[2]++; break;
                            case "Bi": LoginVariables.oppNums[3]++; break;
                            case "ue": LoginVariables.oppNums[4]++; break;
                        }
                    }
                }
            }
        }
    }
}