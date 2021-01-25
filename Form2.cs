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
using System.Net;
using System.Windows.Forms;

namespace Chess
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            MainGame chessMainGame = new MainGame();
            chessMainGame.Show();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
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

            if ((MainGame.name == "") || (MainGame.ipAddress == "...") || (MainGame.portNumber == 0) || (player == ""))
            {
                MessageBox.Show("Please ensure all fields are filled in", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MainGame.portNumber /= 10;
                MainGame.playerState = player;
                MainGame.SetUpBoard(player);

                this.Hide();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
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

            MainGame.name = textBox1.Text;
            if ((MainGame.name == "") || (player == ""))
            {
                MessageBox.Show("Please ensure all fields are filled in", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                startButton.Enabled = false;
                textBox1.Enabled = false;
                WhiteSelect2.Enabled = false;
                BlackSelect2.Enabled = false;

                pleaseWaitLabel.Show();

                MainGame.playerState = player;
            }
        }

        private void HostSelectButton_Click(object sender, EventArgs e)
        {
            WhiteSelect2.TabStop = false;
            BlackSelect2.TabStop = false;
            pictureBox2.Hide();
            label9.Hide();
            ClientSelectButton.Hide();
            HostSelectButton.Hide();
            pleaseWaitLabel.Hide();
            ipInformationLabel.Text = GetIPAddress();
        }

        private void ClientSelectButton_Click(object sender, EventArgs e)
        {
            WhiteRadio.TabStop = false;
            BlackRadio.TabStop = false;
            ipLabel.Hide();
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
        }

        static string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }
    }
}
