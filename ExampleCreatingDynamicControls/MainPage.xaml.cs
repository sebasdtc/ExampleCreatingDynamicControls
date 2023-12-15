using Microsoft.UI.Xaml.Controls;

namespace ExampleCreatingDynamicControls;
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; set; }
    public MainPage()
    {
        ViewModel = new MainViewModel();
        this.InitializeComponent();
        MyListView.ItemsSource = FilterControl.Grids;
    }

    private void AppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.Add();
    }
}
