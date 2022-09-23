using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product : Entity
    {
        private float price;

        //Token współbieżności
        //[ConcurrencyCheck]
        public string Name { get; set; } = string.Empty;
        public  float Price
        {
            get => price; set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public string Description { get; }
        public string Desc => $"{Name} {Price}zł";

        public virtual IEnumerable<Order>? Orders { get; set; }
    }
}