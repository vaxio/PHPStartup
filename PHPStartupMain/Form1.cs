using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonacoAPI;
namespace PHPSimple
{
    public partial class MainUI : Form
    {

        MonacoFuncs MonacoFunc = new MonacoFuncs();

        public MainUI()
        {
            InitializeComponent();
        }


        OpenFileDialog ofd = new OpenFileDialog();
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        private void Run_Click(object sender, EventArgs e)

        {
            var phpExecute = new ProcessStartInfo();
            phpExecute.UseShellExecute = true;
            phpExecute.WorkingDirectory = @"C:\Windows\System32";
            phpExecute.FileName = @"C:\Windows\System32\cmd.exe";
            phpExecute.Verb = "runas";
            phpExecute.Arguments = "/c php -S localhost:4000";
            Process.Start(phpExecute);

            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[]
            {
                this.Text
            };
            string value = document.InvokeScript(scriptName, args).ToString();
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Website"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Website");
            }
            StreamWriter streamWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\Website\\index.php");
            streamWriter.WriteLine(value);
            streamWriter.Close();









        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Open_Click(object sender, EventArgs e)
        {

            Stream Stream1;
            ofd.Filter = "PHP files (*.php)|*.PHP|All files (*.*)|*.*";
            ofd.InitialDirectory = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if ((Stream1 = ofd.OpenFile()) != null)
                {


                    string strfilename = ofd.FileName;
                    //richTextBox1.Text = File.ReadAllText(strfilename);
                    MonacoFunc.SetText(webBrowser1, File.ReadAllText(strfilename));


                }

            }
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            MonacoFunc.Initialize(webBrowser1);
        }


        private void Clear_Click(object sender, EventArgs e)
        {
            MonacoFunc.SetText(webBrowser1, "");
        }

        private void Save_Click(object sender, EventArgs e)
        {
            HtmlDocument document = this.webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[]
            {
                this.Text
            };
            string text = document.InvokeScript(scriptName, args).ToString();
            if (text == "")
            {
                MessageBox.Show("Text must not be empty!", "Error!", MessageBoxButtons.OK);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
            streamWriter.Write(text);
            streamWriter.Close();

        }


            private void button1_Click(object sender, EventArgs e)
            {
                this.WindowState = FormWindowState.Minimized;
            }

        private void button2_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Maximized)
            {
                base.WindowState = FormWindowState.Normal;
                return;
            }
            base.WindowState = FormWindowState.Maximized;
        }
    }
    }
