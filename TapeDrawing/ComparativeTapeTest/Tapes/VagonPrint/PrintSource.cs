using System;
using System.Collections.Generic;
using TapeDrawing.Core.Primitives;
using TapeImplement;
using TapeImplement.TapeModels.VagonPrint.Sources;
using TapeImplement.TapeModels.VagonPrint.Table;

namespace ComparativeTapeTest.Tapes.VagonPrint
{
    class PrintSource : IPrintSource
    {
        public IScalePosition<int> TapePosition { get; set; }

        public string GetRightInfo(IScalePosition<int> position)
        {
            return
                    "ТВЕМА:ПО версия 1.98 от 13.06.2013 (ЦП-515) Интеграл: 002 (Инженер Самодюк А.Ю.) <Д-ВОСТ> <05.07.2013 3:40:12> <Обр> <Сзади><ТКанон><Проезд.sfs>";
            
        }

        public string GetTopInfoLine1(IScalePosition<int> position)
        {
            return "Чита - Хабаровск (13808) Путь:1 Км:8283 ПЧ-2/ПЧУ-2/ПД-5/ПДБ-1 Уст: 70/60/60 Пред:-"; 
        }

        public string GetTopInfoLineLeft(IScalePosition<int> position)
        {
            return "Top left";
        }

        public string GetTopInfoLine2(IScalePosition<int> position)
        {
            return
                "Кол.ст-2:56; 3:1; 4:0. Кол.огр.:1/0 Огр.:40/40/40 Скор:60 КрдПЧ";

        }

        public string GetCenterInfo()
        {
            return
                "center info";

        }

        public string GetBottomInfoLine(IScalePosition<int> position)
        {
            return "bottom line";
        }

        public IEnumerable<Row> GetRows(IScalePosition<int> position)
        {
                var rows = new List<Row>();

                var r = new Random();

                for (var i = 0; i < r.Next(1000) + 1; i++)
                    rows.Add(GenerateRow(r));

                return rows; 
            
        }

        private Row GenerateRow(Random r)
        {
            var row = new Row
                          {
                              IsBorderEnabled = r.Next()%2 == 0,
                              Index =TapePosition.From+ r.Next(TapePosition.To-TapePosition.From),
                              IsCursorEnabled = r.Next()%2 == 0
                          };

            for (var i = 0; i < r.Next(3) + 1; i++)
            {
                var cells = new List<ICell>();
                row.Add(cells);

                for(var j=0;j<r.Next(10);j++)
                    cells.Add(GenerateCell(r));
            }

            return row;
        }

        private static ICell GenerateCell(Random r)
        {
            var als = new [] {Alignment.Top, Alignment.None, Alignment.Left, Alignment.Right, Alignment.Bottom};
            var fs = new [] {FontStyle.None, FontStyle.Bold, FontStyle.Italic};

            return new TextCell
                       {
                           Alignment = als[r.Next(als.Length)],
                           FontStyle = fs[r.Next(fs.Length)],
                           Text = r.Next(1000).ToString()
                       };
        }
    }
}
