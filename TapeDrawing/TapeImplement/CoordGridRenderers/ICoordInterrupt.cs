namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Интерфейс для отметок прерывания ленты (пикеты, отметки, реверс)
    /// </summary>
    public interface ICoordInterrupt
    {
        /// <summary>
        /// Обозначение отметки
        /// </summary>
        string Title { get;  }

        /// <summary>
        /// Положение на ленте в индексах
        /// </summary>
        int Index { get;  }
    }
}
