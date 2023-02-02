using System.Windows;
using EntitiesPresenter.Interfaces;

namespace EntitiesPresenter;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IEntitiesPresenterViewModel entitiesPresenterViewModel)
    {
        DataContext = entitiesPresenterViewModel;
        InitializeComponent();
    }
}