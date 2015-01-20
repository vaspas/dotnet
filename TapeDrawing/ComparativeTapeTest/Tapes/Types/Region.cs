using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TapeImplement.ObjectRenderers;
using TapeImplement.TapeModels.Vagon.ToolTip;

namespace ComparativeTapeTest.Tapes.Types
{
    class Region//:IToolTipObject
    {
        /// <summary>
        /// Начало участка протяженного объекта
        /// </summary>
        public int From { get; set; }
        /// <summary>
        /// Конец участка протяженного объекта
        /// </summary>
        public int To { get; set; }

        public string Text { get; set; }

        public string GetText()
        {
            return Text;
        }
    }
}
