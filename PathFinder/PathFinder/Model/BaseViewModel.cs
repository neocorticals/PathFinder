namespace PathFinder
{
    public class BaseViewModel : BindableBase
    {  
        private string _ImageName;
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (_ImageName != value)
                {
                    _ImageName = value;
                    OnPropertyChanged("ImageName");
                }
            }
        } 
    }
}
