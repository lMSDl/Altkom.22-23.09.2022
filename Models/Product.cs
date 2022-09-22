using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product : Entity
    {
        private float price;

        public string Name { get; set; } = string.Empty;
        public float Price
        {
            get => price; set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        //public IEnumerable<Order>? Orders { get; set; }
    }
}