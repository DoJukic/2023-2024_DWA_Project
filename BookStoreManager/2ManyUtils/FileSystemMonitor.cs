using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooManyUtils
{
    public class FileSystemMonitor : IDisposable
    {
        public readonly string targetPath;
        public readonly bool isSurfaceLevel;
        public readonly long targetRefreshTime;

        public bool wasDisposed = false;

        private readonly object theLock = new();
        private DirectorySnapshot? currentState;
        private Timer? timer;

        public ThreadLockedEvent OnSnapshotStateChanged;
        public ThreadLockedEvent OnErrorOccured;
        public ThreadLockedEvent OnTargetFileDeleted;

        public FileSystemMonitor(string targetPath, bool isSurfaceLevel = false, long targetRefreshTimeMs = 100,
            ThreadLockedEvent? OnFilesystemChangeDetected = null, ThreadLockedEvent? OnErrorOccured = null, ThreadLockedEvent? OnTargetFileDeleted = null)
        {
            this.targetPath = targetPath;
            this.isSurfaceLevel = isSurfaceLevel;
            this.targetRefreshTime = targetRefreshTimeMs;

            OnSnapshotStateChanged = OnFilesystemChangeDetected ?? new();
            this.OnErrorOccured = OnErrorOccured ?? new();
            this.OnTargetFileDeleted = OnTargetFileDeleted ?? new();

            LoadCurrentState(this);
        }

        private static void LoadCurrentState(object? state)
        {
            if (state == null || state is not FileSystemMonitor)
                return;

            var self = (FileSystemMonitor)state;

            if (!Directory.Exists(self.targetPath))
            {
                self.OnTargetFileDeleted.Trigger();
                self.Dispose();
                return;
            }

            lock (self.theLock)
            {
                if (self.wasDisposed)
                    return;

                self.timer?.Dispose();

                try
                {
                    self.currentState = new(self.targetPath, self.isSurfaceLevel);
                    self.timer = new(CheckFileSystem, self, self.targetRefreshTime, Timeout.Infinite);

                    // We can't have any gaps, so this has to go here
                    // Will also fire on init, but it's whatever. Hopefully.
                    self.OnSnapshotStateChanged.SafeTrigger();
                }
                catch (Exception)
                {
                    self.OnErrorOccured.SafeTrigger();
                    // Let's just hope we just caught it at a bad time and try again...
                    self.timer = new(LoadCurrentState, self, self.targetRefreshTime, Timeout.Infinite);
                }
            }
        }

        private static void CheckFileSystem(object? state)
        {
            if (state == null || state is not FileSystemMonitor)
                return;

            var self = (FileSystemMonitor)state;

            lock (self.theLock)
            {
                if (self.wasDisposed)
                    return;

                self.timer?.Dispose();

                try
                {
                    if (self.currentState != null && !self.currentState.ValidateSnapshot())
                        LoadCurrentState(self); // Locks are reentrant! Yippie!
                    else
                        self.timer = new(CheckFileSystem, self, self.targetRefreshTime, Timeout.Infinite);
                }
                catch (Exception)
                {
                    self.OnErrorOccured.SafeTrigger();
                    LoadCurrentState(self); // Snapshot validation failed for some reason, not really sure what we should do in this case.
                }
            }
        }

        public void Dispose()
        {
            OnSnapshotStateChanged.SafeDispose();
            OnTargetFileDeleted.SafeDispose();

            lock (theLock)
            {
                timer?.Dispose();
                timer = null;
                wasDisposed = true;
            }
        }

        private class DirectorySnapshot
        {
            private string fullPath;
            private DateTimeOffset lastModified;
            private bool isSurfaceLevel;
            private List<DirectorySnapshot> directories = new();
            private List<FileSnapshot> files = new();

            private class FileSnapshot
            {
                public string fullPath;
                public DateTimeOffset lastModified;

                public FileSnapshot(string fullPath, DateTimeOffset lastModified)
                {
                    this.fullPath = fullPath;
                    this.lastModified = lastModified;
                }
            }

            // Empty shell exclusively for usage with surface level snapshots
            private DirectorySnapshot(string path)
            {
                DirectoryInfo currDir = new(path);

                fullPath = currDir.FullName;
                lastModified = new(currDir.LastWriteTime);
            }

            public DirectorySnapshot(string path, bool isSurfaceLevel = true)
            {
                DirectoryInfo currDir = new(path);

                fullPath = currDir.FullName;
                lastModified = new(currDir.LastWriteTime);
                this.isSurfaceLevel = isSurfaceLevel;

                var detectedFiles = currDir.EnumerateFiles();
                var detectedDirectories = currDir.EnumerateDirectories();

                foreach (var file in detectedFiles)
                    files.Add(new(file.FullName, file.LastWriteTime));

                if (isSurfaceLevel)
                    foreach (var dir in detectedDirectories)
                        directories.Add(new(dir.FullName));
                else
                    foreach (var dir in detectedDirectories)
                        directories.Add(new(dir.FullName, false));

            }

            public bool ValidateSnapshot()
            {
                // Note: GetLastWriteTime() will change if direct descendants are modified
                // By direct descendants I mean files or folders which are created or renamed, changes within files or folders will not affect the parent's write time
                if (!Directory.Exists(fullPath) || new DateTimeOffset(Directory.GetLastWriteTime(fullPath)) != lastModified)
                    return false;

                if (Directory.GetDirectories(fullPath).Length + Directory.GetFiles(fullPath).Length > directories.Count + files.Count)
                    return false;

                foreach (var file in files)
                    if (!File.Exists(file.fullPath) || new DateTimeOffset(File.GetLastWriteTime(file.fullPath)) != file.lastModified)
                        return false;

                if (!isSurfaceLevel)
                {
                    foreach (var dir in directories)
                        if (!dir.ValidateSnapshot())
                            return false;
                }

                return true;
            }
        }
    }
}
