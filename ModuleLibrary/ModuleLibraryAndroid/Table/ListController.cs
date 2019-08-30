using System;
using Android.App;
using Android.Views;
using Android.Widget;

namespace ModuleLibraryAndroid.Table
{
    public class ListController
    {
        public ListController()
        {
        }

        public static ListView StartListView<Value>(IListActivity<Value> activity) {
			activity.SetContentView(Resource.Layout.ListViewTemplate);
			var lv = activity.FindViewById<ListView>(Resource.Id.ListView);
            lv.Adapter = new ListAdapterController<Value>(activity);
            lv.ItemClick += activity.OnListItemCell;
            return lv;
        }

        public static View BuildCell(Activity activity, View convertView, int resource) {
			View view = convertView;
			if (view == null)
				view = activity.LayoutInflater.Inflate(resource, null);
            return view;
        }
    }
}
