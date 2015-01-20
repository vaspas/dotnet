using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace ComparativeTest2.Ui
{
	/*
	 * В этом файле описаны классы, которые позволяют представлять флаги в PropertyGrid
	 */

	public class FlagCheckedListBox : CheckedListBox
	{
		public FlagCheckedListBox()
		{
			InitializeComponent();
		}

		#region Component Designer generated code

		private void InitializeComponent()
		{
			// 
			// FlaggedCheckedListBox
			// 
			this.CheckOnClick = true;
		}

		#endregion

		// Adds an integer value and its associated description
		public FlagCheckedListBoxItem Add(int v, string c)
		{
			var item = new FlagCheckedListBoxItem(v, c);
			Items.Add(item);
			return item;
		}

		public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
		{
			Items.Add(item);
			return item;
		}

		protected override void OnItemCheck(ItemCheckEventArgs e)
		{
			base.OnItemCheck(e);

			if (_isUpdatingCheckStates)
				return;

			// Get the checked/unchecked item
			var item = Items[e.Index] as FlagCheckedListBoxItem;
			// Update other items
			UpdateCheckedItems(item, e.NewValue);
		}

		// Checks/Unchecks items depending on the give bitvalue
		protected void UpdateCheckedItems(int value)
		{
			// ReSharper disable PossibleNullReferenceExceptio
			_isUpdatingCheckStates = true;

			// Iterate over all items
			for (int i = 0; i < Items.Count; i++)
			{
				var item = Items[i] as FlagCheckedListBoxItem;

				if (item.Value == 0)
				{
					SetItemChecked(i, value == 0);
				}
				else
				{
					// If the bit for the current item is on in the bitvalue, check it
					if ((item.Value & value) == item.Value && item.Value != 0)
						SetItemChecked(i, true);
						// Otherwise uncheck it
					else SetItemChecked(i, false);
				}
			}

			_isUpdatingCheckStates = false;
			// ReSharper restore PossibleNullReferenceException
		}

		// Updates items in the checklistbox
		// composite = The item that was checked/unchecked
		// cs = The check state of that item
		protected void UpdateCheckedItems(FlagCheckedListBoxItem composite, CheckState cs)
		{
			// If the value of the item is 0, call directly.
			if (composite.Value == 0)
				UpdateCheckedItems(0);

			// Get the total value of all checked items
			int sum = 0;
			for (int i = 0; i < Items.Count; i++)
			{
				var item = Items[i] as FlagCheckedListBoxItem;

				// If item is checked, add its value to the sum.
				if (GetItemChecked(i))
					sum |= item.Value;
			}

			// If the item has been unchecked, remove its bits from the sum
			// If the item has been checked, combine its bits with the sum
			if (cs == CheckState.Unchecked) sum = sum & (~composite.Value);
			else sum |= composite.Value;

			// Update all items in the checklistbox based on the final bit value
			UpdateCheckedItems(sum);
		}

		private bool _isUpdatingCheckStates;

		// Gets the current bit value corresponding to all checked items
		public int GetCurrentValue()
		{
			int sum = 0;

			for (int i = 0; i < Items.Count; i++)
			{
				var item = Items[i] as FlagCheckedListBoxItem;

				if (GetItemChecked(i))
					sum |= item.Value;
			}

			return sum;
		}

		private Type _enumType;
		private Enum _enumValue;

		// Adds items to the checklistbox based on the members of the enum
		private void FillEnumMembers()
		{
			foreach (string name in Enum.GetNames(_enumType))
			{
				object val = Enum.Parse(_enumType, name);
				int intVal = (int) Convert.ChangeType(val, typeof (int));

				Add(intVal, name);
			}
		}

		// Checks/unchecks items based on the current value of the enum variable
		private void ApplyEnumValue()
		{
			int intVal = (int) Convert.ChangeType(_enumValue, typeof (int));
			UpdateCheckedItems(intVal);

		}

		[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
		public Enum EnumValue
		{
			get
			{
				object e = Enum.ToObject(_enumType, GetCurrentValue());
				return (Enum) e;
			}
			set
			{

				Items.Clear();
				_enumValue = value; // Store the current enum value
				_enumType = value.GetType(); // Store enum type
				FillEnumMembers(); // Add items for enum members
				ApplyEnumValue(); // Check/uncheck items depending on enum value

			}
		}


	}

	/// <summary>
	/// Represents an item in the checklistbox
	/// </summary>
	public class FlagCheckedListBoxItem
	{
		public FlagCheckedListBoxItem(int v, string c)
		{
			Value = v;
			Caption = c;
		}

		public override string ToString()
		{
			return Caption;
		}

		/// <summary>
		/// Returns true if the value corresponds to a single bit being set
		/// </summary>
		public bool IsFlag
		{
			get { return ((Value & (Value - 1)) == 0); }
		}

		/// <summary>
		/// Returns true if this value is a member of the composite bit value
		/// </summary>
		/// <param name="composite"></param>
		/// <returns></returns>
		public bool IsMemberFlag(FlagCheckedListBoxItem composite)
		{
			return (IsFlag && ((Value & composite.Value) == Value));
		}

		public int Value;
		public string Caption;
	}


	// UITypeEditor for flag enums
	public class FlagEnumUiEditor : UITypeEditor
	{
		// The checklistbox
		private readonly FlagCheckedListBox _flagEnumCb;

		public FlagEnumUiEditor()
		{
			_flagEnumCb = new FlagCheckedListBox {BorderStyle = BorderStyle.None};
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable AssignNullToNotNullAttribute
			if (context != null && context.Instance != null && provider != null)
			{
				var edSvc = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));

				if (edSvc != null)
				{
					var e = (Enum) Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
					_flagEnumCb.EnumValue = e;
					edSvc.DropDownControl(_flagEnumCb);
					return _flagEnumCb.EnumValue;
				}
			}

			return null;
			// ReSharper restore AssignNullToNotNullAttribute
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}


	}

}
