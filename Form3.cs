using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Chess
{
    public partial class ChatForm : Form
    {
        string chatString;
        public ChatForm()
        { 
            InitializeComponent();
            textBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainGame m = new MainGame();
            string textData = textBox2.Text;

            if (textData.Contains("~") || textData.Contains(@"\"))
            {
                DialogResult dlg = MessageBox.Show(@"Due to technical reasons, ~ and \ are not valid characters in chat messages", "Chat Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (dlg == DialogResult.Cancel)
                    textBox2.Text = "";
                return;
            }

            SetTextBox("You: " + textData, true);
            m.SendData("msg~" + MainGame.name + ": " + textData, false);
        }

        public void SetTextBox(string text, bool addToFile)
        {
            try
            {
                if (addToFile)
                {
                    Array.Resize(ref LoginVariables.chatArray, LoginVariables.chatArray.Length + 1);
                    LoginVariables.chatArray[LoginVariables.chatArray.Length - 1] = text;
                }

                chatString = String.Join("\r\n", LoginVariables.chatArray);
                textBox1.Text = "";
                textBox1.Text = chatString;
            }
            catch
            {
                LoginVariables.chatArray = null;
                chatString = null;
                textBox1.Text = "";
                MessageBox.Show("Chat Memory Full. Chat deleted.", "Memory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
