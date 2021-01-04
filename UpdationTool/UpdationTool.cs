using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Diagnostics;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace UpdationTool
{
    public partial class UpdationTool : Form
    {
        #region Private Varibles
        string sUrl;
        string fPath;
        string AuthSecret = "tBwFbHgvRH17fKPMqvQI2AG2QwTU5sH0k3QHrlpS";
        string BasePath = "https://fireapp-5b98f.firebaseio.com/";
        string sTable = "UpdatesLink/";
        IFirebaseClient client;
        IFirebaseConfig config;
        #endregion

        #region Constructor
        public UpdationTool()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public static bool CheckInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Events
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] myProcList = Process.GetProcessesByName("MdiCommon");
                if (myProcList.Count() > 0)
                {
                    foreach (Process Target in myProcList)
                    {
                        Target.Kill();
                    }
                }
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                fPath = path + "\\UpdationTool.zip";
                string directoryPath = path + "\\UpdationTool";
                if (Directory.Exists(directoryPath)) { Directory.Delete(directoryPath); }
                FileDownloader.DownloadFileFromURLToPath(sUrl, fPath);
                if (ExtractFile(fPath, directoryPath))
                {
                    File.Delete(fPath);
                    DirectoryInfo dtry = new DirectoryInfo(directoryPath);
                    FileInfo[] files = dtry.GetFiles();
                    foreach (FileInfo item in files)
                    {
                        string file = path + "\\" + item.ToString();
                        //Do your job with "file"  
                        string str = path + "\\UpdationTool\\" + item.ToString();
                        FileInfo fInfo = new FileInfo(str);
                        // replace the file. 
                        //Thread.Sleep(5000);
                        File.Copy(str, file, true);
                    }
                    MessageBox.Show("Updation completed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool ExtractFile(string zipPath, string extractPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void UpdationTool_Load(object sender, EventArgs e)
        {
            if(CheckInternet())
            {
                List<UpdateItem> updateItems = new List<UpdateItem>();
                config = new FirebaseConfig
                {
                    AuthSecret = AuthSecret,
                    BasePath = BasePath
                };
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get(sTable);
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        updateItems.Add(JsonConvert.DeserializeObject<UpdateItem>(((JProperty)item).Value.ToString()));
                    }
                }
                if (updateItems.Count() > 0)
                {
                    sUrl = updateItems.FirstOrDefault().UpdationLink;
                }
                else
                {
                    MessageBox.Show("No updation available");
                    this.Close();
                }
                
            }
            else
            {
                MessageBox.Show("Please check internet connection and try later.");
                this.Close();
            }
        }
        #endregion
    }
}
