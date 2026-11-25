using System.Collections.ObjectModel;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private BeerType? _selectedItem;

        public ObservableCollection<BeerType> Lists { get; } = new ObservableCollection<BeerType>();

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        // Küsib kasutajalt kustutamise kinnitust (MainWindow seob selle MessageBoxiga)
        public Predicate<BeerType>? ConfirmDelete { get; set; }

        // Näitab veateadet (MainWindow seob selle MessageBoxiga)
        public Action<string>? OnError { get; set; }

        public MainWindowViewModel() : this(new ApiClient())
        {
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;

            NewCommand = new RelayCommand<BeerType>(
                _ => AddNew()
            );

            SaveCommand = new RelayCommand<BeerType>(
                async _ => await Save(),
                _ => SelectedItem != null
            );

            DeleteCommand = new RelayCommand<BeerType>(
                async _ => await Delete(),
                _ => SelectedItem != null && SelectedItem.Id != 0
            );
        }

        public BeerType? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        // Uus kirje lisatakse kohe tabelisse ja valitakse, et vorm oleks tühi
        public void AddNew()
        {
            var item = new BeerType();
            Lists.Add(item);
            SelectedItem = item;
        }

        public async Task Load()
        {
            Lists.Clear();

            var result = await _apiClient.List();
            if (result.HasError)
            {
                OnError?.Invoke(result.Error!);
                return;
            }

            foreach (var item in result.Value ?? new List<BeerType>())
            {
                Lists.Add(item);
            }
        }

        public async Task Save()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var result = await _apiClient.Save(SelectedItem);
            if (result.HasError)
            {
                OnError?.Invoke(result.Error!);
                return;
            }

            await Load();
        }

        public async Task Delete()
        {
            if (SelectedItem == null)
            {
                return;
            }

            if (ConfirmDelete != null && !ConfirmDelete(SelectedItem))
            {
                return;
            }

            var result = await _apiClient.Delete(SelectedItem.Id);
            if (result.HasError)
            {
                OnError?.Invoke(result.Error!);
                return;
            }

            SelectedItem = null;
            await Load();
        }
    }
}
