using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class CharacterFrame : UserControl
    {
        public delegate void CharacterChangeEvent(CharacterDefinition def);
        public static event CharacterChangeEvent CharacterChanged;

        private CharacterDefinition _selectedCharacter;
        public CharacterDefinition SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                _selectedCharacter = value;
                if (CharacterChanged != null)
                    CharacterChanged(_selectedCharacter);
            }
        }

        public CharacterFrame() { InitializeComponent(); }

        public void Initialize()
        {
            _charList.BeginUpdate();

            _charList.Clear();

            foreach (Image img in csfList.Images)
                img.Dispose();
            csfList.Images.Clear();

            int index = 0;
            Image im;
            try
            {

                foreach (CharacterDefinition def in CharacterDefinition.List)
                {
                    if ((im = def.GetCSF()) != null)
                    {
                        csfList.Images.Add(im);
                        def.ImageIndex = index++;
                    }
                    else
                        def.ImageIndex = -1;

                    _charList.Items.Add(def);
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message); }

            _charList.EndUpdate();
        }

        private void _charList_SelectedIndexChanged(object sender, EventArgs e) { SelectedCharacter = _charList.SelectedItems.Count == 0 ? null : _charList.SelectedItems[0] as CharacterDefinition; }

        private void CharacterFrame_Enter(object sender, EventArgs e)
        {
        }

        private void CharacterFrame_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                Initialize();
        }
    }
}
