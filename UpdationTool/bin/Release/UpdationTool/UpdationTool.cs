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
                sUrl = "https://drive.google.com/file/d/1e_llDQ3r6OA8i7U-7px2F8khhCJtkXSI/view?usp=sharing";
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                fPath = path + "\\UpdationTool.zip";
                FileDownloader.DownloadFileFromURLToPath(sUrl, fPath);
                string directoryPath = path + "\\UpdationTool";
                if (ExtractFile(fPath, directoryPath))
                {
                    Thread.Sleep(5000);
                    File.Delete(fPath);
                    DirectoryInfo dtry = new DirectoryInfo(directoryPath);
                    FileInfo[] files = dtry.GetFiles();
                    foreach (FileInfo item in files)
                    {
                        string file = item.ToString();
                        //Do your job with "file"  
                        string str = path + file.ToString();
                        FileInfo fInfo = new FileInfo(str);
                        // replace the file.    
                        fInfo.Replace(file, str, false);
                    }
                    Thread.Sleep(5000);
                    Directory.Delete(directoryPath);
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
