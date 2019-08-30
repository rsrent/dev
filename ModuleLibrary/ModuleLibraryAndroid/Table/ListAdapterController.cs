using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace ModuleLibraryAndroid.Table
{
    public class ListAdapterController<Value> : BaseAdapter<Value>
    {
		IListActivity<Value> controller;
		public ListAdapterController(IListActivity<Value> controller) : base()
        {
			this.controller = controller;
		}

		public override Value this[int position] => controller.Items()[position];

		public override int Count => controller.Items().Count;

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			return controller.GetCell(position, convertView, parent);
		}
    }

	public abstract class IListActivity<Value> : Activity
	{
		public abstract View GetCell(int position, View convertView, ViewGroup parent);
        public abstract List<Value> Items();
        public abstract void OnListItemCell(object sender, AdapterView.ItemClickEventArgs e);
	}
}
