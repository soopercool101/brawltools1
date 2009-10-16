using System;
using System.Windows.Forms;
using System.Drawing;

namespace BrawlScape
{
    public class ModelControl : ModelPanel
    {
        private ModelContextMenu _context;
        private Button _colorButton;

        private ModelDefinition _currentModel;
        public ModelDefinition CurrentModel
        {
            get { return _currentModel; }
            set { if (_currentModel != value) OnModelChanged(_currentModel = value); }
        }

        public ModelControl()
            : base()
        {
            ContextMenuStrip = _context = new ModelContextMenu();
            ColorChanged += OnBackColorChanged;
            //_colorButton = new Button();
            //_colorButton.Text = "Color";
            //_colorButton.Bounds = new Rectangle(Width - 45, Height - 25, 40, 20);

            //this.Controls.Add(_colorButton);
            //_colorButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void OnModelChanged(ModelDefinition model)
        {
            if (model != null)
            {
                _context.ModelReference = model.Reference;
                model.AttachTextures();
                this.TargetModel = model.Model;
            }
            else
            {
                _context.ModelReference = null;
                this.TargetModel = null;
            }
        }
        private void OnBackColorChanged(Color c)
        {
            this.BackColor = c;
        }

        private delegate void ColorChangeEvent(Color c);
        private static event ColorChangeEvent ColorChanged;

        private static ColorDialog _colorDlg;
        public static void ChooseColor()
        {
            if (_colorDlg == null)
                _colorDlg = new ColorDialog();

            if (_colorDlg.ShowDialog() == DialogResult.OK)
            {
                if (ColorChanged != null)
                    ColorChanged(_colorDlg.Color);
            }
        }
    }
}
