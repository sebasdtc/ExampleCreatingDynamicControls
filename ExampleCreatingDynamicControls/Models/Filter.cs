using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExampleCreatingDynamicControls.Models;
public class Filter
{
    public string TypeFilter { get; set; }
    public string OptionFilter { get; set; }
    public string Value { get; set; }
    public Type TypeControl { get; set; }
    public ObservableCollection<string> OptionsList { get; set; } = new();
    public string PlaceHolderText { get; private set; }

    public List<string> FilterTypeList = new()
    {
        "Name",
        "Extension",
        "Size",
        "Path",
        "Date Create",
        "Date Modified",
        "Date Accessed"
    };
    //private readonly Dictionary<string, Type> _optionsDictionary = new()
    //{
    //    { "greater than", typeof(NumberBox) },
    //    { "less than", typeof(NumberBox) },
    //    { "equals", typeof(NumberBox) },
    //    { "last days", typeof(NumberBox) },
    //    { "exactly", typeof(CalendarDatePicker) },
    //    { "before", typeof(CalendarDatePicker) },
    //    { "after", typeof(CalendarDatePicker) }
    //};
    public Filter()
    {
        TypeFilter = FilterTypeList.FirstOrDefault();
        UpdateOptions();
        OptionFilter = OptionsList.FirstOrDefault();
        Value = string.Empty;
        TypeControl = typeof(TextBox);
        PlaceHolderText = "Value";
    }
    public Filter(string typeFilter, string optionFilter, string value, Type typeControl)
    {
        TypeFilter = typeFilter;
        OptionFilter = optionFilter;
        Value = value;
        TypeControl = typeControl;

        UpdateOptions();
    }

    public void UpdateOptions()
    {
        ObservableCollection<string> filterList;
        if(TypeFilter.Equals("Size"))
        {
            filterList = new ObservableCollection<string>()
            {
                "greater than",
                "less than",
                "equals"
            };
        }
        else if(TypeFilter.Contains("Date"))
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
        else if(TypeFilter.Equals("Extension"))
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

        OptionsList.Clear();

        foreach(var item in filterList)
        {
            OptionsList.Add(item);
        }
    }

    private void UpdateTypeControl()
    {
        Dictionary<string, Type> optionsDictionary = new()
        {
            { "greater than", typeof(NumberBox) },
            { "less than", typeof(NumberBox) },
            { "equals", typeof(NumberBox) },
            { "last days", typeof(NumberBox) },
            { "exactly", typeof(CalendarDatePicker) },
            { "before", typeof(CalendarDatePicker) },
            { "after", typeof(CalendarDatePicker) }
        };

        if(optionsDictionary.ContainsKey(OptionFilter))
        {
            TypeControl = optionsDictionary[OptionFilter];
        }
        else
        {
            TypeControl = typeof(TextBox);
        }
    }

    public void Update()
    {
        UpdateTypeControl();
        UpdatePlaceHolderText();
        UpdateValue();
    }
    private void UpdateValue()
    {
        Value = string.Empty;
    }
    private void UpdatePlaceHolderText()
    {
        PlaceHolderText = GetPlaceHolderText();
    }

    private string GetPlaceHolderText()
    {
        if(TypeFilter == "Size") return "100 MB";
        if(TypeFilter == "Extension") return "jpg, png";
        if(TypeFilter == "Name" || TypeFilter == "Path") return "Value";
        if(TypeFilter.Contains("Date") && OptionFilter == "last days") return "5";

        return "Seleccione fecha";
    }
}
