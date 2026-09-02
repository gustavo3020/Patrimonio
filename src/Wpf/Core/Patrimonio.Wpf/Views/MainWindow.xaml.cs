using Patrimonio.Wpf.Navigation;
using System.Windows;

namespace Patrimonio.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(AppNavigation viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
