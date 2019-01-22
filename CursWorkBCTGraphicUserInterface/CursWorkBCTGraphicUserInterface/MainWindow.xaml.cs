using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace CursWorkBCTGraphicUserInterface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void createCourseWorkButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("HEllo");
        }

        private void vkGroupButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://vk.com/evadam_labs");
        }

        private void githubLinkButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/batyrshin-danil/CourseWorkBCT");
        }
    }
}
