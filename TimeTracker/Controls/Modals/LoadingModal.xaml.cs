using MaterialDesignThemes.Wpf;
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

namespace TimeTracker.Controls.Modals
{
    /// <summary>
    /// Interaction logic for LoadingModal.xaml
    /// </summary>
    public partial class LoadingModal : UserControl
    {
        private LoadingModal(
            string title = null,
            string loadingText = null
        )
        {
            InitializeComponent();

            SetTitle(title);
            SetLoadingText(loadingText);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                Title.Visibility = Visibility.Hidden;
                return;
            }

            Title.Visibility = Visibility.Visible;
            Title.Text = title;
        }

        public void SetLoadingText(string loadingText)
        {
            if (string.IsNullOrEmpty(loadingText))
            {
                LoadingText.Visibility = Visibility.Hidden;
            }

            LoadingText.Visibility = Visibility.Visible;
            LoadingText.Text = loadingText;
        }

        public void Remove()
        {
            try
            {
                DialogHost.Close(null);
            }
            catch
            {
                // ignored
            }
        }

        public static LoadingModal Display(
            string title = null,
            string loadingText = null
            )
        {
            LoadingModal modal = new LoadingModal( title, loadingText );
            DialogHost.Show(modal);

            return modal;
        }
    }
}
