using System.Windows;
using EntitiesPresenter.ViewModels;

namespace EntitiesPresenter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(EntitiesPresenterViewModel entitiesPresenterViewModel)
    {
        DataContext = entitiesPresenterViewModel;
        InitializeComponent();
    }
}