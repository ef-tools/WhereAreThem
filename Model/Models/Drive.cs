﻿using System;
using System.IO;
using System.Linq;
using PureLib.Common;

namespace WhereAreThem.Model.Models {
    public class Drive : Folder {
        public const DriveType NETWORK_SHARE = (DriveType)100;
        public const string NETWORK_COMPUTER_PREFIX = @"\\";

        public DriveType DriveType { get; set; }
        public string MachineName { get; private set; }

        public string DriveLetter
            => Name.Contains(Path.DirectorySeparatorChar) ? GetDriveLetter(Name) : Name;

        public Drive(string machineName) {
            MachineName = machineName;
        }

        public Folder GetDrive(string path) {
            int shouldSkip = IsNetworkPath(path) ? 1 : 0;
            string[] parts = path.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(shouldSkip).ToArray();
            if (parts.Length == 1)
                return this;

            Folder parent = this;
            for (int i = 1; i < parts.Length; i++) {
                if (parent.Folders == null)
                    break;

                Folder folder = parent.Folders.SingleOrDefault(f => f.NameEquals(parts[i]));
                if (folder == null)
                    break;

                parent = folder;
            }
            return parent;
        }

        public Folder ToFolder() {
            return new Folder() {
                Name = Name,
                CreatedDateUtc = CreatedDateUtc,
                Folders = Folders,
                Files = Files,
            };
        }

        public static bool IsNetworkPath(string path) {
            return path.StartsWith(NETWORK_COMPUTER_PREFIX);
        }

        public static string GetDriveLetter(string path) {
            if (IsNetworkPath(path)) {
                string[] parts = path.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                return parts[1];
            }
            int i = path.IndexOf(Path.VolumeSeparatorChar);
            return (i >= 0) ? path.Substring(0, i).ToUpper() : path;
        }

        public static string GetDrivePath(string letter) {
            return GetDrivePath(letter, DriveType.Unknown, null);
        }

        public static string GetDrivePath(string letter, DriveType driveType, string machineName) {
            if (driveType == NETWORK_SHARE) {
                return "{0}{0}{1}{0}{2}".FormatWith(Path.DirectorySeparatorChar, machineName.ToUpper(), letter);
            }
            return "{0}{1}{2}".FormatWith(letter.ToUpper(), Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
        }

        public static string GetDriveName(string letter, DriveType driveType) {
            return (driveType == NETWORK_SHARE) ? letter : GetDrivePath(letter);
        }

        public static string GetMachineName(string sharePath) {
            string[] parts = sharePath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            return parts[0];
        }

        public static Drive FromFolder(Folder folder, DriveType driveType, string machineName) {
            return new Drive(machineName) {
                DriveType = driveType,
                Name = folder.Name,
                CreatedDateUtc = folder.CreatedDateUtc,
                Folders = folder.Folders,
                Files = folder.Files,
            };
        }
    }
}
