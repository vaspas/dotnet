using System;
using System.Drawing;
using System.IO;
using ComparativeTest.Renderers;
using TapeDrawing.Core.Area;
using TapeDrawing.Core.Layer;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeDrawing.Layers;
using Color = TapeDrawing.Core.Primitives.Color;
using FontStyle = TapeDrawing.Core.Primitives.FontStyle;

namespace ComparativeTest
{
    public class MainLayerFactory
    {
        public Main Properties { get; set; }

        public Random Random { get; set; }

        public void Create(ILayer mainLayer)
        {
            mainLayer.Add(CreateBackgroundLayer());

            //if (System.IO.File.Exists(Properties.tbImageFilePath.Text))
            //    mainLayer.Add(CreateImageLayer(Image.FromFile(Properties.tbImageFilePath.Text)));

            if (File.Exists(Properties.tbImageFilePath.Text))
                mainLayer.Add(CreateImageLayer(new MemoryStream(File.ReadAllBytes(Properties.tbImageFilePath.Text))));

            if (Properties.cbSolidLine.Checked)
                mainLayer.Add(CreateLinesLayer(LineStyle.Solid));
            if (Properties.cbDashLine.Checked)
                mainLayer.Add(CreateLinesLayer(LineStyle.Dash));
            if (Properties.cbDotLine.Checked)
                mainLayer.Add(CreateLinesLayer(LineStyle.Dot));

            mainLayer.Add(CreateDrawRectangleLayer());
            mainLayer.Add(CreateFillRectangleLayer());
            mainLayer.Add(CreatePolygonLayer());

            var fs = FontStyle.None;
            if (Properties.cbItalicFont.Checked)
                fs |= FontStyle.Italic;
            if (Properties.cbBoldFont.Checked)
                fs |= FontStyle.Bold;
            mainLayer.Add(CreateTextLayer(fs));
        }

        private ILayer CreateBackgroundLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new BackgroundRenderer
                {
                    Color = new Color(255,255,255)
                }
            };
        }

        private ILayer CreateLinesLayer(LineStyle style)
        {
            return new RendererLayer
                            {
                                Area = AreasFactory.CreateRelativeArea(0.2f,0.8f,0.2f,0.8f),
                                Settings = new RendererLayerSettings{Clip = true},
                                Renderer = new LinesRenderer
                                               {
                                                   Color = new Color((byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255)),
                                                   Random = Random,
                                                   Count = (int)Properties.nudLinesCount.Value,
                                                   Style = style,
                                                   Translator = CreatePointTranslator(),
                                                   Width = (int)Properties.nudLineWidth.Value
                                               }
                            };
        }

        private ILayer CreateDrawRectangleLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new DrawRectangleRenderer
                {
                    Color = new Color((byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255)),
                    Random = Random,
                    Count = (int)Properties.nudRectCount.Value,
                    Translator = CreatePointTranslator(),
                    Width = (int)Properties.nudRectWidth.Value
                }
            };
        }

        private ILayer CreateFillRectangleLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new FillRectangleRenderer
                {
                    Color = new Color((byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255)),
                    Random = Random,
                    Count = (int)Properties.nudFillRectCount.Value,
                    Translator = CreatePointTranslator()
                }
            };
        }

        private ILayer CreatePolygonLayer()
        {
            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0.2f, 0.8f, 0.2f, 0.8f),
                Renderer = new PolygonRenderer
                {
                    Color = new Color((byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255)),
                    Random = Random,
                    Count = (int)Properties.nudPolygonCount.Value,
                    Translator = CreatePointTranslator()
                }
            };
        }

        private ILayer CreateTextLayer(FontStyle style)
        {
            var alignment = Alignment.None;
            if (Properties.cbTextAlignmentLeft.Checked)
                alignment |= Alignment.Left;
            if (Properties.cbTextAlignmentTop.Checked)
                alignment |= Alignment.Top;

            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new TextRenderer
                {
                    Color = new Color((byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255), (byte)Random.Next(255)),
                    Random = Random,
                    Count = (int)Properties.nudTextCount.Value,
                    Size = (int)Properties.nudTextSize.Value,
                    Style = style,
                    Text = Properties.tbTextString.Text,
                    Translator = CreatePointTranslator(),
                    Alignment = alignment,
                    AlignmentTranslator = CreateAlignmentTranslator()
                }
            };
        }

        private ILayer CreateImageLayer(Stream image)
        {
            var alignment = Alignment.None;
            if (Properties.cbImageAlignmentLeft.Checked)
                alignment |= Alignment.Left;
            if (Properties.cbImageAlignmentTop.Checked)
                alignment |= Alignment.Top;

            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ImageRenderer
                {
                    Random = Random,
                    Count = (int)Properties.nudImagesCount.Value,
                    Translator = CreatePointTranslator(),
                    ImageStream = image,
                    Alignment = alignment,
                    AlignmentTranslator = CreateAlignmentTranslator()
                }
            };
        }

        private ILayer CreateImageLayer(Image image)
        {
            var alignment = Alignment.None;
            if (Properties.cbImageAlignmentLeft.Checked)
                alignment |= Alignment.Left;
            if (Properties.cbImageAlignmentTop.Checked)
                alignment |= Alignment.Top;

            return new RendererLayer
            {
                Area = AreasFactory.CreateRelativeArea(0, 1, 0, 1),
                Renderer = new ImageRenderer
                {
                    Random = Random,
                    Count = (int)Properties.nudImagesCount.Value,
                    Translator = CreatePointTranslator(),
                    Image = new Bitmap(image),
                    Alignment = alignment,
                    AlignmentTranslator = CreateAlignmentTranslator()
                }
            };
        }


        private IPointTranslator CreatePointTranslator()
        {
            var cfg = PointTranslatorConfigurator.CreateLinear();

            if (Properties.cbMirrorXBefore.Checked)
                cfg.MirrorX();

            if (Properties.cbMirrorYBefore.Checked)
                cfg.MirrorY();

            if (Properties.cbChangeAxels.Checked)
                cfg.ChangeAxels();

            if (Properties.cbMirrorXAfter.Checked)
                cfg.MirrorX();

            if (Properties.cbMirrorYAfter.Checked)
                cfg.MirrorY();

            return cfg.Translator;
        }

        private IAlignmentTranslator CreateAlignmentTranslator()
        {
            var cfg = AlignmentTranslatorConfigurator.Create();

            if (Properties.cbMirrorXBefore.Checked)
                cfg.MirrorX();

            if (Properties.cbMirrorYBefore.Checked)
                cfg.MirrorY();

            if (Properties.cbChangeAxels.Checked)
                cfg.ChangeAxels();

            if (Properties.cbMirrorXAfter.Checked)
                cfg.MirrorX();

            if (Properties.cbMirrorYAfter.Checked)
                cfg.MirrorY();

            return cfg.Translator;
        }
    }
}
