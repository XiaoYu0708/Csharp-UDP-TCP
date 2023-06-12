using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        UdpClient U;//宣告UDP通訊物件
        Thread TH;//宣告監聽用執行緒
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Listen()
        {
            int Port = int.Parse(textBox1.Text);//設定監聽用的通訊埠
            U = new UdpClient(Port);//監聽UDP監聽器的實體

            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            while (true)//持續監聽的無限回圈->有訊息(True)就處理，無訊息就等待
            {
                byte[] B = U.Receive(ref EP);//訊息到達時讀取資料到B陣列
                textBox2.Text = Encoding.Default.GetString(B);//翻譯B陣列字串
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//忽略跨執行緒錯誤
            TH = new Thread(Listen);//建立監聽執行緒，目標副程序->Listen
            TH.Start();//啟動監聽執行緒
            button1.Enabled = false;//使按鈕失效，不能(也不需要)重複開啟監聽
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TH.Abort();//關閉監聽執行緒
                U.Close();//關閉監聽器
            }catch (Exception ex)
            {
                //忽略錯誤，程式繼續執行
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox3.Text;//設定發送目標IP
            int Port = int.Parse(textBox4.Text);//設定發送目標Port
            byte[] B = Encoding.Default.GetBytes(textBox5.Text);//字串翻譯成位元組陣列
            UdpClient S = new UdpClient();//建立UDP通訊器
            S.Send(B,B.Length,IP,Port);//發送資料到指定位置
            S.Close();//關閉通訊器
        }

        private string MyIP()
        {
            string hn = Dns.GetHostName();                        //取得本機電腦名稱
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList;      //取得本機IP陣列

            foreach(IPAddress it in ip)                           //列舉各個IP
            {
                if(it.AddressFamily == AddressFamily.InterNetwork)  //如果是IPv4格式
                {
                    return it.ToString();                        //回傳此IP字串
                }
            }
            return "";                                          //找不到合格IP，回傳空字串
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += " " + MyIP();                            //顯示本機IP於標題列
        }
    }    
}