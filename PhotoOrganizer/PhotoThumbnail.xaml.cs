using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for PhotoThumbnail.xaml
    /// </summary>
    public partial class PhotoThumbnail : UserControl
    {
        public PhotoThumbnail()
        {
            InitializeComponent();
        }

        public static async Task<PhotoThumbnail> CreateThumbnail(WrapPanel panel, string uri)
        {
            var result = new PhotoThumbnail();
            if (panel.Dispatcher.Thread == Thread.CurrentThread)
            {
                panel.Children.Add(result);
            }
            else
            {
                await panel.Dispatcher.BeginInvoke(new Func<UIElement, int>(panel.Children.Add), result);
            }

            if (uri != null && File.Exists(uri))
            {
                await Task.Run(() => LoadThumbnail(result, uri));
            }

            return result;
        }

        private static async Task LoadThumbnail(PhotoThumbnail thumbnailControl, string uri)
        {
            if (thumbnailControl.Dispatcher.Thread == Thread.CurrentThread)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(uri);
                image.DecodePixelWidth = 90;
                image.EndInit();

                thumbnailControl.ThumbnailImage.Source = image;
            }
            else
            {
                await thumbnailControl.Dispatcher.BeginInvoke(new Func<PhotoThumbnail, string, Task>(LoadThumbnail), thumbnailControl, uri);
            }
        }
    }
}
