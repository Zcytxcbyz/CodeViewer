using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;

namespace CodeViewer_Reset
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        double height, width;
        string settingfile = App.settingfile;
        List<Thread> threadlist = new List<Thread>();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void openfile(object obj)
        {
            try
            {
                string filename = obj.ToString();
                Dispatcher.Invoke(delegate ()
                {
                    tbx_context.Clear();
                    Title = "CodeViewer - " + filename;
                });
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                string item;
                for (int i = 0; i < fs.Length; i++)
                {
                    item = Convert.ToString(fs.ReadByte(), 16).ToUpper();
                    if (item.Length == 1) item = '0' + item;
                    Dispatcher.Invoke(delegate ()
                    {
                        tbx_context.Text += item + ' ';
                    });
                }
                fs.Close();
                Dispatcher.Invoke(delegate ()
                {
                    if (tbx_context != null)
                    {
                        tbx_context.Text = tbx_context.Text.Trim();
                    }
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
            finally
            {
                threadlist.Remove(Thread.CurrentThread);
            }
        }

        private void btn_open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "所有文件 (*.*)|*.*";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    tbx_path.Text = ofd.FileName;
                    foreach(Thread item in threadlist)
                    {
                        item.Abort();
                    }
                    Thread thread = new Thread(openfile);
                    thread.IsBackground = true;
                    threadlist.Add(thread);
                    thread.Start(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                if (File.Exists(settingfile))
                {
                    FileStream fs = new FileStream(settingfile, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    this.Top = br.ReadDouble();
                    this.Left = br.ReadDouble();
                    this.Height = br.ReadDouble();
                    this.Width = br.ReadDouble();
                    if (br.ReadBoolean())
                    {
                        this.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                    }
                    br.Close();
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
            */
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                FileStream fs = new FileStream(settingfile, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(this.Top);
                bw.Write(this.Left);
                bw.Write(height);
                bw.Write(width);
                bw.Write(this.WindowState == WindowState.Maximized);
                bw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                height = this.Height;
                width = this.Width;
            }
        }
    }
}
