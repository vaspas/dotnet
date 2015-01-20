using System;
using System.Windows.Forms;
using TapeImplementTest.Factories;

namespace TapeImplementTest
{
    public partial class ParamsForm : Form
    {
        public ParamsForm()
        {
            InitializeComponent();
        }

        public TestParams TestParams { get; set; }

        public event Action<TestParams> TestParamsChanged;

        private void OnParamsChanged()
        {
            if (TestParamsChanged != null)
                TestParamsChanged(TestParams);
        }

        private void ParamsFormLoad(object sender, EventArgs e)
        {
            nudIndexLen.Value = TestParams.IndexLen;
            tbMin.Text = TestParams.Min.ToString("0.######");
            tbMax.Text = TestParams.Max.ToString("0.######");
            nudInterrupts.Value = TestParams.Interrupts;
            nudScale.Value = TestParams.Scale;

            nudIndexLen.ValueChanged += NudIndexLenValueChanged;
            tbMin.TextChanged += TbMinTextChanged;
            tbMax.TextChanged += TbMaxTextChanged;
            nudInterrupts.ValueChanged += NudInterruptsValueChanged;
            nudScale.ValueChanged += NudScaleValueChanged;
        }

        private void NudIndexLenValueChanged(object sender, EventArgs e)
        {
            if (TestParams.IndexLen == (int)nudIndexLen.Value) return;

            TestParams.IndexLen = (int)nudIndexLen.Value;
            OnParamsChanged();
        }
        private void NudInterruptsValueChanged(object sender, EventArgs e)
        {
            if (TestParams.Interrupts == (int)nudInterrupts.Value) return;

            TestParams.Interrupts = (int)nudInterrupts.Value;
            OnParamsChanged();
        }
        private void NudScaleValueChanged(object sender, EventArgs e)
        {
            if (TestParams.IndexLen == (int)nudScale.Value) return;

            TestParams.Scale = (int)nudScale.Value;
            OnParamsChanged();
        }
        private void TbMaxTextChanged(object sender, EventArgs e)
        {
            float val;
            if (!float.TryParse(tbMax.Text, out val)) return;
            if (TestParams.Max == val) return;
            TestParams.Max = val;
            OnParamsChanged();
        }
        private void TbMinTextChanged(object sender, EventArgs e)
        {
            float val;
            if (!float.TryParse(tbMin.Text, out val)) return;
            if (TestParams.Min == val) return;
            TestParams.Min = val;
            OnParamsChanged();
        }

        private void CbVerticalCheckedChanged(object sender, EventArgs e)
        {
            if (TestParams.Vertical == cbVertical.Checked) return;
            TestParams.Vertical = cbVertical.Checked;
            OnParamsChanged();
        }
    }
}
