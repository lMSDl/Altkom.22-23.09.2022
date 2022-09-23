using System.ComponentModel;

namespace Models
{
    public abstract class Entity : INotifyPropertyChanged/*, INotifyPropertyChanging*/
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}