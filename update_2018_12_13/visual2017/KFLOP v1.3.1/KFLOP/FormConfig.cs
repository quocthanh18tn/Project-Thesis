using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
namespace KFLOP
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
        }
        public Form formConfig;
        public string Mode;
        public string W,H,L;
        public string ID;
        public string Hole;
        public string check,ButtonState;
        //public int check_connect;
        /// <summary>
        /// function config
        /// ButtonState: 
        ///  Start
        ///  Stop
        ///  Pause
        ///  Resume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormConfig_Load(object sender, EventArgs e)
        {
            Product.CheckConnectServer = check_connect_server();
            if (Product.CheckConnectServer == 1)
            {
                SynData();
                check = "0";
                FileStream ofs = new FileStream(@"D:\form.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader rd = new StreamReader(ofs);
                txtID.Text = rd.ReadLine();
                comboBoxMode.Text = rd.ReadLine();
                txtW.Text = rd.ReadLine();
                txtH.Text = rd.ReadLine();
                txtL.Text = rd.ReadLine();
                txtHole.Text = rd.ReadLine();
                
            }
            else
            {
                Product.CheckConnectServer = 0;
                DialogResult dlr = MessageBox.Show("Chưa kết nối mạng!!\n Bạn có muốn làm offline không?", "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlr == DialogResult.Yes)
                {
                    FileStream ofs = new FileStream(@"D:\form.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader rd = new StreamReader(ofs);
                    txtID.Text = rd.ReadLine();
                    comboBoxMode.Text = rd.ReadLine();
                    txtW.Text = rd.ReadLine();
                    txtH.Text = rd.ReadLine();
                    txtL.Text = rd.ReadLine();
                    txtHole.Text = rd.ReadLine();
                    //code for Yes
                }
                else if (dlr == DialogResult.No)
                {
                    this.Close();
                    //code for No
                }
                //this.Close();
                
                //this.Close();
            }
        }
        /// <summary>
        /// fucntion send to server
        /// return code :
        ///     0 rot mang
        ///     1 send ok
        ///     2 dang pause 
        ///     3 đã start rồi
        ///     4 chưa start
        ///     5 chưa pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int send_data_to_server(string Mode, string ID, string W, string H, string L, string Hole,string ButtonState,int CountBar,int CountHole)
        {
            try
            {
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create(Product.IpServer+"/Machine_Monitoring/Mayducdong/mayducdong.php");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                string postData = "Mode=" + Mode + "&ID=" + ID + "&L=" + L + "&W=" + W + "&H=" + H + "&Hole=" + Hole + "&ButtonState=" + ButtonState + "&CountBar=" + CountBar.ToString() + "&CountHole=" + CountHole.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;
                //timeout
                request.Timeout = 2000;
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();
                // Get the response.  
                WebResponse response = request.GetResponse();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    // Get the stream containing content returned by the server.  
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.  
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.   
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    int code = Int32.Parse(responseFromServer);
                    return code;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// fucntion send to LOCAL
        /// return code :
        ///     0 rot mang
        ///     1 send ok
        ///     2 dang pause 
        ///     3 đã start rồi
        ///     4 chưa start
        ///     5 chưa pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int send_data_to_local(string Mode, string ID, string W, string H, string L, string Hole, string ButtonState, int CountBar, int CountHole, int State)
        {
            try
            {
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create("http://localhost/Mayducdong/mayducdong.php");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                string postData = "Mode=" + Mode + "&ID=" + ID + "&L=" + L + "&W=" + W + "&H=" + H + "&Hole=" + Hole + "&ButtonState=" + ButtonState + "&CountBar=" + CountBar.ToString() + "&CountHole=" + CountHole.ToString() + "&State=" + State.ToString();
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();
                // Get the response.  
                WebResponse response = request.GetResponse();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    // Get the stream containing content returned by the server.  
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.  
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.   
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    int code = Int32.Parse(responseFromServer);
                    return code;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///  check connect
        /// </summary>
        /// <returns></returns>
        public int check_connect_server()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Product.IpServer);//thay the dia chi phu hop
                req.Timeout = 2000;
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Close();
                    return 1;
                }
                else
                {
                    response.Close();
                 
                    return 0;
                }
            }
            catch (Exception ex)
            {
               // DialogResult text = MessageBox.Show(ex.Message.ToString(), "abc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }
        /// <summary>
        /// click start
        /// code chua state server return
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public int code=0;
        private void buttonStart_Click(object sender, EventArgs e)
        {
            Mode = comboBoxMode.Text;
            W = txtW.Text;
            H = txtH.Text;
            L = txtL.Text;
            ID = txtID.Text;
            Hole = txtHole.Text;
            DialogResult dlr = MessageBox.Show("Bạn chắc muốn bắt đầu chứ?", "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                check = "1";
                ButtonState = "Start";
                if (Product.CheckConnectServer==1) 
                code = send_data_to_server(Mode, ID, W, H, L, Hole, ButtonState,0,0);
                if (code == 1)
                {
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    //
                    DialogResult dlr1 = MessageBox.Show("bắt đầu thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, 0, 0, 1);
                    this.Close();
                    
                    
                    // server xu ly luu data khong viet nua
                }
                else if (code == 0 || Product.CheckConnectServer==0)
                {
                    Product.CheckConnectServer = 0;
                    int codelocal  = send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, 0, 0, 0);
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    checkcode(codelocal);
                    //this.Close();
                     // todo
                    // luu mysql localhost
                }
                else
                {
                    checkcode(code);
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Mode = comboBoxMode.Text;
            W = txtW.Text;
            H = txtH.Text;
            L = txtL.Text;
            ID = txtID.Text;
            Hole = txtHole.Text;
            DialogResult dlr = MessageBox.Show("Bạn chắc muốn kết thúc chứ?", "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                check = "1";
                ButtonState = "Finish";
                if(Product.CheckConnectServer==1)
                code = send_data_to_server(Mode, ID, W, H, L, Hole, ButtonState,Product.CountBar,Product.CountHole);
                if (code == 1)
                {
                    
                    //
                    DialogResult dlr1 = MessageBox.Show("Kết thúc thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, Product.CountBar, Product.CountHole, 1);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    this.Close();

                    // server xu ly luu data khong viet nua
                }
                else if (code == 0||Product.CheckConnectServer==0)
                {
                    Product.CheckConnectServer = 0;
                    int codelocal = send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, Product.CountBar, Product.CountHole, 0);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    checkcode(codelocal);
                    //this.Close();
                    // todo
                    // luu mysql localhost
                }
                else
                {
                    checkcode(code);
                }
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            Mode = comboBoxMode.Text;
            W = txtW.Text;
            H = txtH.Text;
            L = txtL.Text;
            ID = txtID.Text;
            Hole = txtHole.Text;
            DialogResult dlr = MessageBox.Show("Bạn chắc muốn pause chứ?", "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                check = "1";
                ButtonState = "Pause";
                if(Product.CheckConnectServer==1)
                code = send_data_to_server(Mode, ID, W, H, L, Hole, ButtonState,Product.CountBar, Product.CountHole);
                if (code == 1)
                {

                    //
                    DialogResult dlr1 = MessageBox.Show("Pause thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, Product.CountBar, Product.CountHole, 1);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    
                    this.Close();

                    // server xu ly luu data khong viet nua
                }
                else if (code == 0|| Product.CheckConnectServer==0)
                {
                    Product.CheckConnectServer = 0;
                    int codelocal = send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, Product.CountBar, Product.CountHole, 0);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    checkcode(codelocal);
                    //this.Close();
                    // todo
                    // luu mysql localhost
                }
                else
                {
                    checkcode(code);
                }
            }
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            Mode = comboBoxMode.Text;
            W = txtW.Text;
            H = txtH.Text;
            L = txtL.Text;
            ID = txtID.Text;
            Hole = txtHole.Text;
            DialogResult dlr = MessageBox.Show("Bạn chắc muốn resume chứ?", "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.Yes)
            {
                check = "1";
                ButtonState = "Resume";
                if (Product.CheckConnectServer==1)
                code = send_data_to_server(Mode, ID, W, H, L, Hole, ButtonState,0,0);
                if (code == 1)
                {
                    
                    send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, 0, 0, 1);
                    DialogResult dlr1 = MessageBox.Show("Resume thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    //
                    this.Close();

                    // server xu ly luu data khong viet nua
                }
                else if (code == 0)
                {
                    Product.CheckConnectServer = 0;
                    int codelocal = send_data_to_local(Mode, ID, W, H, L, Hole, ButtonState, 0,0, 0);
                    //reset CountProduct
                    Product.CountBar = 0;
                    Product.CountHole = 0;
                    checkcode(codelocal);
                   // this.Close();
                    // todo
                    // luu mysql localhost
                }
                else
                {
                    checkcode(code);
                }
            }

        }
        public void checkcode(int codecheck)
        {
            if (codecheck == 1)
            {
                DialogResult dlr1 = MessageBox.Show("Thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //DialogResult dlr1 = MessageBox.Show("Thành công!!", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (codecheck == 2)
            {
                DialogResult dlr1 = MessageBox.Show("Bạn đang pause trước đó, Vui lòng xem lại!!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (codecheck == 3)
            {
                DialogResult dlr1 = MessageBox.Show("Bạn đã start, Vui lòng xem lại!!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (codecheck == 4)
            {
                DialogResult dlr1 = MessageBox.Show("Bạn chưa start, Vui lòng xem lại!!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (codecheck == 5)
            {
                DialogResult dlr1 = MessageBox.Show("Bạn chưa pause trước đó, Vui lòng xem lại!!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        public void SynData()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Product.IpServer);//thay the dia chi phu hop
                req.Timeout = 2000;
                HttpWebResponse response1 = (HttpWebResponse)req.GetResponse();
                if (response1.StatusCode == HttpStatusCode.OK)
                {

                    response1.Close();
                    string strHostName = Dns.GetHostName();
                    //Console.WriteLine("Host Name: " + strHostName);
                    // Find host by name
                    IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
                    // Enumerate IP addresses
                    IPAddress ip = iphostentry.AddressList[0];


                    // Create a request using a URL that can receive a post.   
                    WebRequest request = WebRequest.Create("http://localhost/Mayducdong/dongbo_local.php");
                    // Set the Method property of the request to POST.  
                    request.Method = "POST";
                    // Create POST data and convert it to a byte array.  
                    string postData = "IP=" + ip.ToString();
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    // Set the ContentType property of the WebRequest.  
                    request.ContentType = "application/x-www-form-urlencoded";
                    // Set the ContentLength property of the WebRequest.  
                    request.ContentLength = byteArray.Length;
                    request.Timeout = 10000;
                    // Get the request stream.  
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.  
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.  
                    dataStream.Close();
                    // Get the response.  
                    WebResponse response = request.GetResponse();
                    if (((HttpWebResponse)response).StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.  
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.  
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.  
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.   
                        reader.Close();
                        dataStream.Close();
                        response.Close();
                        Product.CheckConnectServer = 1;
                    }
                }
                else
                {
                    Product.CheckConnectServer = 0;
                    response1.Close();

                }
            }
            catch (Exception ex)
            {
                DialogResult dlr = MessageBox.Show("Lỗi đồng bộ", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }
        }

    }
}
