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
        //private byte[] timestamp;
        //private byte[] _timestamp;
        private byte[] m_timestamp;
        public byte[] Timestamp { get => m_timestamp; }

        public Point? DeliveryPoint { get; set; }

        public OrderTypes OrderType { get; set; }
    }
}
