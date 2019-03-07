﻿using System;
using System.Collections.Generic;
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
using WhereAreThem.WinViewer.Event;
using WhereAreThem.WinViewer.ViewModel;

namespace WhereAreThem.WinViewer.View {
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window {
        public SearchWindowViewModel VM { get; private set; }

        public SearchWindow() {
            InitializeComponent();

            Activated += (s, e) => {
                txtSearch.SelectAll();
                Keyboard.Focus(txtSearch);
            };

            VM = new SearchWindowViewModel();
            VM.View = this;
            VM.OpeningProperties += OnOpeningProperties;
            DataContext = VM;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            DataGridRow row = MainWindow.GetParent<DataGridRow>((DependencyObject)e.OriginalSource);
            if (row != null)
                VM.OnLocatingItem();
        }

        private void OnOpeningProperties(object sender, ItemsEventArgs e) {
            PropertiesWindow window = new PropertiesWindow(e.Items, e.Stack);
            window.Owner = this;
            window.Show();
        }
    }
}
