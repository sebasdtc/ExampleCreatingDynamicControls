using Microsoft.UI.Xaml.Controls;

namespace ExampleCreatingDynamicControls;
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; set; }
    public MainPage()
    {
        ViewModel = new MainViewModel();
        this.InitializeComponent();

        myGrid.Children.Add(ViewModel.FilterListView.ListView);

        Grid.SetRow(ViewModel.FilterListView.ListView, 1);

        //MyListView.ItemsSource = FilterControl.Grids;
    }

    private void AppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var filter = new Models.Filter();
        ViewModel.FilterListView.AddItem(filter);
    }
}
