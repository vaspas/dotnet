
namespace TapeDrawing.Core.Translators
{
    public class AlignmentTranslatorConfigurator
    {
        public static AlignmentTranslatorConfigurator Create()
        {
            return new AlignmentTranslatorConfigurator
                       {
                           _current = new FakeAlignmentTranslator()
                       };
        }

        private IAlignmentTranslator _current;

        public IAlignmentTranslator Translator
        {
            get { return _current; }
        }

        public AlignmentTranslatorConfigurator MirrorX()
        {
            _current =
                new MirrorXAlignmentTranslatorDecorator {Internal = _current};
            return this;
        }

        public AlignmentTranslatorConfigurator MirrorY()
        {
            _current =
                new MirrorYAlignmentTranslatorDecorator { Internal = _current };
            return this;
        }

        public AlignmentTranslatorConfigurator ChangeAxels()
        {
            _current = new ChangeAxelsAlignmentTranslatorDecorator
                           {
                               Internal = _current
                           };
            return this;
        }
    }
}
