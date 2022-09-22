using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : Entity
    {
        public DateTime DateTime { get; set; }
        public IList<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}
