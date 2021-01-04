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

namespace UpdationTool
{
    public partial class UpdationTool : Form
    {
        string sUrl;
        string fPath;
        public UpdationTool()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] myProcList = Process.GetProcessesByName("MdiCommon");
                foreach (Process Target in myProcList)
                {
                    Target.Kill();
                }
                sUrl = "https://drive.google.com/file/d/1Jg4vGgzIlBFRDFiutYe9NleALf8W8xTY/view?usp=sharing";
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
    }
}
