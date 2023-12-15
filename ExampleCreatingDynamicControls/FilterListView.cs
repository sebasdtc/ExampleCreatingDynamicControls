using ExampleCreatingDynamicControls.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExampleCreatingDynamicControls;
public class FilterListView
{
    public ListView ListView { get; set; }

    private ObservableCollection<Grid> grids = new();
    public List<Filter> Filters { get; set; } = new();


    public FilterListView()
    {
        InitilizeListView();
        ListView.ItemsSource = grids;
    }
    public FilterListView(List<Filter> filters) : this()
    {
        Filters = filters;
        InitializeGridControls();
    }

    private void InitilizeListView()
    {
        ListView = new ListView
        {
            SelectionMode = ListViewSelectionMode.None
        };
    }

    public void AddItem(Filter filter)
    {
        var grid = CreateControls(filter);
        Filters.Add(filter);// Levar el registro de los filter creados
        grids.Add(grid);
    }
    private void InitializeGridControls()
    {
        foreach(var item in Filters)
        {
            var grid = CreateControls(item);
            grids.Add(grid);
        }
    }

    private Grid CreateControls(Filter filter)
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // Creacion de los ComboBox
        var comboBoxFilter = new ComboBox() { Width = 140, Margin = new Thickness(5), Tag = filter };
        var comboBoxOption = new ComboBox() { Width = 140, Margin = new Thickness(5), Tag = filter };

        // Creacion del Button
        var button = new Button { Content = new FontIcon { Glyph = "\uE74D", FontSize = 14 }, Margin = new Thickness(5), Height = 32, Tag = filter };

        // Add control grid
        // Agregamos los controles al grid
        grid.Children.Add(comboBoxFilter);
        grid.Children.Add(comboBoxOption);
        grid.Children.Add(button);

        // Establecemos las columnas al grid
        Grid.SetColumn(comboBoxFilter, 0);
        Grid.SetColumn(comboBoxOption, 1);
        Grid.SetColumn(button, 3);

        DynamicControlCreation(grid, filter);

        // Asociamos las listas a los ComboBox
        comboBoxFilter.ItemsSource = filter.FilterTypeList;
        comboBoxOption.ItemsSource = filter.OptionsList;

        // Asignamos los elementos seleccionados
        comboBoxFilter.SelectedItem = filter.TypeFilter;
        comboBoxOption.SelectedItem = filter.OptionFilter;

        // asingamos los eventos
        comboBoxFilter.SelectionChanged += ComboBoxFilter_SelectionChanged;
        comboBoxOption.SelectionChanged += ComboBoxOption_SelectionChanged;
        button.Click += Button_Click;

        return grid;
    }

    private void DynamicControlCreation(Grid grid, Filter filter)
    {
        var typeControl = filter.TypeControl;
        // Agregamos el control especial
        if(typeControl == typeof(TextBox))
        {
            var textBox = CreateTextBox(filter);
            grid.Children.Add(textBox);
            Grid.SetColumn(textBox, 2);
        }
        else if(typeControl == typeof(CalendarDatePicker))
        {
            var calendarDatePicker = CreateCalendarDatePicker(filter);
            grid.Children.Add(calendarDatePicker);
            Grid.SetColumn(calendarDatePicker, 2);
        }
        else if(typeControl == typeof(NumberBox))
        {
            var numberBox = CreateNumberBox(filter);
            grid.Children.Add(numberBox);
            Grid.SetColumn(numberBox, 2);
        }
        else
        {
            throw new InvalidOperationException($"Tipo de control no compatible: {typeControl.GetType().Name}");
        }
    }

    #region EVENTS
    private void ComboBoxOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var filter = (sender as ComboBox).Tag as Filter;
        filter.OptionFilter = (sender as ComboBox).SelectedItem as string;

        if(filter.OptionFilter != null)
        {
            var grid = (sender as ComboBox).Parent as Grid;
            UpdateDynamicControl(grid, filter);
        }
        else
        {
            (sender as ComboBox).SelectedIndex = 0;
        }
    }
    private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var filter = (sender as ComboBox).Tag as Filter;
        filter.TypeFilter = (sender as ComboBox).SelectedItem as string;
        filter.UpdateOptions();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var filter = (sender as Button).Tag as Filter;
        var grid = (sender as Button).Parent as Grid;
        grids.Remove(grid);
        Filters.Remove(filter);
    }
    #endregion

    #region CREACIOND DE CONTROLES
    private TextBox CreateTextBox(Filter filter)
    {
        var textBox = new TextBox
        {
            Width = 300,
            Margin = new Thickness(5),
            Text = filter.Value,
            PlaceholderText = filter.PlaceHolderText
        };

        if(filter.OptionFilter == "today")
        {
            textBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(filter.OptionFilter == "yesterday")
        {
            textBox.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(filter.OptionFilter == "this month")
        {
            textBox.Text = DateTime.Now.ToString("MMMM");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(filter.OptionFilter == "this year")
        {
            textBox.Text = DateTime.Now.ToString("yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }

        return textBox;
    }
    private CalendarDatePicker CreateCalendarDatePicker(Filter filter)
    {
        var calendarDatePicker = new CalendarDatePicker
        {
            Width = 300,
            Margin = new Thickness(5),
            PlaceholderText = filter.PlaceHolderText
        };

        if(DateTime.TryParse(filter.Value, out DateTime dateTime))
        {
            calendarDatePicker.Date = dateTime;
            return calendarDatePicker;
        }
        if(filter.Value != string.Empty)
        {
            throw new InvalidOperationException($"No se pudo convertir el {filter.Value} en un tipo DateTime");
        }

        return calendarDatePicker;
    }
    private NumberBox CreateNumberBox(Filter filter)
    {
        var numberBox = new NumberBox
        {
            Width = 300,
            Margin = new Thickness(5),
            PlaceholderText = filter.PlaceHolderText,
            SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact
        };


        if(Double.TryParse(filter.Value, out double number))
        {
            numberBox.Value = number;
        }
        return numberBox;
    }
    #endregion


    private void UpdateDynamicControl(Grid grid, Filter filter)
    {
        grid.Children.RemoveAt(3);
        filter.Update();
        DynamicControlCreation(grid, filter);
    }
}
