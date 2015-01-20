using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComparativeTest.Windows;

namespace ComparativeTest
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            DialogsTypes.Add("SharpDx11 (WinForms)", typeof(WinFormsSharpDx11));
            DialogsTypes.Add("DirectX (WinForms)", typeof(WinFormsDx));
            DialogsTypes.Add("SharpDx2D1 (WinForms)", typeof(WinFormsSharpDx2D1));
            DialogsTypes.Add("SharpDx (WinForms)", typeof(WinFormsSharpDx));
            DialogsTypes.Add("GDI+ (SetStyle(ControlStyles.DoubleBuffer, true))", typeof(GdiPlusDoubleBufferedStyle));
            DialogsTypes.Add("GDI+ (using class BufferedGraphics)", typeof(GdiPlusBufferedGraphics));
            DialogsTypes.Add("GDI+", typeof(GdiPlus));
            DialogsTypes.Add("WPF", typeof(WpfWindow));
            //...

            foreach (var key in DialogsTypes.Keys)
                cbWindowType.Items.Add(key);
            cbWindowType.SelectedIndex = 0;

            _timer.Interval = 5;
            _timer.Start();
            _timer.Tick += _timer_Tick;

            _fpstimer.Interval = 2000;
            _fpstimer.Start();
            _fpstimer.Tick += _fpstimer_Tick;
        }

        void _fpstimer_Tick(object sender, EventArgs e)
        {
            _windows.ForEach(w=>w.ShowFps((float)_fpstimer.Interval/1000));
        }

        void _timer_Tick(object sender, EventArgs e)
        {
           _windows.ForEach(w=>w.Redraw());
        }

        private Timer _timer=new Timer();
        private Timer _fpstimer = new Timer();

        private Dictionary<string, Type> DialogsTypes = new Dictionary<string, Type>();

        private List<ITestWindow> _windows=new List<ITestWindow>();

        private void tbImageFilePath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using(var dial=new OpenFileDialog())
            {
                if(dial.ShowDialog()==DialogResult.OK)
                {
                    tbImageFilePath.Text = dial.FileName;
                }
            }
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            var window = (ITestWindow)Activator.CreateInstance(DialogsTypes[cbWindowType.Text]);
            window.Factory = new MainLayerFactory
                                 {
                                     Properties = this,
                                     Random = new Random()
                                 };
            window.Title = cbWindowType.Text;
            _windows.Add(window);
            window.Closed += window_Closed;
            window.Open();
        }

        void window_Closed(object sender, EventArgs e)
        {
            (sender as ITestWindow).Closed -= window_Closed;
            _windows.Remove((ITestWindow)sender);
        }
    }
}
