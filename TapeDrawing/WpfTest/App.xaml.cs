using System.Windows;

namespace WpfTest
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создадим параметры теста
            var parametes = new TestParameters {Difficult = 0};

            // Создадим главное окно
            var window = new MainWindow {TestParameters = parametes};

            // Создадим модель представления
            var viewModel = new TestParametersViewModel { TestParameters = parametes };

            window.DataContext = viewModel;

            window.Show();
        }
	}
}
