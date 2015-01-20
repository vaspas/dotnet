using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace TestDirectXInit2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            

            
        }

        private void Try(int devnumber)
        {
            try
            {
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Set params...";

                // Установим параметры
                var presentParams = new PresentParameters { Windowed = true, SwapEffect = SwapEffect.Discard };
                
                Caps deviceCaps;

                // Выясним характеристики устройства, которое мы можем создать
                var deviceType = DeviceType.Software;
                var createFlags = CreateFlags.SoftwareVertexProcessing;

                // Будем пытаться создать самое производительное устройство DirectX
                try
                {
                    deviceCaps = Manager.GetDeviceCaps(devnumber, DeviceType.Hardware);

                    if ((deviceCaps.VertexShaderVersion >= new Version((int)nudMinVertexVersion.Value, 0)) && (deviceCaps.PixelShaderVersion >= new Version((int)nudMinPixelVersion.Value, 0)))
                    {
                        deviceType = DeviceType.Hardware;


                        textBox1.Text += Environment.NewLine;
                        textBox1.Text += "Device Hardware";

                        if (deviceCaps.DeviceCaps.SupportsHardwareTransformAndLight)
                        {
                            createFlags = CreateFlags.HardwareVertexProcessing;
                            textBox1.Text += Environment.NewLine;
                            textBox1.Text += "Device: HardwareVertexProcessing";
                        }
                        if (deviceCaps.DeviceCaps.SupportsPureDevice)
                        {
                            createFlags |= CreateFlags.PureDevice;
                            textBox1.Text += Environment.NewLine;
                            textBox1.Text += "Device: PureDevice";
                        }
                    }
                    else
                    {
                        textBox1.Text += Environment.NewLine;
                        textBox1.Text += "Device: Software";

                    }
                }
                catch (Exception ex)
                {
                    textBox1.Text += Environment.NewLine;
                    textBox1.Text += "Device: GetDeviceCaps error! " + ex.Message;

                }

                textBox1.Text += Environment.NewLine;
                textBox1.Text += "Create Device...";

                try
                {
                    new Device(devnumber, deviceType, this, createFlags, presentParams);
                }
                catch (Exception ex)
                {

                    textBox1.Text += Environment.NewLine;
                    textBox1.Text += "Warn: " + ex;
                    textBox1.Text += Environment.NewLine;
                    textBox1.Text += "Device Reference";

                    deviceType = DeviceType.Reference;
                    new Device(devnumber, deviceType, this, createFlags, presentParams);
                }

                textBox1.Text += Environment.NewLine;
                textBox1.Text += "OK";

            }
            catch (Exception ex)
            {
                textBox1.Text += Environment.NewLine;
                textBox1.Text += "ERR " + ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text += Environment.NewLine;
            textBox1.Text += "Devices count = " + Manager.Adapters.Count;

            for (var i = 0; i < Manager.Adapters.Count; i++)
            {
                textBox1.Text += Environment.NewLine;
                textBox1.Text += Environment.NewLine;
                textBox1.Text +=
                    string.Format(
                        "Device adapter={0}, CurrentDisplayMode ={1}, Description={2}, DeviceName={3}, DriverName={4}, DriverVersion={5}, Revision={6}",
                        Manager.Adapters[i].Adapter,
                        Manager.Adapters[i].CurrentDisplayMode,
                        Manager.Adapters[i].Information.Description,
                        Manager.Adapters[i].Information.DeviceName,
                        Manager.Adapters[i].Information.DriverName,
                        Manager.Adapters[i].Information.DriverVersion,
                        Manager.Adapters[i].Information.Revision);

                Try(i);
            }
        }
    }
}
