using System;
using System.IO;
using System.Windows;

namespace CodeViewer_Reset
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string settingfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CodeViewer.dat");   
        public App()
        {
            MainWindow window = new MainWindow();
            try
            {
                if (File.Exists(settingfile))
                {
                    FileStream fs = new FileStream(settingfile, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    window.Top = br.ReadDouble();
                    window.Left = br.ReadDouble();
                    window.Height = br.ReadDouble();
                    window.Width = br.ReadDouble();
                    if (br.ReadBoolean())
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    br.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CodeViewer");
            }
            finally
            {
                window.Show();
                window.Activate();
            }   
        }
    }
}
