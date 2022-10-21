using Avalonia.Controls;
using Variant2.Axyonov.ViewModels;

namespace Variant2.Axyonov.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            DataContext = new AddProductViewModel();
        }
    }
}
