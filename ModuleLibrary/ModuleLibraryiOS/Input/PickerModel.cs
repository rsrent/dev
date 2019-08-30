using System;
using UIKit;

namespace ModuleLibraryiOS.Input
{
	public class PickerModel : UIPickerViewModel
	{
		string[][] Items;

		public PickerModel(string[][] items)
		{
			Items = items;
		}

		public override nint GetComponentCount(UIPickerView picker)
		{
			return Items.Length;
		}

		public override nint GetRowsInComponent(UIPickerView picker, nint component)
		{
			return Items[component].Length;
		}

		public override string GetTitle(UIPickerView picker, nint row, nint component)
		{
			return Items[component][row];
		}
	}
}
