using System.Windows.Forms;
using TapeDrawing.Core.Area;
using TapeDrawing.Layers;
using TapeDrawingWinForms;
using TapeImplement;
using TapeImplementTest.Factories;

namespace TapeImplementTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();            

            _testParams = new TestParams {IndexLen = 1000, Min = 0, Max = 10, Interrupts = 0, Scale = 1};

            _tapePosition = new SimpleScalePosition<int> { From = 0, To = 0};

            InitModel();

        }

        private void InitModel()
        {
            _factory = FactoryCreator.GetFactory(_testParams.Vertical, SourceFactory.Generate(_testParams), _tapePosition, _testParams);

            _model.Control = null;
            _model.Control = testControl1;
            _model.Engine.MainLayer = new EmptyLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1)
            };
            _factory.Create(_model.Engine.MainLayer);
        }

        private void InitScroll()
        {
            hScrollBar1.Minimum = -_testParams.IndexLen - 1;
            hScrollBar1.Maximum = _testParams.IndexLen + 1;
            hScrollBar1.Value = 0;
        }

        private readonly TestParams _testParams;

        private readonly IScalePosition<int> _tapePosition;

        private ILayerFactory _factory;

        private readonly ControlTapeModel _model = new ControlTapeModel();

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _model.Control = null;

            base.OnClosing(e);
        }

        private void HScrollBar1ValueChanged(object sender, System.EventArgs e)
        {
            _tapePosition.Set(hScrollBar1.Value, _tapePosition.From + _testParams.IndexLen / _testParams.Scale);
            testControl1.Invalidate();
        }

        

        private void TestFormLoad(object sender, System.EventArgs e)
        {
            InitScroll();

            HScrollBar1ValueChanged(null, null);

            var paramsForm = new ParamsForm {TestParams = _testParams};
            paramsForm.TestParamsChanged += ParamsFormTestParamsChanged;
            paramsForm.Closed += ParamsFormClosed;

            paramsForm.Show();
        }

        void ParamsFormClosed(object sender, System.EventArgs e)
        {
            Close();
        }

        void ParamsFormTestParamsChanged(TestParams obj)
        {
            InitModel();

            InitScroll();
            HScrollBar1ValueChanged(null, null);
        }
    }
}
