﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhereAreThem.Model {
    [Serializable]
    public class File : FileSystemItem {
        public long FileSize { get; set; }
        public DateTime ModifiedDateUtc { get; set; }

        public override long Size {
            get {
                return FileSize;
            }
        }
    }
}
