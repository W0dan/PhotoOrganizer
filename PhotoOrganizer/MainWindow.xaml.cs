using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var path = @"W:\Fotos\2019\08\Part004_WeekOldeKampNederland";

            var unfilteredPath = System.IO.Path.Combine(path, "Unfiltered");
            var RAWPath = System.IO.Path.Combine(path, "RAW");
            var PhotoshopPath = System.IO.Path.Combine(path, "Photoshop");

            var di = new System.IO.DirectoryInfo(unfilteredPath);

            var counter = 0;

            var imageFiles = di.GetFiles("*.JPG").OrderBy(x => x.Name).ToList();

            var taskList = new List<Task>();
            foreach (var fi in imageFiles)
            {
                if (counter > 100) break;

                Debug.WriteLine(fi.Name);

                taskList.Add(AddThumbnail(fi));

                counter++;
            }

            foreach(var t in taskList)
                await t;
        }

        private async Task AddThumbnail(System.IO.FileInfo fi)
        {
            var thumbnail = await PhotoThumbnail.CreateThumbnail(ThumbNailsStackPanel, fi.FullName);

            //ThumbNailsStackPanel.Children.Add(thumbnail);
        }
    }
}
