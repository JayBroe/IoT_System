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

namespace DesktopDataGrabber.View
{
    /** 
     * @brief Interaction logic for MainWindow.xaml 
     */
    public partial class MainWindow : Window
    {
        bool isMenuVisible = true;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            isMenuVisible = !isMenuVisible;
            MenuBtn.Content = "Schowaj Menu";
            if (!isMenuVisible)
            {
                MenuBtn.Content = "Pokaż Menu";
            }
            if (isMenuVisible)
            this.Menu.Visibility = Visibility.Visible;
            else
                this.Menu.Visibility = Visibility.Collapsed;
        }



        private void Button_IMU(object sender, RoutedEventArgs e)
        {
            this.DataPlotView_temp.Visibility = Visibility.Collapsed;
            this.DataPlotView_press.Visibility = Visibility.Collapsed;
            this.DataPlotView_humid.Visibility = Visibility.Collapsed;
            this.Data_table.Visibility = Visibility.Collapsed;
            this.Led_Matrix.Visibility = Visibility.Collapsed;

            if (this.DataPlotView_IMU.Visibility == Visibility.Visible)
            {
                this.DataPlotView_IMU.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.DataPlotView_IMU.Visibility = Visibility.Visible;
            }
        }

        private void Button_ENV(object sender, RoutedEventArgs e)
        {
            this.DataPlotView_IMU.Visibility = Visibility.Collapsed;
            this.Data_table.Visibility = Visibility.Collapsed;
            this.Led_Matrix.Visibility = Visibility.Collapsed;

            if (this.DataPlotView_temp.Visibility == Visibility.Visible && this.DataPlotView_press.Visibility == Visibility.Visible && this.DataPlotView_humid.Visibility == Visibility.Visible)
            {
                this.DataPlotView_temp.Visibility = Visibility.Collapsed;
                this.DataPlotView_press.Visibility = Visibility.Collapsed;
                this.DataPlotView_humid.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.DataPlotView_temp.Visibility = Visibility.Visible;
                this.DataPlotView_press.Visibility = Visibility.Visible;
                this.DataPlotView_humid.Visibility = Visibility.Visible;
            }
        }

        private void Button_Table(object sender, RoutedEventArgs e)
        {
            this.DataPlotView_temp.Visibility = Visibility.Collapsed;
            this.DataPlotView_press.Visibility = Visibility.Collapsed;
            this.DataPlotView_humid.Visibility = Visibility.Collapsed;
            this.DataPlotView_IMU.Visibility = Visibility.Collapsed;
            this.Led_Matrix.Visibility = Visibility.Collapsed;

            if (this.Data_table.Visibility == Visibility.Visible)
            {
                this.Data_table.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Data_table.Visibility = Visibility.Visible;
            }
        }

        private void Button_Led(object sender, RoutedEventArgs e)
        {
            this.DataPlotView_temp.Visibility = Visibility.Collapsed;
            this.DataPlotView_press.Visibility = Visibility.Collapsed;
            this.DataPlotView_humid.Visibility = Visibility.Collapsed;
            this.Data_table.Visibility = Visibility.Collapsed;
            this.DataPlotView_IMU.Visibility = Visibility.Collapsed;

            if (this.Led_Matrix.Visibility == Visibility.Visible)
            {
                this.Led_Matrix.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Led_Matrix.Visibility = Visibility.Visible;
            }
        }
    }
}
