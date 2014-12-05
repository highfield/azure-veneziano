using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cet.UI.Chart
{
    public sealed class ChartPlotCollection
        : ObservableCollection<ChartPlotBase>
    {
        public ChartPlotCollection(ChartModelBase owner)
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


        protected override void InsertItem(int index, ChartPlotBase item)
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


        protected override void SetItem(int index, ChartPlotBase item)
        {
            var child = this[index];
            child.Owner = null;
            base.SetItem(index, item);
            item.Owner = this.Owner;
        }

    }
}
