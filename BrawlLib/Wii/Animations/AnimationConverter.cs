using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Animations
{
    internal static unsafe class AnimationConverter
    {
        public static AnimationKeyframe[] DecodeSequence(CHR0Entry* entry, int numFrames)
        {
            AnimationKeyframe[] keyframes = new AnimationKeyframe[numFrames];
            //keyframes[0] = AnimationKeyframe.Empty;

            int nKeys = 0;

            //AnimationFrame[] frames = new AnimationFrame[numFrames];

            //for (int i = 0; i < numFrames; )
            //    frames[i++] = AnimationFrame.Neutral;

            bfloat* sPtr = (bfloat*)entry->Data;
            AnimationCode code = entry->Code;

            fixed(AnimationKeyframe* pFrame = keyframes)
            //fixed (AnimationFrame* pFrame = frames)
            {

                AnimDataFormat format;

                if (code.HasScale)
                {
                    format = code.ScaleDataFormat;
                    if (code.IsScaleIsotropic)
                    {
                        if (code.IsIsotropicFixed)
                            FillFrame(pFrame, ref nKeys, 0, DecodeMode.ScaleXYZ, *sPtr++);
                        else
                            DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.ScaleXYZ);
                    }
                    else
                    {
                        if (code.IsScaleXFixed)
                            FillFrame(pFrame, ref nKeys, 0, DecodeMode.ScaleX, *sPtr++);
                        else
                            DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.ScaleX);

                        if (code.IsScaleYFixed)
                            FillFrame(pFrame, ref nKeys, 0, DecodeMode.ScaleY, *sPtr++);
                        else
                            DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.ScaleY);

                        if (code.IsScaleZFixed)
                            FillFrame(pFrame, ref nKeys, 0, DecodeMode.ScaleZ, *sPtr++);
                        else
                            DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.ScaleZ);
                    }
                }

                if (code.HasRotation)
                {
                    format = code.RotationDataFormat;
                    if (code.IsRotationXFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.RotX, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.RotX);

                    if (code.IsRotationYFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.RotY, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.RotY);

                    if (code.IsRotationZFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.RotZ, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.RotZ);
                }

                if (code.HasTranslation)
                {
                    format = code.TranslationDataFormat;
                    if (code.IsTranslationXFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.TransX, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.TransX);

                    if (code.IsTranslationYFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.TransY, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.TransY);

                    if (code.IsTranslationZFixed)
                        FillFrame(pFrame, ref nKeys, 0, DecodeMode.TransZ, *sPtr++);
                    else
                        DecodeFrames((VoidPtr)entry + *(buint*)sPtr++, numFrames, pFrame, ref nKeys, format, DecodeMode.TransZ);
                }
            }
            return keyframes;
        }

        private enum DecodeMode
        {
            ScaleX = 0,
            ScaleY = 1,
            ScaleZ = 2,
            RotX = 3,
            RotY = 4,
            RotZ = 5,
            TransX = 6,
            TransY = 7,
            TransZ = 8,
            ScaleXYZ = 9
        }

        //private static void FillValue(AnimationFrame* frames, int numFrames, DecodeMode mode, float value)
        //{
        //    if (mode == DecodeMode.ScaleXYZ)
        //    {
        //        float* dPtr = (float*)frames;
        //        for (int i = 0; i < numFrames; i++, dPtr += 9)
        //        {
        //            dPtr[0] = value;
        //            dPtr[1] = value;
        //            dPtr[2] = value;
        //        }
        //    }
        //    else
        //    {
        //        float* dPtr = (float*)frames + (int)mode;
        //        for (int i = 0; i < numFrames; i++, dPtr += 9)
        //            *dPtr = value;
        //    }
        //}

        private static void FillFrame(AnimationKeyframe* frames, ref int kfCount, int index, DecodeMode mode, float value)
        {
            //Find frame with same index
            int i = -1;
            while ((++i < kfCount) && (frames[i].Index != index)) ;

            //Create new keyframe if not found
            if (i == kfCount)
                frames[i] = new AnimationKeyframe(kfCount++);

            //Set value for keyframe
            if (mode == DecodeMode.ScaleXYZ)
                frames[i].Scale = new Vector3(value);
            else
                *((float*)(&frames[i]) + (int)mode + 1) = value;
        }

        private static void DecodeFrames(void* dataAddr, int numFrames, AnimationKeyframe* frames, ref int kfCount, AnimDataFormat format, DecodeMode mode)
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
                            FillFrame(frames, ref kfCount, frameIndex, mode, *sPtr++);
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
                            FillFrame(frames, ref kfCount, frameIndex, mode, vBase + (value * vStep));
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
                            FillFrame(frames, ref kfCount, frameIndex, mode, vBase + (*sPtr++ * vStep));
                            sPtr++;
                        }
                        break;
                    }
                case AnimDataFormat.F1B:
                    {
                        float vStep = *(bfloat*)dataAddr;
                        float vBase = *((bfloat*)dataAddr + 1);
                        byte* sPtr = (byte*)dataAddr + 8;

                        for (int i = 0; i < numFrames; i++)
                            FillFrame(frames, ref kfCount, i, mode, vBase + (*sPtr++ * vStep));

                        break;
                    }
                case AnimDataFormat.F1F:
                    {
                        float* sPtr = (float*)dataAddr;

                        for (int i = 0; i < numFrames; i++)
                            FillFrame(frames, ref kfCount, i, mode, *sPtr++);

                        break;
                    }
            }

        }
    }
}
