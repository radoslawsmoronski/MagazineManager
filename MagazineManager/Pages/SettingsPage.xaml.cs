using MagazineManager;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    /// 
    public partial class SettingsPage : Page
    {
        UsersManagerPage usersManagerPage = new UsersManagerPage();
        public SettingsPage()
        {
            InitializeComponent();

            settingsFrame.Navigate(usersManagerPage);
        }

        private void changeSettingsClick(object sender, RoutedEventArgs e)
        {
            if (sender == usersManagerButton)
            {
                usersManagerPage.refreshPage();
                settingsFrame.Navigate(usersManagerPage);
            }
        }
    }
}
