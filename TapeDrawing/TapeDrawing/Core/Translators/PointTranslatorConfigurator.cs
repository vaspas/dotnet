
namespace TapeDrawing.Core.Translators
{
    public class PointTranslatorConfigurator
    {
        public static PointTranslatorConfigurator CreateLinear()
        {
            return new PointTranslatorConfigurator
                       {
                           _current = new PointTranslator
                                          {
                                              TranslatorX = new LinearTranslator(),
                                              TranslatorY = new LinearTranslator()
                                          }
                       };
        }

        public static PointTranslatorConfigurator CreateFake()
        {
            return new PointTranslatorConfigurator
            {
                _current = new PointTranslator
                {
                    TranslatorX = new FakeLinearTranslator(),
                    TranslatorY = new FakeLinearTranslator()
                }
            };
        }

        private IPointTranslatorInternal _current;

        public IPointTranslator Translator
        {
            get { return _current; }
        }

        public PointTranslatorConfigurator MirrorX()
        {
            _current.TranslatorX =
                new InverseLinearTranslatorDecorator {Internal = _current.TranslatorX};
            return this;
        }

        public PointTranslatorConfigurator MirrorY()
        {
            _current.TranslatorY =
                new InverseLinearTranslatorDecorator { Internal = _current.TranslatorY };
            return this;
        }

        public PointTranslatorConfigurator ShiftX(float shiftValue)
        {
            _current.TranslatorX =
                new ShiftLinearTranslatorDecorator { Internal = _current.TranslatorX, Shift = shiftValue };
            return this;
        }

        public PointTranslatorConfigurator ShiftY(float shiftValue)
        {
            _current.TranslatorY =
                new ShiftLinearTranslatorDecorator { Internal = _current.TranslatorY, Shift = shiftValue };
            return this;
        }

        public PointTranslatorConfigurator MultiplyX(float multiplyValue)
        {
            _current.TranslatorX =
                new MultiplyLinearTranslatorDecorator { Internal = _current.TranslatorX, MultiplyValue = multiplyValue};
            return this;
        }

        public PointTranslatorConfigurator MultiplyY(float multiplyValue)
        {
            _current.TranslatorY =
                new MultiplyLinearTranslatorDecorator { Internal = _current.TranslatorY, MultiplyValue = multiplyValue };
            return this;
        }

        public PointTranslatorConfigurator ChangeAxels()
        {
            _current = new ChangeAxelsPointTranslatorDecorator
                           {
                               Internal = _current
                           };
            return this;
        }
    }
}
