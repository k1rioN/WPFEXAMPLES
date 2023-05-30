using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public ProductListPage()
        {
            InitializeComponent();
            Update();
            IsCartButtonVisible();
        }
        private void Update()
        {
            ProductListView.ItemsSource = Core.GetContext().Product.ToList();
        }

        private void AddProductToCart_Click(object sender, RoutedEventArgs e)
        {
            var item = ProductListView.SelectedItem as Product;
            if (item != null)
            {
                item.IsInCart = true;
                Core.GetContext().SaveChanges();
                IsCartButtonVisible();
            }
        }

        private void IsCartButtonVisible()
        {
            CartPage cartPage= new CartPage();
            if(cartPage.OrderListView.Items.Count > 0 )
            {
                GoToCartButton.Visibility = Visibility.Visible;
            }
            else
            {
                GoToCartButton.Visibility= Visibility.Hidden;
            }
        }

        private void GoToCartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.CartPage());
        }
    }
}
