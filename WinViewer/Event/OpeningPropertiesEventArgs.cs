﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhereAreThem.Model;

namespace WhereAreThem.WinViewer {
    public class OpeningPropertiesEventArgs : EventArgs {
        public FileSystemItem Item { get; private set; }

        public OpeningPropertiesEventArgs(FileSystemItem item) {
            Item = item;
        }
    }

    public delegate void OpeningPropertiesEventHandler(object sender, OpeningPropertiesEventArgs e);
}