using System;
using System.Collections.Generic;
using System.Linq;
using TapeDrawing.Core.Shapes;
using TapeDrawing.Core.Translators;

namespace TapeDrawing.ShapesDecorators
{
    public static class DecoratorsFactory
    {
        static DecoratorsFactory()
        {
            Decorators = new List<Type>
                              {
                                  typeof (DrawRectangleShapeDecorator),
                                  typeof (DrawRectangleAreaShapeDecorator),
                                  typeof (FillRectangleShapeDecorator),
                                  typeof (FillRectangleAreaShapeDecorator),
                                  typeof (TextShapeDecorator),
                                  typeof (LinesShapeDecorator),
                                  typeof (PolygonShapeDecorator),
                                  typeof (ImageShapeDecorator),
                                  typeof (AlignmentTranslatorShapesFactoryDecorator),
                                  typeof (PointTranslatorShapesFactoryDecorator)
                              };
        }

        public static void Register(Type t)
        {
            if (Decorators.Contains(t))
                return;

            Decorators.Add(t);
        }

        private static readonly List<Type> Decorators=new List<Type>();

        public static T CreateFor<T, TTranslator>(T target, TTranslator translator) where T : class
        {
            var type = Decorators.FirstOrDefault(
                t => 
                    t.GetInterfaces().Contains(typeof (IDecorator<T>))
                    && t.GetInterfaces().Contains(typeof(ITranslatorDecorator<TTranslator>)));
            if(type==null)
                return target;

            var result = Activator.CreateInstance(type);
            (result as ITranslatorDecorator<TTranslator>).Translator = translator;
            (result as IDecorator<T>).Target = target;

            return result as T;
        }
    }
}
