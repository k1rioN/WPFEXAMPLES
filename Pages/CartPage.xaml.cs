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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        int cost = 0;
        public CartPage()
        {
            InitializeComponent();
            Update();
            PickUpPoint.ItemsSource = Core.GetContext().PickPoint.ToList();
            TotalCost();
        }

        private void TotalCost()
        {
            if(OrderListView.Items.Count > 0)
            {
                foreach(Product prod in OrderListView.Items)
                {
                    cost += Convert.ToInt32(prod.Cost);
                    CostBox.Text = $"Итоговая сумма заказа: {cost}";
                }
            }
        }

        private void Update()
        {
            OrderListView.ItemsSource = Core.GetContext().Product.Where(i => i.IsInCart == true).ToList();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ProductListPage());
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var item = PickUpPoint.SelectedItem as PickPoint;
            if(item != null)
            {
                Order order = new Order();
                order.StatusID = 1;
                order.PickPointID = item.ID;
                Core.GetContext().Order.Add(order);
                Core.GetContext().SaveChanges();

                foreach(Product prod in OrderListView.Items)
                {
                    ProductOrder productOrder = new ProductOrder
                    {
                        ProductID = prod.ID,
                        OrderID = order.ID,
                    };
                    Core.GetContext().ProductOrder.Add(productOrder);
                }
                Core.GetContext().SaveChanges();
                MessageBox.Show("Заказ успешно создан!");
            }
            else
            {
                MessageBox.Show("Выберите адрес доставки!");
            }
        }

        private void IsNullOrderItems()
        {
            if(OrderListView.Items.Count == 0)
            {
                NavigationService.Navigate(new Pages.ProductListPage());
            }
        }

        private void DeleteProductFromCart_Click(object sender, RoutedEventArgs e)
        {
            var item = OrderListView.SelectedItem as Product;
            if (item != null)
            {
                item.IsInCart = false;
                Core.GetContext().SaveChanges();
                Update();
                cost = 0;
                IsNullOrderItems();
                TotalCost();
            }
        }
    }
}
