using ExampleCreatingDynamicControls.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExampleCreatingDynamicControls;
public class FilterControl
{
    private string _value;
    private string _selectedFilter;
    private string _selectedOption;
    private string _placeHolderText;
    private Type _typeControl;
    private ComboBox _comboBoxFilter;
    private ComboBox _comboBoxOption;
    private Button _button;
    private Grid _grid;
    private ObservableCollection<string> _optionsList;
    private readonly List<string> _filterTypeList = new()
    {
        "Name",
        "Extension",
        "Size",
        "Path",
        "Date Create",
        "Date Modified",
        "Date Accessed"
    };

    public Grid Gird => _grid;
    private readonly Dictionary<string, Type> _optionsDictionary = new()
    {
        { "greater than", typeof(NumberBox) },
        { "less than", typeof(NumberBox) },
        { "equals", typeof(NumberBox) },
        { "last days", typeof(NumberBox) },
        { "exactly", typeof(CalendarDatePicker) },
        { "before", typeof(CalendarDatePicker) },
        { "after", typeof(CalendarDatePicker) }
    };

    public static ObservableCollection<Grid> Grids = new();

    #region Constructors
    public FilterControl()
    {
        _placeHolderText = "Value";
        _typeControl = typeof(TextBox);
        _value = string.Empty;
        _selectedFilter = _filterTypeList.FirstOrDefault();
        _optionsList = new();
        UpdateOptions();
        _selectedOption = _optionsList.FirstOrDefault();

        InitializeComponents();

        Grids.Add(this.Gird);
    }
    public FilterControl(Filter filter)
    {
        _selectedFilter = filter.TypeFilter;
        _selectedOption = filter.OptionFilter;
        _value = filter.Value;
        _typeControl = filter.TypeControl;
        _optionsList = new();
        UpdateOptions();

        InitializeComponents();

        Grids.Add(this.Gird);
    }
    #endregion

    private void InitializeComponents()
    {
        InitializeControls();
        AddControlsToGrid();
        DynamicControlCreation();
        AssignItemssSource();
        AsssingEvents();
    }

    private void UpdateOptions()
    {
        ObservableCollection<string> filterList;
        if(_selectedFilter.Equals("Size"))
        {
            filterList = new ObservableCollection<string>()
            {
                "greater than",
                "less than",
                "equals"
            };
        }
        else if(_selectedFilter.Contains("Date"))
        {
            filterList = new ObservableCollection<string>()
            {
                "last days",
                "exactly",
                "before",
                "after",
                "today",
                "yesterday",
                "this month",
                "this year"
            };
        }
        else if(_selectedFilter.Equals("Extension"))
        {
            filterList = new ObservableCollection<string>()
            {
                "is"
            };
        }
        else
        {
            filterList = new ObservableCollection<string>()
            {
                "contains",
                "starts with",
                "ends with",
                "is"
            };
        }

        _optionsList.Clear();

        foreach(var item in filterList)
        {
            _optionsList.Add(item);
        }
    }

    private void InitializeControls()
    {
        _grid = new Grid();
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // Creacion de los ComboBox
        _comboBoxFilter = new ComboBox() { Width = 140, Margin = new Thickness(5) };
        _comboBoxOption = new ComboBox() { Width = 140, Margin = new Thickness(5) };

        // Creacion del Button
        _button = new Button { Content = new FontIcon { Glyph = "\uE74D", FontSize = 14 }, Margin = new Thickness(5), Height = 32 };

    }

    private void AssignItemssSource()
    {
        // Asociamos las listas a los ComboBox
        _comboBoxFilter.ItemsSource = _filterTypeList;
        _comboBoxOption.ItemsSource = _optionsList;

        // Asignamos los elementos seleccionados
        _comboBoxFilter.SelectedItem = _selectedFilter;
        _comboBoxOption.SelectedItem = _selectedOption;


    }

    private void AddControlsToGrid()
    {

        // Agregamos los controles al grid
        _grid.Children.Add(_comboBoxFilter);
        _grid.Children.Add(_comboBoxOption);
        _grid.Children.Add(_button);

        // Establecemos las columnas al grid
        Grid.SetColumn(_comboBoxFilter, 0);
        Grid.SetColumn(_comboBoxOption, 1);
        Grid.SetColumn(_button, 3);
    }

    private void AsssingEvents()
    {
        // Asignamos los eventos
        _comboBoxFilter.SelectionChanged += ComboBoxFilter_SelectionChanged;
        _comboBoxOption.SelectionChanged += ComboBoxOption_SelectionChanged;
        _button.Click += Button_Click;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Grids.Remove(this.Gird);
    }

    private void DynamicControlCreation()
    {
        // Agregamos el control especial
        if(_typeControl == typeof(TextBox))
        {
            var textBox = CreateTextBox();
            _grid.Children.Add(textBox);
            Grid.SetColumn(textBox, 2);
        }
        else if(_typeControl == typeof(CalendarDatePicker))
        {
            var calendarDatePicker = CreateCalendarDatePicker();
            _grid.Children.Add(calendarDatePicker);
            Grid.SetColumn(calendarDatePicker, 2);
        }
        else if(_typeControl == typeof(NumberBox))
        {
            var numberBox = CreateNumberBox();
            _grid.Children.Add(numberBox);
            Grid.SetColumn(numberBox, 2);
        }
        else
        {
            throw new InvalidOperationException($"Tipo de control no compatible: {_typeControl.GetType().Name}");
        }
    }


    #region EVENTS
    private void ComboBoxOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Al realizar un cambio reseteamos el value
        _value = string.Empty;
        _selectedOption = _comboBoxOption.SelectedItem as string;
        if(_selectedOption != null)
        {
            UpdateDynamicControl();
        }
    }
    private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Al realizar un cambio reseteamos el value
        _value = string.Empty;
        _selectedFilter = _comboBoxFilter.SelectedItem as string;
        UpdateOptions();
        _comboBoxOption.SelectedItem = _optionsList.FirstOrDefault();
    }
    #endregion

    #region CREACIOND DE CONTROLES
    private TextBox CreateTextBox()
    {
        var textBox = new TextBox { Width = 300, Margin = new Thickness(5), Text = _value, PlaceholderText = _placeHolderText };

        if(_selectedOption == "today")
        {
            textBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(_selectedOption == "yesterday")
        {
            textBox.Text = DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(_selectedOption == "this month")
        {
            textBox.Text = DateTime.Now.ToString("MMMM");
            textBox.IsEnabled = false;
            return textBox;
        }
        if(_selectedOption == "this year")
        {
            textBox.Text = DateTime.Now.ToString("yyyy");
            textBox.IsEnabled = false;
            return textBox;
        }

        return textBox;
    }
    private CalendarDatePicker CreateCalendarDatePicker()
    {
        var calendarDatePicker = new CalendarDatePicker { Width = 300, Margin = new Thickness(5), PlaceholderText = _placeHolderText };

        if(DateTime.TryParse(_value, out DateTime dateTime))
        {
            calendarDatePicker.Date = dateTime;
            return calendarDatePicker;
        }
        if(_value != string.Empty)
        {
            throw new InvalidOperationException($"No se pudo convertir el {_value} en un tipo DateTime");
        }

        return calendarDatePicker;
    }
    private NumberBox CreateNumberBox()
    {
        var numberBox = new NumberBox { Width = 300, Margin = new Thickness(5), PlaceholderText = _placeHolderText, SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact };


        if(Double.TryParse(_value, out double number))
        {
            numberBox.Value = number;
        }
        return numberBox;
    }
    #endregion

    private void UpdateDynamicControl()
    {
        _grid.Children.RemoveAt(3);
        UpdateFrameworkElement();
        SetPlaceHolder();
        DynamicControlCreation();
    }
    private void UpdateFrameworkElement()
    {
        if(_optionsDictionary.ContainsKey(_selectedOption))
        {
            _typeControl = _optionsDictionary[_selectedOption];
        }
        else
        {
            _typeControl = typeof(TextBox);
        }
    }
    private void SetPlaceHolder()
    {
        if(_selectedFilter == "Size")
        {
            _placeHolderText = "100 MB";
        }
        else if(_selectedFilter == "Extension")
        {
            _placeHolderText = "jpg, png";
        }
        else if(_selectedFilter == "Name" || _selectedFilter == "Path")
        {
            _placeHolderText = "Value";
        }
        else if(_selectedFilter.Contains("Date"))
        {
            if(_selectedOption == "last days")
            {
                _placeHolderText = "5";
            }
            else
            {
                _placeHolderText = "Seleccione fecha";
            }
        }
    }
}