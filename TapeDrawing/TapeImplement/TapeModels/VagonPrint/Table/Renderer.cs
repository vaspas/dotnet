
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core;
using TapeDrawing.Core.Primitives;
using TapeDrawing.Core.Translators;
using TapeImplement.TapeModels.VagonPrint.Sources;

namespace TapeImplement.TapeModels.VagonPrint.Table
{
    class Renderer : IRenderer
    {
        public Renderer()
        {
            ScaleFactor = 1;
        }

        public IPrintSource PrintSource;

        /// <summary>
        /// Название шрифта.
        /// </summary>
        public string FontName;

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        public int FontSize;

        public Color FontColor;

        public float LineWidth;

        public Color LineColor;

        public float CursorLineWidth;

        public Color CursorLineColor;

        /// <summary>
        /// Высота строки.
        /// </summary>
        public int RowHeight;

        /// <summary>
        /// Размер линии курсора.
        /// </summary>
        public int CursorLineSize;

        public float ScaleFactor;

        /// <summary>
        /// Позиция ленты
        /// </summary>
        public IScalePosition<int> TapePosition;

        public List<Column> Columns;

        public IEnumerable<Row> OverflowRows { get; private set; }

        public void DropOverflowRows()
        {
            OverflowRows = null;
        }


        /// <summary>
        /// Метод для рисования на слое.
        /// </summary>
        /// <param name="gr">Объект для рисования.</param>
        /// <param name="rect">Область рисования.</param>
        public void Draw(IGraphicContext gr, Rectangle<float> rect)
        {
            if(OverflowRows!=null)
                DrawOverflow(gr, rect);
            else
                DrawTable(gr, rect);
        }


        private void DrawOverflow(IGraphicContext gr, Rectangle<float> rect)
        {
            var currentY = rect.Bottom;
            
            foreach (var row in OverflowRows)
            {
                if (currentY >= rect.Top)
                {
                    OverflowRows = OverflowRows.SkipWhile(r => r != row);
                    return;
                }

                var rowRect = new Rectangle<float>
                                  {
                                      Left = rect.Left,
                                      Right = rect.Right,
                                      Bottom = currentY,
                                      Top = currentY + row.Count * RowHeight
                                  }; 
                DrawRowContent(gr, row, rowRect);
                if (row.IsBorderEnabled)
                    DrawRowBorder(gr, rowRect);

                currentY += row.Count * RowHeight;
            }

            OverflowRows = null;
        }

        private void DrawTable(IGraphicContext gr, Rectangle<float> rect)
        {
            if (TapePosition.From >= TapePosition.To)
                return;

            var translator = LinearTranslatorConfigurator.Create().Translator;
            translator.SrcFrom = TapePosition.From;
            translator.SrcTo = TapePosition.From + (TapePosition.To - TapePosition.From)/ScaleFactor;
            translator.DstFrom = rect.Bottom;
            translator.DstTo = rect.Top;

            var currentY = rect.Bottom;

            var rows = PrintSource.GetRows(TapePosition).ToList().OrderBy(r => r.Index).ToList();

            foreach (var row in rows)
            {
                var requeredY = translator.Translate(row.Index) - (row.Count * RowHeight / 2f);

                var fromTopY = rect.Top - rows.SkipWhile(r => r != row).Sum(r => r.Count * RowHeight);
                if (requeredY > fromTopY)
                    requeredY = fromTopY;

                if (requeredY < currentY)
                    requeredY = currentY;

                OverflowRows = rows.SkipWhile(r => r != row);
                OverflowRows = null;

                if (requeredY >= rect.Top)
                {
                    OverflowRows = rows.SkipWhile(r => r != row);
                    return;
                }

                var rowRect = new Rectangle<float>
                                  {
                                      Left = rect.Left,
                                      Right = rect.Right,
                                      Bottom = requeredY,
                                      Top = requeredY + row.Count * RowHeight
                                  };
                DrawRowContent(gr, row, rowRect);
                if (row.IsBorderEnabled)
                    DrawRowBorder(gr, rowRect);
                if (row.IsCursorEnabled)
                    DrawRowCursor(gr, rect.Right, requeredY + (row.Count * RowHeight / 2f), translator.Translate(row.Index));

                currentY = requeredY + row.Count * RowHeight;
            }
        }

        private void DrawRowContent(IGraphicContext gr, Row row, Rectangle<float> rect)
        {
            var currentPositionY = rect.Top - RowHeight;

            foreach (var cells in row)
            {
                var currentPositionX = rect.Left;
                for (var col = 0; col < Columns.Count; col++)
                {
                    if (cells.Count <= col)
                        break;

                    if (cells[col] != null)
                    {
                        var cellRect =new Rectangle<float>
                                          {
                                              Left = currentPositionX,
                                              Right = currentPositionX + Columns[col].Size,
                                              Bottom = currentPositionY,
                                              Top = currentPositionY + RowHeight
                                          };

                        if (cells[col] is TextCell)
                            DrawTextCell(gr, cells[col] as TextCell, cellRect);
                        if (cells[col] is ImageCell)
                            DrawImageCell(gr, cells[col] as ImageCell, cellRect);
                    }

                    currentPositionX += Columns[col].Size;
                }

                currentPositionY -= RowHeight;
            }
        }


        private void DrawTextCell(IGraphicContext gr, TextCell cell, Rectangle<float> rect)
        {
            using(var font=gr.Instruments.CreateFont(FontName, FontSize, cell.Color??FontColor, cell.FontStyle ))
            using (var shape=gr.Shapes.CreateText(font, cell.Alignment, 0))
            {
                var x = (rect.Left + rect.Right)/2;
                var y = (rect.Bottom + rect.Top)/2;

                if((cell.Alignment&Alignment.Left)!=0)
                    x = rect.Left;
                if((cell.Alignment&Alignment.Right)!=0)
                    x = rect.Right;
                if((cell.Alignment&Alignment.Top)!=0)
                    y = rect.Top;
                if((cell.Alignment&Alignment.Bottom)!=0)
                    y = rect.Bottom;

                shape.Render(cell.Text, new Point<float> {X = x, Y = y});
            }
        }

        private void DrawImageCell(IGraphicContext gr, ImageCell cell, Rectangle<float> rect)
        {

        }


        private void DrawRowBorder(IGraphicContext gr, Rectangle<float> rect)
        {
            using (var pen = gr.Instruments.CreatePen(LineColor, LineWidth, LineStyle.Solid))
            using (var shape = gr.Shapes.CreateDrawRectangle(pen))
                shape.Render(rect);
        }

        private void DrawRowCursor(IGraphicContext gr, float fromX, float rowY, float indexY)
        {
            using (var pen = gr.Instruments.CreatePen(CursorLineColor, CursorLineWidth, LineStyle.Dot))
            using (var shape = gr.Shapes.CreateLines(pen))
                shape.Render(new [] 
                { 
                    new Point<float>{X=fromX,Y=rowY}, 
                    new Point<float>{X=fromX,Y=indexY}, 
                    new Point<float>{X=fromX+CursorLineSize,Y=indexY}
                });
        }
    }
}
