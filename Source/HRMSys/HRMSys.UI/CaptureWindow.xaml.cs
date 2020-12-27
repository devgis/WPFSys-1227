using System;
using System.Collections.Generic;
using System.IO;
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
using WPFMediaKit.DirectShow.Controls;

namespace HRMSys.UI
{
    /// <summary>
    /// CaptureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureWindow : Window
    {
        public byte[] CaptureData { get; set; }

        public CaptureWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbCameras.ItemsSource = MultimediaUtil.VideoInputNames;
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                cbCameras.SelectedIndex = 0;//第0个摄像头为默认摄像头
            }
            else
            {
                DialogResult = false;
                MessageBox.Show("电脑没有安装任何可用摄像头");

                return;
            }
        }

        private void cbCameras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            captureElement.VideoCaptureSource = (string)cbCameras.SelectedItem;
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            //captureElement. 怎么抓取高清的原始图像
            //能不能抓视频。
            //todo：怎么只抓取一部分
            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)captureElement.ActualWidth, (int)captureElement.ActualHeight,
                96, 96, PixelFormats.Default);
            bmp.Render(captureElement);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                CaptureData = ms.ToArray();
            }

            //captureElement.Pause();
            //todo:自己完成重拍的代码
            //DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            btnCapture_Click(sender, null);
            DialogResult = true;
        }
    }
}