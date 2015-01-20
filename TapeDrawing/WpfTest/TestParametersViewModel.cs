using System.ComponentModel;

namespace WpfTest
{
    class TestParametersViewModel : ViewModelBase, IDataErrorInfo
    {
        public TestParameters TestParameters { get; set; }

        /// <summary>
        /// Сложность
        /// </summary>
        public int Difficult
        {
            get { return TestParameters.Difficult; }
            set
            {
                if (value == TestParameters.Difficult) return;
                TestParameters.Difficult = value;
                OnPropertyChanged("Difficult");
            }
        }

        #region Implementation of IDataErrorInfo

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "Difficult":
                        if (Difficult < 0 || Difficult > 10) return "Сложность должна быть от 0 до 10";
                        return null;
                }
                return null;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion
    }
}
