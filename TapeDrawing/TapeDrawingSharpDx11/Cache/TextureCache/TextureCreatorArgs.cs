namespace TapeDrawingSharpDx11.Cache.TextureCache
{
    /// <summary>
    /// Аргументы для создания текстуры
    /// </summary>
    class TextureCreatorArgs
    {
        /// <summary>
        /// Объект, из которого создаются текстуры
        /// </summary>
        public object Source { get; set; }
        /// <summary>
        /// Ширина загруженной текстуры
        /// </summary>
        public float Width { get; set; }
        /// <summary>
        /// Высота загруженной текстуры
        /// </summary>
        public float Height { get; set; }
    }
}
