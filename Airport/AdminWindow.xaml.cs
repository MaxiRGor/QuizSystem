using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly AppContext _context = new AppContext();

        private CollectionViewSource servicesViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            _context.AirportServices.Load();
            servicesViewSource.Source = _context.AirportServices.Local.ToObservableCollection();
        }

        public AdminWindow()
        {
            InitializeComponent();
            servicesViewSource = (CollectionViewSource)FindResource(nameof(servicesViewSource));
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            _context.SaveChanges();
            _context.Dispose();
            base.OnClosing(e);
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
        }
    }
}
