using System;
using System.Collections.Generic;
using UIKit;

namespace FlashCardsPort.iOS
{
	class RepeatViewModal : UIPickerViewModel
	{
		List<string> repeat;

		public event EventHandler repeatChanged;

		public string SelectedNum { get; private set; }

		public RepeatViewModal(List<string> repeat)
		{
			this.repeat = repeat;
		}

		public override System.nint GetRowsInComponent(UIPickerView pickerView, System.nint component)
		{
			return repeat.Count;
		}

		public override System.nint GetComponentCount(UIPickerView pickerView)
		{
			return 1;
		}

		public override string GetTitle(UIPickerView pickerView, nint row, nint component)
		{
			return repeat[(int)row];
		}

		public override void Selected(UIPickerView pickerView, nint row, nint component)
		{
			var num = repeat[(int)row];

			SelectedNum = num;

			repeatChanged?.Invoke(null, null);

		}
	}
}