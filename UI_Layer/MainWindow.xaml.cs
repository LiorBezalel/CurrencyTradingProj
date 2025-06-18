using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TradingUI.ViewModels;

namespace UI_Layer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            const String connectionString = @"Server=localhost\SQLEXPRESS;Database=CurrencyTradingDB;Trusted_Connection=True;TrustServerCertificate=True;";
            this.DataContext = new CurrencyPairViewModel(connectionString);

        }
    }
}