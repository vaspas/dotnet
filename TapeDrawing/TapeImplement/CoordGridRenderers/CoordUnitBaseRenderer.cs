namespace TapeImplement.CoordGridRenderers
{
    public abstract class CoordUnitBaseRenderer : BaseCoordGridRenderer
    {
        /// <summary>
        /// Минимальное расстояние между обозначениями в пикселах, а также расстояние до отметок прерываний
        /// </summary>
        public int MinPixelsDistance { get; set; }
        /// <summary>
        /// Маска со значениями в пределах (0:1], например если указано {0,2;0,5;1} 
        /// то отображаться могут значения …1, 2,5,10,20,50,100,200,500,1000,2000…
        /// </summary>
        public float[] Mask { get; set; }
        /// <summary>
        /// Список более приоритетных рендереров
        /// </summary>
        public CoordUnitBaseRenderer[] PriorityRenderers { get; set; }
    }
}
