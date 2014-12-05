using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cet.UI.Chart
{
    public sealed class ChartAxisCollection
        : ObservableCollection<ChartAxisBase>
    {
        public ChartAxisCollection(ChartModelBase owner)
        {
            this.Owner = owner;
        }


        public ChartModelBase Owner { get; private set; }


        protected override void ClearItems()
        {
            foreach (var child in this.Items)
            {
                child.Owner = null;
            }

            base.ClearItems();
        }


        protected override void InsertItem(int index, ChartAxisBase item)
        {
            base.InsertItem(index, item);
            item.Owner = this.Owner;
        }


        protected override void RemoveItem(int index)
        {
            var child = this[index];
            child.Owner = null;
            base.RemoveItem(index);
        }


        protected override void SetItem(int index, ChartAxisBase item)
        {
            var child = this[index];
            child.Owner = null;
            base.SetItem(index, item);
            item.Owner = this.Owner;
        }

    }
}
