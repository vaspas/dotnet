using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Sigflow.Performance;
using Sigflow.Schema;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Primitives;
using TapeDrawingWinForms;
using TdGraphsModels.Ekyta;
using TdGraphsParts;
using TdGraphsParts.Scales;
using WindowsFormsGenerator.Schemes;

namespace WindowsFormsGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            
            PreviewKeyDown += OnPreviewKeyDown;

            comboBox1.Items.Add(typeof(Schema1).Name);
            comboBox1.Items.Add(typeof(Schema2).Name);
            comboBox1.Items.Add(typeof(Schema3).Name);
            comboBox1.Items.Add(typeof(Mio4400).Name);
            comboBox1.Items.Add(typeof(Schema5).Name);
            comboBox1.Items.Add(typeof(UdpGroupClient).Name);
            comboBox1.Items.Add(typeof(AsioInputSchema).Name);
            comboBox1.Items.Add(typeof(TalkStreamClient).Name);
            comboBox1.Items.Add(typeof(AsioOutputSchema).Name);
            comboBox1.Items.Add(typeof(FourierTest).Name);

            _sync = System.Threading.SynchronizationContext.Current;
            
            var model = new ControlTapeModel();
            model.Control = this;
            model.Engine.MainLayer = new TapeDrawing.Layers.EmptyLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
            };

            var scaleX = new LinearScale();
            var scaleY = new LinearScale();
            _viewModel = new EkytaViewModel
            {
                ScaleX = new ScaleModel
                {
                    Diapazone = new ScaleDiapazone
                    {
                        Min = 0,
                        Max = 2000,
                        MinWidth = 10
                    },
                    Scale = scaleX,
                    ScalePresentation = new EasyScalePresentation<float> { Scale = scaleX },
                    DefaultFrom = 0,
                    DefaultTo = 2000
                },
                ScaleY = new ScaleModel
                {
                    Diapazone = new ScaleDiapazone
                    {
                        Min = -3000,
                        Max = 3000,
                        MinWidth = 1
                    },
                    Scale = scaleY,
                    ScalePresentation = new EasyScalePresentation<float> { Scale = scaleX },
                    DefaultFrom = -120,
                    DefaultTo = 120
                },
                Redraw =
                    () => this.Invalidate(false),
                SelectedLinePositionStart = 0,
                SelectedLinePositionStep = 2
            };
            _viewModel.ScaleX.Diapazone.Set(0, 2000);
            _viewModel.ScaleY.Diapazone.Set(-120, 120);

            _viewModel.MainLayerFactory = new MainLayerFactory
            {
                Settings = new MainLayerSettings(),
                SyncUi = a => _sync.Send(o => a(), null),
                ViewModel = _viewModel,
                MainLayer = model.Engine.MainLayer
            };

            _viewModel.RebuildMainLayer();

            
        }

        void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                e.IsInputKey = true;
        }

        private System.Threading.SynchronizationContext _sync;

        private EkytaViewModel _viewModel;

        private ISchema _current;

        private Random _random=new Random();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_current != null)
                _current.Stop();

            _viewModel.RemoveAllGraphs();

            _current = (ISchema)Activator.CreateInstance(Type.GetType("WindowsFormsGenerator.Schemes." + comboBox1.Text));
            _current.OnRedraw = () => _sync.Post(o => this.Invalidate(false), null);

            int i = 0;
            try
            {



                foreach (var signalSource in _current.Build())
                {
                    _viewModel.AddGraph(new IntegratedMinMaxSignalSource{Internal = signalSource}, i.ToString());
                    _viewModel.SetColor(i.ToString(),
                                        PrimitivesFactory.CreateColor((byte)_random.Next(), (byte)_random.Next(),(byte)_random.Next()));
                                        //PrimitivesFactory.CreateColor(0, 0, 0));
                    _viewModel.SetLineWidth(i.ToString(), (byte)(_random.NextDouble() * 3));
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    XmlSchemaFactoryLogger.BuildLog() 
                    + Environment.NewLine
                    + "Inner exception:"
                    + Environment.NewLine
                    + ((ex is TargetInvocationException)?ex.InnerException.Message:ex.Message));

                _current = null;
                return;
            }

            _current.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_current != null)
                _current.Stop();
            base.OnClosed(e);
        }
    }
}
