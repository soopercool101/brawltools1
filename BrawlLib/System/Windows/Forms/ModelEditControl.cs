using System;
using BrawlLib.OpenGL;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.Modeling;
using System.Drawing;
using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class ModelEditControl : UserControl
    {
        private bool _updating, _loop;
        private ResourceNode _externalNode;
        private object _transformObject;
        private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        private MDL0BoneNode _selectedBone;
        private CHR0Node _selectedAnim;
        private NumericInputBox[] _transBoxes = new NumericInputBox[9];

        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set
            {
                if (_targetModel == value)
                    return;

                _targetModel = value;
                ModelChanged();
            }
        }

        public ModelEditControl()
        {
            InitializeComponent();
            listAnims.Groups.Add(_CHRGroup);
            _transBoxes[0] = numScaleX; numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY; numScaleY.Tag = 1;
            _transBoxes[2] = numScaleZ; numScaleZ.Tag = 2;
            _transBoxes[3] = numRotX; numRotX.Tag = 3;
            _transBoxes[4] = numRotY; numRotY.Tag = 4;
            _transBoxes[5] = numRotZ; numRotZ.Tag = 5;
            _transBoxes[6] = numTransX; numTransX.Tag = 6;
            _transBoxes[7] = numTransY; numTransY.Tag = 7;
            _transBoxes[8] = numTransZ; numTransZ.Tag = 8;
        }

        private void ModelChanged()
        {
            listPolygons.BeginUpdate();
            listPolygons.Items.Clear();
            lstBones.BeginUpdate();
            lstBones.Items.Clear();

            chkAllPoly.CheckState = CheckState.Checked;

            if (_targetModel != null)
            {
                ResourceNode n;// = _targetModel.FindChild("Polygons", false);

                if((n = _targetModel.FindChild("Polygons", false)) != null)
                    foreach (MDL0PolygonNode poly in n.Children)
                        listPolygons.Items.Add(poly, CheckState.Checked);


                if ((n = _targetModel.FindChild("Bones", false)) != null)
                    foreach (MDL0BoneNode bone in n.Children)
                        WrapBone(bone);
            }
            else
            {
                if (_externalNode != null)
                {
                    _externalNode.Dispose();
                    _externalNode = null;
                }
            }
            modelPanel1.TargetModel = _targetModel;

            listPolygons.EndUpdate();
            lstBones.EndUpdate();

            UpdateReferences();

            _animFrame = -1;
            SetFrame(0);
        }

        private bool UpdateReferences()
        {
            listAnims.BeginUpdate();
            listAnims.Items.Clear();

            if (_targetModel != null)
                LoadAnims(_targetModel.RootNode);

            int count = listAnims.Items.Count;

            if (_externalNode != null)
                LoadAnims(_externalNode.RootNode);

            listAnims.EndUpdate();

            return count != listAnims.Items.Count;
        }

        private void LoadAnims(ResourceNode root)
        {
            foreach (ResourceNode n in root.Children)
            {
                switch (n.ResourceType)
                {
                    case ResourceType.ARC:
                    case ResourceType.BRES:
                    case ResourceType.BRESGroup:
                        LoadAnims(n);
                        break;

                    case ResourceType.CHR0:
                        listAnims.Items.Add(new ListViewItem(n.Name, (int)n.ResourceType, _CHRGroup) { Tag = n });
                        break;
                }
            }
        }

        private void BoneSelected(MDL0BoneNode bone)
        {
            if (bone == _selectedBone)
                return;

            if (_selectedBone != null)
            {
                _selectedBone._boneColor = _selectedBone._nodeColor = Color.Transparent;
            }

            _selectedBone = bone;
            if (_selectedBone != null)
            {
                _selectedBone._boneColor = Color.FromArgb(0, 128, 255);
                _selectedBone._nodeColor = Color.FromArgb(255, 128, 0);
            }


            _transformObject = bone;
            UpdatePropDisplay();
            modelPanel1.Invalidate();
        }


        #region AnimationControls

        private void AnimChanged()
        {
            if (_selectedAnim == null)
            {
                numFrameIndex.Maximum = _maxFrame = 0;
                SetFrame(0);
            }
            else
            {
                if (_selectedAnim._numFrames < _maxFrame)
                {
                    SetFrame(1);
                    numFrameIndex.Maximum = _maxFrame = _selectedAnim._numFrames;
                }
                else
                {
                    numFrameIndex.Maximum = _maxFrame = _selectedAnim._numFrames;
                    SetFrame(1);
                }
            }

            lblFrameCount.Text = String.Format("/ {0}", _maxFrame);
        }

        private void SetFrame(int index)
        {
            if (_animFrame == index)
                return;

            if (_targetModel != null)
            {
                if (_selectedAnim != null)
                    _targetModel.ApplyCHR(_selectedAnim, index);
                else
                    _targetModel.ApplyCHR(null, 0);
            }
            else
                index = 0;

            _animFrame = index;

            btnNextFrame.Enabled = index <= _maxFrame;

            btnPrevFrame.Enabled = index > 0;
            numFrameIndex.Value = index;

            UpdatePropDisplay();

            modelPanel1.Invalidate();
        }

        private void PlayAnim()
        {
            if (_selectedAnim == null)
                return;

            grpTransform.Enabled = false;

            if (_animFrame >= _maxFrame) //Reset anim
                SetFrame(1);

            if (_animFrame < _maxFrame)
                animTimer.Start();

            btnPlay.Text = "Stop";
        }
        private void StopAnim()
        {
            animTimer.Stop();
            btnPlay.Text = "Play";

            grpTransform.Enabled = _transformObject != null;
        }

        private void UpdatePropDisplay()
        {
            if (_transformObject == null)
            {
                grpTransform.Enabled = false;
            }
            else
            {
                grpTransform.Enabled = true;
                for (int i = 0; i < 9; i++)
                    ResetBox(i);
            }
        }

        private unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[index];

            if (_transformObject is MDL0BoneNode)
            {

                MDL0BoneNode bone = _transformObject as MDL0BoneNode;
                CHR0EntryNode entry;
                if ((_selectedAnim != null) && (_animFrame > 0) && ((entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode) != null))
                {
                    float val = entry.Keyframes.GetKeyframe((KeyFrameMode)index, _animFrame - 1);
                    if (float.IsNaN(val))
                    {
                        box.Value = entry.Keyframes.AnimFrames[_animFrame - 1][index];
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = val;
                        box.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    FrameState state = bone._bindState;
                    box.Value = ((float*)&state)[index];
                    box.BackColor = Color.White;
                }
            }
            else
            {
                box.Value = 0;
                box.BackColor = Color.White;
            }
        }
        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (_transformObject == null)
                return;

            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            if (_transformObject is MDL0BoneNode)
            {
                MDL0BoneNode bone = _transformObject as MDL0BoneNode;

                if ((_selectedAnim != null) && (_animFrame > 0))
                {
                    //Find bone anim and change transform
                    CHR0EntryNode entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode;
                    if (entry == null) //Create new bone animation
                    {
                        if (!float.IsNaN(box.Value))
                        {
                            entry = _selectedAnim.CreateEntry();
                            entry.Keyframes.SetKeyFrame((KeyFrameMode)index, _animFrame - 1, box.Value);
                        }
                    }
                    else //Set existing 
                    {
                        if (float.IsNaN(box.Value))
                            entry.Keyframes.RemoveKeyframe((KeyFrameMode)index, _animFrame - 1);
                        else
                            entry.Keyframes.SetKeyFrame((KeyFrameMode)index, _animFrame - 1, box.Value);
                    }
                }
                else
                {
                    //Change base transform
                    FrameState state = bone._bindState;
                    float* p = (float*)&state;
                    p[index] = float.IsNaN(box.Value) ? 0.0f : box.Value;
                    state.CalcTransforms();
                    bone._bindState = state;
                    bone.RecalcBindState();
                }

                //bone.ApplyCHR0(_selectedAnim, _animFrame);
                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
                ResetBox(index);
                modelPanel1.Invalidate();
            }
        }

        private bool LoadExternal()
        {
            int count;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                ResourceNode node = null;
                listAnims.BeginUpdate();
                try
                {
                    if ((node = NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    {
                        if (!CloseExternal())
                            return false;

                        count = listAnims.Items.Count;
                        LoadAnims(node);

                        if (count == listAnims.Items.Count)
                            MessageBox.Show(this, "No animations could be found in external file, closing.", "Error");
                        else
                        {
                            _externalNode = node;
                            node = null;
                            txtExtPath.Text = Path.GetFileName(dlgOpen.FileName);
                            return true;
                        }
                    }
                    else
                        MessageBox.Show(this, "Unable to recognize input file.");
                }
                catch (Exception x) { MessageBox.Show(this, x.ToString()); }
                finally
                {
                    if (node != null)
                        node.Dispose();
                    listAnims.EndUpdate();
                }
            }
            return false;
        }
        private bool CloseExternal()
        {
            if (_externalNode != null)
            {
                if (_externalNode.IsDirty)
                {
                    DialogResult res = MessageBox.Show(this, "You have made changes to an external file. Would you like to save those changes?", "Closing external file.", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!SaveExternal())) || (res == DialogResult.Cancel))
                        return false;
                }
                _externalNode.Dispose();
                _externalNode = null;
                txtExtPath.Text = "";
                UpdateReferences();
            }
            return true;
        }
        private bool SaveExternal()
        {
            if ((_externalNode == null) || (!_externalNode.IsDirty))
                return true;

            try
            {
                _externalNode.Merge();
                _externalNode.Export(_externalNode._origPath);
                return true;
            }
            catch (Exception x) { MessageBox.Show(this, x.ToString()); }
            return false;
        }

        #endregion

        private void WrapBone(MDL0BoneNode bone)
        {
            lstBones.Items.Add(bone, CheckState.Checked);
            foreach (MDL0BoneNode b in bone.Children)
                WrapBone(b);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                modelPanel1.BackColor = label2.BackColor = dlgColor.Color;
            }
        }

        private void btnOptionToggle_Click(object sender, EventArgs e)
        {
            if (pnlOptions.Visible = !pnlOptions.Visible)
                btnOptionToggle.Text = "<";
            else
                btnOptionToggle.Text = ">";
        }

        private void btnAnimToggle_Click(object sender, EventArgs e)
        {
            if (pnlAnim.Visible = !pnlAnim.Visible)
                btnAnimToggle.Text = ">";
            else
                btnAnimToggle.Text = "<";
        }
        private void listPolygons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_updating)
            {
                if (e.CurrentValue == CheckState.Checked)
                    e.NewValue = CheckState.Indeterminate;
            }

            MDL0PolygonNode poly = listPolygons.Items[e.Index] as MDL0PolygonNode;

            poly._render = e.NewValue == CheckState.Checked || e.NewValue == CheckState.Indeterminate;
            poly._wireframe = e.NewValue == CheckState.Indeterminate;

            if (!_updating)
                modelPanel1.Invalidate();
        }
        private void ModelEditControl_Load(object sender, EventArgs e)
        {
            label2.BackColor = modelPanel1.BackColor;
        }
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (listPolygons.Items.Count == 0)
                return;

            _updating = true;

            listPolygons.BeginUpdate();
            for (int i = 0; i < listPolygons.Items.Count; i++)
                listPolygons.SetItemCheckState(i, chkAllPoly.CheckState);
            listPolygons.EndUpdate();

            _updating = false;
            modelPanel1.Invalidate();
        }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAnims.SelectedItems.Count > 0)
                _selectedAnim = listAnims.SelectedItems[0].Tag as CHR0Node;
            else
                _selectedAnim = null;
            AnimChanged();
        }
        private void chkAllBones_CheckedChanged(object sender, EventArgs e)
        {
            if (lstBones.Items.Count == 0)
                return;

            _updating = true;

            lstBones.BeginUpdate();
            for (int i = 0; i < lstBones.Items.Count; i++)
                lstBones.SetItemCheckState(i, chkAllBones.CheckState);
            lstBones.EndUpdate();

            _updating = false;
            modelPanel1.Invalidate();
        }
        private void lstBones_SelectedValueChanged(object sender, EventArgs e)        {            BoneSelected( lstBones.SelectedItem as MDL0BoneNode);        }
        private void lstBones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0BoneNode bone = lstBones.Items[e.Index] as MDL0BoneNode;

            bone._render = e.NewValue == CheckState.Checked;

            if (!_updating)
                modelPanel1.Invalidate();
        }

        private void btnOpen_Click(object sender, EventArgs e) { LoadExternal(); }
        private void textBox1_Click(object sender, EventArgs e) { LoadExternal(); }
        private void btnClose_Click(object sender, EventArgs e) { CloseExternal(); }
        private void btnSave_Click(object sender, EventArgs e) { SaveExternal(); }

        private void numFrameIndex_ValueChanged(object sender, EventArgs e) { SetFrame((int)numFrameIndex.Value); }

        private void btnPrevFrame_Click(object sender, EventArgs e) { numFrameIndex.Value--; }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (animTimer.Enabled)
                StopAnim();
            else
                PlayAnim();
        }
        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            if (_animFrame >= _maxFrame)
                if (!_loop)
                    StopAnim();
                else
                    SetFrame(1);
            else
                SetFrame(_animFrame + 1);
        }
        private void numFPS_ValueChanged(object sender, EventArgs e) { animTimer.Interval = 1000 / (int)numFPS.Value; }
        private void chkLoop_CheckedChanged(object sender, EventArgs e) { _loop = chkLoop.Checked; }

        #region Designer

        private ModelPanel modelPanel1;
        private Panel pnlOptions;
        private CheckedListBox listPolygons;
        private Panel panel1;
        private Label label2;
        private Label label1;
        private CheckBox chkAllPoly;
        private ColorDialog dlgColor;
        private Panel pnlAnim;
        private Button btnOptionToggle;
        private Button btnAnimToggle;
        private System.ComponentModel.IContainer components;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label3;
        private CheckedListBox lstBones;
        private CheckBox chkAllBones;
        private Label label4;
        private ListView listAnims;
        private Panel panel2;
        private ColumnHeader nameColumn;
        private GroupBox grpExt;
        private TextBox txtExtPath;
        private Button btnClose;
        private Button btnOpen;
        private Button btnSave;
        private OpenFileDialog dlgOpen;
        private Button btnPrevFrame;
        private Button btnNextFrame;
        private Button button1;
        private NumericUpDown numFrameIndex;
        private int _animFrame, _maxFrame;
        private NumericInputBox numScaleZ;
        private NumericInputBox numScaleY;
        private NumericInputBox numScaleX;
        private NumericInputBox numRotZ;
        private NumericInputBox numRotY;
        private NumericInputBox numRotX;
        private NumericInputBox numTransZ;
        private NumericInputBox numTransY;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private NumericInputBox numTransX;
        private GroupBox grpTransform;
        private Button btnPlay;
        private Timer animTimer;
        private NumericUpDown numFPS;
        private CheckBox chkLoop;
        private Label lblFrameCount;

        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Animations", System.Windows.Forms.HorizontalAlignment.Left);
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listPolygons = new System.Windows.Forms.CheckedListBox();
            this.chkAllPoly = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lstBones = new System.Windows.Forms.CheckedListBox();
            this.chkAllBones = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.pnlAnim = new System.Windows.Forms.Panel();
            this.grpTransform = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.listAnims = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.txtExtPath = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.btnAnimToggle = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.numFrameIndex = new System.Windows.Forms.NumericUpDown();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.animTimer = new System.Windows.Forms.Timer();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.pnlOptions.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlAnim.SuspendLayout();
            this.grpTransform.SuspendLayout();
            this.grpExt.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.White;
            this.pnlOptions.Controls.Add(this.splitContainer1);
            this.pnlOptions.Controls.Add(this.panel1);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlOptions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.pnlOptions.Size = new System.Drawing.Size(98, 546);
            this.pnlOptions.TabIndex = 2;
            this.pnlOptions.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 30);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(96, 516);
            this.splitContainer1.SplitterDistance = 258;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listPolygons);
            this.groupBox1.Controls.Add(this.chkAllPoly);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(96, 258);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Polygons";
            // 
            // listPolygons
            // 
            this.listPolygons.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listPolygons.CausesValidation = false;
            this.listPolygons.CheckOnClick = true;
            this.listPolygons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPolygons.IntegralHeight = false;
            this.listPolygons.Location = new System.Drawing.Point(3, 36);
            this.listPolygons.Margin = new System.Windows.Forms.Padding(0);
            this.listPolygons.Name = "listPolygons";
            this.listPolygons.Size = new System.Drawing.Size(90, 219);
            this.listPolygons.TabIndex = 2;
            this.listPolygons.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listPolygons_ItemCheck);
            // 
            // chkAllPoly
            // 
            this.chkAllPoly.Checked = true;
            this.chkAllPoly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllPoly.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllPoly.Location = new System.Drawing.Point(3, 16);
            this.chkAllPoly.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllPoly.Name = "chkAllPoly";
            this.chkAllPoly.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllPoly.Size = new System.Drawing.Size(90, 20);
            this.chkAllPoly.TabIndex = 3;
            this.chkAllPoly.Text = "All";
            this.chkAllPoly.ThreeState = true;
            this.chkAllPoly.UseVisualStyleBackColor = false;
            this.chkAllPoly.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lstBones);
            this.groupBox2.Controls.Add(this.chkAllBones);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(96, 254);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bones";
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(59, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 18);
            this.label3.TabIndex = 4;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstBones
            // 
            this.lstBones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBones.CausesValidation = false;
            this.lstBones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBones.IntegralHeight = false;
            this.lstBones.Location = new System.Drawing.Point(3, 56);
            this.lstBones.Margin = new System.Windows.Forms.Padding(0);
            this.lstBones.Name = "lstBones";
            this.lstBones.Size = new System.Drawing.Size(90, 195);
            this.lstBones.TabIndex = 5;
            this.lstBones.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstBones_ItemCheck);
            this.lstBones.SelectedValueChanged += new System.EventHandler(this.lstBones_SelectedValueChanged);
            // 
            // chkAllBones
            // 
            this.chkAllBones.Checked = true;
            this.chkAllBones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllBones.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllBones.Location = new System.Drawing.Point(3, 36);
            this.chkAllBones.Margin = new System.Windows.Forms.Padding(0);
            this.chkAllBones.Name = "chkAllBones";
            this.chkAllBones.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllBones.Size = new System.Drawing.Size(90, 20);
            this.chkAllBones.TabIndex = 4;
            this.chkAllBones.Text = "All";
            this.chkAllBones.UseVisualStyleBackColor = false;
            this.chkAllBones.CheckedChanged += new System.EventHandler(this.chkAllBones_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Location = new System.Drawing.Point(3, 16);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(90, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Color:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 30);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(67, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 18);
            this.label2.TabIndex = 2;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Back Color:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // pnlAnim
            // 
            this.pnlAnim.BackColor = System.Drawing.Color.White;
            this.pnlAnim.Controls.Add(this.listAnims);
            this.pnlAnim.Controls.Add(this.grpTransform);
            this.pnlAnim.Controls.Add(this.grpExt);
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAnim.Location = new System.Drawing.Point(581, 0);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(173, 546);
            this.pnlAnim.TabIndex = 4;
            this.pnlAnim.Visible = false;
            // 
            // grpTransform
            // 
            this.grpTransform.Controls.Add(this.label5);
            this.grpTransform.Controls.Add(this.numScaleZ);
            this.grpTransform.Controls.Add(this.numTransX);
            this.grpTransform.Controls.Add(this.numScaleY);
            this.grpTransform.Controls.Add(this.label6);
            this.grpTransform.Controls.Add(this.numScaleX);
            this.grpTransform.Controls.Add(this.label7);
            this.grpTransform.Controls.Add(this.numRotZ);
            this.grpTransform.Controls.Add(this.label8);
            this.grpTransform.Controls.Add(this.numRotY);
            this.grpTransform.Controls.Add(this.label9);
            this.grpTransform.Controls.Add(this.numRotX);
            this.grpTransform.Controls.Add(this.label10);
            this.grpTransform.Controls.Add(this.numTransZ);
            this.grpTransform.Controls.Add(this.label11);
            this.grpTransform.Controls.Add(this.numTransY);
            this.grpTransform.Controls.Add(this.label12);
            this.grpTransform.Controls.Add(this.label13);
            this.grpTransform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTransform.Location = new System.Drawing.Point(0, 326);
            this.grpTransform.Name = "grpTransform";
            this.grpTransform.Size = new System.Drawing.Size(173, 220);
            this.grpTransform.TabIndex = 21;
            this.grpTransform.TabStop = false;
            this.grpTransform.Text = "Transform";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Translation X:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Translation Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Translation Z:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Rotation X:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Rotation Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Rotation Z:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 20);
            this.label11.TabIndex = 10;
            this.label11.Text = "Scale X:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 20);
            this.label12.TabIndex = 11;
            this.label12.Text = "Scale Z:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 172);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Scale Y:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listAnims
            // 
            this.listAnims.AutoArrange = false;
            this.listAnims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.listAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Animations";
            listViewGroup1.Name = "grpAnims";
            this.listAnims.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listAnims.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAnims.HideSelection = false;
            this.listAnims.Location = new System.Drawing.Point(0, 69);
            this.listAnims.MultiSelect = false;
            this.listAnims.Name = "listAnims";
            this.listAnims.Size = new System.Drawing.Size(173, 257);
            this.listAnims.TabIndex = 0;
            this.listAnims.UseCompatibleStateImageBehavior = false;
            this.listAnims.View = System.Windows.Forms.View.Details;
            this.listAnims.SelectedIndexChanged += new System.EventHandler(this.listAnims_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 120;
            // 
            // grpExt
            // 
            this.grpExt.Controls.Add(this.txtExtPath);
            this.grpExt.Controls.Add(this.btnClose);
            this.grpExt.Controls.Add(this.btnOpen);
            this.grpExt.Controls.Add(this.btnSave);
            this.grpExt.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpExt.Location = new System.Drawing.Point(0, 0);
            this.grpExt.Name = "grpExt";
            this.grpExt.Padding = new System.Windows.Forms.Padding(6, 4, 6, 3);
            this.grpExt.Size = new System.Drawing.Size(173, 69);
            this.grpExt.TabIndex = 2;
            this.grpExt.TabStop = false;
            this.grpExt.Text = "External File";
            // 
            // txtExtPath
            // 
            this.txtExtPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtExtPath.Location = new System.Drawing.Point(6, 17);
            this.txtExtPath.Name = "txtExtPath";
            this.txtExtPath.ReadOnly = true;
            this.txtExtPath.Size = new System.Drawing.Size(161, 20);
            this.txtExtPath.TabIndex = 3;
            this.txtExtPath.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClose.Location = new System.Drawing.Point(116, 42);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 20);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpen.Location = new System.Drawing.Point(8, 42);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(50, 20);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Load";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Location = new System.Drawing.Point(62, 42);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 20);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOptionToggle.Location = new System.Drawing.Point(98, 0);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(15, 546);
            this.btnOptionToggle.TabIndex = 5;
            this.btnOptionToggle.Text = ">";
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(566, 0);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAnimToggle.TabIndex = 6;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkLoop);
            this.panel2.Controls.Add(this.numFPS);
            this.panel2.Controls.Add(this.btnPlay);
            this.panel2.Controls.Add(this.lblFrameCount);
            this.panel2.Controls.Add(this.numFrameIndex);
            this.panel2.Controls.Add(this.btnPrevFrame);
            this.panel2.Controls.Add(this.btnNextFrame);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(113, 509);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(453, 37);
            this.panel2.TabIndex = 7;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(112, 11);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 17);
            this.chkLoop.TabIndex = 6;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // numFPS
            // 
            this.numFPS.Location = new System.Drawing.Point(50, 10);
            this.numFPS.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFPS.Name = "numFPS";
            this.numFPS.Size = new System.Drawing.Size(46, 20);
            this.numFPS.TabIndex = 5;
            this.numFPS.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.ValueChanged += new System.EventHandler(this.numFPS_ValueChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(185, 7);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrameCount.Location = new System.Drawing.Point(336, 6);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(51, 24);
            this.lblFrameCount.TabIndex = 3;
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFrameIndex
            // 
            this.numFrameIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrameIndex.Location = new System.Drawing.Point(266, 10);
            this.numFrameIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFrameIndex.Name = "numFrameIndex";
            this.numFrameIndex.Size = new System.Drawing.Size(64, 20);
            this.numFrameIndex.TabIndex = 2;
            this.numFrameIndex.ValueChanged += new System.EventHandler(this.numFrameIndex_ValueChanged);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevFrame.Enabled = false;
            this.btnPrevFrame.Location = new System.Drawing.Point(393, 6);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(24, 24);
            this.btnPrevFrame.TabIndex = 1;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextFrame.Enabled = false;
            this.btnNextFrame.Location = new System.Drawing.Point(423, 6);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(24, 24);
            this.btnNextFrame.TabIndex = 0;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "All Files (*.*)|*.*";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(113, 494);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(453, 15);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = -100;
            this.modelPanel1.InitialZoomFactor = -5;
            this.modelPanel1.Location = new System.Drawing.Point(113, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(453, 494);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            // 
            // numScaleZ
            // 
            this.numScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleZ.Location = new System.Drawing.Point(86, 192);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 20;
            this.numScaleZ.Text = "0";
            this.numScaleZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransX
            // 
            this.numTransX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransX.Location = new System.Drawing.Point(86, 16);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 3;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numScaleY
            // 
            this.numScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleY.Location = new System.Drawing.Point(86, 172);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 19;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numScaleX
            // 
            this.numScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleX.Location = new System.Drawing.Point(86, 152);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 18;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotZ
            // 
            this.numRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotZ.Location = new System.Drawing.Point(86, 124);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 17;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotY
            // 
            this.numRotY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotY.Location = new System.Drawing.Point(86, 104);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 16;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotX
            // 
            this.numRotX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotX.Location = new System.Drawing.Point(86, 84);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 15;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransZ
            // 
            this.numTransZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransZ.Location = new System.Drawing.Point(86, 56);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 14;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransY
            // 
            this.numTransY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransY.Location = new System.Drawing.Point(86, 36);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 13;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // ModelEditControl
            // 
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.pnlAnim);
            this.Controls.Add(this.pnlOptions);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(754, 546);
            this.Load += new System.EventHandler(this.ModelEditControl_Load);
            this.pnlOptions.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlAnim.ResumeLayout(false);
            this.grpTransform.ResumeLayout(false);
            this.grpTransform.PerformLayout();
            this.grpExt.ResumeLayout(false);
            this.grpExt.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion









    }
}
