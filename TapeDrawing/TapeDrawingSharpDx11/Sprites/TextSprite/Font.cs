using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace TapeDrawingSharpDx11.Sprites.TextSprite
{

    /// <summary>
    /// This class is responsible for rendering arbitrary text. Every TextRenderer is specialized for a specific font and relies on
    /// a SpriteRenderer for rendering the text.
    /// </summary>
    public class Font : IDisposable
    {
        private readonly TextSprite _sprite;
        private readonly TextFormat _font;


        
        private readonly SharpDX.Direct3D11.Device _device;

        private readonly SharpDX.DirectWrite.Factory _writeFactory;


        private readonly SharpDX.Direct2D1.Factory _d2DFactory;



        private float _fontSize;

        /// <summary>
        /// Gets or sets whether this TextRenderer should behave PIX compatibly.
        /// </summary>
        /// <remarks>
        /// PIX compatibility means that no shared resource is used.
        /// However, this will result in no visible text being drawn. 
        /// The geometry itself will be visible in PIX.
        /// </remarks>
        public static bool PixCompatible { get; set; }

        static Font()
        {
            PixCompatible = false;
        }

        /// <summary>
        /// Contains information about every char table that has been created.
        /// </summary>
        private readonly Dictionary<byte, CharTableDescription> _charTables = new Dictionary<byte, CharTableDescription>();


        private readonly RenderTargetProperties _rtp;

        /// <summary>
        /// Creates a new text renderer for a specific font.
        /// </summary>
        /// <param name="sprite">The sprite renderer that is used for rendering</param>
        /// <param name="fontName">Name of font. The font has to be installed on the system. 
        /// If no font can be found, a default one is used.</param>
        /// <param name="fontSize">Size in which to prerender the text. FontSize should be equal to render size for best results.</param>
        /// <param name="fontStretch">Font stretch parameter</param>
        /// <param name="fontStyle">Font style parameter</param>
        /// <param name="fontWeight">Font weight parameter</param>
        public Font(TextSprite sprite, String fontName, FontWeight fontWeight,
            SharpDX.DirectWrite.FontStyle fontStyle, FontStretch fontStretch, float fontSize)
        {
            _device = new SharpDX.Direct3D11.Device(new SharpDX.DXGI.Factory().GetAdapter(0), DeviceCreationFlags.BgraSupport, SharpDX.Direct3D.FeatureLevel.Level_10_0);
            _writeFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
            _d2DFactory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            _sprite = sprite;
            _fontSize = fontSize*1.33f;


                _rtp = new RenderTargetProperties
                {

                    DpiX = 96,
                    DpiY = 96,
                    Type = RenderTargetType.Default,
                    PixelFormat = new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied),
                    MinLevel = FeatureLevel.Level_10
                };

                _font = new TextFormat(_writeFactory, fontName,fontWeight, fontStyle, fontStretch, _fontSize);

            CreateCharTable(0);
        }

        /// <summary>
        /// Calculates the text layout for a given string.
        /// </summary>
        /// <param name="s">The string to layout</param>
        /// <returns>The string's layout information</returns>
        private TextLayout GetTextLayout(string s)
        {
            return new TextLayout(_writeFactory, s, _font, 1.0f, 1.0f);
        }

        /// <summary>
        /// Creates a new shared texture and performs the specified draw calls on it.
        /// </summary>
        /// <param name="width">The texture's width</param>
        /// <param name="height">The texture's height</param>
        /// <param name="drawCalls">The draw calls to perform</param>
        private Texture2D CreateFontMapTexture(int width, int height, IEnumerable<CharRenderCall> drawCalls)
        {
            var texDesc = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R8G8B8A8_UNorm,
                Height = height,
                Width = width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.Shared,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default
            };

            var texture = new Texture2D(_device, texDesc);

            var rtv = new RenderTargetView(_device, texture);
            _device.ImmediateContext.ClearRenderTargetView(rtv, new Color4(1, 1, 1, 0));


            var surface = texture.QueryInterface<Surface>();
            var target = new RenderTarget(_d2DFactory, surface, _rtp);
            var color = new SolidColorBrush(target, new Color4(1, 1, 1, 1));

            target.BeginDraw();

            foreach (var drawCall in drawCalls)
            {
                target.DrawTextLayout(drawCall.Position,drawCall.TextLayout, color);
            }

            target.EndDraw();

            color.Dispose();

            //This is a workaround for Windows 8.1 machines. 
            //If these lines would not be present, the shared resource would be empty.
            //TODO: find a nicer solution
            #region WorkAround
            var textureDescDummy = new Texture2DDescription
            {
                ArraySize = 1,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                Format = Format.R8G8B8A8_UNorm,
                Height = height,
                Width = width,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Staging
            };
            var textureDummy = new Texture2D(_device, textureDescDummy);
            DataStream dataStreamDummy;
            _device.ImmediateContext.CopyResource(texture, textureDummy);

            var databox = _device.ImmediateContext.MapSubresource(textureDummy, 0, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None, out dataStreamDummy);
            dataStreamDummy.Dispose();
            textureDummy.Dispose();
            #endregion

            target.Dispose();
            surface.Dispose();
            rtv.Dispose();
            return texture;
        }

        /// <summary>
        /// Creates a DirectX11 texture from the DirectX10 texture by opening the shared resource (or by creating a new one in PIX compatibility mode).
        /// </summary>
        /// <param name="width">The texture's width for PIX compatibility mode</param>
        /// <param name="height">The texture's height for PIX compatibility mode</param>
        /// <param name="texture10">The DirectX10 texture</param>
        /// <param name="texture11">Output parameter, the DirectX11 texture</param>
        /// <param name="srv11">Output parameter, the texture's ShaderResourceView</param>
        private void CreateDeviceCompatibleTexture(int width, int height, Texture2D texture10, out Texture2D texture11, out ShaderResourceView srv11)
        {
            var device11 = _sprite.Device;

            lock (device11)
            {
                var dxgiResource = texture10.QueryInterface<SharpDX.DXGI.Resource>();

                Texture2D tex11;
                if (PixCompatible)
                {
                    tex11 = new Texture2D(device11, new Texture2DDescription
                    {
                        ArraySize = 1,
                        BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                        CpuAccessFlags = CpuAccessFlags.None,
                        Format = Format.R8G8B8A8_UNorm,
                        Height = height,
                        Width = width,
                        MipLevels = 1,
                        OptionFlags = ResourceOptionFlags.Shared,
                        SampleDescription = new SampleDescription(1, 0),
                        Usage = ResourceUsage.Default
                    });
                }
                else
                {
                    tex11 = device11.OpenSharedResource<Texture2D>(dxgiResource.SharedHandle);
                }
                srv11 = new ShaderResourceView(device11, tex11);
                texture11 = tex11;
                dxgiResource.Dispose();
            }
        }

        /// <summary>
        /// Creates the texture and necessary structures for 256 chars whose unicode number starts with the given byte.
        /// The table containing ASCII has a prefix of 0 (0x00/00 - 0x00/FF).
        /// </summary>
        /// <param name="bytePrefix">The byte prefix of characters.</param>
        protected void CreateCharTable(byte bytePrefix)
        {
            int sizeX;
            int sizeY;
            TextLayout[] tl;
            //Get appropriate texture width height and layout accoring to 'Font' field member
            GenerateTextLayout(bytePrefix, out sizeX,out sizeY, out tl);

            CharTableDescription tableDesc;
            CharRenderCall[] drawCalls;
            
            //Get Draw calls and table description
            GenerateDrawCalls(sizeX, sizeY, tl, out tableDesc,out drawCalls);

            //Create font map texture from previously created draw calls
            var fontMapTexture = CreateFontMapTexture(sizeX, sizeY, drawCalls);

            //Create a texture to be used by the associated sprite renderer's graphics device from the font map texture.
            CreateDeviceCompatibleTexture(sizeX, sizeY, fontMapTexture, out tableDesc.Texture, out tableDesc.SRV);

            fontMapTexture.Dispose();

            foreach (var layout in tl)
            {
                layout.Dispose();
            }
            #if DEBUG 
                System.Diagnostics.Debug.WriteLine("Created Char Table " + bytePrefix + " in " + sizeX + " x " + sizeY);
            #endif


            _charTables.Add(bytePrefix, tableDesc);           
        }

        private void GenerateTextLayout(byte bytePrefix, out int sizeX, out int sizeY, out TextLayout[] tl)
        {
            sizeX = (int)(_fontSize * 12);
            sizeX = (int)Math.Pow(2, Math.Ceiling(Math.Log(sizeX, 2)));
            //Try how many lines are needed:
            tl = new TextLayout[256];
            int line = 0, xPos = 0, yPos = 0;
            for (int i = 0; i < 256; ++i)
            {
                tl[i] = GetTextLayout(Convert.ToChar(i + (bytePrefix << 8)).ToString());
                int charWidth = 2 + (int)Math.Ceiling(tl[i].Metrics.LayoutWidth + tl[i].OverhangMetrics.Left + tl[i].OverhangMetrics.Right);
                int charHeight = 2 + (int)Math.Ceiling(tl[i].Metrics.LayoutWidth + tl[i].OverhangMetrics.Top + tl[i].OverhangMetrics.Bottom);
                line = Math.Max(line, charHeight);
                if (xPos + charWidth >= sizeX)
                {
                    xPos = 0;
                    yPos += line;
                    line = 0;
                }
                xPos += charWidth;
            }

            sizeY = line + yPos;
            sizeY = (int)Math.Pow(2, Math.Ceiling(Math.Log(sizeY, 2)));
        }

        private static void GenerateDrawCalls(int sizeX, int sizeY, TextLayout[] tl, out CharTableDescription tableDesc, out CharRenderCall[] drawCalls)
        {
            drawCalls = new CharRenderCall[256];
            tableDesc = new CharTableDescription();
            int line = 0, xPos = 0, yPos = 0;            
            for (var i = 0; i < 256; ++i)
            {
                //1 additional pixel on each side
                var charWidth = 2 + (int)Math.Ceiling(tl[i].Metrics.LayoutWidth + tl[i].OverhangMetrics.Left + tl[i].OverhangMetrics.Right);
                var charHeight = 2 + (int)Math.Ceiling(tl[i].Metrics.LayoutHeight + tl[i].OverhangMetrics.Top + tl[i].OverhangMetrics.Bottom);
                line = Math.Max(line, charHeight);
                if (xPos + charWidth >= sizeX)
                {
                    xPos = 0;
                    yPos += line;
                    line = 0;
                }
                var charDesc = new CharDescription();

                charDesc.CharSize = new Vector2(tl[i].Metrics.WidthIncludingTrailingWhitespace, tl[i].Metrics.Height);
                charDesc.OverhangLeft = tl[i].OverhangMetrics.Left + 1;
                charDesc.OverhangTop = tl[i].OverhangMetrics.Top + 1;
                //Make XPos + CD.Overhang.Left an integer number in order to draw at integer positions
                charDesc.OverhangLeft += (float)Math.Ceiling(xPos + charDesc.OverhangLeft) - (xPos + charDesc.OverhangLeft);
                //Make YPos + CD.Overhang.Top an integer number in order to draw at integer positions
                charDesc.OverhangTop += (float)Math.Ceiling(yPos + charDesc.OverhangTop) - (yPos + charDesc.OverhangTop);

                charDesc.OverhangRight = charWidth - charDesc.CharSize.X - charDesc.OverhangLeft;
                charDesc.OverhangBottom = charHeight - charDesc.CharSize.Y - charDesc.OverhangTop;

                charDesc.TexCoordsStart = new Vector2(((float)xPos / sizeX), ((float)yPos / sizeY));
                charDesc.TexCoordsSize = new Vector2((float)charWidth / sizeX, (float)charHeight / sizeY);

                charDesc.TableDescription = tableDesc;

                tableDesc.Chars[i] = charDesc;

                drawCalls[i] = new CharRenderCall { Position = new Vector2(xPos + charDesc.OverhangLeft, yPos + charDesc.OverhangTop), TextLayout = tl[i]};
                
                xPos += charWidth;
            }
    }
        

        /// <summary>
        /// Draws the string untransformed in absolute coordinate system.
        /// </summary>
        /// <param name="text">The text to draw</param>
        /// <param name="position">A position in absolute coordinates where the top left corner of the first character will be</param>
        /// <param name="color">The color in which to draw the text</param>
        /// <returns>The StringMetrics for the rendered text</returns>
        public StringMetrics DrawString(string text, Vector2 position, Color4 color)
        {
            StringMetrics sm;
            IterateString(text, position, true, color, out sm);
            return sm;
        }

        /// <summary>
        /// Measures the untransformed string in absolute coordinate system.
        /// </summary>
        /// <param name="text">The text to measure</param>
        /// <returns>The StringMetrics for the text</returns>
        public StringMetrics MeasureString(string text)
        {
            StringMetrics sm;
            IterateString(text, Vector2.Zero, false, new SharpDX.Color(), out sm);
            return sm;
        }



        private void IterateString(string text, Vector2 position, bool draw, Color4 color, out StringMetrics metrics)
        {
            metrics = new StringMetrics();

            var visualText =  NBidi.NBidi.LogicalToVisual(text);
            
            for (var i = 0; i < visualText.Length; i += char.IsSurrogatePair(visualText, i) ? 2 : 1)
            {
                var c = char.ConvertToUtf32(visualText, i);

                var charDesc = GetCharDescription(c);
                var charMetrics = charDesc.ToStringMetrics(position);
                if (draw)
                {
                    if (charMetrics.FullRectSize.X != 0 && charMetrics.FullRectSize.Y != 0)
                    {
                        var posY = position.Y - charMetrics.OverhangTop;
                        var posX = position.X - charMetrics.OverhangLeft;
                        _sprite.Draw(charDesc.TableDescription.SRV, new Vector2(posX, posY), charMetrics.FullRectSize, Vector2.Zero, charDesc.TexCoordsStart, charDesc.TexCoordsSize, color);
                    }
                }

                metrics.Merge(charMetrics);

                position.X += charMetrics.Size.X;

                //Break newlines
                if (c == '\r')
                    position.X = metrics.TopLeft.X;

                if (c == '\n')
                    position.Y = metrics.BottomRight.Y - charMetrics.Size.Y / 2;
            }
        }

        private CharDescription GetCharDescription(int c)
        {
            var b = (byte)(c & 0x000000FF);
            var bytePrefix = (byte)((c & 0x0000FF00) >> 8);
            if (!_charTables.ContainsKey(bytePrefix))
                CreateCharTable(bytePrefix);
            return _charTables[bytePrefix].Chars[b];
        }

        
        
        
        public bool Disposed { get; private set; }
        /// <summary>
        /// Disposes of the SpriteRenderer.
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
                return;

            _device.Dispose();
            _writeFactory.Dispose();
            _d2DFactory.Dispose();

            _font.Dispose();

            foreach (var table in _charTables)
            {
                table.Value.SRV.Dispose();
                table.Value.Texture.Dispose();
            }

            Disposed = true;

        }
    }

    
    
}
