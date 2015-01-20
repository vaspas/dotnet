using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace WpfTest
{
    /// <summary>
    /// Базовый класс модели представления
    /// - Поддерживает уведомления об изменении свойств INotifyPropertyChanged
    /// - Проверяет имена свойств на правильность
    /// - Позволяет вызывать событие без указания имени свойства, например OnPropertyChanged(() => PropName);
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region - Конструктор -
        /// <summary>
        /// Закрытый конструктор
        /// </summary>
        protected ViewModelBase()
        {
            ThrowOnInvalidPropertyName = true;
        }
        #endregion // Конструктор

        #region - Открытые свойства -
        /// <summary>
        /// Это название объекта, которое будет отображаться в пользовательском интерфейсе
        /// Может быть переопределено при наследовании
        /// </summary>
        public virtual string DisplayName { get; protected set; }
        #endregion // Открытые свойства

        #region - Для отладки -
        /// <summary>
        /// Метод проверяет, что у объекта существует указанное
        /// открытое свойство
        /// Метод работает только в режиме отладки
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName) throw new Exception(msg);
                Debug.Fail(msg);
            }
        }
        /// <summary>
        /// Свойство определяет что нежно делать если обнаружено свойство, которое отсутствует 
        /// в объекте
        /// Если свойство true, то генерируется ислючение, если false то генерируется Debug.Fail()
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }
        #endregion // Для отладки

        #region - Реализация INotifyPropertyChanged -
        /// <summary>
        /// Вызывается когда изменилось некоторое свойство объекта
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // Проверим свойство на правильность
            VerifyPropertyName(propertyName);

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(Expression<Func<object>> expression)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(GetPropertyName(expression)));
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T, object>> expression)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(GetPropertyName(expression)));
        }
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            var propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo.Name;
        }
        private static string GetPropertyName(Expression<Func<object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            var propertyInfo = (PropertyInfo)memberExpression.Member;
            return propertyInfo.Name;
        }
        #endregion // Реализация INotifyPropertyChanged

        #region - Реализация IDisposable -
        /// <summary>
        /// Вызывается перед тем, как сборщик мусора уничтожает объект
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }
        /// <summary>
        /// Наследованные классы переопределяют этот метод для выполнения очистки ресурсов
        /// перед удалением объекта
        /// </summary>
        protected virtual void OnDispose()
        {
        }
        #if DEBUG
        /// <summary>
        /// Деструктор вызывается для того, чтобы убедиться, что все объекты ViewModel нормально освободили
        /// свои ресурсы
        /// </summary>
        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode());
            Debug.WriteLine(msg);
        }
        #endif
        #endregion // Реализация IDisposable
    }
}
