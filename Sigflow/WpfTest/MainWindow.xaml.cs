using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            

        }

        private Schema1 _s;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (_s == null)
            {
                _s = new Schema1
                         {
                             OnRedraw = ()=> { }
                         };
                _s.Build();
            }


            _s.Start();
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            _s.Stop();
        }

        
    }
}
