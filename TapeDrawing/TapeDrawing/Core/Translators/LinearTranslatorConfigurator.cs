
namespace TapeDrawing.Core.Translators
{
    public class LinearTranslatorConfigurator
    {
        public static LinearTranslatorConfigurator Create()
        {
            return new LinearTranslatorConfigurator
                       {
                           _current = new LinearTranslator()
                       };
        }

        private ILinearTranslator _current;

        public ILinearTranslator Translator
        {
            get { return _current; }
        }

        public LinearTranslatorConfigurator Inverce()
        {
            _current = new InverseLinearTranslatorDecorator {Internal = _current};
            return this;
        }

        public LinearTranslatorConfigurator Shift(float shiftValue)
        {
            _current = new ShiftLinearTranslatorDecorator { Internal = _current,Shift = shiftValue};
            return this;
        }

        public LinearTranslatorConfigurator Multiply(float multiplyValue)
        {
            _current = new MultiplyLinearTranslatorDecorator { Internal = _current, MultiplyValue = multiplyValue };
            return this;
        }
    }
}
