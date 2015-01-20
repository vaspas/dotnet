using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComparativeTapeTest.Generators;
using ComparativeTapeTest.Tapes;
using ComparativeTapeTest.Windows;
using TapeImplement;

namespace ComparativeTapeTest
{
    public partial class Main : Form
    {
        static Main()
        {
            DialogsTypes.Add("SharpDX11", typeof(WinFormsDx11));
            DialogsTypes.Add("DirectX", typeof(WinFormsDx));
            DialogsTypes.Add("GDI+", typeof(GdiPlusDoubleBufferedStyle));
            DialogsTypes.Add("WPF", typeof(WpfWindow));
            DialogsTypes.Add("Draw to GDI+ image", typeof(GdiPlusImage));

            TapesTypes.Add("vagon print", typeof(Tapes.VagonPrint.VagonPrintTapeFactory));
            TapesTypes.Add("vagon vertical tape", typeof(VagonVerticalTapeFactory));
            TapesTypes.Add("vagon horizontal tape", typeof(VagonHorizontalTapeFactory));
            TapesTypes.Add("kuges horizontal tape", typeof(KugesHorizontalTapeFactory));
            TapesTypes.Add("horizontal tape", typeof(HorizontalTapeFactory));
            TapesTypes.Add("TestPlayerTape", typeof(TestPlayerTapeFactory));
            TapesTypes.Add("zones tape", typeof(ZonesViewTapeFactory));
            
        }

        public Main()
        {
            InitializeComponent();
            
            foreach (var key in DialogsTypes.Keys)
                cbWindowType.Items.Add(key);
            cbWindowType.SelectedIndex = 0;

            foreach (var key in TapesTypes.Keys)
                cbTapeType.Items.Add(key);
            cbTapeType.SelectedIndex = 0;

            _player=new Player();
            _player.Redraw = () => _windows.ForEach(w => w.Redraw());
            _player.Min = 0;
            _player.Max = 50000;

            hScrollBar.Minimum = _player.Min;
            hScrollBar.Maximum = _player.Max;

            GenerateNewDataSources();
        }

        private readonly static Dictionary<string, Type> DialogsTypes = new Dictionary<string, Type>();

        private readonly static Dictionary<string, Type> TapesTypes = new Dictionary<string, Type>();

        private readonly List<IWindow> _windows = new List<IWindow>();
        
        private Player _player;

        private List<object> _dataSources=new List<object>();

        private void GenerateNewDataSources()
        {
            var factory = new GeneratorsFactory
                              {
                                  Random = new Random(),
                                  From = _player.Min,
                                  To = _player.Max
                              };

            _dataSources = new List<object>();
            _dataSources.Add(factory.CreateCoordinateSource());

            var levelNullLines = factory.CreateLevelNullLineSource("NullLineLevel");
            _dataSources.Add(levelNullLines);
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, -30, "NullLineLevelExt4m"));
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, -20, "NullLineLevelExt3m"));
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, -10, "NullLineLevelExt2m"));
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, 10, "NullLineLevelExt2p"));
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, 20, "NullLineLevelExt3p"));
            _dataSources.Add(factory.CreateExtendValue(levelNullLines, 30, "NullLineLevelExt4p"));

            _dataSources.Add(factory.CreateLevelSignalSource(levelNullLines, "SignalLevel"));


            var trackNullLines = factory.CreateTrackNullLineSource("NullLineTrack");
            _dataSources.Add(trackNullLines);
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, -2, "NullLineTrackExt4m"));
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, -6, "NullLineTrackExt3m"));
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, -10, "NullLineTrackExt2m"));
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, 4, "NullLineTrackExt2p"));
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, 8, "NullLineTrackExt3p"));
            _dataSources.Add(factory.CreateExtendValue(trackNullLines, 12, "NullLineTrackExt4p"));

            _dataSources.Add(factory.CreateTrackSignalSource(trackNullLines, "SignalTrack"));

            _dataSources.Add(factory.CreateRegionsSource("Regions"));
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            var window = (IWindow)Activator.CreateInstance(DialogsTypes[cbWindowType.Text]);
            
            window.Title = cbWindowType.Text;
            _windows.Add(window);
            window.Closed += window_Closed;
            window.Open();

            var tape = new Tape();
            tape.Position = new BoundedScalePosition<int>
                                {
                                    Min = 0,
                                    Max = int.MaxValue,
                                    MinWidth = 10,
                                    MaxWidth = 5000
                                };
            tape.Position.Set(_player.TapePosition.From, _player.TapePosition.To);
            tape.Engine = window.Engine;
            _player.Tapes.Add(tape);

            var factory=(IMainLayerFactory) Activator.CreateInstance(TapesTypes[cbTapeType.Text]);
            factory.TapePosition = tape.Position;
            factory.Player = _player;
            factory.DataSources = _dataSources;
            factory.Create(window.Engine.MainLayer);

        }

        void window_Closed(object sender, EventArgs e)
        {
            var window = sender as IWindow;
            _player.Tapes.RemoveAll(t => t.Engine == window.Engine);

            window.Closed -= window_Closed;
            _windows.Remove(window);
        }

        private void nudShift_ValueChanged(object sender, EventArgs e)
        {
            _player.StartShift((float)nudShift.Value, (int)nudPeriod.Value);
        }

        private void nudPeriod_ValueChanged(object sender, EventArgs e)
        {
            
            _player.StartShift((float)nudShift.Value, (int)nudPeriod.Value);
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _player.Stop();
            _player.GoTo(hScrollBar.Value);
        }
    }
}
