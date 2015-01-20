
using Sigflow.Dataflow;
using Sigflow.Module;

namespace IppModules.Generator
{
    /// <remarks>
    /// INSPECTED 01/05/2013
    /// </remarks>
    public abstract class CreateImpulseModule<T>:IExecuteModule, IMasterModule
            where T : struct
    {

        /// <summary>
        /// Количество отсчетов для импульса.
        /// </summary>
        public int DataSamplesCount { get; set; }

        /// <summary>
        /// Количество отсчетов для паузы между импульсами.
        /// </summary>
        public int NullSamplesCount { get; set; }

        /// <summary>
        /// Задержка старта.
        /// </summary>
        public int StartDelaySamplesCount { get; set; }

        protected T[] Data;

        public T NullValue { get; set; }

        public ISignalReader<T> In;
        public ISignalWriter<T> Out;

        /// <summary>
        /// Счетчик отсчетов.
        /// </summary>
        private int _samplesCnt;

        /// <summary>
        /// Флаг указывает на то, что сейчас генерируется сигнал, а не пауза.
        /// </summary>
        private bool _impulse;


        /// <summary>
        /// Устанавливает значения в блоке.
        /// </summary>
        /// <param name="fromInd">От.</param>
        /// <param name="len">Длина.</param>
        protected abstract void SetValue(int fromInd, int len);

        public bool Start()
        {
            _impulse = false;
            _samplesCnt = StartDelaySamplesCount;
            
            return true;
        }

        public void BeforeStop()
        {
        }
        public void AfterStop()
        {
        }

        /// <summary>
        /// Вырезает из сигнала данные, для того чтобы получить паузу между импульсами.
        /// </summary>
        public bool? Execute()
        {
            if (!In.NextBlockSize.HasValue)
                return false;

            if (Data == null || Data.Length != In.NextBlockSize.Value)
                Data = new T[In.NextBlockSize.Value];

            if(!In.ReadTo(Data))
                return false;
                
            DoWork();
            Out.Write(Data);
            
            return true;
        }

        private void DoWork()
        {
            //счетчик текущего отсчета блока
            int cnt = 0;
            //цикл пока не обработаем весь блок
            while (cnt < Data.Length)
            {
                //длина необработанной части блока (всегда конечная часть блока)
                int len = Data.Length - cnt;

                if (_impulse)
                {
                    //случай когда идет импульс
                    if (_samplesCnt - len > 0)
                    {
                        //импульс полность накрывает отстаток блока, оставляем без изменений
                        //смещаем счетчик импульса
                        _samplesCnt -= len;
                        //смещаем счетчик
                        cnt += len;
                    }
                    else
                    {
                        //импульс накрывает часть блока
                        int lenImp = _samplesCnt;

                        //переходим в паузу
                        _impulse = false;
                        //устанавливаем счетчик паузы
                        _samplesCnt = NullSamplesCount;
                        //смещаем счетчик
                        cnt += lenImp;
                    }
                }
                else
                {
                    //случай когда идет пауза
                    if (_samplesCnt - len > 0)
                    {
                        //пауза полность накрывает отстаток блока

                        //обнуляем данные
                        SetValue(cnt, len);
                        //смещаем счетчик паузы
                        _samplesCnt -= len;
                        //смещаем счетчик
                        cnt += len;
                    }
                    else
                    {
                        //пауза накрывает только часть блока
                        int lenPause = _samplesCnt;

                        //обнуляем данные
                        SetValue(cnt, lenPause);

                        //переходим в импульс
                        _impulse = true;
                        //устанавливаем счетчик импульса
                        _samplesCnt = DataSamplesCount;
                        //смещаем счетчик
                        cnt += lenPause;
                    }
                }
            }
        }
    }

    public class CreateImpulseModuleFloat : CreateImpulseModule<float>
    {
        protected override unsafe void SetValue(int fromInd, int len)
        {
            fixed (float* pBlock = Data)
                ipp.sp.ippsSet_32f(NullValue, pBlock + fromInd, len);
        }
    }
}
