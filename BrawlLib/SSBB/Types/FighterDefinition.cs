using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefHeader
    {
        public bint _fileSize;
        public bint _lookupOffset;
        public bint _numLookupEntries;
        public bint _numEntries1; //Has string entry
        public bint _numEntries2; //Has string entry
        int _pad1, _pad2, _pad3;

        //From here begins file data. All offsets are relative to this location (0x20).


        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        //public FDefLookupHeader* LookupHeader { get { return (FDefLookupHeader*)(Address + _lookupOffset + 0x20); } }

        public bint* LookupEntries { get { return (bint*)(Address + _lookupOffset + 0x20); } }
        public FDefStringEntry* StringEntries { get { return (FDefStringEntry*)(Address + _lookupOffset + 0x20) + (_numLookupEntries * 4); } }
        public VoidPtr StringTable { get { return (Address + _lookupOffset + 0x20) + (_numLookupEntries * 4) + (_numEntries1 * 8) + (_numEntries2 * 8); } }
        public FDefAttributes* Attributes { get { return (FDefAttributes*)(Address + 0x20); } }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefEntry1
    {
        public bint _part1Offset;
        public bint _unk1; //0
        public bint _unk2; //0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefEntry1Part1
    {
        public bint _unk1; //0
        public bint _unk2; //6
        public bint _unk3; //0x18000100
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefStringEntry
    {
        public bint _dataOffset;
        public bint _stringOffset;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefAttributes
    {
        public bfloat _walkInitVelocity;
        public bfloat _walkAcceleration;
        public bfloat _walkMaxVelocity;
        public bfloat _stopVelocity;
        public bfloat _dashInitValocity;
        public bfloat _stopTurnDecel;
        public bfloat _stopTurnAccel;
        public bfloat _runInitVelocity;
        public bfloat _unk01; //30
        public bfloat _unk02; //3
        public bint _unk03; //1
        public bfloat _unk04; //1.3
        public bint _unk05; //5
        public bfloat _unk06; //0.9
        public bfloat _jumpYInitVelocity;
        public bfloat _unk07;
        public bfloat _jumpXInitVelocity;
        public bfloat _hopYInitVelocity;
        public bfloat _airJumpMultiplier;
        public bfloat _unk08;
        public bfloat _stoolYInitVelocity;
        public bfloat _unk09;
        public bfloat _unk10;
        public bfloat _unk11;
        public bint _unk12;
        public bfloat _gravity;
        public bfloat _termVelocity;
        public bfloat _unk13;
        public bfloat _unk14;
        public bfloat _airMobility;
        public bfloat _airStopMobility;
        public bfloat _airMaxXVelocity;
        public bfloat _unk15;
        public bfloat _unk16;
        public bfloat _unk17;
        public bint _unk18;
        public bfloat _unk19;
        public bfloat _unk20;
        public bfloat _unk21;
        public bint _unk22;
        public bint _unk23;
        public bint _unk24;
        public bfloat _unk25;
        public bfloat _unk26;
        public bfloat _weight;
        public bfloat _unk27;
        public bfloat _unk28;
        public bfloat _unk29;
        public bfloat _unk30;
        public bfloat _shieldSize;
        public bfloat _shieldBreakBounce;
        public bfloat _unk31;
        public bfloat _unk32;
        public bfloat _unk33;
        public bfloat _unk34;
        public bfloat _unk35;
        public bfloat _unk36;
        public bfloat _unk37;
        public bint _unk38;
        public bint _unk39;
        public bint _unk40;
        public bfloat _unk41;
        public bfloat _edgeJumpYVelocity;
        public bfloat _edgeJumpXVelocity;
        public bfloat _unk42;
        public bfloat _unk43;
        public bfloat _unk44;
        public bfloat _unk45;
        public bfloat _unk46;
        public bint _unk47;
        public bfloat _itemThrowStrength;
        public bfloat _unk48;
        public bfloat _unk49;
        public bfloat _unk50;
        public bfloat _fireMoveSpeed;
        public bfloat _fireFDashSpeed;
        public bfloat _fireBDashSpeed;
        public bfloat _unk51;
        public bfloat _unk52;
        public bfloat _unk53;
        public bfloat _unk54;
        public bfloat _unk55;
        public bfloat _unk56;
        public bfloat _unk57;
        public bfloat _unk58;
        public bint _unk59;
        public bint _unk60;
        public bfloat _unk61;
        public bfloat _unk62;
        public bfloat _wallJumpYVelocity;
        public bfloat _wallJumpXVelocity;
        public bfloat _unk63;
        public bfloat _unk64;
        public bint _unk65;
        public bfloat _unk66;
        public bfloat _unk67;
        public bint _unk68;
        public bint _unk69;
        public bfloat _unk70;
        public bfloat _unk71;
        public bfloat _unk72;
        public bfloat _unk73;
        public bfloat _unk74;
        public bfloat _unk75;
        public bfloat _unk76;
        public bfloat _unk77;
        public bint _unk78;
        public bfloat _unk79;
        public bint _unk80;


    }

}
