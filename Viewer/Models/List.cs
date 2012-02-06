﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;
using WhereIsThem.Model;

namespace WhereIsThem.Viewer.Models {
    public static class List {
        private static string _path {
            get {
                return ConfigurationManager.AppSettings["path"].WrapPath();
            }
        }
        private static Dictionary<string, Folder> _machineCache = new Dictionary<string, Folder>();

        public static string[] MachineNames {
            get {
                return Directory.GetDirectories(_path)
                    .Select(p => Path.GetFileName(p)).ToArray();
            }
        }

        public static List<Folder> GetDrives(string machineName) {
            return Directory.GetFiles(Path.Combine(_path, machineName), "*.{0}".FormatWith(Constant.ListExt))
                .Select(p => new Folder() {
                    Name = "{0}:\\".FormatWith(Path.GetFileNameWithoutExtension(p)),
                    CreatedDateUtc = new FileInfo(p).LastWriteTimeUtc
                }).ToList();
        }

        public static Folder GetDrive(string machineName, string drive) {
            string machinePath = Path.Combine(_path, machineName);
            if (!_machineCache.ContainsKey(machineName) && Directory.Exists(machinePath))
                _machineCache.Add(machineName, new Folder() { Name = machineName, Folders = new List<Folder>() });

            string listPath = Path.Combine(machinePath, Path.ChangeExtension(drive.First().ToString(), Constant.ListExt));
            Folder machine = _machineCache[machineName];
            if (!machine.Folders.Any(d => d.Name == drive) && System.IO.File.Exists(listPath))
                machine.Folders.Add(Persistence.Load(listPath));

            return machine.Folders.Single(d => d.Name == drive);
        }
    }
}