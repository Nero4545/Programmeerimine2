namespace KooliProjekt.WpfApp.Api
{
    public class BeerType : NotifyPropertyChangedBase
    {
        private int _id;
        private string _name = string.Empty;
        private string? _description;

        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged(); }
        }

        public string? Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged(); }
        }
    }
}
