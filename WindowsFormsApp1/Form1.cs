using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UdpClient C = new UdpClient();
            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2019);
                                        //應為伺服器程式所在之IP
            C.Connect(EP);
            byte[] B = Encoding.Default.GetBytes(textBox1.Text);    //送出問題
            C.Send(B,B.Length);
            byte[] R = C.Receive(ref EP);                         //原路接收訊息
            textBox2.Text = Encoding.Default.GetString(R);          //接收到的byte轉換文字
        }
    }
}
