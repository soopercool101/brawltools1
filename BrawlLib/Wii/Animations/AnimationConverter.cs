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
            switch (format)
            {
                case AnimDataFormat.F3F:
                    {
                        int fCount = *(bushort*)dataAddr;
                        bfloat* sPtr = (bfloat*)dataAddr + 2;
                        for (int i = 0; i < fCount; i++)
                        {
                            int frameIndex = (int)*sPtr++;
                            kf.SetKeyFrame(mode, frameIndex, *sPtr++);
                            sPtr++;
                        }
                        break;
                    }
                case AnimDataFormat.F4B:
                    {
                        int fCount = *(bushort*)dataAddr;
                        bint* sPtr = (bint*)dataAddr + 2;
                        float vStep = *(bfloat*)sPtr++;
                        float vBase = *(bfloat*)sPtr++;
                        for (int i = 0; i < fCount; i++)
                        {
                            int v = *sPtr++;
                            int frameIndex = (v >> 24) & 0xFF;
                            int value = (v >> 12) & 0xFFF;
                            kf.SetKeyFrame(mode, frameIndex, vBase + (value * vStep));
                        }
                        break;
                    }
                case AnimDataFormat.F6B:
                    {
                        int fCount = *(bushort*)dataAddr;

                        float vStep = *((bfloat*)dataAddr + 2);
                        float vBase = *((bfloat*)dataAddr + 3);

                        bushort* sPtr = (bushort*)dataAddr + 8;
                        for (int i = 0; i < fCount; i++)
                        {
                            int frameIndex = *sPtr++ >> 5;
                            kf.SetKeyFrame(mode, frameIndex, vBase + (*sPtr++ * vStep));
                            sPtr++;
                        }
                        break;
                    }
                case AnimDataFormat.F1B:
                    {
                        float vStep = *(bfloat*)dataAddr;
                        float vBase = *((bfloat*)dataAddr + 1);
                        byte* sPtr = (byte*)dataAddr + 8;

                        for (int i = 0; i < kf.Count; i++)
                            kf.SetKeyFrame(mode, i, vBase + (*sPtr++ * vStep));

                        break;
                    }
                case AnimDataFormat.F1F:
                    {
                        float* sPtr = (float*)dataAddr;

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
            kf._evalCode = AnimationCode.Default;


            for (int i = 0; i < 3; i++)
                dataSize += EvaluateGroup(ref kf._evalCode, kf, i, ref entrySize);

            if (kf._evalCode != AnimationCode.Default)
                kf._evalCode.ExtraData = 0x09;

            return dataSize;
        }

        private const float scaleError = 0.00005f;
        private static int EvaluateGroup(ref AnimationCode code, KeyframeCollection kf, int group, ref int entrySize)
        {
            int index = group * 3;
            int numFrames = kf.Count;
            int dataLen = 0;

            bool exist = false;
            bool isotropic = true;
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

                for (int i = 0; i < 3; i++)
                {
                    int* pOrder = order[i];
                    float* pValue = value[i];
                    float v;
                    float min = float.MaxValue, max = float.MinValue;
                    eCount = 0;

                    for (entry = arr[i][0]; entry._next != 0; eCount++)
                    {
                        *pOrder++ = entry._next;
                        entry = arr[i][entry._next];
                        v = entry._value;
                        *pValue++ = v;
                        min = Math.Min(min, v);
                        max = Math.Max(max, v);
                    }
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

                int evalCount = isotropic ? 1 : 3;
                int maxEntries = (isotropic) ? count[0] : (Math.Max(Math.Max(count[0], count[1]), count[2]));
                int scaleSpan = (group == 1) ? 254 : (maxEntries > 255 ? 65534 : 4094);

                //Determine if values are scalable
                for (int i = 0; i < evalCount; i++)
                {
                    isScalable[i] = true;
                    if ((isFixed[i]) || (scaleSpan == -1))
                        continue;

                    float* pValue = value[i];
                    eCount = count[i];

                    float basev, range, step, distance, val;

                    //Determine step by getting range and dividing by max index (span)
                    basev = floor[i];
                    range = ceil[i] - basev;
                    if (range != 0.0f)
                    {
                    SpanBegin:

                        step = range / scaleSpan;

                        //calculate error
                        for (int x = 0; x < eCount; x++)
                        {
                            val = pValue[x];
                            distance = ((val - basev) / step) + 0.5f;
                            distance = Math.Abs(val - (basev + ((int)distance * step)));

                            //If distance is too large change span and retry
                            if (distance > scaleError)
                            {
                                if (scaleSpan == 254)
                                    scaleSpan = maxEntries > 255 ? 65534 : 4094;
                                else if (scaleSpan == 4094)
                                    scaleSpan = 65534;
                                else
                                {
                                    scaleSpan = -1;
                                    isScalable[i] = false;
                                    break;
                                }
                                goto SpanBegin;
                            }
                            //scaleSpan = Math.Max(scaleSpan, step);
                        }
                    }
                }

                //Determine format only if there are unfixed entries
                if (!isFixed[0] || !isFixed[1] || !isFixed[2])
                {
                    bool scale = (isotropic) ? isScalable[0] : (isScalable[0] && isScalable[1] && isScalable[2]);
                    if (group == 1) //rotation
                    {
                        float entryScale = (float)numFrames / maxEntries;

                        if (scale)
                        {
                            if ((entryScale <= 4.0f) && (scaleSpan == 254))
                                format = AnimDataFormat.F1B;
                            else if (scaleSpan <= 4094) // ((entryScale > 1.0f) && (scaleSpan <= 4094))
                                format = AnimDataFormat.F4B;
                            else// if (entryScale >= 1.6f)
                                format = AnimDataFormat.F6B;
                            //else
                            //    format = AnimDataFormat.F1F;

                            //We don't want to fall back to F1F because then every frame will be key.
                            //This would ruin organization, and I don't want to interpolate on build...
                        }
                        else if (entryScale == 1.0f)
                            format = AnimDataFormat.F1F;
                        else
                            format = AnimDataFormat.F3F;
                    }
                    else
                    {
                        if (scale)
                        {
                            if (scaleSpan <= 4094)
                                format = AnimDataFormat.F4B;
                            else
                                format = AnimDataFormat.F6B;
                        }
                        else
                            format = AnimDataFormat.F3F;
                    }
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

            code.SetExists(group, exist);
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
            KeyframeEntry entry;
            bfloat* pVal = (bfloat*)addr;


            if (format == AnimDataFormat.F1F)
            {
                for (int i = 1; i < numFrames; i++)
                    *pVal++ = arr[i]._value;
                return numFrames * 4;
            }

            float val;
            float min = float.MaxValue, max = float.MinValue, step;
            float* value = stackalloc float[numFrames];
            int* order = stackalloc int[numFrames];
            int keyCount = 0, next;

            for (entry = arr[0], next = entry._next; next != 0; next = entry._next )
            {
                order[keyCount] = next - 1;
                entry = arr[next];
                value[keyCount++] = val = entry._value;
                min = Math.Min(min, val);
                max = Math.Max(max, val);
            }

            if (format == AnimDataFormat.F3F)
            {
                *(bushort*)pVal = (ushort)keyCount;
                pVal += 2;

                for (int i = 0; i < keyCount; i++)
                {
                    *pVal++ = *order++;
                    *pVal++ = *value++;
                    *pVal++ = 0;
                }

                return keyCount * 12 + 8;
            }
            else if (format == AnimDataFormat.F1B)
            {
                step = (max - min) / 254;

                *pVal++ = step;
                *pVal++ = min;
                byte* dPtr = (byte*)pVal;
                int i;
                for (i = 0; i < numFrames; i++)
                    *dPtr++ = (byte)(((*value++ - min) / step) + 0.5f);

                //Fill remaining bytes
                while (i++ % 4 != 0)
                    *dPtr++ = 0;

                return (8 + numFrames).Align(4);
            }
            else if (format == AnimDataFormat.F4B)
            {
                step = (max - min) / 4094;

                *(bushort*)pVal = (ushort)keyCount;
                pVal += 2;
                *pVal++ = step;
                *pVal++ = min;
                bint* dPtr = (bint*)pVal;
                for (int i = 0; i < keyCount; i++)
                    *dPtr++ = (*order++ << 24) | ((int)(((*value++ - min) / step) + 0.5f) << 12);

                return keyCount * 4 + 16;
            }
            else if (format == AnimDataFormat.F6B)
            {
                step = (max - min) / 65534;

                *(bushort*)pVal = (ushort)keyCount;
                pVal += 2;
                *pVal++ = step;
                *pVal++ = min;
                bushort* dPtr = (bushort*)pVal;
                int i;
                for (i = 0; i < keyCount; i++)
                {
                    *dPtr++ = (ushort)(*order++ << 5);
                    *dPtr++ = (ushort)(((*value++ - min) / step) + 0.5f);
                    *dPtr++ = 0;
                }

                //Fill remaining bytes
                while (i++ % 2 != 0)
                    *dPtr++ = 0;

                return ((keyCount * 6) + 16).Align(4);
            }

            return 0;
        }
    }
}
