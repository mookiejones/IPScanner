using CommunityToolkit.Mvvm.DependencyInjection;
using IPScanner.ViewModels;
using System.Windows;

namespace IPScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext=Ioc.Default.GetRequiredService<MainViewModel>();
        }
    }
}