using Microsoft.EntityFrameworkCore.Infrastructure;
using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;

namespace Models
{
    public class Order : Entity, IUpdated
    {
        private ILazyLoader _lazyLoader;
        private IList<Product> products = new ObservableCollection<Product>();

        public Order(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public Order()
        {
        }

        public DateTime DateTime { get; set; }
        //public DateTime Created { get; }

        public DateTime Updated { get; set; }

        public virtual IList<Product> Products
        {
            get => _lazyLoader?.Load(this, ref products) ?? products;
            set => products = value;
        }
        //Konfigurujemy token współbieżności za pomocą sygnatury czasowej
        //[Timestamp]
        public byte[] Timestamp { get; set; }

        public Point? DeliveryPoint { get; set; }
    }
}
