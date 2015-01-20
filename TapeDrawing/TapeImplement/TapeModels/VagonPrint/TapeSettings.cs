using TapeDrawing.Core.Primitives;

namespace TapeImplement.TapeModels.VagonPrint
{
    public class TapeSettings
    {
        

        /// <summary>
        /// Ширина области информационной строки справа.
        /// </summary>
        public int RightInfoWidth=14;

        /// <summary>
        /// Ширина области информационной строки сверху.
        /// </summary>
        public int TopInfoHeight = 27;


        /// <summary>
        /// Высота подписей шкал.
        /// </summary>
        public int ScaleInfoHeigth=13;

        /// <summary>
        /// Высота шкал.
        /// </summary>
        public int ScaleHeigth=26;

        /// <summary>
        /// Высота графиков.
        /// </summary>
        public int GraphsHeigth=988;


        /// <summary>
        /// Ширина промежутка между графиками.
        /// </summary>
        public int GraphsSplitWidth=4;

        public int TopEmptyAreaHeight = 6;


        public int Height
        {
            get { return TopInfoHeight + TopEmptyAreaHeight + GraphsHeigth + ScaleInfoHeigth + ScaleHeigth; }
        }


        public string FontName = "Nina";// "Arial Narrow";
        public float DefaultLineWidth = 0.1f;
        public Color DefaultColor = new Color(120,120,120);
        public Color LightColor = new Color(170, 170, 170);
    }
}
