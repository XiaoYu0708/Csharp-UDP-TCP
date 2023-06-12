using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        UdpClient U;//�ŧiUDP�q�T����
        Thread TH;//�ŧi��ť�ΰ����
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Listen()
        {
            int Port = int.Parse(textBox1.Text);//�]�w��ť�Ϊ��q�T��
            U = new UdpClient(Port);//��ťUDP��ť��������

            IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            while (true)//�����ť���L���^��->���T��(True)�N�B�z�A�L�T���N����
            {
                byte[] B = U.Receive(ref EP);//�T����F��Ū����ƨ�B�}�C
                textBox2.Text = Encoding.Default.GetString(B);//½ĶB�}�C�r��
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//�������������~
            TH = new Thread(Listen);//�إߺ�ť������A�ؼаƵ{��->Listen
            TH.Start();//�Ұʺ�ť�����
            button1.Enabled = false;//�ϫ��s���ġA����(�]���ݭn)���ƶ}�Һ�ť
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                TH.Abort();//������ť�����
                U.Close();//������ť��
            }catch (Exception ex)
            {
                //�������~�A�{���~�����
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IP = textBox3.Text;//�]�w�o�e�ؼ�IP
            int Port = int.Parse(textBox4.Text);//�]�w�o�e�ؼ�Port
            byte[] B = Encoding.Default.GetBytes(textBox5.Text);//�r��½Ķ���줸�հ}�C
            UdpClient S = new UdpClient();//�إ�UDP�q�T��
            S.Send(B,B.Length,IP,Port);//�o�e��ƨ���w��m
            S.Close();//�����q�T��
        }

        private string MyIP()
        {
            string hn = Dns.GetHostName();                        //���o�����q���W��
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList;      //���o����IP�}�C

            foreach(IPAddress it in ip)                           //�C�|�U��IP
            {
                if(it.AddressFamily == AddressFamily.InterNetwork)  //�p�G�OIPv4�榡
                {
                    return it.ToString();                        //�^�Ǧ�IP�r��
                }
            }
            return "";                                          //�䤣��X��IP�A�^�ǪŦr��
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += " " + MyIP();                            //��ܥ���IP����D�C
        }
    }    
}