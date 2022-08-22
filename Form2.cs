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
using System.Net.NetworkInformation;


namespace Chess
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            bool createFile = false;
            if (LoginVariables.Player1_Current.Length != 16)
                Array.Resize(ref LoginVariables.Player1_Current, 16);
            if (LoginVariables.Player2_Current.Length != 16)
                Array.Resize(ref LoginVariables.Player2_Current, 16);

            if (File.Exists("chess.chessdat"))
            {
                try
                {
                    string fakeGUID = File.ReadAllLines("chess.chessdat")[0].ToString();
                    if (fakeGUID != "aef769c0-5207-4b29-b38a-f1209252e9bb")
                    {
                        MessageBox.Show("chess.chessdat already exists in this location. Some features may not be available.", "File System Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoginVariables.replayAvailable = false;
                    }
                    else
                    {
                        createFile = true;
                    }
                }
                catch
                {
                    MessageBox.Show("chess.chessdat already exists in this location. This game cannot be replayed.", "File System Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoginVariables.replayAvailable = false;
                }
            }
            else
            {
                createFile = true;
            }

            if (createFile)
            {
                FileStream stream = new FileStream("chess.chessdat", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("aef769c0-5207-4b29-b38a-f1209252e9bb");
                writer.Close();
                stream.Close();
            }
        }

        public static void AlterMainGameState(string hideOrShow)
        {
            if (LoginVariables.mainGame == null)
                LoginVariables.mainGame = new MainGame();
            if (hideOrShow == "show")
                LoginVariables.mainGame.Show();
            else
                LoginVariables.mainGame.Hide();
        }

        public static void ExitProgram()
        {
            Environment.Exit(0);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            int testComplete = 0;
            if (int.TryParse(IPBox1.Text, out testComplete) && int.TryParse(IPBox2.Text, out testComplete) && int.TryParse(IPBox3.Text, out testComplete) && int.TryParse(IPBox4.Text, out testComplete) && int.TryParse(PortBox.Text, out testComplete))
            {
                MainGame.name = NameBox.Text;
                MainGame.ipAddress = IPBox1.Text + "." + IPBox2.Text + "." + IPBox3.Text + "." + IPBox4.Text;
                MainGame.portNumber = Int32.Parse(PortBox.Text + "0");

                bool whiteChecked = WhiteRadio.Checked;
                bool blackChecked = BlackRadio.Checked;

                string player = "";
                if (whiteChecked)
                {
                    player = "white";
                    MainGame.moveAllowed = 1;
                }
                else if (blackChecked)
                {
                    player = "black";
                    MainGame.moveAllowed = 0;
                }

                if ((MainGame.name == "") || (IPBox1.Text == "") || (IPBox2.Text == "") || (IPBox3.Text == "") || (IPBox4.Text == "") || (MainGame.portNumber == 0) || (player == ""))
                {
                    MessageBox.Show("Please ensure all fields are filled in", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if ((Int32.Parse(IPBox1.Text) > 255) || (Int32.Parse(IPBox2.Text) > 255) || (Int32.Parse(IPBox3.Text) > 255) || (Int32.Parse(IPBox4.Text) > 255) || (Int32.Parse(IPBox1.Text) < 0) || (Int32.Parse(IPBox2.Text) < 0) || (Int32.Parse(IPBox3.Text) < 0) || (Int32.Parse(IPBox4.Text) < 0) || ((MainGame.portNumber / 10) > 65535) || ((MainGame.portNumber / 10) < 0))
                {
                    MessageBox.Show("Invalid IP address or port number. Each value in the IP address must be 255 or less and 0 or greater, and the port number must be 65535 or less and 0 or greater", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ConnectButton.Enabled = false;
                    IPBox1.Enabled = false;
                    IPBox2.Enabled = false;
                    IPBox3.Enabled = false;
                    IPBox4.Enabled = false;
                    PortBox.Enabled = false;
                    NameBox.Enabled = false;
                    WhiteRadio.Enabled = false;
                    BlackRadio.Enabled = false;
                    MainGame.portNumber /= 10;
                    MainGame.playerState = player;
                    MainGame.gameStart = 1;

                    if (LoginVariables.mainGame == null)
                        LoginVariables.mainGame = new MainGame();
                    LoginVariables.mainGame.Show();
                }
            }
            else
            {
                MessageBox.Show("Please ensure that all IP fields are integers", "IP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            bool whiteChecked = WhiteSelect2.Checked;
            bool blackChecked = BlackSelect2.Checked;
            string player = "";
            if (whiteChecked)
            {
                player = "white";
                MainGame.moveAllowed = 1;
            }
            else if (blackChecked)
            {
                player = "black";
                MainGame.moveAllowed = 0;
            }

            int portNumber = 0;
            if (int.TryParse(portTextBox.Text, out portNumber))
            {
                portNumber = Int32.Parse(portTextBox.Text + "0");
            }

            MainGame.name = textBox1.Text;
            if ((MainGame.name == "") || (player == "") || (portNumber == 0))
            {
                MessageBox.Show("Please ensure all fields are filled in", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (((portNumber / 10) > 65535) || ((portNumber / 10) < 0))
            {
                MessageBox.Show("Please ensure the port number is 65535 or less and 0 or greater");
            }
            else
            {
                portNumber /= 10;
                MainGame.portNumber = portNumber;
                startButton.Enabled = false;
                textBox1.Enabled = false;
                WhiteSelect2.Enabled = false;
                BlackSelect2.Enabled = false;
                portTextBox.Enabled = false;
                PortForwardInfoButton.Enabled = false;
                WhiteSelect2.Enabled = false;
                BlackSelect2.Enabled = false;

                MainGame.playerState = player;
                pleaseWaitLabel.Show();

                await PutTaskDelay();

                if (LoginVariables.mainGame == null)
                    LoginVariables.mainGame = new MainGame();
                LoginVariables.mainGame.Show();
                LoginVariables.mainGame.Hide();
            }
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(50);
        }

        public static void HideLogin()
        {
            Login l = new Login();
            l.WindowState = FormWindowState.Minimized;
            l.ShowInTaskbar = false;
            l.Visible = false;
        }

        private void HostSelectButton_Click(object sender, EventArgs e)
        {
            ipInformationLabel.Text = GetIPAddress();
            WhiteSelect2.TabStop = false;
            BlackSelect2.TabStop = false;
            pictureBox2.Hide();
            label9.Hide();
            ClientSelectButton.Hide();
            HostSelectButton.Hide();
            pleaseWaitLabel.Hide();
            MainGame.serverState = 1;
        }

        private void ClientSelectButton_Click(object sender, EventArgs e)
        {
            ipInformationLabel.Text = GetIPAddress();
            WhiteRadio.TabStop = false;
            BlackRadio.TabStop = false;
            ipLabel.Hide();
            PortForwardInfoButton.Hide();
            ipInformationLabel.Hide();
            label8.Hide();
            textBox1.Hide();
            startButton.Hide();
            pleaseWaitLabel.Hide();
            pictureBox1.Hide();
            pictureBox2.Hide();
            label9.Hide();
            ClientSelectButton.Hide();
            HostSelectButton.Hide();
            WhiteSelect2.Hide();
            BlackSelect2.Hide();
            label11.Hide();
            portLabel.Hide();
            portTextBox.Hide();
            MainGame.serverState = 0;
        }

        static string GetIPAddress()
        {
            String address = "";
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }

                int first = address.IndexOf("Address: ") + 9;
                int last = address.LastIndexOf("</body>");
                address = address.Substring(first, last - first);

                int isDotPresent = address.IndexOf('.');
                string[] createdIP = null;
                if ((isDotPresent != -1) && (isDotPresent != 0))
                    createdIP = address.Split('.').ToArray();

                int createdIPLength = createdIP.Length;

                if (createdIPLength == 4)
                {
                    string address1 = createdIP[0];
                    string address2 = createdIP[1];
                    string address3 = createdIP[2];
                    string address4 = createdIP[3];
                    int testAddress = 0;

                    if (!(int.TryParse(address1, out testAddress) && int.TryParse(address2, out testAddress) && int.TryParse(address3, out testAddress) && int.TryParse(address4, out testAddress)))
                        address = "Error";
                }
                else
                    address = "Error";
            }
            catch(Exception)
            {
                address = "Error";
            }
            return address;
        }
        private void PortForwardInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                string hostName;
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    hostName = endPoint.Address.ToString();
                }

                MessageBox.Show("Private IP Address: " + hostName, "Port Forwarding Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Information Unavailable", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitProgram();
        }
    }
}
