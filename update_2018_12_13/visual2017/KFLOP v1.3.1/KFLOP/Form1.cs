using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KMotion_dotNet;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Net;
namespace KFLOP
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Parameters
        /// </summary>
        //******************************************************************************
        KM_Controller KM;
        string MyDocumentsOfPath, KFLOPPath, FileKFLOPPath, FilesPathName, FilesPathNameOpen, FilesPathNameSave;
        string FilesPathNameJogSpeed, FilesPathNameNumIO;
        string FilesPathNameXYpos;
        bool bolCheckConnected;
        bool bolCheckCardConnected;
        bool CheckSaved;
        bool butStartPushed;
        string MainPath;
        bool JoggingX = false, JoggingY = false;
        bool CheckDataOfListView;
        public bool CheckEnaInOut = false;
        bool PreInorOut01, PreInorOut02, PreInorOut03, PreInorOut04; //false: Input and Limit; true: Output

        struct Mydata
        {
            public string strcode, strcoorx, strcoory;
            public string strvel, strperor, strloop, strnumloop;

            Mydata(string tstrcode, string tstrcoorx, string tstrcooy,
                   string tstrvel, string tstrperor, string tstrloop, string tstrnumloop)
            {
                strcode = tstrcode;
                strcoorx = tstrcoorx;
                strcoory = tstrcooy;
                strvel = tstrvel;
                strperor = tstrperor;
                strloop = tstrloop;
                strnumloop = tstrnumloop;
            }
        }

        Mydata[] arraydata = new Mydata[1000];
        //******************************************************************************
        public Form1()
        {
            InitializeComponent();
            KM = new KMotion_dotNet.KM_Controller();
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            MyDocumentsOfPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            KFLOPPath = MyDocumentsOfPath + "\\KFLOP_Programs";
            if (!Directory.Exists(KFLOPPath))
            {
                Directory.CreateDirectory(KFLOPPath);
            }

            MainPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            FileKFLOPPath = MainPath + "\\Factors";
            FilesPathName = MainPath + "\\ScaleFactor.txt"; //File "*.txt" contains scale factors
            FilesPathNameXYpos = MainPath + "\\XYpos.txt"; //File "*.txt" contains scale factors
            FilesPathNameOpen = MainPath + "\\OpenPath.txt"; //File "*.txt" contains path Open
            FilesPathNameSave = MainPath + "\\SavePath.txt"; //File "*.txt" contains path Save
            FilesPathNameJogSpeed = MainPath + "\\JogSpeed.txt"; //File "*.txt" contains path JogSpeed
            FilesPathNameNumIO = MainPath + "\\NumIO.txt"; //File "*.txt" contains path number Input/Output

            //Read Jog Speed
            FileStream ofs = new FileStream(FilesPathNameJogSpeed, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader rd = new StreamReader(ofs);
            string strJogSpeed = rd.ReadLine();

            rd.Close();
            ofs.Close();
            //Read number Input/Output
            FileStream ofsN = new FileStream(FilesPathNameNumIO, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader rdN = new StreamReader(ofsN);
            string strNumIO01 = rdN.ReadLine();
            string strNumIO02 = rdN.ReadLine();
            string strNumIO03 = rdN.ReadLine();
            string strNumIO04 = rdN.ReadLine();

            rdN.Close();
            ofsN.Close();
            //Read Scale Factors
            double dounumX, doudenX, dounumY, doudenY;
            FileStream ofsS = new FileStream(FilesPathName, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader rdS = new StreamReader(ofsS);
            dounumX = Convert.ToDouble(rdS.ReadLine());
            doudenX = Convert.ToDouble(rdS.ReadLine());
            dounumY = Convert.ToDouble(rdS.ReadLine());
            doudenY = Convert.ToDouble(rdS.ReadLine());

            rdS.Close();
            ofsS.Close();

            douscaleX = dounumX / doudenX;
            douscaleY = dounumY / doudenY;
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Init parameters
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Tabs.SelectedIndex = 1;
            Jogspeed.Text = strJogSpeed;
            cbIO01.Text = strNumIO01;
            if ((strNumIO01.Substring(0, 1) == "I") | (strNumIO01.Substring(0, 1) == "L")) PreInorOut01 = false;
            else if (strNumIO01.Substring(0, 1) == "O") PreInorOut01 = true;
            cbIO02.Text = strNumIO02;
            if ((strNumIO02.Substring(0, 1) == "I") | (strNumIO02.Substring(0, 1) == "L")) PreInorOut02 = false;
            else if (strNumIO02.Substring(0, 1) == "O") PreInorOut02 = true;
            cbIO03.Text = strNumIO03;
            if ((strNumIO03.Substring(0, 1) == "I") | (strNumIO03.Substring(0, 1) == "L")) PreInorOut03 = false;
            else if (strNumIO03.Substring(0, 1) == "O") PreInorOut03 = true;
            cbIO04.Text = strNumIO04;
            if ((strNumIO04.Substring(0, 1) == "I") | (strNumIO04.Substring(0, 1) == "L")) PreInorOut04 = false;
            else if (strNumIO04.Substring(0, 1) == "O") PreInorOut04 = true;
            CheckEnaInOut = false;
            butStartPushed = false;
            bolCheckConnected = false;
            bolCheckCardConnected = false;
            CheckDataOfListView = false;
            CheckSaved = true;
            timerCheckConnect.Enabled = true;

            // goi ham dong bo data tai local
            SynData();
            //
           // TimerSendToServer.Start();
        }

        /// <summary>
        /// Fix column width of a listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = DataView.Columns[e.ColumnIndex].Width;
        }

        /// <summary>
        /// Command Buttons
        /// </summary>
        int TmpNum = 0;
        byte TmpCmd; //Choose butCmd, butInsertCmd, butEditCmd

        public void SaveDataToArray(int oTmpNum, string[] oarr)
        {
            //Save data to array
            arraydata[oTmpNum].strcode = oarr[2];
            arraydata[oTmpNum].strcoorx = oarr[3];
            arraydata[oTmpNum].strcoory = oarr[4];
            arraydata[oTmpNum].strvel = oarr[5];
            arraydata[oTmpNum].strperor = oarr[6];
            arraydata[oTmpNum].strloop = oarr[7];
            arraydata[oTmpNum].strnumloop = oarr[8];
        }

        public void DisplayDataToListview(int oindex)
        {
            DataView.Items[oindex - 1].SubItems[2].Text = arraydata[oindex].strcode;
            DataView.Items[oindex - 1].SubItems[3].Text = arraydata[oindex].strcoorx;
            DataView.Items[oindex - 1].SubItems[4].Text = arraydata[oindex].strcoory;
            DataView.Items[oindex - 1].SubItems[5].Text = arraydata[oindex].strvel;
            DataView.Items[oindex - 1].SubItems[6].Text = arraydata[oindex].strperor;
            DataView.Items[oindex - 1].SubItems[7].Text = arraydata[oindex].strloop;
            DataView.Items[oindex - 1].SubItems[8].Text = arraydata[oindex].strnumloop;
        }

        private void butCmd_Click(object sender, EventArgs e)
        {
            string[] arr = new string[9];
            ListViewItem itm;
            TmpCmd = 0;
            Command frmCommand = new Command();
            frmCommand.frmForm1 = this;
            frmCommand.F2TmpCmd = TmpCmd;
            frmCommand.ShowDialog();
            try
            {
                if (frmCommand.CheckOK == true)
                {
                    //Check Data in Listview whether changed
                    CheckDataOfListView = true;
                    //Incre parameter No.
                    TmpNum++;
                    //Add data to listview
                    if (TmpNum < 10)
                    {
                        arr[1] = "00" + TmpNum.ToString();

                    }
                    else if (TmpNum < 100)
                    {
                        arr[1] = "0" + TmpNum.ToString();
                    }
                    else arr[1] = TmpNum.ToString();

                    arr[2] = frmCommand.F2Code;
                    arr[3] = frmCommand.F2CoorX;
                    arr[4] = frmCommand.F2CoorY;
                    arr[5] = frmCommand.F2Velocity;
                    if (frmCommand.F2TmpPerfor == true) arr[6] = "X";
                    else arr[6] = "";
                    if (frmCommand.F2Loop == "None") arr[7] = "";
                    else if (frmCommand.F2Loop == "Start") arr[7] = "1";
                    else arr[7] = "9";
                    arr[8] = frmCommand.F2NumLoop;
                    itm = new ListViewItem(arr);
                    DataView.Items.Add(itm);
                    //Save data to array
                    SaveDataToArray(TmpNum, arr);
                    DataView.EnsureVisible(DataView.Items.Count - 1); //See line at DataView.Items.Count - 1
                    CheckSaved = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butInsertCmd_Click(object sender, EventArgs e)
        {
            string[] arr = new string[9];
            ListViewItem itm;
            try
            {
                if (DataView.SelectedItems.Count == 1)
                {
                    TmpCmd = 1;
                    Command frmCommand = new Command();
                    frmCommand.frmForm1 = this;
                    frmCommand.F2TmpCmd = TmpCmd;
                    frmCommand.ShowDialog();
                    if (frmCommand.CheckOK == true)
                    {
                        //Check Data in Listview whether changed
                        CheckDataOfListView = true;
                        //Incre parameter No.
                        TmpNum++;
                        //Add data to listview
                        if (TmpNum < 10)
                        {
                            arr[1] = "00" + TmpNum.ToString();

                        }
                        else if (TmpNum < 100)
                        {
                            arr[1] = "0" + TmpNum.ToString();
                        }
                        else arr[1] = TmpNum.ToString();
                        arr[2] = arraydata[TmpNum - 1].strcode;
                        arr[3] = arraydata[TmpNum - 1].strcoorx;
                        arr[4] = arraydata[TmpNum - 1].strcoory;
                        arr[5] = arraydata[TmpNum - 1].strvel;
                        arr[6] = arraydata[TmpNum - 1].strperor;
                        arr[7] = arraydata[TmpNum - 1].strloop;
                        arr[8] = arraydata[TmpNum - 1].strnumloop;
                        itm = new ListViewItem(arr);
                        DataView.Items.Add(itm);
                        //Save data to array
                        SaveDataToArray(TmpNum, arr);

                        for (int i = 0; i < DataView.Items.Count; i++)
                        {
                            if (DataView.Items[i].Selected)
                            {
                                index = i;
                                //Transfer data
                                for (int m = TmpNum; m > (index + 1); m--)
                                {
                                    arraydata[m - 1] = arraydata[m - 2];
                                }
                                //Save data to array
                                arr[2] = frmCommand.F2Code;
                                arr[3] = frmCommand.F2CoorX;
                                arr[4] = frmCommand.F2CoorY;
                                arr[5] = frmCommand.F2Velocity;
                                if (frmCommand.F2TmpPerfor == true) arr[6] = "X";
                                else arr[6] = "";
                                if (frmCommand.F2Loop == "None") arr[7] = "";
                                else if (frmCommand.F2Loop == "Start") arr[7] = "1";
                                else if (frmCommand.F2Loop == "End") arr[7] = "9";
                                arr[8] = frmCommand.F2NumLoop;

                                SaveDataToArray(index + 1, arr);
                                //Insert data to Listview
                                for (int k = 0; k < DataView.Items.Count; k++)
                                {
                                    DisplayDataToListview(k + 1);
                                }
                            }
                        }
                    }
                    CheckSaved = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int index;
        private void butEditCmd_Click(object sender, EventArgs e)
        {
            string[] arr = new string[9];
            try
            {
                if (DataView.SelectedItems.Count == 1)
                {
                    TmpCmd = 2;
                    Command frmCommand = new Command();
                    frmCommand.frmForm1 = this;
                    frmCommand.F2TmpCmd = TmpCmd;
                    for (int i = 0; i < DataView.Items.Count; i++)
                    {
                        if (DataView.Items[i].Selected)
                        {
                            //Display old data to "Command" Form
                            index = i;
                            frmCommand.F2Code = DataView.Items[index].SubItems[2].Text;
                            frmCommand.F2CoorX = DataView.Items[index].SubItems[3].Text;
                            frmCommand.F2CoorY = DataView.Items[index].SubItems[4].Text;
                            frmCommand.F2Velocity = DataView.Items[index].SubItems[5].Text;
                            frmCommand.F2Perfor = DataView.Items[index].SubItems[6].Text;
                            if (DataView.Items[index].SubItems[7].Text == "") frmCommand.F2Loop = "None";
                            else if (DataView.Items[index].SubItems[7].Text == "1") frmCommand.F2Loop = "Start";
                            else if (DataView.Items[index].SubItems[7].Text == "9") frmCommand.F2Loop = "End";
                            frmCommand.F2NumLoop = DataView.Items[index].SubItems[8].Text;
                        }
                    }
                    frmCommand.ShowDialog();
                    if (frmCommand.CheckOK == true)
                    {
                        //Check Data in Listview whether changed
                        CheckDataOfListView = true;
                        //Update new data to listview
                        DataView.Items[index].SubItems[2].Text = frmCommand.F2Code;
                        DataView.Items[index].SubItems[3].Text = frmCommand.F2CoorX;
                        DataView.Items[index].SubItems[4].Text = frmCommand.F2CoorY;
                        DataView.Items[index].SubItems[5].Text = frmCommand.F2Velocity;
                        if (frmCommand.F2TmpPerfor == true) DataView.Items[index].SubItems[6].Text = "X";
                        else DataView.Items[index].SubItems[6].Text = "";
                        if (frmCommand.F2Loop == "None") DataView.Items[index].SubItems[7].Text = "";
                        else if (frmCommand.F2Loop == "Start") DataView.Items[index].SubItems[7].Text = "1";
                        else if (frmCommand.F2Loop == "End") DataView.Items[index].SubItems[7].Text = "9";
                        DataView.Items[index].SubItems[8].Text = frmCommand.F2NumLoop;
                        //Save data to array
                        arr[2] = DataView.Items[index].SubItems[2].Text;
                        arr[3] = DataView.Items[index].SubItems[3].Text;
                        arr[4] = DataView.Items[index].SubItems[4].Text;
                        arr[5] = DataView.Items[index].SubItems[5].Text;
                        arr[6] = DataView.Items[index].SubItems[6].Text;
                        arr[7] = DataView.Items[index].SubItems[7].Text;
                        arr[8] = DataView.Items[index].SubItems[8].Text;

                        SaveDataToArray(index + 1, arr);
                    }
                    CheckSaved = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butDelCmd_Click(object sender, EventArgs e)
        {
            try
            {
                if (DataView.SelectedItems.Count == 1)
                {
                    for (int i = 0; i < DataView.Items.Count; i++)
                    {
                        if (DataView.Items[i].Selected)
                        {
                            DataView.Items[i].Remove();
                            //Check Data in Listview whether changed
                            CheckDataOfListView = true;

                            for (int m = (i + 1); m <= TmpNum; m++)
                            {
                                arraydata[m] = arraydata[m + 1];
                            }
                            TmpNum--;
                        }
                    }
                    for (int i = 0; i < DataView.Items.Count; i++)
                    {
                        if (i < 10)
                        {
                            DataView.Items[i].SubItems[1].Text = "00" + (i + 1).ToString();

                        }
                        else if (i < 100)
                        {
                            DataView.Items[i].SubItems[1].Text = "0" + (i + 1).ToString();
                        }
                        else DataView.Items[i].SubItems[1].Text = (i + 1).ToString();
                    }
                    CheckSaved = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Control motors (Start Button)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void MoveXY(double Xaxis, double Yaxis, double Vel)
        {
            WriteLineHandleException(String.Format("Vel0={0}", Vel));
            WriteLineHandleException(String.Format("Vel1={0}", Vel));
            WriteLineHandleException(String.Format("MoveXYZABC {0} {1} {2} {3} {4} {5}", Xaxis, Yaxis, 0, 0, 0, 0));
        }

        double vtmpG = 0;
        double dPosX, dPosY;
        double tmpPosX, tmpPosY;
        public void CodeG1(int tmpig1)
        {
            try
            {
                double Xtmp1, Ytmp1, vtmp1;

                Xtmp1 = Convert.ToDouble(GetStringItem(tmpig1, 3)) / douscaleX;
                Ytmp1 = Convert.ToDouble(GetStringItem(tmpig1, 4)) / douscaleY;
                Xtmp1 = Math.Round(Xtmp1, 1);
                Ytmp1 = Math.Round(Ytmp1, 1);
                if ("".CompareTo(GetDataViewValue(tmpig1, 5)) != 0)
                {
                    vtmpG = Convert.ToDouble(GetStringItem(tmpig1, 5));
                }
                vtmp1 = vtmpG * 30;
                MoveXY(Xtmp1, Ytmp1, vtmp1);

                do
                {
                    if (double.TryParse(txtPosX.Text, out tmpPosX) &&
                        double.TryParse(txtPosY.Text, out tmpPosY))
                    {
                        dPosX = Math.Round((tmpPosX / douscaleX), 1);
                        dPosY = Math.Round((tmpPosY / douscaleY), 1);
                    }
                } while (((dPosX != Xtmp1) | (dPosY != Ytmp1)) & (bolStop == true));
            }
            catch (DMException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        public void CodeG2(int tmpig2)
        {
            try
            {
                double Xtmp2, Ytmp2, vtmp2;

                Xtmp2 = (Double.Parse(txtPosX.Text) + Double.Parse(GetStringItem(tmpig2, 3))) / douscaleX;
                Ytmp2 = (Double.Parse(txtPosY.Text) + Double.Parse(GetStringItem(tmpig2, 4))) / douscaleY;
                Xtmp2 = Math.Round(Xtmp2, 1);
                Ytmp2 = Math.Round(Ytmp2, 1);
                if ("".CompareTo(GetDataViewValue(tmpig2, 5)) != 0)
                {
                    vtmpG = Double.Parse(GetStringItem(tmpig2, 5));
                }
                vtmp2 = vtmpG * 30;
                MoveXY(Xtmp2, Ytmp2, vtmp2);
                do
                {
                    if (double.TryParse(txtPosX.Text, out tmpPosX) &&
                        double.TryParse(txtPosY.Text, out tmpPosY))
                    {
                        dPosX = Math.Round((tmpPosX / douscaleX), 1);
                        dPosY = Math.Round((tmpPosY / douscaleY), 1);
                    }
                } while (((dPosX != Xtmp2) | (dPosY != Ytmp2)) & (bolStop == true));
            }
            catch (DMException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        public void CodeG3(int iCodeG3)
        {
            if (bolStop == true)
            {
                PrintColor(iCodeG3, Color.White);
            }
        }

        public void PerporateCode()
        {
            var Input01 = KM.GetIO(1, IO_TYPE.DIGITAL_IN, "I01");
            var Output01 = KM.GetIO(28, IO_TYPE.DIGITAL_OUT, "O01");
            Output01.SetDigitalValue(true);

            do
            {
                while ((Input01.GetDigitalValue() == true) && (bolStop == true) && (bolPause == false)) { };
                Thread.Sleep(1000);
                while ((Input01.GetDigitalValue() == false) && (bolStop == true) && (bolPause == false)) { };
                Thread.Sleep(0);
            }
            while (bolPause == true);
            //code add to check mode, if hole ++ variable CountHole
            if (labelMode.Text == "Hole")
                Product.CountHole++;
            //
            Output01.SetDigitalValue(false);
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Invoke Required
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        delegate object GetControlValueCallback(int row, int col);
        private object GetDataViewValue(int row, int col)
        {
            if (DataView.InvokeRequired)
            {
                // On a different thread
                GetControlValueCallback callback = new GetControlValueCallback(this.GetDataViewValue);
                return (this.Invoke(callback, row, col));
            }
            else
            {
                return DataView.Items[row].SubItems[col].Text;
            }
        }
        private void PrintColor(int Item, Color color)
        {
            if (DataView.InvokeRequired)
            {
                DataView.BeginInvoke(new MethodInvoker(() =>
                {
                    DataView.Items[Item].BackColor = color;
                }));
            }
            else
            {
                DataView.Items[Item].BackColor = color;
            }
        }

        private void EnablebutStart(bool bStart)
        {
            if (DataView.InvokeRequired)
            {
                DataView.BeginInvoke(new MethodInvoker(() =>
                {
                    butStart.Enabled = bStart;
                }));
            }
            else
            {
                butStart.Enabled = bStart;
            }
        }

        delegate string getCurrentItemCallBack(int irow, int icol);
        private string GetStringItem(int irow, int icol)
        {
            if (this.DataView.InvokeRequired)
            {
                getCurrentItemCallBack d = new getCurrentItemCallBack(GetStringItem);
                return this.Invoke(d, new object[] { irow, icol }).ToString();
            }
            else
            {
                return this.DataView.Items[irow].SubItems[icol].Text;
            }
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Create a thread to run motor XY
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void HandleRunMotor()
        {
            bool CheckLoop = false;
            try
            {
                if (bolCheckConnected == true)
                {
                    if (DataView.Items.Count > 0)
                    {
                        if ("0".CompareTo(GetDataViewValue((DataView.Items.Count - 1), 2)) == 0)
                        {
                            StatusRun.Text = "Status: RUN";
                            for (int i = 0; i < DataView.Items.Count; i++)
                            {
                                //Print Code Running Line
                                if (bolStop == true)
                                {
                                    PrintColor(i, Color.SkyBlue);
                                }
                                //Code
                                if (bolStop == true)
                                {
                                    if ("0".CompareTo(GetDataViewValue(i, 2)) == 0)
                                    {
                                        //Print Code Stopping Line
                                        PrintColor(i, Color.White);
                                        // check nêu mode = bar thi tang bien đếm lên
                                        if (labelMode.Text == "Bar")
                                            Product.CountBar++;
                                        //
                                        break;
                                    }
                                    else if ("1".CompareTo(GetDataViewValue(i, 2)) == 0) CodeG1(i);
                                    else if ("2".CompareTo(GetDataViewValue(i, 2)) == 0) CodeG2(i);
                                    else if ("3".CompareTo(GetDataViewValue(i, 2)) == 0) CodeG3(i);
                                }
                                //Perporate 
                                if (bolStop == true)
                                {
                                    if ("X".CompareTo(GetDataViewValue(i, 6)) == 0)
                                    {
                                        PerporateCode();
                                    }
                                }
                                //Loop
                                if (bolStop == true)
                                {
                                    if ("1".CompareTo(GetDataViewValue(i, 7)) == 0)
                                    {
                                        if (numofloop == 0)
                                        {
                                            arrloop[numofloop].strnum = i.ToString();
                                            arrloop[numofloop].strloop = GetStringItem(i, 8);
                                            numofloop++;
                                        }
                                        else
                                        {
                                            if (arrloop[numofloop - 1].strnum != i.ToString())
                                            {
                                                arrloop[numofloop].strnum = i.ToString();
                                                arrloop[numofloop].strloop = GetStringItem(i, 8);
                                                numofloop++;
                                            }
                                        }
                                    }
                                    else if ("9".CompareTo(GetDataViewValue(i, 7)) == 0)
                                    {
                                        numofloop--;
                                        arrloop[numofloop].strloop = (Convert.ToInt32(arrloop[numofloop].strloop) - 1).ToString();
                                        if (arrloop[numofloop].strloop == "0")
                                        {
                                            arrloop[numofloop].strnum = null;
                                            arrloop[numofloop].strloop = null;
                                        }
                                        else
                                        {
                                            PrintColor(i, Color.White);
                                            CheckLoop = true;
                                            i = Convert.ToInt32(arrloop[numofloop].strnum) - 1;
                                            numofloop++;
                                        }
                                    }
                                }
                                //Print Code Stopping Line
                                if (bolStop == true)
                                {
                                    if (CheckLoop == false)
                                    {
                                        PrintColor(i, Color.White);
                                    }
                                    CheckLoop = false;
                                }
                                //End
                            }
                            if (bolStop == true)
                            {
                                butStartPushed = false;
                                StatusRun.Text = "Status: STOP";
                            }
                        }
                        else MessageBox.Show("Please add code G0 at the end of this program!");
                    }
                    else MessageBox.Show("Please add code!");
                }
            }
            catch (Exception ex)
            {
                butStartPushed = false;
                for (int i = 0; i < DataView.Items.Count; i++)
                {
                    PrintColor(i, Color.White);
                }
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
            }
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++

        struct DataLoop
        {
            public string strnum, strloop;

            DataLoop(string tstrnum, string tstrloop)
            {
                strnum = tstrnum;
                strloop = tstrloop;
            }
        }

        DataLoop[] arrloop = new DataLoop[1000];
        int numofloop = 0;

        double douscaleX, douscaleY;
        bool bolStop;
        private void butStart_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (DataView.Items.Count > 0)
                {
                    if (DataView.Items[0].SubItems[5].Text != "")
                    {
                        if (butStart.Text == "Start")
                        {
                            butStart.Text = "Stop";
                            bolStop = true;
                            for (int i = 0; i < DataView.Items.Count; i++)
                            {
                                if (DataView.Items[i].Selected == true)
                                {
                                    DataView.Items[i].Selected = false;
                                }
                                DataView.Items[i].BackColor = Color.White;
                            }

                            double dounumX, doudenX, dounumY, doudenY;
                            FileStream ofs = new FileStream(FilesPathName, FileMode.Open, FileAccess.Read, FileShare.None);
                            StreamReader rd = new StreamReader(ofs);
                            dounumX = Convert.ToDouble(rd.ReadLine());
                            doudenX = Convert.ToDouble(rd.ReadLine());
                            dounumY = Convert.ToDouble(rd.ReadLine());
                            doudenY = Convert.ToDouble(rd.ReadLine());

                            rd.Close();
                            ofs.Close();

                            douscaleX = dounumX / doudenX;
                            douscaleY = dounumY / doudenY;

                            Thread RunThreadMotor = new Thread(HandleRunMotor);
                            RunThreadMotor.Start();
                        }
                        else
                        {
                            butStart.Text = "Start";
                            StatusRun.Text = "Status: STOP";
                            bolStop = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter Velocity of Line 1");
                    }
                }
            }
        }

        /// <summary>
        /// Manual Form: I/O and up/down/left/right button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool bolIO01 = false, bolIO02 = false, bolIO03 = false, bolIO04 = false;

        private void butO01_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strtmpoutputfull = cbIO01.Text;
                    string strtmpoutputio = strtmpoutputfull.Substring(0, 1);
                    string strtmpoutputnum = strtmpoutputfull.Substring(1, 2);
                    if (strtmpoutputnum == "0+") strtmpoutputnum = "12";
                    else if (strtmpoutputnum == "0-") strtmpoutputnum = "13";
                    else if (strtmpoutputnum == "1+") strtmpoutputnum = "14";
                    else if (strtmpoutputnum == "1-") strtmpoutputnum = "15";

                    if (strtmpoutputio == "O")
                    {
                        var Output01 = KM.GetIO(Convert.ToInt32(strtmpoutputnum), IO_TYPE.DIGITAL_OUT, "O01");
                        {
                            if (bolIO01 == false)
                            {
                                bolIO01 = true;
                                Output01.SetDigitalValue(true);
                                light1.BackColor = Color.Lime;
                            }
                            else
                            {
                                bolIO01 = false;
                                Output01.SetDigitalValue(false);
                                light1.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void cbIO01_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    TimerInput01.Enabled = false;
                    string strInorOutfull = cbIO01.Text;
                    string strInorOutio = strInorOutfull.Substring(0, 1);

                    if ((strInorOutio == "I") | (strInorOutio == "L"))
                    {
                        butO01.Enabled = false;
                        TimerInput01.Enabled = true;
                    }
                    else if (strInorOutio == "O")
                    {
                        butO01.Enabled = true;
                        TimerInput01.Enabled = false;
                    }
                }
            }
        }

        private void TimerInput01_Tick(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strInorOutfull = cbIO01.Text;
                    string strInorOutnum = strInorOutfull.Substring(1, 2);
                    if (strInorOutnum == "0+") strInorOutnum = "12";
                    else if (strInorOutnum == "0-") strInorOutnum = "13";
                    else if (strInorOutnum == "1+") strInorOutnum = "14";
                    else if (strInorOutnum == "1-") strInorOutnum = "15";

                    var Input01 = KM.GetIO(Convert.ToInt32(strInorOutnum), IO_TYPE.DIGITAL_IN, "I01");
                    if (Input01.GetDigitalValue() == true)
                    {
                        light1.BackColor = Color.Red;
                    }
                    else
                    {
                        light1.BackColor = Color.Lime;
                    }
                }
            }
        }

        private void butO02_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strtmpoutputfull = cbIO02.Text;
                    string strtmpoutputio = strtmpoutputfull.Substring(0, 1);
                    string strtmpoutputnum = strtmpoutputfull.Substring(1, 2);
                    if (strtmpoutputnum == "0+") strtmpoutputnum = "12";
                    else if (strtmpoutputnum == "0-") strtmpoutputnum = "13";
                    else if (strtmpoutputnum == "1+") strtmpoutputnum = "14";
                    else if (strtmpoutputnum == "1-") strtmpoutputnum = "15";

                    if (strtmpoutputio == "O")
                    {
                        var Output02 = KM.GetIO(Convert.ToInt32(strtmpoutputnum), IO_TYPE.DIGITAL_OUT, "O02");
                        {
                            if (bolIO02 == false)
                            {
                                bolIO02 = true;
                                Output02.SetDigitalValue(true);
                                light2.BackColor = Color.Lime;
                            }
                            else
                            {
                                bolIO02 = false;
                                Output02.SetDigitalValue(false);
                                light2.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void cbIO02_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    TimerInput02.Enabled = false;
                    string strInorOutfull = cbIO02.Text;
                    string strInorOutio = strInorOutfull.Substring(0, 1);

                    if ((strInorOutio == "I") | (strInorOutio == "L"))
                    {
                        butO02.Enabled = false;
                        TimerInput02.Enabled = true;
                    }
                    else if (strInorOutio == "O")
                    {
                        butO02.Enabled = true;
                        TimerInput02.Enabled = false;
                    }
                }
            }
        }

        private void TimerInput02_Tick(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strInorOutfull = cbIO02.Text;
                    string strInorOutnum = strInorOutfull.Substring(1, 2);
                    if (strInorOutnum == "0+") strInorOutnum = "12";
                    else if (strInorOutnum == "0-") strInorOutnum = "13";
                    else if (strInorOutnum == "1+") strInorOutnum = "14";
                    else if (strInorOutnum == "1-") strInorOutnum = "15";

                    var Input02 = KM.GetIO(Convert.ToInt32(strInorOutnum), IO_TYPE.DIGITAL_IN, "I02");
                    if (Input02.GetDigitalValue() == true)
                    {
                        light2.BackColor = Color.Red;
                    }
                    else
                    {
                        light2.BackColor = Color.Lime;
                    }
                }
            }
        }

        private void butO03_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strtmpoutputfull = cbIO03.Text;
                    string strtmpoutputio = strtmpoutputfull.Substring(0, 1);
                    string strtmpoutputnum = strtmpoutputfull.Substring(1, 2);
                    if (strtmpoutputnum == "0+") strtmpoutputnum = "12";
                    else if (strtmpoutputnum == "0-") strtmpoutputnum = "13";
                    else if (strtmpoutputnum == "1+") strtmpoutputnum = "14";
                    else if (strtmpoutputnum == "1-") strtmpoutputnum = "15";

                    if (strtmpoutputio == "O")
                    {
                        var Output03 = KM.GetIO(Convert.ToInt32(strtmpoutputnum), IO_TYPE.DIGITAL_OUT, "O03");
                        {
                            if (bolIO03 == false)
                            {
                                bolIO03 = true;
                                Output03.SetDigitalValue(true);
                                light3.BackColor = Color.Lime;
                            }
                            else
                            {
                                bolIO03 = false;
                                Output03.SetDigitalValue(false);
                                light3.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void cbIO03_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    TimerInput03.Enabled = false;
                    string strInorOutfull = cbIO03.Text;
                    string strInorOutio = strInorOutfull.Substring(0, 1);

                    if ((strInorOutio == "I") | (strInorOutio == "L"))
                    {
                        butO03.Enabled = false;
                        TimerInput03.Enabled = true;
                    }
                    else if (strInorOutio == "O")
                    {
                        butO03.Enabled = true;
                        TimerInput03.Enabled = false;
                    }
                }
            }
        }

        private void TimerInput03_Tick(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strInorOutfull = cbIO03.Text;
                    string strInorOutnum = strInorOutfull.Substring(1, 2);
                    if (strInorOutnum == "0+") strInorOutnum = "12";
                    else if (strInorOutnum == "0-") strInorOutnum = "13";
                    else if (strInorOutnum == "1+") strInorOutnum = "14";
                    else if (strInorOutnum == "1-") strInorOutnum = "15";

                    var Input03 = KM.GetIO(Convert.ToInt32(strInorOutnum), IO_TYPE.DIGITAL_IN, "I03");
                    if (Input03.GetDigitalValue() == true)
                    {
                        light3.BackColor = Color.Red;
                    }
                    else
                    {
                        light3.BackColor = Color.Lime;
                    }
                }
            }
        }

        private void butO04_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strtmpoutputfull = cbIO04.Text;
                    string strtmpoutputio = strtmpoutputfull.Substring(0, 1);
                    string strtmpoutputnum = strtmpoutputfull.Substring(1, 2);
                    if (strtmpoutputnum == "0+") strtmpoutputnum = "12";
                    else if (strtmpoutputnum == "0-") strtmpoutputnum = "13";
                    else if (strtmpoutputnum == "1+") strtmpoutputnum = "14";
                    else if (strtmpoutputnum == "1-") strtmpoutputnum = "15";

                    if (strtmpoutputio == "O")
                    {
                        var Output04 = KM.GetIO(Convert.ToInt32(strtmpoutputnum), IO_TYPE.DIGITAL_OUT, "O04");
                        {
                            if (bolIO04 == false)
                            {
                                bolIO04 = true;
                                Output04.SetDigitalValue(true);
                                light4.BackColor = Color.Lime;
                            }
                            else
                            {
                                bolIO04 = false;
                                Output04.SetDigitalValue(false);
                                light4.BackColor = Color.Red;
                            }
                        }
                    }
                }
            }
        }

        private void cbIO04_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    TimerInput04.Enabled = false;
                    string strInorOutfull = cbIO04.Text;
                    string strInorOutio = strInorOutfull.Substring(0, 1);

                    if ((strInorOutio == "I") | (strInorOutio == "L"))
                    {
                        butO04.Enabled = false;
                        TimerInput04.Enabled = true;
                    }
                    else if (strInorOutio == "O")
                    {
                        butO04.Enabled = true;
                        TimerInput04.Enabled = false;
                    }
                }
            }
        }

        private void TimerInput04_Tick(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (CheckEnaInOut == true)
                {
                    string strInorOutfull = cbIO04.Text;
                    string strInorOutnum = strInorOutfull.Substring(1, 2);
                    if (strInorOutnum == "0+") strInorOutnum = "12";
                    else if (strInorOutnum == "0-") strInorOutnum = "13";
                    else if (strInorOutnum == "1+") strInorOutnum = "14";
                    else if (strInorOutnum == "1-") strInorOutnum = "15";

                    var Input04 = KM.GetIO(Convert.ToInt32(strInorOutnum), IO_TYPE.DIGITAL_IN, "I04");
                    if (Input04.GetDigitalValue() == true)
                    {
                        light4.BackColor = Color.Red;
                    }
                    else
                    {
                        light4.BackColor = Color.Lime;
                    }
                }
            }
        }

        /// <summary>
        /// Left, Right, Down, Up Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void WriteLineHandleException(string s)
        {
            try
            {
                KM.WriteLine(s);
            }
            catch (DMException ex) // in case disconnect in the middle of reading status
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        public void JogStop(int iaxis, string straxis)
        {
            if (bolCheckConnected == true)
            {
                var axis = KM.GetAxis(iaxis, straxis);
                axis.Jog(0);
            }
        }

        private void butLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (Jogspeed.Text != "")
                {
                    var Input13 = KM.GetIO(13, IO_TYPE.DIGITAL_IN, "I01");
                    StatusRun.Text = "Status: RUN";
                    if (Input13.GetDigitalValue() == false)
                    {
                        WriteLineHandleException(String.Format("EnableAxis0"));
                    }
                    int iTempJog = Convert.ToInt32(Jogspeed.Text) * 30;
                    WriteLineHandleException(String.Format("Jog0={0}", iTempJog));
                    JoggingX = true;
                }
                else MessageBox.Show("Please enter Jog speed!");
            }
        }

        private void butRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (Jogspeed.Text != "")
                {
                    var Input12 = KM.GetIO(12, IO_TYPE.DIGITAL_IN, "I02");
                    StatusRun.Text = "Status: RUN";
                    if (Input12.GetDigitalValue() == false)
                    {
                        WriteLineHandleException(String.Format("EnableAxis0"));
                    }
                    int iTempJog = Convert.ToInt32(Jogspeed.Text) * 30;
                    WriteLineHandleException(String.Format("Jog0={0}", -iTempJog));
                    JoggingX = true;
                }
                else MessageBox.Show("Please enter Jog speed!");
            }
        }

        private void butX_Stop(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                StatusRun.Text = "Status: STOP";
                if (JoggingX)
                {
                    WriteLineHandleException(String.Format("Jog0={0}", 0));
                    JoggingX = false;
                }
            }
        }

        private void butUp_MouseDown(object sender, MouseEventArgs e)
        {

            if (bolCheckConnected == true)
            {
                if (Jogspeed.Text != "")
                {
                    var Input15 = KM.GetIO(15, IO_TYPE.DIGITAL_IN, "I03");
                    StatusRun.Text = "Status: RUN";
                    if (Input15.GetDigitalValue() == false)
                    {
                        WriteLineHandleException(String.Format("EnableAxis1"));
                    }
                    int iTempJog = Convert.ToInt32(Jogspeed.Text) * 30;
                    WriteLineHandleException(String.Format("Jog1={0}", iTempJog));
                    JoggingY = true;
                }
                else MessageBox.Show("Please enter Jog speed!");
            }
        }

        private void butDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (Jogspeed.Text != "")
                {
                    var Input14 = KM.GetIO(14, IO_TYPE.DIGITAL_IN, "I04");
                    StatusRun.Text = "Status: RUN";
                    if (Input14.GetDigitalValue() == false)
                    {
                        WriteLineHandleException(String.Format("EnableAxis1"));
                    }
                    int iTempJog = Convert.ToInt32(Jogspeed.Text) * 30;
                    WriteLineHandleException(String.Format("Jog1={0}", -iTempJog));
                    JoggingY = true;
                }
                else MessageBox.Show("Please enter Jog speed!");
            }
        }

        private void butY_Stop(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                StatusRun.Text = "Status: STOP";
                if (JoggingY)
                {
                    WriteLineHandleException(String.Format("Jog1={0}", 0));
                    JoggingY = false;
                }
            }
        }

        /// <summary>
        /// Determine Board Type connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void SetCurrenPosition(int iPosNum, string strPos, double iPosi)
        {
            var axis = KM.GetAxis(iPosNum, strPos);
            axis.SetCurrentPosition(iPosi);
        }

        int iLoopStart = 0;
        private void timerCheckConnect_Tick(object sender, EventArgs e)
        {
            int[] List;
            int nBoards = 0;

            // check how many boards connected
            List = KM.GetBoards(out nBoards);
            if (nBoards > 0)
            {
                if (this.Text == "Busbar Punching Controller - Disconnected")
                {
                    this.Text = String.Format("Busbar Punching Controller - Connected - USB location {0:X}", List[0]);
                    bolCheckCardConnected = true;
                    if (bolCheckConnected == true)
                    {
                        string pathtxt = MainPath + @"\CPrograms\DefineXYAxis.c";

                        try
                        {
                            //Load and Run Thread 1
                            string Result = KM.CompileAndLoadCoff(1, pathtxt, false);
                            if (Result != "")
                            {
                                MessageBox.Show(Result, "Compile Error");
                            }
                            else
                            {
                                // everything ok, execute the Thread
                                KM.ExecuteProgram(1);
                                KM.WaitForThreadComplete(1, 500);
                                KM.KillProgramThreads(1);
                            }
                            //Read XY current position
                            FileStream ofs = new FileStream(FilesPathNameXYpos, FileMode.Open, FileAccess.Read, FileShare.None);
                            StreamReader rd = new StreamReader(ofs);
                            string strXaxis = rd.ReadLine();
                            string strYaxis = rd.ReadLine();
                            rd.Close();
                            ofs.Close();

                            SetCurrenPosition(0, "X", Math.Round((Convert.ToDouble(strXaxis) / douscaleX), 1));
                            SetCurrenPosition(1, "Y", Math.Round((Convert.ToDouble(strYaxis) / douscaleY), 1));

                            //Load and Run Thread 1
                            string strResult = KM.CompileAndLoadCoff(1, pathtxt, false);
                            if (strResult != "")
                            {
                                MessageBox.Show(strResult, "Compile Error");
                            }
                            else
                            {
                                // everything ok, execute the Thread
                                KM.ExecuteProgram(1);
                                KM.WaitForThreadComplete(1, 500);
                                KM.KillProgramThreads(1);
                            }
                        }
                        catch (DMException ex)
                        {
                            MessageBox.Show(ex.InnerException.Message);
                        }
                    }
                }
                else
                {
                    this.Text = String.Format("Busbar Punching Controller - Connected - USB location {0:X}", List[0]);
                    bolCheckCardConnected = true;
                }
            }
            else
            {
                if (this.Text.Contains("Connected") == true)
                {
                    //Save Current Position
                    FileStream rfs = new FileStream(FilesPathNameXYpos, FileMode.Create, FileAccess.Write, FileShare.None);
                    StreamWriter wr = new StreamWriter(rfs);
                    wr.WriteLine(txtPosX.Text);
                    wr.WriteLine(txtPosY.Text);

                    wr.Flush();
                    wr.Close();
                    rfs.Close();
                }
                this.Text = "Busbar Punching Controller - Disconnected";
                bolCheckConnected = false;
                bolCheckCardConnected = false;
            }

            if (KM.WaitToken(100) == KMOTION_TOKEN.KMOTION_LOCKED)
            {
                KM_MainStatus MainStatus;

                try
                {
                    MainStatus = KM.GetStatus(false);  // we already have a lock
                    KM.ReleaseToken();

                    double dounumX, doudenX, dounumY, doudenY;
                    FileStream ofs = new FileStream(FilesPathName, FileMode.Open, FileAccess.Read, FileShare.None);
                    StreamReader rd = new StreamReader(ofs);
                    dounumX = Convert.ToDouble(rd.ReadLine());
                    doudenX = Convert.ToDouble(rd.ReadLine());
                    dounumY = Convert.ToDouble(rd.ReadLine());
                    doudenY = Convert.ToDouble(rd.ReadLine());

                    rd.Close();
                    ofs.Close();
                    douscaleX = dounumX / doudenX;
                    douscaleY = dounumY / doudenY;
                    double tmpX, tmpY;
                    tmpX = Math.Round(((Convert.ToDouble(String.Format("{0:F1}", MainStatus.Destination[0]))) * douscaleX), 1);
                    tmpY = Math.Round(((Convert.ToDouble(String.Format("{0:F1}", MainStatus.Destination[1]))) * douscaleY), 1);
                    txtPosX.Text = String.Format("{0:0.0}", tmpX);
                    txtPosY.Text = String.Format("{0:0.0}", tmpY);
                    txtPulseX.Text = String.Format("{0:0}", MainStatus.Destination[0]);
                    txtPulseY.Text = String.Format("{0:0}", MainStatus.Destination[1]);
                }
                catch (DMException ex) // in case disconnect in the middle of reading status
                {
                    KM.ReleaseToken();
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
            //Check whether Start Button pushed
            if (bolCheckConnected == true)
            {
                if (Tabs.SelectedTab.Name == "Auto")
                {
                    if (butStartPushed == false)
                    {
                        var Input00 = KM.GetIO(0, IO_TYPE.DIGITAL_IN, "I00");
                        if (Input00.GetDigitalValue() == false)
                        {
                            iLoopStart++;
                            if (iLoopStart == 5)
                            {
                                butStartPushed = true;
                                Thread RunThreadMotor = new Thread(HandleRunMotor);
                                RunThreadMotor.Start();
                            }
                        }
                        else iLoopStart = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Functions in File: New, Open, Save, Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///

        private void menufileNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tabs.SelectedTab.Name == "Auto")
                {
                    labNameProgram.Text = "";
                    for (int i = 1; i <= TmpNum; i++)
                    {
                        arraydata[i] = arraydata[0];
                    }
                    DataView.Items.Clear();
                    TmpNum = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int checkOpenorSave = 0;
        string pathnameopen;
        private void menufileOpen_Click(object sender, EventArgs e)
        {
            if (Tabs.SelectedTab.Name == "Auto")
            {
                string[] arr = new string[9];
                ListViewItem itm;
                pathnameopen = null;
                //Read path Open
                FileStream ofs = new FileStream(FilesPathNameOpen, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader rd = new StreamReader(ofs);
                string strPathOpen = rd.ReadLine();

                rd.Close();
                ofs.Close();
                //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                OpenFileDialog loadcodeprogram = new OpenFileDialog();
                loadcodeprogram.InitialDirectory = strPathOpen;
                loadcodeprogram.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                loadcodeprogram.RestoreDirectory = true;

                if (loadcodeprogram.ShowDialog() == DialogResult.OK)
                {
                    pathnameopen = loadcodeprogram.FileName;
                    for (int i = 1; i <= TmpNum; i++)
                    {
                        arraydata[i] = arraydata[0];
                    }
                    DataView.Items.Clear();
                    TmpNum = 0;
                    //Filter File Name(no full name)
                    string findfilename = new DirectoryInfo(pathnameopen).Name;
                    int tmplength = findfilename.Length;
                    string filename = findfilename.Substring(0, tmplength - 4);
                    //Load data to listview from file *.xml
                    DataSet ds = new DataSet();
                    ds.ReadXml(pathnameopen);
                    try
                    {
                        for (int m = 0; m < ds.Tables[filename].Rows.Count; m++)
                        {
                            TmpNum++;
                            arr[1] = ds.Tables[filename].Rows[m][0].ToString();
                            arr[2] = ds.Tables[filename].Rows[m][1].ToString();
                            arr[3] = ds.Tables[filename].Rows[m][2].ToString();
                            arr[4] = ds.Tables[filename].Rows[m][3].ToString();
                            arr[5] = ds.Tables[filename].Rows[m][4].ToString();
                            arr[6] = ds.Tables[filename].Rows[m][5].ToString();
                            arr[7] = ds.Tables[filename].Rows[m][6].ToString();
                            arr[8] = ds.Tables[filename].Rows[m][7].ToString();
                            itm = new ListViewItem(arr);
                            DataView.Items.Add(itm);
                            //Save data to array
                            SaveDataToArray(TmpNum, arr);
                        }
                        labNameProgram.Text = filename;
                        checkOpenorSave = 1;
                        //Save path Open
                        string strnameopen = new FileInfo(pathnameopen).DirectoryName;
                        FileStream rfs = new FileStream(FilesPathNameOpen, FileMode.Create, FileAccess.Write, FileShare.None);
                        StreamWriter wr = new StreamWriter(rfs);
                        wr.WriteLine(strnameopen);

                        wr.Flush();
                        wr.Close();
                        rfs.Close();
                    }
                    catch (Exception ex)
                    {
                        //Save path Open
                        string strnameopen = new FileInfo(pathnameopen).DirectoryName;
                        FileStream rfs = new FileStream(FilesPathNameOpen, FileMode.Create, FileAccess.Write, FileShare.None);
                        StreamWriter wr = new StreamWriter(rfs);
                        wr.WriteLine(strnameopen);

                        wr.Flush();
                        wr.Close();
                        rfs.Close();

                        labNameProgram.Text = filename;
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        string pathnamesave;
        private void menufileSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tabs.SelectedTab.Name == "Auto")
                {
                    if (labNameProgram.Text == "")
                    {
                        //Read path Open
                        FileStream ofs = new FileStream(FilesPathNameSave, FileMode.Open, FileAccess.Read, FileShare.None);
                        StreamReader rd = new StreamReader(ofs);
                        string strPathSave = rd.ReadLine();

                        rd.Close();
                        ofs.Close();
                        //Create data table
                        DataSet ds = new DataSet();
                        SaveFileDialog savecodedialog = new SaveFileDialog();
                        savecodedialog.InitialDirectory = strPathSave;
                        savecodedialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                        savecodedialog.RestoreDirectory = true;
                        if (savecodedialog.ShowDialog() == DialogResult.OK)
                        {
                            //Filter File Name(no full name)
                            pathnamesave = savecodedialog.FileName;
                            string findfilenamesave = new DirectoryInfo(pathnamesave).Name;
                            int tmplengthsave = findfilenamesave.Length;
                            string filenamesave = findfilenamesave.Substring(0, tmplengthsave - 4);
                            //Save File
                            DataTable savecodeprogram = new DataTable();
                            savecodeprogram.TableName = filenamesave;
                            savecodeprogram.Columns.Add("Num");
                            savecodeprogram.Columns.Add("Code");
                            savecodeprogram.Columns.Add("CoorX");
                            savecodeprogram.Columns.Add("CoorY");
                            savecodeprogram.Columns.Add("Vel");
                            savecodeprogram.Columns.Add("Perfor");
                            savecodeprogram.Columns.Add("Loop");
                            savecodeprogram.Columns.Add("NumLoop");
                            ds.Tables.Add(savecodeprogram);

                            for (int i = 0; i < DataView.Items.Count; i++)
                            {
                                DataRow row = ds.Tables[filenamesave].NewRow();
                                row["Num"] = DataView.Items[i].SubItems[1].Text;
                                row["Code"] = DataView.Items[i].SubItems[2].Text;
                                row["CoorX"] = DataView.Items[i].SubItems[3].Text;
                                row["CoorY"] = DataView.Items[i].SubItems[4].Text;
                                row["Vel"] = DataView.Items[i].SubItems[5].Text;
                                row["Perfor"] = DataView.Items[i].SubItems[6].Text;
                                row["Loop"] = DataView.Items[i].SubItems[7].Text;
                                row["NumLoop"] = DataView.Items[i].SubItems[8].Text;

                                ds.Tables[filenamesave].Rows.Add(row);
                            }

                            ds.WriteXml(pathnamesave);
                            ds.Tables.Clear();
                            labNameProgram.Text = filenamesave;
                            checkOpenorSave = 2;
                            //Save path Save
                            string strnamesave = new FileInfo(pathnamesave).DirectoryName;
                            FileStream rfs = new FileStream(FilesPathNameSave, FileMode.Create, FileAccess.Write, FileShare.None);
                            StreamWriter wr = new StreamWriter(rfs);
                            wr.WriteLine(strnamesave);

                            wr.Flush();
                            wr.Close();
                            rfs.Close();
                        }
                    }
                    else
                    {
                        //Create data table
                        DataSet ds = new DataSet();
                        string strNameSave = labNameProgram.Text;
                        //Save File
                        DataTable savecodeprogram = new DataTable();
                        savecodeprogram.TableName = strNameSave;
                        savecodeprogram.Columns.Add("Num");
                        savecodeprogram.Columns.Add("Code");
                        savecodeprogram.Columns.Add("CoorX");
                        savecodeprogram.Columns.Add("CoorY");
                        savecodeprogram.Columns.Add("Vel");
                        savecodeprogram.Columns.Add("Perfor");
                        savecodeprogram.Columns.Add("Loop");
                        savecodeprogram.Columns.Add("NumLoop");
                        ds.Tables.Add(savecodeprogram);

                        for (int i = 0; i < DataView.Items.Count; i++)
                        {
                            DataRow row = ds.Tables[strNameSave].NewRow();
                            row["Num"] = DataView.Items[i].SubItems[1].Text;
                            row["Code"] = DataView.Items[i].SubItems[2].Text;
                            row["CoorX"] = DataView.Items[i].SubItems[3].Text;
                            row["CoorY"] = DataView.Items[i].SubItems[4].Text;
                            row["Vel"] = DataView.Items[i].SubItems[5].Text;
                            row["Perfor"] = DataView.Items[i].SubItems[6].Text;
                            row["Loop"] = DataView.Items[i].SubItems[7].Text;
                            row["NumLoop"] = DataView.Items[i].SubItems[8].Text;

                            ds.Tables[strNameSave].Rows.Add(row);
                        }
                        if (checkOpenorSave == 1)
                        {
                            ds.WriteXml(pathnameopen);
                        }
                        else if (checkOpenorSave == 2)
                        {
                            ds.WriteXml(pathnamesave);
                        }
                        ds.Tables.Clear();
                    }
                    CheckSaved = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string pathnamesaveas;
        private void menufileSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                if (Tabs.SelectedTab.Name == "Auto")
                {
                    //Read path Open
                    FileStream ofs = new FileStream(FilesPathNameSave, FileMode.Open, FileAccess.Read, FileShare.None);
                    StreamReader rd = new StreamReader(ofs);
                    string strPathSave = rd.ReadLine();

                    rd.Close();
                    ofs.Close();
                    //Create data table
                    DataSet ds = new DataSet();
                    SaveFileDialog savecodedialog = new SaveFileDialog();
                    savecodedialog.InitialDirectory = strPathSave;
                    savecodedialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                    savecodedialog.RestoreDirectory = true;
                    if (savecodedialog.ShowDialog() == DialogResult.OK)
                    {
                        //Filter File Name(no full name)
                        pathnamesaveas = savecodedialog.FileName;
                        string findfilenamesave = new DirectoryInfo(pathnamesaveas).Name;
                        int tmplengthsave = findfilenamesave.Length;
                        string filenamesave = findfilenamesave.Substring(0, tmplengthsave - 4);
                        //Save File
                        DataTable savecodeprogram = new DataTable();
                        savecodeprogram.TableName = filenamesave;
                        savecodeprogram.Columns.Add("Num");
                        savecodeprogram.Columns.Add("Code");
                        savecodeprogram.Columns.Add("CoorX");
                        savecodeprogram.Columns.Add("CoorY");
                        savecodeprogram.Columns.Add("Vel");
                        savecodeprogram.Columns.Add("Perfor");
                        savecodeprogram.Columns.Add("Loop");
                        savecodeprogram.Columns.Add("NumLoop");
                        ds.Tables.Add(savecodeprogram);

                        for (int i = 0; i < DataView.Items.Count; i++)
                        {
                            DataRow row = ds.Tables[filenamesave].NewRow();
                            row["Num"] = DataView.Items[i].SubItems[1].Text;
                            row["Code"] = DataView.Items[i].SubItems[2].Text;
                            row["CoorX"] = DataView.Items[i].SubItems[3].Text;
                            row["CoorY"] = DataView.Items[i].SubItems[4].Text;
                            row["Vel"] = DataView.Items[i].SubItems[5].Text;
                            row["Perfor"] = DataView.Items[i].SubItems[6].Text;
                            row["Loop"] = DataView.Items[i].SubItems[7].Text;
                            row["NumLoop"] = DataView.Items[i].SubItems[8].Text;

                            ds.Tables[filenamesave].Rows.Add(row);
                        }

                        ds.WriteXml(pathnamesaveas);
                        ds.Tables.Clear();
                        labNameProgram.Text = filenamesave;
                        //Save path Save
                        string strnamesave = new FileInfo(pathnamesaveas).DirectoryName;
                        FileStream rfs = new FileStream(FilesPathNameSave, FileMode.Create, FileAccess.Write, FileShare.None);
                        StreamWriter wr = new StreamWriter(rfs);
                        wr.WriteLine(strnamesave);

                        wr.Flush();
                        wr.Close();
                        rfs.Close();
                    }
                    CheckSaved = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menufileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolsOptions_Click(object sender, EventArgs e)
        {
            Config frmConfig = new Config();
            frmConfig.frmForm3 = this;
            frmConfig.ShowDialog();
            CheckEnaInOut = frmConfig.CheckInOutMode;
            if (CheckEnaInOut == true)
            {
                groupInOut.Enabled = true;
                if (PreInorOut01 == false)
                {
                    butO01.Enabled = false;
                    TimerInput01.Enabled = true;
                }
                else
                {
                    butO01.Enabled = true;
                    TimerInput01.Enabled = false;
                }

                if (PreInorOut02 == false)
                {
                    butO02.Enabled = false;
                    TimerInput02.Enabled = true;
                }
                else
                {
                    butO02.Enabled = true;
                    TimerInput02.Enabled = false;
                }

                if (PreInorOut03 == false)
                {
                    butO03.Enabled = false;
                    TimerInput03.Enabled = true;
                }
                else
                {
                    butO03.Enabled = true;
                    TimerInput03.Enabled = false;
                }

                if (PreInorOut04 == false)
                {
                    butO04.Enabled = false;
                    TimerInput04.Enabled = true;
                }
                else
                {
                    butO04.Enabled = true;
                    TimerInput04.Enabled = false;
                }
            }
            else
            {
                groupInOut.Enabled = false;
                TimerInput01.Enabled = false;
                TimerInput02.Enabled = false;
                TimerInput03.Enabled = false;
                TimerInput04.Enabled = false;
            }
        }

        /// <summary>
        /// Zero Button/ Scale Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetZero(int iaxis, string straxis)
        {
            var axisXY = KM.GetAxis(iaxis, straxis);
            axisXY.ZeroAxis();
        }

        private void butZero_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                SetZero(0, "X");
                SetZero(1, "Y");
            }
        }

        private void butMoveOri_Click(object sender, EventArgs e)
        {
            //Move Axis X, Y to origin
            if (bolCheckConnected == true)
            {
                int iTempMoveXY = Convert.ToInt32(Jogspeed.Text) * 30;
                MoveXY(0, 0, iTempMoveXY);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Save Current Position
                if ((txtPosX.Text != "") & (txtPosY.Text != ""))
                {
                    FileStream rfs = new FileStream(FilesPathNameXYpos, FileMode.Create, FileAccess.Write, FileShare.None);
                    StreamWriter wr = new StreamWriter(rfs);
                    wr.WriteLine(txtPosX.Text);
                    wr.WriteLine(txtPosY.Text);

                    wr.Flush();
                    wr.Close();
                    rfs.Close();
                }
                //Save Jog Speed
                FileStream rfsJ = new FileStream(FilesPathNameJogSpeed, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter wrJ = new StreamWriter(rfsJ);
                wrJ.WriteLine(Jogspeed.Text);

                wrJ.Flush();
                wrJ.Close();
                rfsJ.Close();
                //Save number Input/Output
                FileStream rfsN = new FileStream(FilesPathNameNumIO, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter wrN = new StreamWriter(rfsN);
                wrN.WriteLine(cbIO01.Text);
                wrN.WriteLine(cbIO02.Text);
                wrN.WriteLine(cbIO03.Text);
                wrN.WriteLine(cbIO04.Text);

                wrN.Flush();
                wrN.Close();
                rfsN.Close();
                //Save data of Data Table in "Auto" tab
                if ((CheckDataOfListView == true) && (CheckSaved == false))
                {
                    DialogResult Result = MessageBox.Show("Do you want to save changes you made to Data Table ?", "Busbar Punching Controller",
                    MessageBoxButtons.YesNo);
                    if (Result == DialogResult.Yes)
                    {
                        if (labNameProgram.Text == "")
                        {
                            //Read path Open
                            FileStream ofs = new FileStream(FilesPathNameSave, FileMode.Open, FileAccess.Read, FileShare.None);
                            StreamReader rd = new StreamReader(ofs);
                            string strPathSave = rd.ReadLine();

                            rd.Close();
                            ofs.Close();
                            //Create data table
                            DataSet ds = new DataSet();
                            SaveFileDialog savecodedialog = new SaveFileDialog();
                            savecodedialog.InitialDirectory = strPathSave;
                            savecodedialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                            savecodedialog.RestoreDirectory = true;
                            if (savecodedialog.ShowDialog() == DialogResult.OK)
                            {
                                //Filter File Name(no full name)
                                pathnamesave = savecodedialog.FileName;
                                string findfilenamesave = new DirectoryInfo(pathnamesave).Name;
                                int tmplengthsave = findfilenamesave.Length;
                                string filenamesave = findfilenamesave.Substring(0, tmplengthsave - 4);
                                //Save File
                                DataTable savecodeprogram = new DataTable();
                                savecodeprogram.TableName = filenamesave;
                                savecodeprogram.Columns.Add("Num");
                                savecodeprogram.Columns.Add("Code");
                                savecodeprogram.Columns.Add("CoorX");
                                savecodeprogram.Columns.Add("CoorY");
                                savecodeprogram.Columns.Add("Vel");
                                savecodeprogram.Columns.Add("Perfor");
                                savecodeprogram.Columns.Add("Loop");
                                savecodeprogram.Columns.Add("NumLoop");
                                ds.Tables.Add(savecodeprogram);

                                for (int i = 0; i < DataView.Items.Count; i++)
                                {
                                    DataRow row = ds.Tables[filenamesave].NewRow();
                                    row["Num"] = DataView.Items[i].SubItems[1].Text;
                                    row["Code"] = DataView.Items[i].SubItems[2].Text;
                                    row["CoorX"] = DataView.Items[i].SubItems[3].Text;
                                    row["CoorY"] = DataView.Items[i].SubItems[4].Text;
                                    row["Vel"] = DataView.Items[i].SubItems[5].Text;
                                    row["Perfor"] = DataView.Items[i].SubItems[6].Text;
                                    row["Loop"] = DataView.Items[i].SubItems[7].Text;
                                    row["NumLoop"] = DataView.Items[i].SubItems[8].Text;

                                    ds.Tables[filenamesave].Rows.Add(row);
                                }

                                ds.WriteXml(pathnamesave);
                                ds.Tables.Clear();
                                labNameProgram.Text = filenamesave;
                                checkOpenorSave = 2;
                                //Save path Save
                                string strnamesave = new FileInfo(pathnamesave).DirectoryName;
                                FileStream rfs = new FileStream(FilesPathNameSave, FileMode.Create, FileAccess.Write, FileShare.None);
                                StreamWriter wr = new StreamWriter(rfs);
                                wr.WriteLine(strnamesave);

                                wr.Flush();
                                wr.Close();
                                rfs.Close();
                            }
                        }
                        else
                        {
                            //Create data table
                            DataSet ds = new DataSet();
                            string strNameSave = labNameProgram.Text;
                            //Save File
                            DataTable savecodeprogram = new DataTable();
                            savecodeprogram.TableName = strNameSave;
                            savecodeprogram.Columns.Add("Num");
                            savecodeprogram.Columns.Add("Code");
                            savecodeprogram.Columns.Add("CoorX");
                            savecodeprogram.Columns.Add("CoorY");
                            savecodeprogram.Columns.Add("Vel");
                            savecodeprogram.Columns.Add("Perfor");
                            savecodeprogram.Columns.Add("Loop");
                            savecodeprogram.Columns.Add("NumLoop");
                            ds.Tables.Add(savecodeprogram);

                            for (int i = 0; i < DataView.Items.Count; i++)
                            {
                                DataRow row = ds.Tables[strNameSave].NewRow();
                                row["Num"] = DataView.Items[i].SubItems[1].Text;
                                row["Code"] = DataView.Items[i].SubItems[2].Text;
                                row["CoorX"] = DataView.Items[i].SubItems[3].Text;
                                row["CoorY"] = DataView.Items[i].SubItems[4].Text;
                                row["Vel"] = DataView.Items[i].SubItems[5].Text;
                                row["Perfor"] = DataView.Items[i].SubItems[6].Text;
                                row["Loop"] = DataView.Items[i].SubItems[7].Text;
                                row["NumLoop"] = DataView.Items[i].SubItems[8].Text;

                                ds.Tables[strNameSave].Rows.Add(row);
                            }
                            if (checkOpenorSave == 1)
                            {
                                ds.WriteXml(pathnameopen);
                            }
                            else if (checkOpenorSave == 2)
                            {
                                ds.WriteXml(pathnamesave);
                            }
                            ds.Tables.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Check Power 24V
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCheckPower_Tick(object sender, EventArgs e)
        {
            try
            {
                if (bolCheckCardConnected == true)
                {
                    var Input03 = KM.GetIO(03, IO_TYPE.DIGITAL_IN, "I03");
                    if (Input03.GetDigitalValue() == false)
                    {
                        bolCheckConnected = false;
                    }
                    else
                    {
                        if (bolCheckConnected == false)
                        {
                            string pathtxt = MainPath + @"\DefineXYAxis.c";

                            try
                            {
                                //Load and Run Thread 1
                                string Result = KM.CompileAndLoadCoff(1, pathtxt, false);
                                if (Result != "")
                                {
                                    MessageBox.Show(Result, "Compile Error");
                                }
                                else
                                {
                                    // everything ok, execute the Thread
                                    KM.ExecuteProgram(1);
                                    KM.WaitForThreadComplete(1, 500);
                                    KM.KillProgramThreads(1);
                                }
                                //Read XY current position
                                FileStream ofs = new FileStream(FilesPathNameXYpos, FileMode.Open, FileAccess.Read, FileShare.None);
                                StreamReader rd = new StreamReader(ofs);
                                string strXaxis = rd.ReadLine();
                                string strYaxis = rd.ReadLine();
                                rd.Close();
                                ofs.Close();

                                SetCurrenPosition(0, "X", Math.Round((Convert.ToDouble(strXaxis) / douscaleX), 1));
                                SetCurrenPosition(1, "Y", Math.Round((Convert.ToDouble(strYaxis) / douscaleY), 1));

                                //Load and Run Thread 1
                                string strResult = KM.CompileAndLoadCoff(1, pathtxt, false);
                                if (strResult != "")
                                {
                                    MessageBox.Show(strResult, "Compile Error");
                                }
                                else
                                {
                                    // everything ok, execute the Thread
                                    KM.ExecuteProgram(1);
                                    KM.WaitForThreadComplete(1, 500);
                                    KM.KillProgramThreads(1);
                                }
                            }
                            catch (DMException ex)
                            {
                                MessageBox.Show(ex.InnerException.Message);
                            }
                        }
                        bolCheckConnected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("KFLOP Card - Disconnected" + ex.Message);
            }
        }

        /// <summary>
        /// Display WindowsForm which shows designed busbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void butReview_Click(object sender, EventArgs e)
        {
            if ((txtHeightRev.Text != "") && (txtWidthRev.Text != ""))
            {
                if (DataView.Items.Count > 0)
                {
                    Review frmReview = new Review();
                    frmReview.frmWFReview = this;
                    frmReview.tmptxtW = txtWidthRev.Text;
                    frmReview.tmptxtH = txtHeightRev.Text;
                    DataLoop[] arrloop = new DataLoop[10];
                    int numofloop = 0;
                    int tmpdata = 0;
                    //-----------------------------------------------------------------------
                    if ("0".CompareTo(GetDataViewValue((DataView.Items.Count - 1), 2)) == 0)
                    {
                        for (int i = 0; i < DataView.Items.Count; i++)
                        {
                            //Code
                            if ("0".CompareTo(GetDataViewValue(i, 2)) == 0)
                            {
                                frmReview.tmpdatarev = tmpdata;
                                break;
                            }
                            else if ("1".CompareTo(GetDataViewValue(i, 2)) == 0)
                            {
                                int Xtmp1 = Convert.ToInt32(GetStringItem(i, 3));
                                int Ytmp1 = Convert.ToInt32(GetStringItem(i, 4));
                                frmReview.arrdatarev[tmpdata, 0] = Xtmp1;
                                frmReview.arrdatarev[tmpdata, 1] = Ytmp1;
                                tmpdata++;
                            }
                            else if ("2".CompareTo(GetDataViewValue(i, 2)) == 0)
                            {
                                int Xtmp2 = Convert.ToInt32(GetStringItem(i, 3)) + frmReview.arrdatarev[tmpdata - 1, 0];
                                int Ytmp2 = Convert.ToInt32(GetStringItem(i, 4)) + frmReview.arrdatarev[tmpdata - 1, 1];
                                frmReview.arrdatarev[tmpdata, 0] = Xtmp2;
                                frmReview.arrdatarev[tmpdata, 1] = Ytmp2;
                                tmpdata++;
                            }
                            //Loop
                            if ("1".CompareTo(GetDataViewValue(i, 7)) == 0)
                            {
                                if (numofloop == 0)
                                {
                                    arrloop[numofloop].strnum = i.ToString();
                                    arrloop[numofloop].strloop = GetStringItem(i, 8);
                                    numofloop++;
                                }
                                else
                                {
                                    if (arrloop[numofloop - 1].strnum != i.ToString())
                                    {
                                        arrloop[numofloop].strnum = i.ToString();
                                        arrloop[numofloop].strloop = GetStringItem(i, 8);
                                        numofloop++;
                                    }
                                }
                            }
                            else if ("9".CompareTo(GetDataViewValue(i, 7)) == 0)
                            {
                                numofloop--;
                                arrloop[numofloop].strloop = (Convert.ToInt32(arrloop[numofloop].strloop) - 1).ToString();
                                if (arrloop[numofloop].strloop == "0")
                                {
                                    arrloop[numofloop].strnum = null;
                                    arrloop[numofloop].strloop = null;
                                }
                                else
                                {
                                    i = Convert.ToInt32(arrloop[numofloop].strnum) - 1;
                                    numofloop++;
                                }
                            }
                            //End
                        }
                    }
                    //-----------------------------------------------------------------------
                    frmReview.ShowDialog();
                }
            }
        }

        /// <summary>
        /// textbox txtWidthRev and txtHeightRev always enter 0-9.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txtWidthRev_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtHeightRev_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        bool bolPause = false;
        private void butPause_Click(object sender, EventArgs e)
        {
            if (bolCheckConnected == true)
            {
                if (DataView.Items.Count > 0)
                {
                    if (DataView.Items[0].SubItems[5].Text != "")
                    {
                        if (butPause.Text == "Pause")
                        {
                            butPause.Text = "Resume";
                            bolPause = true;
                        }
                        else
                        {
                            butPause.Text = "Pause";
                            bolPause = false;
                        }
                    }
                }
            }
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            FormConfig FormConfig = new FormConfig();
            FormConfig.formConfig = this;
            FormConfig.ShowDialog();
            TimerSendToServer.Start();
            if (FormConfig.check == "1")
            {

                labelID.Text = FormConfig.ID;
                labelMode.Text = FormConfig.Mode;
                labelW.Text = FormConfig.W;
                labelH.Text = FormConfig.H;
                labelL.Text = FormConfig.L;
                labelNH.Text = FormConfig.Hole;
                FileStream rfs = new FileStream(@"D:\form.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter wr = new StreamWriter(rfs);
                wr.WriteLine(labelID.Text);
                wr.WriteLine(labelMode.Text);
                wr.WriteLine(labelW.Text);
                wr.WriteLine(labelH.Text);
                wr.WriteLine(labelL.Text);
                wr.WriteLine(labelNH.Text);

                wr.Flush();
                wr.Close();
                rfs.Close();

            }
            if (Product.CheckConnectServer == 0)
                TimerCheckConnectInternet.Start();
        }

        //code add

        private void button1_Click(object sender, EventArgs e)
        {
            if (labelMode.Text == "Bar")
            {
                Product.CountBar++;
            }
            else if (labelMode.Text == "Hole")
            {
                Product.CountHole++;
            }
        }

        private void TimerSendToServer_Tick(object sender, EventArgs e)
        {
            int code=0;
            FormConfig FormConfigSend = new FormConfig();
            if ( Product.CheckConnectServer==1)
            code = FormConfigSend.send_data_to_server(labelMode.Text, labelID.Text, labelW.Text, labelH.Text, labelL.Text, labelNH.Text, "Count", Product.CountBar, Product.CountHole);
            if (code == 1)
            {
                
                FormConfigSend.send_data_to_local(labelMode.Text, labelID.Text, labelW.Text, labelH.Text, labelL.Text, labelNH.Text, "Count", Product.CountBar, Product.CountHole,1);
                Product.CountBar = 0;
                Product.CountHole = 0;
            }
            else if (code == 0 || Product.CheckConnectServer == 0)
            {
                TimerCheckConnectInternet.Start();
                Product.CheckConnectServer = 0;
                FormConfigSend.send_data_to_local(labelMode.Text, labelID.Text, labelW.Text, labelH.Text, labelL.Text, labelNH.Text, "Count", Product.CountBar, Product.CountHole, 0);
                Product.CountBar = 0;
                Product.CountHole = 0;
            }
        }

        private void TimerCheckConnectInternet_Tick(object sender, EventArgs e)
        {
            SynData();
        }
        
        public void SynData()
        {
            try
            {
                //dia chi server
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
                   
                    
                    TimerCheckConnectInternet.Stop();
                    // Create a request using a URL that can receive a post.   // nay la đia chi may local
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
                    TimerCheckConnectInternet.Start();
                    response1.Close();
                 
                }
            }
            catch ( Exception ex) 
            {
                DialogResult dlr = MessageBox.Show("Lỗi đồng bộ", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
                
            }
        }



    }
    public static class Product
    {
        static public int CountBar;
        static public int CountHole;
        static public int CheckConnectServer;
        static public string IpServer="http://192.168.137.1";
    }
    
}

