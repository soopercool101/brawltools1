using System;
using System.Runtime.InteropServices;
using System.IO;

namespace BrawlLib.IO
{
    public abstract class FileMap : IDisposable
    {
        protected VoidPtr _addr;
        protected int _length;
        protected string _path;

        public VoidPtr Address { get { return _addr; } }
        public int Length { get { return _length; } }
        public string FilePath { get { return _path; } }

        ~FileMap() { Dispose(); }
        public virtual void Dispose() { GC.SuppressFinalize(this); }

        public static FileMap FromFile(string path) { return FromFile(path, FileMapProtect.ReadWrite, 0, 0); }
        public static FileMap FromFile(string path, FileMapProtect prot) { return FromFile(path, prot, 0, 0); }
        public static FileMap FromFile(string path, FileMapProtect prot, int offset, int length)
        {
            FileAccess access = (prot == FileMapProtect.ReadWrite) ? FileAccess.ReadWrite : FileAccess.Read;
            using (FileStream stream = new FileStream(path, FileMode.Open, access, FileShare.Read, 0x1000, FileOptions.RandomAccess))
                return FromStream(stream, prot, offset, length);
        }

        public static FileMap FromStream(FileStream stream) { return FromStream(stream, FileMapProtect.ReadWrite, 0, 0); }
        public static FileMap FromStream(FileStream stream, FileMapProtect prot) { return FromStream(stream, prot, 0, 0); }
        public static FileMap FromStream(FileStream stream, FileMapProtect prot, int offset, int length)
        {
            if (length == 0)
                length = (int)stream.Length;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return new wFileMap(stream.SafeFileHandle.DangerousGetHandle(), prot, offset, (uint)length) { _path = stream.Name };
                case PlatformID.Unix:
                    return new lFileMap(stream.Handle, prot, (uint)offset, (uint)length) { _path = stream.Name };
            }
            return null;
        }
        public static FileMap FromTempFile(int length)
        {
            using (FileStream stream = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite, FileShare.Read, 0x1000, FileOptions.RandomAccess | FileOptions.DeleteOnClose))
            {
                stream.SetLength(length);
                return FromStream(stream, FileMapProtect.ReadWrite, 0, length);
            }
        }
    }

    public enum FileMapProtect : uint
    {
        Read = 0x01,
        ReadWrite = 0x02
    }

    public class wFileMap : FileMap
    {
        //Win32.SafeHandle fHandle;

        internal wFileMap(VoidPtr hFile, FileMapProtect protect, long offset, uint length)
        {
            long maxSize = offset + length;
            uint maxHigh = (uint)(maxSize >> 32);
            uint maxLow = (uint)maxSize;
            Win32._FileMapProtect mProtect; Win32._FileMapAccess mAccess;
            if (protect == FileMapProtect.ReadWrite)
            {
                mProtect = Win32._FileMapProtect.ReadWrite;
                mAccess = Win32._FileMapAccess.Write;
            }
            else
            {
                mProtect = Win32._FileMapProtect.ReadOnly;
                mAccess = Win32._FileMapAccess.Read;
            }

            //fHandle = Win32.SafeHandle.Duplicate(hFile);
            using (Win32.SafeHandle h = Win32.CreateFileMapping(hFile, null, mProtect, maxHigh, maxLow, null))
            {
                h.ErrorCheck();
                _addr = Win32.MapViewOfFile(h.Handle, mAccess, (uint)(offset >> 32), (uint)offset, length);
                if (!_addr) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                _length = (int)length;
            }
        }

        public override void Dispose()
        {
            if (_addr) { Win32.UnmapViewOfFile(_addr); _addr = null; }
            //fHandle.Dispose();
            base.Dispose();
        }
    }

    public unsafe class lFileMap : FileMap
    {
        public lFileMap(VoidPtr hFile, FileMapProtect protect, uint offset, uint length)
        {
            Linux.MMapProtect mProtect = (protect == FileMapProtect.ReadWrite) ? Linux.MMapProtect.Read | Linux.MMapProtect.Write : Linux.MMapProtect.Read;
            _addr = Linux.mmap(null, length, mProtect, Linux.MMapFlags.Shared, hFile, offset);
            _length = (int)length;
        }

        public override void Dispose()
        {
            if (_addr) { Linux.munmap(_addr, (uint)_length); _addr = null; }
            base.Dispose();
        }
    }
}
