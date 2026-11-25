using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            viewModel.ConfirmDelete = item =>
            {
                var answer = MessageBox.Show(
                    "Kas soovid õllesordi \"" + item.Name + "\" kustutada?",
                    "Kustutamine",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                return answer == MessageBoxResult.Yes;
            };
            viewModel.OnError = error =>
            {
                MessageBox.Show(error, "Viga", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DataContext = viewModel;

            Loaded += async (sender, e) => await viewModel.Load();
        }
    }
}
