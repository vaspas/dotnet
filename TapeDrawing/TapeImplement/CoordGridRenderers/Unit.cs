namespace TapeImplement.CoordGridRenderers
{
    /// <summary>
    /// Класс описывает один участок без прерываний
    /// </summary>
    struct Unit
    {
        /// <summary>
        /// Индекс начала участка
        /// </summary>
        public int BeginIndex;

        /// <summary>
        /// Индекс конца участка
        /// </summary>
        public int EndIndex;

        /// <summary>
        /// Координата начала участка
        /// </summary>
        public float BeginCoordinate;

        /// <summary>
        /// Координата конца участка
        /// </summary>
        public float EndCoordinate;

        /// <summary>
        /// Слева на участке находится прерывание
        /// </summary>
        public bool LeftInterrupt;

        /// <summary>
        /// Справа на участке находится прерывание
        /// </summary>
        public bool RightInterrupt;

        public Unit Revert()
        {
            return new Unit
                       {
                           BeginCoordinate = EndCoordinate,
                           BeginIndex = EndIndex,
                           EndCoordinate = BeginCoordinate,
                           EndIndex = BeginIndex,
                           LeftInterrupt = RightInterrupt,
                           RightInterrupt = LeftInterrupt
                       };
        }
    }
}
