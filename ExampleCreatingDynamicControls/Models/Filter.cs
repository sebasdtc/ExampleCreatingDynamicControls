using System;

namespace ExampleCreatingDynamicControls.Models;
public class Filter
{
    public string TypeFilter { get; set; }
    public string OptionFilter { get; set; }
    public string Value { get; set; }
    public Type TypeControl { get; set; }
    public Filter(string typeFilter, string optionFilter, string value, Type typeControl)
    {
        TypeFilter = typeFilter;
        OptionFilter = optionFilter;
        Value = value;
        TypeControl = typeControl;
    }
}
