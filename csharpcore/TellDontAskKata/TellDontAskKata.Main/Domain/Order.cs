using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public string Currency { get; set; }
        public ObservableCollection<OrderItem> Items { get; private set; } = new ObservableCollection<OrderItem>();
        public OrderStatus Status { get; set; }
        public int Id { get; set; }

        public Order()
        {
            Items.CollectionChanged += HandleChange;
        }

        private void HandleChange(object s, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                e.NewItems.Cast<OrderItem>().ToList().ForEach(item =>
                {
                    Total += item.TaxedAmount;
                    Tax += item.Tax;
                });
            }
        }
    }
}
