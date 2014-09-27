using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace AndroidDrawableRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public ObservableCollection<DrawableResource> Resources
        {
            get { return (ObservableCollection<DrawableResource>)GetValue(ResourcesProperty); }
            set { SetValue(ResourcesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Resources.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResourcesProperty =
            DependencyProperty.Register("Resources", typeof(ObservableCollection<DrawableResource>), typeof(MainWindow), new PropertyMetadata(new ObservableCollection<DrawableResource>()));



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var filesInLDPI = Directory.EnumerateFiles(Path.Combine(inputTextBox.Text, "drawable-ldpi"), "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
            var filesInMDPI = Directory.EnumerateFiles(Path.Combine(inputTextBox.Text, "drawable-mdpi"), "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
            var filesInHDPI = Directory.EnumerateFiles(Path.Combine(inputTextBox.Text, "drawable-hdpi"), "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
            var filesInXHDPI = Directory.EnumerateFiles(Path.Combine(inputTextBox.Text, "drawable-xhdpi"), "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);

            Dictionary<string, DrawableResource> fileList = new Dictionary<string, DrawableResource>();

            foreach (var file in filesInLDPI)
            {
                // de resource bestaat al in de lijst, updaten dat er een LDPI versie van is
                if (fileList.ContainsKey(file))
                    fileList[file].LDPIAvailable = true;
                else
                    fileList.Add(file, new DrawableResource { Filename = file, LDPIAvailable = true });
            }

            foreach (var file in filesInMDPI)
            {
                // de resource bestaat al in de lijst, updaten dat er een LDPI versie van is
                if (fileList.ContainsKey(file))
                    fileList[file].MDPIAvailable = true;
                else
                    fileList.Add(file, new DrawableResource { Filename = file, MDPIAvailable = true });
            }

            foreach (var file in filesInHDPI)
            {
                // de resource bestaat al in de lijst, updaten dat er een LDPI versie van is
                if (fileList.ContainsKey(file))
                    fileList[file].HDPIAvailable = true;
                else
                    fileList.Add(file, new DrawableResource { Filename = file, HDPIAvailable = true });
            }

            foreach (var file in filesInXHDPI)
            {
                // de resource bestaat al in de lijst, updaten dat er een LDPI versie van is
                if (fileList.ContainsKey(file))
                    fileList[file].XHDPIAvailable = true;
                else
                    fileList.Add(file, new DrawableResource { Filename = file, XHDPIAvailable = true });
            }

            Resources = new ObservableCollection<DrawableResource>(fileList.OrderBy(fl => fl.Key).Select(fl => fl.Value));


        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = listView1.SelectedItem as DrawableResource;
            if (item != null)
            {
                outputFilenameTextBox.Text = item.Filename;
                Keyboard.Focus(outputFilenameTextBox);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = listView1.SelectedItem as DrawableResource;
            if (item != null)
            {
                var outputFilename = outputFilenameTextBox.Text;
                var inputDirname = inputTextBox.Text;
                var outputDirname = outputTextBox.Text;
                if (item.LDPIAvailable)
                    File.Copy(Path.Combine(inputDirname, "drawable-ldpi", item.Filename), Path.Combine(outputDirname, "drawable-ldpi", outputFilename));
                if (item.MDPIAvailable)
                    File.Copy(Path.Combine(inputDirname, "drawable-mdpi", item.Filename), Path.Combine(outputDirname, "drawable-mdpi", outputFilename));
                if (item.HDPIAvailable)
                    File.Copy(Path.Combine(inputDirname, "drawable-hdpi", item.Filename), Path.Combine(outputDirname, "drawable-hdpi", outputFilename));
                if (item.XHDPIAvailable)
                    File.Copy(Path.Combine(inputDirname, "drawable-xhdpi", item.Filename), Path.Combine(outputDirname, "drawable-xhdpi", outputFilename));
                MessageBox.Show("Done");
            }
        }


    }

    public class DrawableResource
    {
        public string Filename { get; set; }
        public bool LDPIAvailable { get; set; }
        public bool MDPIAvailable { get; set; }
        public bool HDPIAvailable { get; set; }
        public bool XHDPIAvailable { get; set; }
    }
}
