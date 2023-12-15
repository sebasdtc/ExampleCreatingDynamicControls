using CommunityToolkit.Mvvm.ComponentModel;
using ExampleCreatingDynamicControls.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExampleCreatingDynamicControls;
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FilterControl> _allFilter;
    public FilterListView FilterListView { get; set; }

    public MainViewModel()
    {
        init();
    }

    public void Add()
    {
        _ = new FilterControl();
    }
    public void init()
    {
        var filters = new List<Filter>
        {
            new ("Date Create", "before", "10-12-2023", typeof(CalendarDatePicker)),
            new ("Size", "equals", "60", typeof(NumberBox)),
            new ("Name", "contains", "archivo", typeof(TextBox)),
            new ("Extension", "is", "doc,xmls,pdf", typeof(TextBox)),
        };

        FilterListView = new(filters);
    }

}