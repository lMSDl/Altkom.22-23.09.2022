using System.ComponentModel;
using System.Diagnostics;

namespace Models
{
    public abstract class Entity : INotifyPropertyChanged/*, INotifyPropertyChanging*/
    {
        public int Id { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
}
}