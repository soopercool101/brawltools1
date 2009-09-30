﻿using System;
using System.Collections.Generic;
using BrawlLib.IO;
using System.ComponentModel;
using BrawlLib.Wii.Compression;
using System.Reflection;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public delegate void ResourceEventHandler(ResourceNode node);
    public delegate void ResourceChildEventHandler(ResourceNode node, ResourceNode child);

    public struct DataSource
    {
        public static readonly DataSource Empty = new DataSource();

        public VoidPtr Address;
        public int Length;
        public FileMap Map;
        public CompressionType Compression;

        public DataSource(VoidPtr addr, int len) : this(addr, len, CompressionType.None) { }
        public DataSource(VoidPtr addr, int len, CompressionType compression)
        {
            Address = addr;
            Length = len;
            Map = null;
            Compression = compression;
        }
        public DataSource(FileMap map) : this(map, CompressionType.None) { }
        public DataSource(FileMap map, CompressionType compression)
        {
            Address = map.Address;
            Length = map.Length;
            Map = map;
            Compression = compression;
        }

        public void Close()
        {
            if (Map != null) { Map.Dispose(); Map = null; }
            Address = null;
            Length = 0;
            Compression = CompressionType.None;
        }

        public static bool operator ==(DataSource src1, DataSource src2) { return (src1.Address == src2.Address) && (src1.Length == src2.Length) && (src1.Map == src2.Map); }
        public static bool operator !=(DataSource src1, DataSource src2) { return (src1.Address != src2.Address) || (src1.Length != src2.Length) || (src1.Map != src2.Map); }
        public override bool Equals(object obj)
        {
            if (obj is DataSource)
                return this == (DataSource)obj;
            return base.Equals(obj);
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }

    //Lower byte is resource type
    //Upper byte is entry type/flags
    public enum ResourceType : int
    {
        Unknown = 0x0000,

        ARC = 0x0101,
        BRES = 0x0102,
        TEX0 = 0x0203,
        PLT0 = 0x0204,
        //BRESGroup = 0x0003,

        ARCEntry = 0x0100,
        BRESEntry = 0x0200
    }

    public abstract class ResourceNode : IDisposable//, ICustomTypeDescriptor
    {
        internal protected DataSource _origSource, _uncompSource;
        internal protected DataSource _replSrc, _replUncompSrc;

        internal protected bool _changed, _merged;
        internal protected CompressionType _compression;

        internal protected string _name, _origPath;
        internal protected ResourceNode _parent;
        internal protected List<ResourceNode> _children;
        internal int _calcSize;

        //Occurs when a property or value has changed, but not when the data itself changes.
        public event ResourceEventHandler Dirty, Clean, Changed;
        public event ResourceChildEventHandler ChildChanged;
        //public event ResourceEventHandler Replaced;
        //public event ResourceEventHandler Restored;

        //public event ResourceEventHandler StateChanged;

        [Browsable(false)]
        public string FilePath { get { return _origPath; } }
        [Browsable(false)]
        public ResourceNode RootNode { get { return _parent == null ? this : _parent.RootNode; } }
        [Browsable(false)]
        public DataSource OriginalSource { get { return _origSource; } }
        [Browsable(false)]
        public DataSource UncompressedSource { get { return _uncompSource; } }
        [Browsable(false)]
        public DataSource WorkingRawSource { get { return _replSrc != DataSource.Empty ? _replSrc : _origSource; } }
        [Browsable(false)]
        public DataSource WorkingSource { get { return _replUncompSrc != DataSource.Empty ? _replUncompSrc : _uncompSource; } }

        [Browsable(false)]
        public virtual bool HasChildren { get { return (_children == null) || (_children.Count != 0); } }
        [Browsable(false)]
        public virtual ResourceType ResourceType { get { return ResourceType.Unknown; } }
        [Browsable(false)]
        public virtual string TreePath { get { return _parent == null ? Name : _parent.TreePath + "/" + Name; } }
        [Browsable(false)]
        public virtual int Level { get { return _parent == null ? 0 : _parent.Level + 1; } }

        [Browsable(false)]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [Browsable(false)]
        public ResourceNode Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                    return;

                Remove();
                _parent = value;
                if (_parent != null)
                    _parent.Children.Add(this);
            }
        }
        [Browsable(false)]
        public List<ResourceNode> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<ResourceNode>();
                    if (WorkingRawSource != DataSource.Empty)
                        OnPopulate();
                }
                return _children;
            }
        }
        [Browsable(false)]
        public int Index { get { return _parent == null ? -1 : _parent.Children.IndexOf(this); } }

        [Browsable(false)]
        public bool IsCompressed { get { return _compression != CompressionType.None; } }

        //Properties or compression have changed
        [Browsable(false)]
        public bool HasChanged 
        { 
            get { return _changed; }
            set
            {
                if (_changed != value)
                {
                    _changed = value;
                    SignalChange();
                }
                else if (value)
                    SignalChange();
            }
        }

        public void SignalChange()
        {
            if (Changed != null)
                Changed(this);
            if (_parent != null)
                _parent.SignalChildChange(this);
        }
        public void SignalChildChange(ResourceNode child)
        {
            if (ChildChanged != null)
                ChildChanged(this, child);
            if (_parent != null)
                _parent.SignalChildChange(child);
        }

        //Has the node deviated from its parent?
        [Browsable(false)]
        public bool IsBranch { get { return _replSrc.Map != null; } }

        [Browsable(false)]
        public bool HasMerged { get { return _merged; } }

        //Can be any of the following: children have branched, children have changed, current has changed
        //Node needs to be rebuilt.
        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                if (HasChanged)
                    return true;
                if (_children != null)
                    foreach (ResourceNode n in _children)
                        if (n.HasChanged || n.IsBranch || n.IsDirty)
                            return true;
                return false;
            }
            //set
            //{
            //    if (value == IsDirty)
            //    {
            //        if (!value)
            //        {
            //            if (Clean != null)
            //                Clean(this);
            //        }
            //        else
            //        {
            //            if (Dirty != null)
            //                Dirty(this);
            //        }

            //        if (_parent != null)
            //            _parent.IsDirty = value;
            //    }
            //}
        }

        public virtual CompressionType Compression { get { return _compression; } set { _compression = value; _changed = true; } }

        ~ResourceNode() { Dispose(); }
        public virtual void Dispose()
        {
            //Remove();
            if (_children != null)
            {
                foreach (ResourceNode node in _children)
                    node.Dispose();
                _children.Clear();
                _children = null;
            }

            //_currentSource.Close();
            _uncompSource.Close();
            _origSource.Close();
            _replUncompSrc.Close();
            _replSrc.Close();

            GC.SuppressFinalize(this);
        }

        //Called when children are first requested. Allows node to cache child nodes.
        protected virtual void OnPopulate() { }

        //Called when property values are requested. Allows node to cache values from source data.
        //Return true to indicate there are child nodes.
        protected virtual bool OnInitialize() { return false; }

        //Restores node to its original form using the backing tree. 
        public virtual void Restore()
        {
            if ((!IsDirty) && (!IsBranch))
                return;

            if (_children != null)
            {
                foreach (ResourceNode n in _children)
                    n.Dispose();
                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();
            _compression = _origSource.Compression;

            if (!OnInitialize())
                _children = new List<ResourceNode>();

            HasChanged = false;
        }

        internal void Initialize(ResourceNode parent, FileMap source) { Initialize(parent, new DataSource(source)); }
        internal void Initialize(ResourceNode parent, VoidPtr address, int length) { Initialize(parent, new DataSource(address, length)); }
        internal void Initialize(ResourceNode parent, DataSource origSource) { Initialize(parent, origSource, origSource); }
        internal virtual void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            _origSource = origSource;
            _uncompSource = uncompSource;
            _compression = _origSource.Compression;

            if (origSource.Map != null)
                _origPath = origSource.Map.FilePath;

            Parent = parent;
            if (!OnInitialize())
                _children = new List<ResourceNode>();
        }

        public virtual void Remove()
        {
            if (_parent != null)
            {
                _parent.Children.Remove(this);
                _parent.HasChanged = true;
                _parent = null;
            }
        }

        //Causes a deviation in the resource tree. This node and all child nodes will be backed by a temporary file until the tree is merged.
        //Causes parent node(s) to become dirty.
        //Replace will reference the file in a new DataSource, 
        public unsafe virtual void Replace(string fileName)
        {
            ReplaceRaw(FileMap.FromFile(fileName, FileMapProtect.Read));
        }
        public unsafe virtual void ReplaceRaw(VoidPtr address, int length)
        {
            FileMap map = FileMap.FromTempFile(length);
            Memory.Move(map.Address, address, (uint)length);
            ReplaceRaw(map);
        }
        internal unsafe virtual void ReplaceRaw(FileMap map)
        {
            if (_children != null)
            {
                foreach (ResourceNode node in _children)
                    node.Dispose();
                _children.Clear();
                _children = null;
            }

            _replUncompSrc.Close();
            _replSrc.Close();

            //FileMap map = FileMap.FromFile(fileName, FileMapProtect.Read);
            if (Compressor.IsDataCompressed(map.Address, map.Length))
            {
                CompressionHeader* cmpr = (CompressionHeader*)map.Address;
                FileMap tMap = FileMap.FromTempFile(cmpr->ExpandedSize);
                Compressor.Expand(cmpr, tMap.Address, tMap.Length);

                _compression = cmpr->Algorithm;
                _replSrc = new DataSource(map, cmpr->Algorithm);
                _replUncompSrc = new DataSource(tMap);
            }
            else
            {
                _compression = CompressionType.None;
                _replSrc = _replUncompSrc = new DataSource(map);
            }

            if (!OnInitialize())
                _children = new List<ResourceNode>();

            HasChanged = true;
        }

        public unsafe virtual void Export(string outPath)
        {
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                Export(stream);
        }
        public void Export(FileStream outStream)
        {
            outStream.SetLength(WorkingRawSource.Length);
            using (FileMap map = FileMap.FromStream(outStream))
                Memory.Move(map.Address, WorkingRawSource.Address, (uint)WorkingRawSource.Length);
        }

        public virtual void ExportUncompressed(string outPath)
        {
            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                ExportUncompressed(stream);
        }
        public void ExportUncompressed(FileStream outStream)
        {
            outStream.SetLength(WorkingSource.Length);
            using (FileMap map = FileMap.FromStream(outStream))
                Memory.Move(map.Address, WorkingSource.Address, (uint)WorkingSource.Length);
        }

        //Combines node and children into single (temp) file map.
        //Does nothing if node is not dirty or rebuild is not forced.
        //Calls OnCalculateSize on self, which will allow the node to cache any values for OnRebuild
        public virtual void Rebuild() { Rebuild(false); }
        public virtual void Rebuild(bool force)
        {
            if (!IsDirty && !force)
                return;

            //Get uncompressed size
            int size = OnCalculateSize(force);

            //Create temp map
            FileMap uncompMap = FileMap.FromTempFile(size);

            //Rebuild node
            Rebuild(uncompMap.Address, size, force);
            _replSrc.Map = _replUncompSrc.Map = uncompMap;

            //If compressed, compress resulting data.
            if (_compression != CompressionType.None)
            {
                //Compress node to temp file
                using (FileStream stream = new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.ReadWrite, FileShare.None, 0x2000, FileOptions.DeleteOnClose | FileOptions.SequentialScan))
                {
                    Compressor.Compact(_compression, uncompMap.Address, uncompMap.Length, stream);
                    _replSrc = new DataSource(FileMap.FromStream(stream, FileMapProtect.Read), _compression);
                }
            }
        }

        //internal protected virtual void RebuildUncompressed(VoidPtr address, int length, bool force)
        //{
        //    if (!IsDirty && !force)
        //    {
        //        Memory.Move(address, WorkingSource.Address, (uint)WorkingSource.Length);
        //        DataSource newsrc = new DataSource(address, WorkingSource.Length);
        //        _replSrc.Close();
        //        _replUncompSrc.Close();
        //        _replSrc = _replUncompSrc = newsrc;
        //    }
        //    else
        //        OnRebuild(address, length, force);

        //    HasChanged = false;
        //}

        //Called on child nodes in order to rebuild them at a specified address.
        //This will occur after CalculateSize, so compressed nodes will already be rebuilt.
        internal protected virtual void Rebuild(VoidPtr address, int length, bool force)
        {
            if (!IsDirty && !force)
            {
                Memory.Move(address, WorkingSource.Address, (uint)WorkingSource.Length);
                DataSource newsrc = new DataSource(address, WorkingSource.Length);
                _replSrc.Close();
                _replUncompSrc.Close();
                _replSrc = _replUncompSrc = newsrc;
            }
            else
                OnRebuild(address, length, force);

            HasChanged = false;
        }


        //Overridden by parent nodes in order to rebuild children.
        //Size is the value returned by OnCalculateSize (or _calcSize)
        //Node MUST dispose of and assign both repl sources before exiting.
        internal protected virtual void OnRebuild(VoidPtr address, int size, bool force)
        {
            Memory.Move(address, WorkingSource.Address, (uint)WorkingSource.Length);
            DataSource newsrc = new DataSource(address, WorkingSource.Length);
            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = newsrc;
        }

        //Calculate size to be passed to parent node.
        //If node is compressed, rebuild now and compress to temp file. Return temp file size.
        //Called on child nodes only, because it can trigger a rebuild.
        internal protected virtual int CalculateSize(bool force)
        {
            if (IsDirty || force)
            {
                if (_compression == CompressionType.None)
                    return _calcSize = OnCalculateSize(force);
                Rebuild(force);
            }
            return _calcSize = WorkingRawSource.Length;
        }

        internal protected virtual void WriteCompressed(VoidPtr address, int length)
        {
            Memory.Move(address, WorkingRawSource.Address, (uint)length);
            _replSrc.Close();
            _replSrc = new DataSource(address, length, _compression);
        }

        //Returns uncompressed size of node data.
        //It's up to the child nodes to return compressed sizes.
        //If this has been called, it means a rebuild must happen.
        protected virtual int OnCalculateSize(bool force)
        {
            return WorkingSource.Length;
        }

        //Combines deviated tree into backing tree. Backing tree will have moved completely to a temporary file.
        //All references to backing tree will be gone! Including file handles.
        public void Merge() { Merge(false); }
        public void Merge(bool forceBuild)
        {
            if (_parent != null)
                throw new InvalidOperationException("Merge can only be called on the root node!");

            if (forceBuild || IsDirty)
                Rebuild(forceBuild);

            //Merging when the tree isn't dirty does nothing!
            //if (!forceBuild && !IsDirty)
            //    return;

            //Rebuild node
            //Rebuild(forceBuild);

            //Copy new data to original file. This causes the original file to be freed.
            //if (_origSource.Map != null)
            //{
            //    string path = _origSource.Map.FilePath;
            //    _origSource.Close();
            //    using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.WriteThrough))
            //    {
            //        stream.SetLength(_replSrc.Length);
            //        using (FileMap map = FileMap.FromStream(stream))
            //            Memory.Move(map.Address, _replSrc.Address, (uint)_replSrc.Length);
            //    }
            //}

            MergeInternal();
            _merged = true;
        }

        //Swap data sources to only use new temp file. Closes original sources.
        protected virtual void MergeInternal()
        {
            if (_children != null)
                foreach (ResourceNode n in Children)
                    n.MergeInternal();

            if (_replSrc != DataSource.Empty)
            {
                _origSource.Close();
                _origSource = _replSrc;
                _replSrc = DataSource.Empty;

                if (_replUncompSrc != DataSource.Empty)
                {
                    _uncompSource.Close();
                    _uncompSource = _replUncompSrc;
                    _replUncompSrc = DataSource.Empty;
                }
            }
        }


        //public virtual MethodInfo GetAction(string name)
        //{
        //    foreach (MethodInfo info in GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
        //        if (((info.GetAttribute<NodeActionAttribute>()) != null) && (info.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        //            return info;
        //    return null;
        //}
        //public virtual MethodInfo[] GetActions()
        //{
        //    List<MethodInfo> infos = new List<MethodInfo>();
        //    NodeActionAttribute action;
        //    foreach (MethodInfo info in GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
        //        if ((action = info.GetAttribute<NodeActionAttribute>()) != null)
        //            infos.Add(info);
        //    return infos.ToArray();
        //}

        //#region ICustomTypeDescriptor Members

        //public AttributeCollection GetAttributes() { return TypeDescriptor.GetAttributes(this, true); }
        //public string GetClassName() { return TypeDescriptor.GetClassName(this, true); }
        //public string GetComponentName() { return TypeDescriptor.GetComponentName(this, true); }
        //public TypeConverter GetConverter() { return TypeDescriptor.GetConverter(this, true); }
        //public EventDescriptor GetDefaultEvent() { return TypeDescriptor.GetDefaultEvent(this, true); }
        //public PropertyDescriptor GetDefaultProperty() { return TypeDescriptor.GetDefaultProperty(this, true); }
        //public object GetEditor(Type editorBaseType) { return TypeDescriptor.GetEditor(this, editorBaseType, true); }
        //public EventDescriptorCollection GetEvents(Attribute[] attributes) { return TypeDescriptor.GetEvents(this, attributes, true); }
        //public EventDescriptorCollection GetEvents() { return TypeDescriptor.GetEvents(this, true); }
        //public PropertyDescriptorCollection GetProperties(Attribute[] attributes) { return GetProperties(); }
        //public PropertyDescriptorCollection GetProperties()
        //{
        //    throw new NotImplementedException();
        //}
        //public object GetPropertyOwner(PropertyDescriptor pd) { return this; }

        //#endregion

        public static ResourceNode FindNode(ResourceNode root, string path, bool searchChildren)
        {
            if (String.IsNullOrEmpty(path))
                return root;

            if (root.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                return root;

            if ((path.Contains("/")) && (path.Substring(0, path.IndexOf('/')).Equals(root.Name, StringComparison.OrdinalIgnoreCase)))
                return root.FindChild(path.Substring(path.IndexOf('/') + 1), searchChildren);

            return root.FindChild(path, searchChildren);
        }

        public ResourceNode FindChild(string path, bool searchChildren)
        {
            ResourceNode node = null;
            if (path.Contains("/"))
            {
                string next = path.Substring(0, path.IndexOf('/'));
                foreach (ResourceNode n in Children)
                    if (n.Name.Equals(next, StringComparison.OrdinalIgnoreCase))
                        if ((node = FindNode(n, path.Substring(next.Length + 1), searchChildren)) != null)
                            return node;
            }
            else
            {
                //Search direct children first
                foreach (ResourceNode n in Children)
                    if (n.Name.Equals(path, StringComparison.OrdinalIgnoreCase))
                        return n;

            }
            if (searchChildren)
                foreach (ResourceNode n in Children)
                    if ((node = n.FindChild(path, true)) != null)
                        return node;

            return null;
        }

        public ResourceNode[] FindChildrenByType(string path, ResourceType type)
        {
            if (!String.IsNullOrEmpty(path))
            {
                ResourceNode node = FindChild(path, false);
                if (node != null)
                    return node.FindChildrenByType(null, type);
            }

            List<ResourceNode> nodes = new List<ResourceNode>();
            this.EnumTypeInternal(nodes, type);
            return nodes.ToArray();
        }
        private void EnumTypeInternal(List<ResourceNode> list, ResourceType type)
        {
            if (this.ResourceType == type)
                list.Add(this);
            foreach (ResourceNode n in Children)
                n.EnumTypeInternal(list, type);
        }
    }
}
