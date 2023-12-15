using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace ExampleCreatingDynamicControls;
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<FilterControl> _allFilter;

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
        _ = new FilterControl(new Models.Filter("Date Create", "before", "10-12-2023", typeof(CalendarDatePicker)));
        _ = new FilterControl(new Models.Filter("Size", "equals", "60", typeof(NumberBox)));
        _ = new FilterControl(new Models.Filter("Name", "contains", "archivo", typeof(TextBox)));
        _ = new FilterControl(new Models.Filter("Extension", "is", "doc,xmls,pdf", typeof(TextBox)));
    }

}