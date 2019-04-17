namespace DocCN.Utility
{
    public static class ObservableUtil
    {
        public static Observable<string> currentPath { get; set; }

        static ObservableUtil()
        {
            currentPath = new Observable<string>($"{Configuration.Instance.pageBase}/");
        }
    }

    public class Observable<T>
    {
        public Observable(T value)
        {
            _value = value;
        }

        private T _value;

        public T value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public delegate void OnValueChanged(T newValue);

        public OnValueChanged OnChanged;
    }
}