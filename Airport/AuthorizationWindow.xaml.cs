using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Airport
{
    /// <summary>
    /// Interaction logic for AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private string adminName = "Администратор";
        private string adminPassword = "123456";
        private readonly AppContext _context = new AppContext();

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            _context.AirportServices.Load();

            if (_context.AirportServices.Count() == 0)
            {
                _context.AirportServices.Add(new AirportService(adminName, adminPassword));
                _context.SaveChanges();
            }

            serviceComboBox.ItemsSource = _context.AirportServices.Local.ToObservableCollection();
            serviceComboBox.DisplayMemberPath = "Title";
            serviceComboBox.SelectedValuePath = "AirportServiceId";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // clean up database connections
            _context.Dispose();
            base.OnClosing(e);
        }

        private void OnEnterEditModButtonClick(object sender, RoutedEventArgs e)
        {
            if (serviceComboBox.SelectedItem != null)
            {
                AirportService currentService = _context.AirportServices.Local.Single(s => s.AirportServiceId == int.Parse(serviceComboBox.SelectedValue.ToString()));
                if (currentService.PasswordForRedacting == passwordTextTextBox.Password)
                {
                    if (currentService.AirportServiceId == 1)
                    {
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                    }
                    else
                    {
                        MainWindow mainWindow = new MainWindow(currentService);
                        mainWindow.Show();
                    }

                    Close();
                }
                else
                {
                    message.Content = "Неверный пароль для режима редактирования";
                    DialogHost.IsOpen = true;
                }

            }
        }

        private void OnEnterTestModButtonClick(object sender, RoutedEventArgs e)
        {
            if (serviceComboBox.SelectedItem != null)
            {
                AirportService currentService = _context.AirportServices.Local.Single(s => s.AirportServiceId == int.Parse(serviceComboBox.SelectedValue.ToString()));
                if (currentService.PasswordForTesting == passwordTextTextBox.Password)
                {
                    if (currentService.AirportServiceId == 1)
                    {
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                    }
                    else
                    {
                        TestSelectionWindow testSelectionWindow = new TestSelectionWindow(currentService);
                        testSelectionWindow.Show();
                    }

                    Close();
                }
                else
                {
                    message.Content = "Неверный пароль для режима тестирования";
                    DialogHost.IsOpen = true;
                }

            }
        }
    }
}
