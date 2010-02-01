using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    public enum KeyFrameMode
    {
        ScaleX = 0x10,
        ScaleY = 0x11,
        ScaleZ = 0x12,
        RotX = 0x13,
        RotY = 0x14,
        RotZ = 0x15,
        TransX = 0x16,
        TransY = 0x17,
        TransZ = 0x18,
        ScaleXYZ = 0x30,
        RotXYZ = 0x33,
        TransXYZ = 0x36,
        All = 0x90
    }

    public unsafe class KeyframeCollection
    {
        internal KeyframeEntry[] _keyRoots = new KeyframeEntry[9]{
        new KeyframeEntry(-1, 1.0f), new KeyframeEntry(-1, 1.0f), new KeyframeEntry(-1, 1.0f), 
        new KeyframeEntry(-1, 0.0f), new KeyframeEntry(-1, 0.0f), new KeyframeEntry(-1, 0.0f), 
        new KeyframeEntry(-1, 0.0f), new KeyframeEntry(-1, 0.0f), new KeyframeEntry(-1, 0.0f)};

        internal int[] _keyCounts = new int[9];

        internal AnimationCode _evalCode;

        public int this[KeyFrameMode mode] { get { return _keyCounts[(int)mode]; } }

        internal int _frameLimit;
        public int FrameLimit
        {
            get { return _frameLimit; }
            set
            {
                _frameLimit = value;
                for (int i = 0; i < 9; i++)
                {
                    KeyframeEntry root = _keyRoots[i];
                    while (root._prev._index >= value)
                    {
                        root._prev.Remove();
                        _keyCounts[i]--;
                    }
                }
            }
        }

        internal bool _linearRot;
        public bool LinearRotation { get { return _linearRot; } set { _linearRot = value; } }

        public float this[KeyFrameMode mode, int index]
        {
            get { return GetFrameValue(mode, index); }
            set { SetFrameValue(mode, index, value); }
        }

        public KeyframeCollection(int limit) { _frameLimit = limit; }

        //private AnimationFrame[] _frames;
        //public KeyframeEntry[][] _keyFrames = new KeyframeEntry[9][];

        //public AnimationFrame[] AnimFrames
        //{
        //    get
        //    {
        //        if (_frames == null)
        //        {
        //            _frames = new AnimationFrame[_count];
        //            ResetFrames();
        //        }
        //        return _frames;
        //    }
        //}
        //public AnimationKeyframe[] Keyframes
        //{
        //    get
        //    {
        //        List<AnimationKeyframe> list = new List<AnimationKeyframe>(_count);

        //        AnimationKeyframe current;
        //        float* dPtr = (float*)&current;
        //        bool found;
        //        float val;

        //        for (int i = 1; i <= _count; i++)
        //        {
        //            found = false;
        //            for (int x = 0; x < 9; x++)
        //            {
        //                val = _keyFrames[x][i]._value;
        //                if (!float.IsNaN(val))
        //                {
        //                    found = true;
        //                    dPtr[x + 1] = val;
        //                }
        //                else
        //                    dPtr[x + 1] = float.NaN;
        //            }
        //            if (found)
        //            {
        //                current.Index = i - 1;
        //                list.Add(current);
        //            }
        //        }

        //        return list.ToArray();
        //    }
        //}

        //internal KeyframeCollection(int capacity)
        //{
        //    _count = capacity++;

        //    for (int i = 0; i < 9; i++)
        //    {
        //        KeyframeEntry[] arr = new KeyframeEntry[capacity];

        //        arr[0] = (i < 3) ? new KeyframeEntry(0, 0, 1.0f) : new KeyframeEntry(0, 0, 0.0f);

        //        for (int x = 1; x < capacity; x++)
        //            arr[x]._value = float.NaN;

        //        _keyFrames[i] = arr;
        //    }
        //}

        //public void SetKeyFrame(KeyFrameMode mode, int index, float value)
        //{
        //    index++;
        //    if (mode >= KeyFrameMode.ScaleXYZ)
        //    {
        //        int offset = ((int)mode - 9) * 3;
        //        for (int i = 0; i < 3; i++)
        //            SetKeyframe(index, value, offset++);
        //    }
        //    else
        //        SetKeyframe(index, value, (int)mode);
        //}
        //public float GetKeyframe(KeyFrameMode mode, int index)
        //{
        //    index++;
        //    if (mode >= KeyFrameMode.ScaleXYZ)
        //        mode = (KeyFrameMode)(((int)mode - 9) * 3);

        //    return _keyFrames[(int)mode][index]._value;
        //}

        //public void RemoveKeyframe(KeyFrameMode mode, int index)
        //{
        //    index++;
        //    if (mode >= KeyFrameMode.ScaleXYZ)
        //    {
        //        int offset = ((int)mode - 9) * 3;
        //        for (int i = 0; i < 3; i++)
        //            RemoveKeyframe(index, offset++);
        //    }
        //    else
        //        RemoveKeyframe(index, (int)mode);
        //}

        //private void ResetFrames()
        //{
        //    int prev, next;
        //    KeyframeEntry[] arr;

        //    for (int i = 0; i < 9; i++)
        //    {
        //        arr = _keyFrames[i];

        //        prev = 0;
        //        next = arr[0]._next;
        //        while (next != 0)
        //        {
        //            CalcAnimFrames(prev, next, i);
        //            prev = next;
        //            next = arr[next]._next;
        //        }
        //        CalcAnimFrames(prev, next, i);
        //    }
        //}
        //private void CalcAnimFrames(int lower, int upper, int set)
        //{
        //    KeyframeEntry[] arr = _keyFrames[set];
        //    float floor = arr[lower]._value;
        //    float ceil = floor;
        //    float step;
        //    int count;
        //    int diff = upper - lower;

        //    if (upper == 0)
        //    {
        //        count = _count - lower;
        //        if (count <= 0)
        //            return;
        //    }
        //    else if (diff < 1)
        //        return;
        //    else
        //    {
        //        count = diff - 1;
        //        ceil = arr[upper]._value;
        //    }

        //    fixed (AnimationFrame* p = _frames)
        //    {
        //        float* fPtr = (float*)(p + lower) + set;

        //        if ((diff > 1) && (ceil != floor))
        //        {
        //            step = (ceil - floor) / diff;
        //            for (int i = 1; i <= count; i++, fPtr += 9)
        //                *fPtr = floor + (step * i);
        //        }
        //        else
        //            for (int i = 0; i < count; i++, fPtr += 9)
        //                *fPtr = floor;

        //        if (upper != 0)
        //            *fPtr = ceil;
        //    }
        //}

        //public void Clean()
        //{
        //    KeyframeEntry[] arr;
        //    int current, next;
        //    float left, mid, right;

        //    for (int i = 0; i < 9; i++)
        //    {
        //        arr = _keyFrames[i];
        //        left = arr[0]._value;
        //        current = arr[0]._next;

        //        //if no entries ignore
        //        if (current == 0)
        //            continue;

        //        //Account for empty first frame?

        //        mid = arr[current]._value;
        //        next = arr[current]._next;

        //        while (next != 0)
        //        {
        //            right = arr[next]._value;

        //            //Are they equal (or mostly)
        //            if ((Math.Abs(left - mid) < 0.00001f) && (Math.Abs(left - right) < 0.00001f))
        //                RemoveKeyframe(current, i);
        //            else
        //                left = mid;

        //            mid = right;
        //            current = next;
        //            next = arr[next]._next;
        //        }

        //        //Remove last frame if values are equal
        //        if (Math.Abs(left - mid) < 0.00001f)
        //            RemoveKeyframe(current, i);
        //    }
        //}

        private const float _cleanDistance = 0.00001f;
        public void Clean()
        {
            int flag, res;
            KeyframeEntry entry, root;
            for (int i = 0; i < 9; i++)
            {
                root = _keyRoots[i];

                //Eliminate redundant values
                for (entry = root._next._next; entry != root; entry = entry._next)
                {
                    flag = res = 0;

                    if (entry._prev == root)
                    {
                        if (entry._next != root)
                            flag = 1;
                    }
                    else if (entry._next == root)
                        flag = 2;
                    else
                        flag = 3;

                    if((flag & 1) != 0)
                        res |= (Math.Abs(entry._next._value - entry._value) <= _cleanDistance) ? 1 : 0;
                    
                    if((flag & 2) != 0)
                        res |= (Math.Abs(entry._prev._value - entry._value) <= _cleanDistance) ? 2 : 0;

                    if ((flag == res) && (res != 0))
                    {
                        entry = entry._prev;
                        entry._next.Remove();
                        _keyCounts[i]--;
                    }
                }
            }
        }

        public KeyframeEntry GetKeyframe(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry, root = _keyRoots[(int)mode & 0xF];
            for (entry = root._next; (entry != root) && (entry._index < index); entry = entry._next) ;
            if (entry._index == index)
                return entry;
            return null;
        }
        public float GetFrameValue(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry, root = _keyRoots[(int)mode & 0xF];

            if (index >= root._prev._index)
                return root._prev._value;
            if (index <= root._next._index)
                return root._next._value;

            for (entry = root._next; (entry != root) && (entry._index < index); entry = entry._next) ;

            if (entry._index == index)
                return entry._value;

            return entry._prev.Interpolate(index - entry._prev._index, _linearRot);
        }
        public KeyframeEntry SetFrameValue(KeyFrameMode mode, int index, float value)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];

                if ((root._prev == root) || (root._prev._index < index))
                    entry = root;
                else
                    for (entry = root._next; (entry != root) && (entry._index <= index); entry = entry._next) ;

                entry = entry._prev;
                if (entry._index != index)
                {
                    _keyCounts[x]++;
                    entry.InsertAfter(entry = new KeyframeEntry(index, value));
                }
                else
                    entry._value = value;
            }
            return entry;
        }

        //private void SetKeyframe(int index, float value, int set)
        //{
        //    if ((index > _count) || (index < 1))
        //        return;

        //    KeyframeEntry entry, root = _keyRoots[set];
        //    if (root == null)
        //    {

        //    }

        //    KeyframeEntry[] arr = _keyFrames[set];
        //    int prev, next;

        //    if (float.IsNaN(arr[index]._value))
        //    {
        //        prev = 0;
        //        next = arr[0]._next;
        //        while ((index > next) && (next != 0))
        //        {
        //            prev = next;
        //            next = arr[next]._next;
        //        }
        //        arr[index] = new KeyframeEntry(prev, next, value);
        //        arr[prev]._next = (ushort)index;
        //        arr[next]._prev = (ushort)index;
        //    }
        //    else
        //    {
        //        arr[index]._value = value;
        //        prev = arr[index]._prev;
        //        next = arr[index]._next;
        //    }

        //    if (_frames != null)
        //    {
        //        CalcAnimFrames(prev, index, set);
        //        CalcAnimFrames(index, next, set);
        //    }
        //}
        //private void RemoveKeyframe(int index, int offset)
        //{
        //    if ((index > _count) || (index < 1))
        //        return;

        //    KeyframeEntry[] arr = _keyFrames[offset];
        //    int prev, next;

        //    if (float.IsNaN(arr[index]._value))
        //        return;

        //    prev = arr[index]._prev;
        //    next = arr[index]._next;
        //    arr[index]._value = float.NaN;
        //    arr[prev]._next = (ushort)next;
        //    arr[next]._prev = (ushort)prev;

        //    if (_frames != null)
        //        CalcAnimFrames(prev, next, offset);
        //}

        //private void MoveKeyframe(int index, int offset, int newIndex)
        //{
        //    KeyframeEntry[] arr = _keyFrames[offset];
        //    KeyframeEntry entry = arr[index];

        //    if (float.IsNaN(entry._value))
        //        return;

        //    arr[newIndex] = entry;
        //    arr[entry._prev]._next = (ushort)newIndex;
        //    arr[entry._next]._prev = (ushort)newIndex;
        //    arr[index] = KeyframeEntry.Empty;

        //    if (_frames != null)
        //    {
        //        CalcAnimFrames(entry._prev, newIndex, offset);
        //        CalcAnimFrames(newIndex, entry._next, offset);
        //    }
        //}

        //public void SetSize(int count)
        //{
        //    if (_count == count)
        //        return;

        //    AnimationFrame[] frames;
        //    KeyframeEntry[] arr, narr;
        //    int cap = count + 1;
        //    int min = Math.Min(_count + 1, cap);
        //    int prev;

        //    _count = count;

        //    //Copy animation frames if they exist
        //    if (_frames != null)
        //    {
        //        frames = new AnimationFrame[count];
        //        Array.Copy(_frames, frames, min - 1);
        //        _frames = frames;
        //    }

        //    for (int i = 0; i < 9; i++)
        //    {
        //        arr = _keyFrames[i];
        //        narr = new KeyframeEntry[cap];

        //        prev = arr[0]._prev;
        //        //pull down ends if smaller
        //        if (prev > count)
        //        {
        //            while (prev > count)
        //                prev = arr[prev]._prev;

        //            arr[0]._prev = (ushort)prev;
        //            arr[prev]._next = 0;
        //        }

        //        //Copy elements
        //        Array.Copy(arr, narr, min);

        //        //Zero remainder
        //        for (int x = min; x < cap; x++)
        //            narr[x]._value = float.NaN;

        //        _keyFrames[i] = narr;

        //        //Update anim frames
        //        if (_frames != null)
        //            CalcAnimFrames(prev, 0, i);
        //    }
        //}

        public AnimationFrame GetFullFrame(int index)
        {
            AnimationFrame frame;
            float* dPtr = (float*)&frame;
            for (int x = 0x10; x < 0x19; x++)
                *dPtr++ = GetFrameValue((KeyFrameMode)x, index);
            return frame;
        }

        public KeyframeEntry Remove(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];

                for (entry = root._next; (entry != root) && (entry._index < index); entry = entry._next) ;

                if (entry._index == index)
                {
                    entry.Remove();
                    _keyCounts[x]--;
                }
                else
                    entry = null;
            }
            return entry;
        }

        public void Insert(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];
                for (entry = root._prev; (entry != root) && (entry._index >= index); entry = entry._prev)
                    if (++entry._index >= _frameLimit)
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyCounts[x]--;
                    }
            }
        }

        public void Delete(KeyFrameMode mode, int index)
        {
            KeyframeEntry entry = null, root;
            for (int x = (int)mode & 0xF, y = x + ((int)mode >> 4); x < y; x++)
            {
                root = _keyRoots[x];
                for (entry = root._prev; (entry != root) && (entry._index >= index); entry = entry._prev)
                    if (--entry._index < 0)
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyCounts[x]--;
                    }
            }
        }

        //public void Insert(int index)
        //{
        //    index++;
        //    if ((index > _count) || (index < 1))
        //        return;

        //    for (int i = 0; i < 9; i++)
        //        ShiftRight(index, i);
        //}
        //public void Insert(KeyFrameMode mode, int index)
        //{
        //    index++;
        //    if ((index > _count) || (index < 1))
        //        return;

        //    if (mode >= KeyFrameMode.ScaleXYZ)
        //    {
        //        int offset = ((int)mode - 9) * 3;
        //        for (int i = 0; i < 3; i++)
        //            ShiftRight(index, offset++);
        //    }
        //    ShiftRight(index, (int)mode);
        //}

        //public void Delete(int index)
        //{
        //    index++;
        //    if ((index > _count) || (index < 1))
        //        return;

        //    for (int i = 0; i < 9; i++)
        //        ShiftLeft(index, i);
        //}
        //public void Delete(KeyFrameMode mode, int index)
        //{
        //    index++;
        //    if ((index > _count) || (index < 1))
        //        return;

        //    if (mode >= KeyFrameMode.ScaleXYZ)
        //    {
        //        int offset = ((int)mode - 9) * 3;
        //        for (int i = 0; i < 3; i++)
        //            ShiftLeft(index, offset++);
        //    }
        //    ShiftLeft(index, (int)mode);
        //}

        //private void ShiftRight(int index, int offset)
        //{
        //    KeyframeEntry[] arr = _keyFrames[offset];

        //    //Delete last keyframe
        //    RemoveKeyframe(_count, offset);

        //    //Move remaining keyframes
        //    for (int i = _count; --i >= index; )
        //        MoveKeyframe(i, offset, i + 1);
        //}

        //private void ShiftLeft(int index, int offset)
        //{
        //    KeyframeEntry[] arr = _keyFrames[offset];

        //    //Delete keyframe
        //    RemoveKeyframe(index, offset);

        //    //Move remaining keyframes
        //    for (int i = index; i++ < _count; )
        //        MoveKeyframe(i, offset, i - 1);
        //}
    }

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class KeyframeEntry
    {
        public int _index;
        public KeyframeEntry _prev, _next;

        public float _value;
        public float _tangent;

        //public KeyframeEntry()
        //{
        //    _prev = _next = this;
        //    _value = _tangent = 0.0f;
        //}
        //public KeyframeEntry(float value) { _prev = _next = this; _value = value; _index = -1; }
        public KeyframeEntry(int index, float value)
        {
            _index = index;
            _prev = _next = this;
            _value = value;
        }

        //public KeyframeEntry(int prev, int next, float value)
        //{
        //    _prev = (ushort)prev;
        //    _next = (ushort)next;
        //    _value = value;
        //}

        public void InsertBefore(KeyframeEntry entry)
        {
            _prev._next = entry;
            entry._prev = _prev;
            entry._next = this;
            _prev = entry;
        }
        public void InsertAfter(KeyframeEntry entry)
        {
            _next._prev = entry;
            entry._next = _next;
            entry._prev = this;
            _next = entry;
        }
        public void Remove()
        {
            _next._prev = _prev;
            _prev._next = _next;
            //_prev = _next = this;
        }

        public float Interpolate(int offset, bool linear)
        {
            if (offset == 0)
                return _value;

            int span = _next._index - _index;
            if (offset == span)
                return _next._value;

            float diff = _next._value - _value;
            if (linear)
                return _value + (diff / span * offset);

            float time = (float)offset / span;
            float inv = time - 1.0f;
            return (offset * inv * ((inv * _tangent) + (time * _next._tangent)))
                + ((time * time) * (3.0f - 2.0f * time) * diff)
                + _value;
        }

        public float GenerateTangent()
        {
            float tan = 0.0f;
            if(_prev._index != -1)
                tan += (_value - _prev._value) / (_index - _prev._index);
            if (_next._index != -1)
            {
                tan += (_next._value - _value) / (_next._index - _index);
                if (_prev._index != -1)
                    tan *= 0.5f;
            }
            return _tangent = tan;
        }

        //public override string ToString()
        //{
        //    return String.Format("Prev={0}, Next={1}, {2}", _prev, _next, _value);
        //}
    }
}
