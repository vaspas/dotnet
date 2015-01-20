
namespace TapeDrawing.Core.Layer
{
    /// <summary>
    /// Настройки слоя обработчика событий мыши.
    /// </summary>
    public class MouseListenerLayerSettings
    {
        public MouseListenerLayerSettings()
        {
            MouseUpOutside = true;
            ControlMouseLeave = true;
        }

        //public bool MouseDownOutside { get; set; }
        /// <summary>
        /// Обработка события перемещения курсора мыши за пределами слоя.
        /// </summary>
        public bool MouseMoveOutside { get; set; }
        public bool MouseUpOutside { get; set; }

        
        public bool ControlMouseLeave { get; set; }
    }
}
