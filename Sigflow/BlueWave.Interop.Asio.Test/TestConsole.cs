#region
//
// BlueWave.Interop.Asio by Rob Philpott. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk.  This file and the code contained within is freeware and may be
// distributed and edited without restriction. You may be bound by licencing restrictions
// imposed by Steinberg - check with them prior to distributing anything.
// 

#endregion

using System;
using System.Threading;
using BlueWave.Interop.Asio;

namespace BlueWave.Interop.Asio.Test
{
	/// <summary>
	/// A simple test console application
	/// </summary>
	public class TestConsole
	{
        
        // STAThread is ESSENTIAL to make this work
		[STAThread] public static void Main(string[] args)
		{
            // no messing, this is high priority stuff
			//Thread.CurrentThread.Priority = ThreadPriority.Highest;
		    Console.WriteLine(int.MaxValue);
			// make sure we have at least one ASIO driver installed);
			if (AsioDriver.InstalledDrivers.Length == 0)
			{
				Console.WriteLine("There appears to be no ASIO drivers installed on your system.");
				Console.WriteLine("If your soundcard supports ASIO natively, install the driver");
				Console.WriteLine("from the support disc. If your soundcard has no native ASIO support");
				Console.WriteLine("you can probably use the generic ASIO4ALL driver.");
				Console.WriteLine("You can download this from: http://www.asio4all.com/");
				Console.WriteLine("It's very good!");
				Console.WriteLine();
				Console.WriteLine("Hit Enter to exit...");
				Console.ReadLine();
				return;
			}

            // bingo, we've go at least one
			Console.WriteLine("Your system has the following ASIO drivers installed:");
			Console.WriteLine();

            // so iterate through them
			for (int index = 0; index < AsioDriver.InstalledDrivers.Length; index++)
			{
                // and display them
				Console.WriteLine(string.Format("  {0}. {1}", index + 1, AsioDriver.InstalledDrivers[index]));
			}

			Console.WriteLine();

			int driverNumber = 0;

            // get them to choose one
			while (driverNumber < 1 || driverNumber > AsioDriver.InstalledDrivers.Length)
			{
				// we'll keep telling them this until they make a valid selection
				Console.Write("Select which driver you wish to use (x for exit): ");
				ConsoleKeyInfo key = Console.ReadKey();
				Console.WriteLine();

				// deal with exit condition
				if (key.KeyChar == 'x') return;

				// convert from ASCII to int
				driverNumber = key.KeyChar - 48;
			}

			Console.WriteLine();
			Console.WriteLine("Using: " + AsioDriver.InstalledDrivers[driverNumber - 1]);
			Console.WriteLine();

			// load and activate the desited driver
            
			AsioDriver driver = AsioDriver.SelectDriver(AsioDriver.InstalledDrivers[driverNumber - 1]);
            
            
			// popup the driver's control panel for configuration
            //driver.ShowControlPanel();

            driver.SetSampleRate(48000);
			// now dump some details
            Console.WriteLine("  Driver name = " + driver.DriverName);
            Console.WriteLine("  Driver version = " + driver.Version);
            Console.WriteLine("  Input channels = " + driver.NumberInputChannels);
            Console.WriteLine("  Output channels = " + driver.NumberOutputChannels);
            Console.WriteLine("  Min buffer size = " + driver.BufferSizex.MinSize);
            Console.WriteLine("  Max buffer size = " + driver.BufferSizex.MaxSize);
            Console.WriteLine("  Preferred buffer size = " + driver.BufferSizex.PreferredSize);
            Console.WriteLine("  Granularity = " + driver.BufferSizex.Granularity);
            Console.WriteLine("  Sample rate = " + driver.SampleRate);

			// get our driver wrapper to create its buffers
		    var bufSize = 64;
            //driver.CreateBuffers(bufSize);
            
            driver.CreateOutputBuffers(bufSize);
            driver.CreateInputBuffers(bufSize);

            
            
            
            
            Console.WriteLine(driver.ErrorMessage);

			// write out the input channels
            /*Console.WriteLine("  Input channels found = " + driver.InputChannels.Length);
			Console.WriteLine("  ----");

            foreach (Channel channel in driver.InputChannels)
			{
				Console.WriteLine(channel.Name);
			}*/

			// and the output channels
            Console.WriteLine("  Output channels found = " + driver.OutputChannels.Length);
            Console.WriteLine("----");

            foreach (Channel channel in driver.OutputChannels)
			{
				Console.WriteLine(channel.Name);
			}
            
            // this is our buffer fill event we need to respond to
            driver.BufferUpdate += AsioDriver_BufferUpdate;

            // and off we go
            driver.Start();

            // wait for enter key
            Console.WriteLine();
            Console.WriteLine("Press Enter to end");
            Console.WriteLine(driver.ErrorMessage);
			Console.ReadLine();

            // and all donw
            driver.DisposeBuffers();
            driver.Stop();
            Console.WriteLine(driver.ErrorMessage);
            driver.Release();

           
            Console.ReadLine();
		}

		/// <summary>
		/// Called when a buffer update is required
		/// </summary>
		private static void AsioDriver_BufferUpdate(object sender, EventArgs e)
		{
			// the driver is the sender
            AsioDriver driver = sender as AsioDriver;
	

			// get the input channel and the stereo output channels
            Channel input = driver.InputChannels[0];
			Channel leftOutput = driver.OutputChannels[0];
			Channel rightOutput = driver.OutputChannels[1];

            Random r=new Random();

			for (int index = 0; index < leftOutput.BufferSize; index++)
			{
				// and copy from the delay array to the output buffers (wrapping as needed)
			    leftOutput[index] = input[index];// r.Next();// _delayBuffer[index, (_counter - _leftDelay) >= 0 ? _counter - _leftDelay : _counter - _leftDelay + MaxBuffers];
                rightOutput[index] = input[index];// r.Next();// _delayBuffer[index, (_counter - _rightDelay) >= 0 ? _counter - _rightDelay : _counter - _rightDelay + MaxBuffers];
			}
		}
	}
}
