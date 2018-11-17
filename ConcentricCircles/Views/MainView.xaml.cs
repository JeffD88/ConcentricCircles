using ConcentricCircles.Services;
using ConcentricCircles.ViewModel;

using System.Windows;


namespace ConcentricCircles.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewViewModel(this, new ConcentricPatternService());
        }
    }
}
