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
        Thread Th;//網路監聽執行緒
        private void Form1_Load(object sender, EventArgs e)
        {
            Th = new Thread(Listen);//建立監聽執行緒
            Th.IsBackground = true;//設定為背景執行緒
            Th.Start();//開始監聽
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Th.Abort();//關閉執行緒
        }

        //伺服端監聽程式
        private void Listen()
        {
            UdpClient U = new UdpClient(2019);
            while (true)
            {
                IPEndPoint EP = new IPEndPoint(IPAddress.Any, 2019);//建立監聽端點資訊
                byte[] B = U.Receive(ref EP);
                string A = Encoding.Default.GetString(B);//翻譯資訊為字串
                string M = "Unkown Command";
                if (A == "Time?")
                {
                    M = DateTime.Now.ToString();
                }
                B = Encoding.Default.GetBytes(M);
                U.Send(B, B.Length, EP);//回應詢問資料(循原路回去)
            }
        }
    }
}
