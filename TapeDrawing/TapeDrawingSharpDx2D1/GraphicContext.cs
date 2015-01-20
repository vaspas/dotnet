using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using TapeDrawing.Core;
using TapeDrawing.Core.Instruments;
using TapeDrawing.Core.Shapes;
using TapeDrawingSharpDx2D1.Instruments;
using TapeDrawingSharpDx2D1.Shapes;

namespace TapeDrawingSharpDx2D1
{
	/// <summary>
	/// Графический контекст
	/// </summary>
	public class GraphicContext : IGraphicContext,IDisposable
	{
		public GraphicContext()
		{
			Graphics = new DirectxGraphics();
			MaxCacheSize = 100;
			_cacheCreated = false;
		}

		public int MaxCacheSize { get; set; }
        
		internal DirectxGraphics Graphics { get; private set; }

		public IInstrumentsFactory Instruments
		{
			get
			{
				if (!_cacheCreated) CreateCacheObjects();

				return new InstrumentsFactory
				       	{
                            Device = Graphics.Device,
                           // TextureCacher = _textureCacher
				       	};
			}
		}

		public IShapesFactory Shapes
		{
			get
            {
                if (!_cacheCreated) CreateCacheObjects();
                return new ShapesFactory { Device = Graphics.Device}; 
            }
		}

        public IClip CreateClip()
        {
            return new Clip(Graphics);
        }

	    private void CreateCacheObjects()
		{
            //TODO

            _cacheCreated = true;
		}


	    private bool _cacheCreated;


		#region Implementation of IDisposable

		public void Dispose()
		{
			if(_cacheCreated)
			{
                //TODO
			}
			Graphics.Dispose();
		}

		#endregion
	}
}
