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
using WhereAreThem.Model.Models;
using WhereAreThem.WinViewer.ViewModel;

namespace WhereAreThem.WinViewer.View {
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window {
        public PropertiesWindowViewModel VM { get; private set; }

        public PropertiesWindow(IEnumerable<FileSystemItem> items, List<Folder> parentStack) {
            InitializeComponent();

            VM = new PropertiesWindowViewModel(items, parentStack);
            VM.View = this;
            DataContext = VM;
        }
    }
}