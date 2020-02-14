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
using System.IO;

namespace CodeViewer_Reset
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        double height, width;
        string settingfile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CodeViewer.dat");
        public MainWindow()
        {
            InitializeComponent();
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
                    FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    string strdata = null; string item;
                    for (int i = 0; i < fs.Length; i++)
                    {
                        item = Convert.ToString(fs.ReadByte(), 16).ToUpper();
                        if (item.Length == 1) item = '0' + item;
                        strdata += item + ' ';
                    }
                    fs.Close();
                    if (strdata != null) tbx_context.Text = strdata.Trim();
                    Title.Content = "CodeViewer - " + tbx_path.Text.Trim();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                        MaxButton.Visibility = Visibility.Hidden;
                        RetButton.Visibility = Visibility.Visible;
                        ImageBrush brush = new ImageBrush();
                    }
                    else
                    {
                        this.WindowState = WindowState.Normal;
                        MaxButton.Visibility = Visibility.Visible;
                        RetButton.Visibility = Visibility.Hidden;
                    }
                    br.Close();
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
            finally
            {
                this.Opacity = 1;
            }
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                MaxButton.Visibility = Visibility.Hidden;
                RetButton.Visibility = Visibility.Visible;
            }    
            else
            {
                this.WindowState = WindowState.Normal;
                MaxButton.Visibility = Visibility.Visible;
                RetButton.Visibility = Visibility.Hidden;
            }
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
