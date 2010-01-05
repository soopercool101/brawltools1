using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Animations
{
    internal static unsafe class AnimationConverter
    {
        public static KeyframeCollection DecodeKeyframes(CHR0Entry* entry, int numFrames)
        {
            KeyframeCollection kf = new KeyframeCollection(numFrames);
            bfloat* sPtr = (bfloat*)entry->Data;
            AnimationCode code = entry->Code;
            AnimDataFormat format;

            if (code.HasScale)
            {
                format = code.ScaleDataFormat;
                if (code.IsScaleIsotropic)
                {
                    if (code.IsScaleXFixed)
                        kf.SetKeyFrame(KeyFrameMode.ScaleXYZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.ScaleXYZ);
                }
                else
                {
                    if (code.IsScaleXFixed)
                        kf.SetKeyFrame(KeyFrameMode.ScaleX, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.ScaleX);

                    if (code.IsScaleYFixed)
                        kf.SetKeyFrame(KeyFrameMode.ScaleY, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.ScaleY);

                    if (code.IsScaleZFixed)
                        kf.SetKeyFrame(KeyFrameMode.ScaleZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.ScaleZ);
                }
            }

            if (code.HasRotation)
            {
                format = code.RotationDataFormat;
                if (code.IsRotationIsotropic)
                {
                    if (code.IsRotationXFixed)
                        kf.SetKeyFrame(KeyFrameMode.RotXYZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.RotXYZ);
                }
                else
                {
                    if (code.IsRotationXFixed)
                        kf.SetKeyFrame(KeyFrameMode.RotX, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.RotX);

                    if (code.IsRotationYFixed)
                        kf.SetKeyFrame(KeyFrameMode.RotY, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.RotY);

                    if (code.IsRotationZFixed)
                        kf.SetKeyFrame(KeyFrameMode.RotZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.RotZ);
                }
            }

            if (code.HasTranslation)
            {
                format = code.TranslationDataFormat;
                if (code.IsTranslationIsotropic)
                {
                    if (code.IsTranslationXFixed)
                        kf.SetKeyFrame(KeyFrameMode.TransXYZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.TransXYZ);
                }
                else
                {
                    if (code.IsTranslationXFixed)
                        kf.SetKeyFrame(KeyFrameMode.TransX, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.TransX);

                    if (code.IsTranslationYFixed)
                        kf.SetKeyFrame(KeyFrameMode.TransY, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.TransY);

                    if (code.IsTranslationZFixed)
                        kf.SetKeyFrame(KeyFrameMode.TransZ, 0, *sPtr++);
                    else
                        DecodeFrames(kf, (VoidPtr)entry + *(buint*)sPtr++, format, KeyFrameMode.TransZ);
                }
            }

            return kf;
        }

        private static void DecodeFrames(KeyframeCollection kf, void* dataAddr, AnimDataFormat format, KeyFrameMode mode)
        {
            int fCount;
            float vStep, vBase;
            switch (format)
            {
                case AnimDataFormat.F3F:
                    {
                        F3FHeader* header = (F3FHeader*)dataAddr;
                        fCount = header->_numFrames;

                        F3FEntry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                            kf.SetKeyFrame(mode, (int)entry->_index, entry->_value);
                        break;
                    }
                case AnimDataFormat.F4B:
                    {
                        F4BHeader* header = (F4BHeader*)dataAddr;
                        fCount = header->_entries;
                        vStep = header->_step;
                        vBase = header->_base;

                        F4BEntry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                            kf.SetKeyFrame(mode, entry->FrameIndex, vBase + (entry->Step * vStep));
                        break;
                    }
                case AnimDataFormat.F6B:
                    {
                        F6BHeader* header = (F6BHeader*)dataAddr;
                        fCount = header->_numFrames;
                        vStep = header->_step;
                        vBase = header->_base;

                        F6BEntry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                            kf.SetKeyFrame(mode, entry->FrameIndex, vBase + (entry->_step * vStep));
                        break;
                    }
                case AnimDataFormat.F1B:
                    {
                        F1BHeader* header = (F1BHeader*)dataAddr;
                        vStep = header->_step;
                        vBase = header->_base;

                        byte* sPtr = header->Data;
                        for (int i = 0; i < kf.Count; i++)
                            kf.SetKeyFrame(mode, i, vBase + (*sPtr++ * vStep));

                        break;
                    }
                case AnimDataFormat.F1F:
                    {
                        bfloat* sPtr = (bfloat*)dataAddr;

                        for (int i = 0; i < kf.Count; i++)
                            kf.SetKeyFrame(mode, i, *sPtr++);

                        break;
                    }
            }

        }

        public static int CalculateSize(KeyframeCollection kf, out int entrySize)
        {
            int dataSize = 0;
            entrySize = 8;
            int count = kf.Count;

#if DEBUG
            kf.Clean();
#endif
            kf._evalCode = AnimationCode.Default;

            for (int i = 0; i < 3; i++)
                dataSize += EvaluateGroup(ref kf._evalCode, kf, i, ref entrySize);

            return dataSize;
        }

        private const float scaleError = 0.000637755f;
        private static int EvaluateGroup(ref AnimationCode code, KeyframeCollection kf, int group, ref int entrySize)
        {
            int index = group * 3;
            int numFrames = kf.Count;
            int dataLen = 0;
            int maxEntries;
            int evalCount;
            int scaleSpan;
            //bool useLinear = group == 1;

            bool exist = false;
            bool isotropic = group == 0;
            AnimDataFormat format = AnimDataFormat.None;

            KeyframeEntry[][] arr = new KeyframeEntry[3][];
            int* count = stackalloc int[3];
            bool* isExist = stackalloc bool[3];
            bool* isFixed = stackalloc bool[3];
            bool* isScalable = stackalloc bool[3];
            float* floor = stackalloc float[3];
            float* ceil = stackalloc float[3];

            int* orders = stackalloc int[numFrames * 3];
            int* oPtr = stackalloc int[3];
            float* values = stackalloc float[numFrames * 3];
            int* vPtr = stackalloc int[3];

            int** order = (int**)oPtr;
            float** value = (float**)vPtr;

            KeyframeEntry entry;
            int eCount = 0;

            //KeyframeEntry* entry = (KeyframeEntry*)kfData;

            //Initialize values
            for (int i = 0; i < 3; i++)
            {
                arr[i] = kf._keyFrames[index + i];
                entry = arr[i][0];
                isExist[i] = entry._next != 0;
                isFixed[i] = isExist[i] ? (entry._next == 1 && entry._prev == 1) : true;

                order[i] = orders + (i * numFrames);
                value[i] = values + (i * numFrames);
            }

            if (exist = isExist[0] || isExist[1] || isExist[2])
            {
                if ((isFixed[0] != isFixed[1]) || (isFixed[0] != isFixed[2]))
                    isotropic = false;

                int maxIndex = 0;
                for (int i = 0; i < 3; i++)
                {
                    int* pOrder = order[i];
                    float* pValue = value[i];
                    float v;
                    float min = float.MaxValue, max = float.MinValue;
                    eCount = 0;

                    for (int x = arr[i][0]._next; x != 0; x = arr[i][x]._next, eCount++)
                    {
                        *pOrder++ = x;
                        maxIndex = Math.Max(maxIndex, x);

                        *pValue++ = v = arr[i][x]._value;
                        min = Math.Min(min, v);
                        max = Math.Max(max, v);
                    }

                    //for (entry = arr[i][0]; entry._next != 0; eCount++)
                    //{
                    //    entry = arr[i][index = entry._next];
                    //    *pOrder++ = index;
                    //    *pValue++ = v = entry._value;
                    //    maxIndex = Math.Max(maxIndex, index);
                    //    min = Math.Min(min, v);
                    //    max = Math.Max(max, v);
                    //}
                    count[i] = eCount;
                    floor[i] = min;
                    ceil[i] = max;
                }

                //Compare orders/values/counts to see whether or not it's isotropic
                if ((count[0] != count[1]) || (count[0] != count[2]))
                    isotropic = false;
                else
                    for (int i = 0; i < eCount; i++)
                        if ((order[0][i] != order[1][i]) || (order[0][i] != order[2][i])
                            || (value[0][i] != value[1][i]) || (value[0][i] != value[2][i]))
                        {
                            isotropic = false;
                            break;
                        }

                if (isotropic)
                {
                    evalCount = 1;
                    maxEntries = count[0];
                    //useLinear &= count[0] == numFrames;
                }
                else
                {
                    evalCount = 3;
                    maxEntries = Math.Max(Math.Max(count[0], count[1]), count[2]);
                    //useLinear &= (count[0] == numFrames) && (count[1] == numFrames) && (count[2] == numFrames);
                }

                scaleSpan = (group == 1) ? 255 : (maxIndex <= 255) ? 4095 : (maxIndex <= 2047) ? 65535 : -1;
                //scaleSpan = useLinear ? 255 : 4095;

                //Determine if values are scalable
                for (int i = 0; i < evalCount; i++)
                {
                    isScalable[i] = true;
                    if ((isFixed[i]) || (scaleSpan == -1))
                        continue;

                    float* pValue = value[i];
                    eCount = count[i];

                    float basev, range, step, distance, val;

                    basev = floor[i];
                    range = ceil[i] - basev;

                    if (range == 0.0f)
                        continue;

                //Evaluate spans until we reach a success.
                //A success means that compression using that span is possible. 
                //No further evaluation necessary.
                SpanBegin:
                    int span = scaleSpan;
                    int spanEval = scaleSpan - 32;

                SpanStep:
                    if (span > spanEval)
                    {
                        step = range / span;
                        //calculate error
                        for (int x = 0; x < eCount; x++)
                        {
                            val = pValue[x];
                            distance = ((val - basev) / step) + 0.5f;
                            distance = Math.Abs(val - (basev + ((int)distance * step)));

                            //If distance is too large change span and retry
                            if (distance > scaleError)
                            {
                                span--;
                                goto SpanStep;
                            }
                        }
                    }
                    else
                    {
                        if ((scaleSpan <= 255) && (maxIndex <= 255))
                            scaleSpan = 4095;
                        else if ((scaleSpan <= 4095) && (maxIndex <= 2047))
                            scaleSpan = 65535;
                        else
                        {
                            scaleSpan = -1;
                            isScalable[i] = false;
                            continue;
                        }
                        goto SpanBegin;
                    }
                }

                //Determine format only if there are unfixed entries
                if (!isFixed[0] || !isFixed[1] || !isFixed[2])
                {
                    bool scale = (isotropic) ? isScalable[0] : (isScalable[0] && isScalable[1] && isScalable[2]);
                    float frameSpan = (float)numFrames / maxEntries;

                    if (scale)
                    {
                        if ((group == 1) && (scaleSpan <= 255) && (frameSpan < 4.0f))
                            format = AnimDataFormat.F1B;
                        else if ((scaleSpan <= 4095) && (maxIndex <= 255))
                            format = AnimDataFormat.F4B;
                        else if ((frameSpan > 1.5f) && (maxIndex <= 2047))
                            format = AnimDataFormat.F6B;
                        else if ((group == 1) && (frameSpan <= 3.0f))
                            format = AnimDataFormat.F1F;
                        else
                            format = AnimDataFormat.F3F;
                    }
                    else if ((group == 1) && (frameSpan <= 3.0f))
                        format = AnimDataFormat.F1F;
                    else
                        format = AnimDataFormat.F3F;

                    //else
                    //{
                    //    if ((group != 1) || (frameSpan > 3.0f))
                    //        format = AnimDataFormat.F3F;
                    //    else
                    //        format = AnimDataFormat.F1F;
                    //}

                    //if (useLinear) //rotation
                    //{
                    //    if (scaleSpan == 255)
                    //        format = AnimDataFormat.F1B;
                    //    else
                    //        format = AnimDataFormat.F1F;
                    //}
                    //else
                    //{
                    //    if (scale)
                    //    {
                    //        if (scaleSpan == 4095)
                    //            format = AnimDataFormat.F4B;
                    //        else
                    //            format = AnimDataFormat.F6B;
                    //    }
                    //    else
                    //        format = AnimDataFormat.F3F;
                    //}
                }

                //calculate size
                for (int i = 0; i < evalCount; i++)
                {
                    entrySize += 4;

                    if (!isFixed[i])
                    {
                        switch (format)
                        {
                            case AnimDataFormat.F3F:
                                dataLen += 8 + (count[i] * 12);
                                break;

                            case AnimDataFormat.F4B:
                                dataLen += 16 + (count[i] * 4);
                                break;

                            case AnimDataFormat.F6B:
                                dataLen += (16 + (count[i] * 6)).Align(4);
                                break;

                            case AnimDataFormat.F1B:
                                dataLen += (8 + numFrames).Align(4);
                                break;

                            case AnimDataFormat.F1F:
                                dataLen += numFrames * 4;
                                break;
                        }
                    }
                }
                //Should we compress here?
            }
            else //Set isotropic to true, so it sets the default value.
                isotropic = true;

            //Set scale flag
            if (group == 0)
                code.IgnoreScale = !exist;

            //Set data flags
            if (exist)
                code.ExtraData = 0;

            code.SetExists(group, exist);
            //code.SetIgnore(group, !exist);
            code.SetIsIsotropic(group, isotropic);
            for (int i = 0; i < 3; i++)
                code.SetIsFixed(index + i, isFixed[i]);
            code.SetFormat(group, format);

            return dataLen;
        }


        public static void EncodeKeyframes(KeyframeCollection kf, VoidPtr entryAddress, VoidPtr dataAddress)
        {
            AnimationCode code = kf._evalCode;
            //VoidPtr dataAddr = addr + 8;

            CHR0Entry* header = (CHR0Entry*)entryAddress;
            header->_code = code._data;
            header->_stringOffset = 0;

            //entryAddress += 8;
            bint* pOffset = (bint*)entryAddress + 2;

            //Calculate lookup size and set data address
            //for (int i = 0; i < 3; i++)
            //{
            //    if (code.GetExists(i))
            //    {
            //        if (code.GetIsIsotropic(i))
            //            dataAddr += 4;
            //        else
            //            dataAddr += 12;
            //    }
            //}

            //Write values/offset and encode groups
            for (int i = 0, x = 0; i < 3; i++, x += 3)
            {
                if (code.GetExists(i))
                {
                    AnimDataFormat format = code.GetFormat(i);
                    if (code.GetIsIsotropic(i))
                    {
                        if (code.GetIsFixed(x))
                            *(bfloat*)pOffset++ = kf._keyFrames[x][kf._keyFrames[x][0]._next]._value;
                        else
                        {
                            *pOffset++ = (int)(dataAddress - entryAddress);
                            dataAddress += EncodeEntry(x, format, kf, dataAddress);
                        }
                        //entryAddress += 4;
                    }
                    else
                    {
                        for (int y = 0, z = x; y < 3; y++, z++)
                        {
                            if (code.GetIsFixed(z))
                                *(bfloat*)pOffset++ = kf._keyFrames[z][kf._keyFrames[z][0]._next]._value;
                            else
                            {
                                *pOffset++ = (int)(dataAddress - entryAddress);
                                dataAddress += EncodeEntry(z, format, kf, dataAddress);
                            }
                        }
                        //entryAddress += 12;
                    }
                }
            }
        }
        private static int EncodeEntry(int index, AnimDataFormat format, KeyframeCollection kf, VoidPtr addr)
        {
            int numFrames = kf.Count;
            KeyframeEntry[] arr = kf._keyFrames[index];
            KeyframeEntry frame;
            bfloat* pVal = (bfloat*)addr;
            float val;
            float min = float.MaxValue, max = float.MinValue, stride, step;
            int span, i;
            float* fPtr;

            if (format == AnimDataFormat.F1F)
            {
                //Use anim frames, just in case not all frames are key.
                fixed (AnimationFrame* p = kf.AnimFrames)
                {
                    fPtr = (float*)p + index;
                    for (i = 0; i < numFrames; i++, fPtr += 9)
                        *pVal++ = *fPtr;
                }

                //for (int i = 1; i <= numFrames; i++)
                //    *pVal++ = arr[i]._value;
                return numFrames * 4;
            }

            if (format == AnimDataFormat.F1B)
            {
                float* value = stackalloc float[numFrames];

                //Use anim frames, just in case not all frames are key.
                fixed (AnimationFrame* p = kf.AnimFrames)
                {
                    fPtr = (float*)p + index;
                    //Get min/max
                    for (i = 0; i < numFrames; i++, fPtr += 9)
                    {
                        value[i] = val = *fPtr;
                        if (val > max) max = val;
                        if (val < min) min = val;
                    }
                }
                stride = max - min;

                //Find best span
                span = EvalSpan(255, 32, min, stride, value, numFrames);
                step = stride / span;

                F1BHeader* header = (F1BHeader*)addr;
                *header = new F1BHeader(step, min);

                byte* dPtr = header->Data;
                for (i = 0; i < numFrames; i++)
                    *dPtr++ = (byte)(((*value++ - min) / step) + 0.5f);

                //Fill remaining bytes
                while ((i++ & 3) != 0)
                    *dPtr++ = 0;

                return (8 + numFrames).Align(4);

            }

            {
                float* value = stackalloc float[numFrames];
                int* order = stackalloc int[numFrames];
                int keyCount = 0, next;
                float frameScale = 1.0f / numFrames;

                for (frame = arr[0], next = frame._next; next != 0; next = frame._next)
                {
                    order[keyCount] = next - 1;
                    frame = arr[next];
                    value[keyCount++] = val = frame._value;
                    min = Math.Min(min, val);
                    max = Math.Max(max, val);
                }

                if (format == AnimDataFormat.F3F)
                {
                    F3FHeader* header = (F3FHeader*)addr;
                    *header = new F3FHeader(keyCount, frameScale);

                    F3FEntry* entry = header->Data;
                    for (i = 0; i < keyCount; i++)
                        *entry++ = new F3FEntry(*order++, *value++, 0.0f);

                    return keyCount * 12 + 8;
                }

                stride = max - min;
                //if (format == AnimDataFormat.F1B)
                //{
                //    //Find best span
                //    span = EvalSpan(255, 32, min, stride, value, keyCount);
                //    step = stride / span;

                //    F1BHeader* header = (F1BHeader*)addr;
                //    *header = new F1BHeader(step, min);

                //    int i;
                //    byte* dPtr = header->Data;
                //    for (i = 0; i < keyCount; i++)
                //        *dPtr++ = (byte)(((*value++ - min) / step) + 0.5f);

                //    //Fill remaining bytes
                //    while (i++ % 4 != 0)
                //        *dPtr++ = 0;

                //    return (8 + numFrames).Align(4);
                //}

                if (format == AnimDataFormat.F4B)
                {
                    //Find best span
                    span = EvalSpan(4095, 32, min, stride, value, keyCount);
                    step = stride / span;

                    F4BHeader* header = (F4BHeader*)addr;
                    *header = new F4BHeader(keyCount, frameScale, step, min);

                    F4BEntry* entry = header->Data;
                    for (i = 0; i < keyCount; i++)
                        *entry++ = new F4BEntry(*order++, (int)((*value++ - min) / step + 0.5f), 0);//(int)((0 - min) / step + 0.5f));

                    return keyCount * 4 + 16;
                }

                if (format == AnimDataFormat.F6B)
                {
                    //Find best span
                    span = EvalSpan(65535, 32, min, stride, value, keyCount);
                    step = stride / span;

                    F6BHeader* header = (F6BHeader*)addr;
                    *header = new F6BHeader(keyCount, frameScale, step, min);

                    F6BEntry* entry = header->Data;
                    for (i = 0; i < keyCount; i++)
                        *entry++ = new F6BEntry(*order++, (int)((*value++ - min) / step + 0.5f), 0);//(int)((0 - min) / step + 0.5f));

                    //Fill remaining bytes
                    if ((i & 1) == 0)
                        entry->_data = 0;

                    return ((keyCount * 6) + 16).Align(4);
                }
            }

            return 0;
        }

        public static int EvalSpan(int maxSpan, int maxIterations, float valBase, float valStride, float* values, int count)
        {
            if (maxIterations <= 0)
                maxIterations = maxSpan - 2;

            float bestError = float.MaxValue;
            int bestSpan = maxSpan;
            for (int i = 0; i < maxIterations; i++)
            {
                float worstError = float.MinValue;
                float step = valStride / maxSpan;
                for (int x = 0; x < count; x++)
                {
                    float error = ((values[x] - valBase) / step) + 0.5f;
                    error = Math.Abs(values[x] - (valBase + ((int)error * step)));
                    if (error > worstError)
                        worstError = error;
                    if (error > scaleError)
                        break;
                }
                if (worstError < bestError)
                {
                    bestError = worstError;
                    bestSpan = maxSpan;
                }
                maxSpan--;
            }

            return bestSpan;
        }
    }
}
