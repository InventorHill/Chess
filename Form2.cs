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
                    MainGame.portNumber /= 10;
                    MainGame.playerState = player;
                    MainGame.gameStart = 1;

                    MainGame main = new MainGame();
                    main.Show();
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

                MainGame.playerState = player;
                pleaseWaitLabel.Show();

                await PutTaskDelay();

                MainGame main = new MainGame();
                main.Show();
            }
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(50);
        }

        public static void HideLogin()
        {
            Login l = new Login();
            l.HideMe();
        }

        private void HideMe()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
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
            MainGame.serverState = 1;
        }

        private void ClientSelectButton_Click(object sender, EventArgs e)
        {
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

        private void PortForwardInfoButton_Click(object sender, EventArgs e)
        {
            string hostName = Dns.GetHostName();
            MessageBox.Show("Private IP Address: " + Dns.GetHostEntry(hostName).AddressList[2].ToString(), "Port Forwarding Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
