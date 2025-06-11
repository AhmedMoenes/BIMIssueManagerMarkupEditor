using System.Collections;
using HandyControl.Controls;

public static class CheckComboBoxBehavior
{
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(CheckComboBoxBehavior),
            new PropertyMetadata(null, OnSelectedItemsChanged));

    public static void SetSelectedItems(DependencyObject element, IList value)
    {   
        element.SetValue(SelectedItemsProperty, value);
    }

    public static IList GetSelectedItems(DependencyObject element)
    {
        return (IList)element.GetValue(SelectedItemsProperty);
    }

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CheckComboBox comboBox)
        {
            comboBox.SelectionChanged -= ComboBoxSelectedItemsChanged;
            comboBox.SelectionChanged += ComboBoxSelectedItemsChanged;

            if (e.NewValue is IList newSelectedItems)
            {
                comboBox.SelectedItems.Clear();
                foreach (var item in newSelectedItems)
                {
                    comboBox.SelectedItems.Add(item);
                }
            }
        }
    }


    private static void ComboBoxSelectedItemsChanged(object sender, RoutedEventArgs e)
    {
        var comboBox = (CheckComboBox)sender;
        var selectedItems = GetSelectedItems(comboBox);
        selectedItems?.Clear();
        if (comboBox.SelectedItems != null)
        {
            foreach (var item in comboBox.SelectedItems)
                selectedItems?.Add(item);
        }
    }
}