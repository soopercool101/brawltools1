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
        #region Designer

        private ModelPanel modelPanel1;
        private Button btnAssetToggle;
        private Button btnAnimToggle;
        private System.ComponentModel.IContainer components;
        private Panel pnlPlayback;
        private Button btnPrevFrame;
        private Button btnNextFrame;
        private Button btnPlaybackToggle;
        private NumericUpDown numFrameIndex;
        private Button btnPlay;
        private Timer animTimer;
        private NumericUpDown numFPS;
        private CheckBox chkLoop;
        private Label label14;
        private ModelAssetPanel pnlAssets;
        private Splitter spltAssets;
        private NumericUpDown numTotalFrames;
        private Label lblFrameCount;
        private ModelOptionPanel pnlOptions;
        private Button btnOptionToggle;
        private ModelAnimPanel pnlAnim;
        private Label label15;

        private void InitializeComponent()
        {
            this.btnAssetToggle = new System.Windows.Forms.Button();
            this.btnAnimToggle = new System.Windows.Forms.Button();
            this.pnlPlayback = new System.Windows.Forms.Panel();
            this.btnPlay = new System.Windows.Forms.Button();
            this.numTotalFrames = new System.Windows.Forms.NumericUpDown();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.numFrameIndex = new System.Windows.Forms.NumericUpDown();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnPlaybackToggle = new System.Windows.Forms.Button();
            this.animTimer = new System.Windows.Forms.Timer();
            this.spltAssets = new System.Windows.Forms.Splitter();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.pnlOptions = new System.Windows.Forms.ModelOptionPanel();
            this.pnlAssets = new System.Windows.Forms.ModelAssetPanel();
            this.pnlAnim = new System.Windows.Forms.ModelAnimPanel();
            this.pnlPlayback.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAssetToggle
            // 
            this.btnAssetToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAssetToggle.Location = new System.Drawing.Point(105, 0);
            this.btnAssetToggle.Name = "btnAssetToggle";
            this.btnAssetToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAssetToggle.TabIndex = 5;
            this.btnAssetToggle.TabStop = false;
            this.btnAssetToggle.Text = ">";
            this.btnAssetToggle.UseVisualStyleBackColor = false;
            this.btnAssetToggle.Click += new System.EventHandler(this.btnAssetToggle_Click);
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(566, 0);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAnimToggle.TabIndex = 6;
            this.btnAnimToggle.TabStop = false;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.BackColor = System.Drawing.Color.White;
            this.pnlPlayback.Controls.Add(this.btnPlay);
            this.pnlPlayback.Controls.Add(this.numTotalFrames);
            this.pnlPlayback.Controls.Add(this.numFPS);
            this.pnlPlayback.Controls.Add(this.label14);
            this.pnlPlayback.Controls.Add(this.chkLoop);
            this.pnlPlayback.Controls.Add(this.numFrameIndex);
            this.pnlPlayback.Controls.Add(this.btnPrevFrame);
            this.pnlPlayback.Controls.Add(this.btnNextFrame);
            this.pnlPlayback.Controls.Add(this.label15);
            this.pnlPlayback.Controls.Add(this.lblFrameCount);
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPlayback.Location = new System.Drawing.Point(120, 509);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(446, 37);
            this.pnlPlayback.TabIndex = 7;
            this.pnlPlayback.Visible = false;
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(129, 7);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(104, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // numTotalFrames
            // 
            this.numTotalFrames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numTotalFrames.Enabled = false;
            this.numTotalFrames.Location = new System.Drawing.Point(339, 9);
            this.numTotalFrames.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTotalFrames.Name = "numTotalFrames";
            this.numTotalFrames.Size = new System.Drawing.Size(52, 20);
            this.numTotalFrames.TabIndex = 9;
            this.numTotalFrames.ValueChanged += new System.EventHandler(this.numTotalFrames_ValueChanged);
            // 
            // numFPS
            // 
            this.numFPS.Location = new System.Drawing.Point(43, 10);
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
            this.numFPS.Size = new System.Drawing.Size(35, 20);
            this.numFPS.TabIndex = 5;
            this.numFPS.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.ValueChanged += new System.EventHandler(this.numFPS_ValueChanged);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.Location = new System.Drawing.Point(3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 37);
            this.label14.TabIndex = 7;
            this.label14.Text = "Speed:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLoop
            // 
            this.chkLoop.Location = new System.Drawing.Point(82, 12);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 17);
            this.chkLoop.TabIndex = 6;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // numFrameIndex
            // 
            this.numFrameIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrameIndex.Location = new System.Drawing.Point(277, 9);
            this.numFrameIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFrameIndex.Name = "numFrameIndex";
            this.numFrameIndex.Size = new System.Drawing.Size(52, 20);
            this.numFrameIndex.TabIndex = 2;
            this.numFrameIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFrameIndex.ValueChanged += new System.EventHandler(this.numFrameIndex_ValueChanged);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevFrame.Enabled = false;
            this.btnPrevFrame.Location = new System.Drawing.Point(394, 7);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(23, 23);
            this.btnPrevFrame.TabIndex = 1;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextFrame.Enabled = false;
            this.btnNextFrame.Location = new System.Drawing.Point(419, 7);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(23, 23);
            this.btnNextFrame.TabIndex = 0;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(237, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 37);
            this.label15.TabIndex = 8;
            this.label15.Text = "Frame:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrameCount.Location = new System.Drawing.Point(314, 7);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(40, 24);
            this.lblFrameCount.TabIndex = 3;
            this.lblFrameCount.Text = "/";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPlaybackToggle
            // 
            this.btnPlaybackToggle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPlaybackToggle.Location = new System.Drawing.Point(120, 494);
            this.btnPlaybackToggle.Name = "btnPlaybackToggle";
            this.btnPlaybackToggle.Size = new System.Drawing.Size(446, 15);
            this.btnPlaybackToggle.TabIndex = 8;
            this.btnPlaybackToggle.TabStop = false;
            this.btnPlaybackToggle.UseVisualStyleBackColor = false;
            this.btnPlaybackToggle.Click += new System.EventHandler(this.btnPlaybackToggle_Click);
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // spltAssets
            // 
            this.spltAssets.Location = new System.Drawing.Point(101, 0);
            this.spltAssets.Name = "spltAssets";
            this.spltAssets.Size = new System.Drawing.Size(4, 546);
            this.spltAssets.TabIndex = 9;
            this.spltAssets.TabStop = false;
            this.spltAssets.Visible = false;
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOptionToggle.Location = new System.Drawing.Point(120, 19);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(446, 15);
            this.btnOptionToggle.TabIndex = 11;
            this.btnOptionToggle.TabStop = false;
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = 100;
            this.modelPanel1.InitialZoomFactor = 5;
            this.modelPanel1.Location = new System.Drawing.Point(120, 34);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(446, 460);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.White;
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOptions.Location = new System.Drawing.Point(120, 0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(446, 19);
            this.pnlOptions.TabIndex = 10;
            this.pnlOptions.Visible = false;
            this.pnlOptions.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlOptions.ClearColorChanged += new System.EventHandler(this.pnlOptions_ClearColorChanged);
            this.pnlOptions.CamResetClicked += new System.EventHandler(this.pnlOptions_CamResetClicked);
            // 
            // pnlAssets
            // 
            this.pnlAssets.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssets.Location = new System.Drawing.Point(0, 0);
            this.pnlAssets.Name = "pnlAssets";
            this.pnlAssets.Size = new System.Drawing.Size(101, 546);
            this.pnlAssets.TabIndex = 4;
            this.pnlAssets.Visible = false;
            this.pnlAssets.SelectedBoneChanged += new System.EventHandler(this.pnlAssets_SelectedBoneChanged);
            this.pnlAssets.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            // 
            // pnlAnim
            // 
            this.pnlAnim.BackColor = System.Drawing.Color.White;
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAnim.Location = new System.Drawing.Point(581, 0);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(173, 546);
            this.pnlAnim.TabIndex = 12;
            this.pnlAnim.Visible = false;
            this.pnlAnim.RenderStateChanged += new System.EventHandler(this.RenderStateChanged);
            this.pnlAnim.AnimStateChanged += new System.EventHandler(this.pnlAnim_AnimStateChanged);
            this.pnlAnim.SelectedAnimationChanged += new System.EventHandler(this.pnlAnim_SelectedAnimationChanged);
            this.pnlAnim.ReferenceLoaded += new System.Windows.Forms.ModelAnimPanel.ReferenceEventHandler(this.pnlAnim_ReferenceLoaded);
            this.pnlAnim.ReferenceClosed += new System.Windows.Forms.ModelAnimPanel.ReferenceEventHandler(this.pnlAnim_ReferenceClosed);
            // 
            // ModelEditControl
            // 
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.btnPlaybackToggle);
            this.Controls.Add(this.pnlPlayback);
            this.Controls.Add(this.btnAssetToggle);
            this.Controls.Add(this.spltAssets);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.pnlAssets);
            this.Controls.Add(this.pnlAnim);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(754, 546);
            this.pnlPlayback.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private int _animFrame, _maxFrame;
        private bool _updating, _loop;
        //private ResourceNode _externalNode;
        private object _transformObject;
        //private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        private CHR0Node _selectedAnim;
        //private NumericInputBox[] _transBoxes = new NumericInputBox[9];

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

        public ModelEditControl() { InitializeComponent(); }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == 0x100)
            {
                Keys key = (Keys)m.WParam;
                if (key == Keys.PageUp)
                {
                    if (_selectedAnim != null)
                    {
                        if (_animFrame >= _maxFrame)
                            SetFrame(1);
                        else
                            SetFrame(_animFrame + 1);
                    }
                    return true;
                }
                else if (key == Keys.PageDown)
                {
                    if (_selectedAnim != null)
                    {
                        if (_animFrame == 0)
                            SetFrame(_maxFrame);
                        else
                            SetFrame(_animFrame - 1);
                    }
                    return true;
                }
                else if (key == Keys.Left)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnAssetToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Right)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnAnimToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Up)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnOptionToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
                else if (key == Keys.Down)
                {
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt))
                    {
                        btnPlaybackToggle_Click(null, null);
                        return true;
                    }
                    return false;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        public void AddReference(ResourceNode node)
        {
            modelPanel1.AddReference(node);
        }

        public bool CloseFiles()
        {
            return pnlAnim.CloseReferences();
        }

        private void ModelChanged()
        {
            //if (_externalNode != null)
            //{
            //    _externalNode.Dispose();
            //    _externalNode = null;
            //}

            modelPanel1.TargetModel = _targetModel;
            pnlOptions.TargetModel = _targetModel;
            pnlAnim.TargetModel = _targetModel;
            pnlAssets.Attach(_targetModel);

            //UpdateReferences();

            //_animFrame = -1;
            SetFrame(0);
        }

        #region AnimationControls

        private void AnimChanged()
        {
            if (_selectedAnim == null)
            {
                numFrameIndex.Maximum = _maxFrame = 0;
                numTotalFrames.Minimum = 0;
                numTotalFrames.Value = 0;
                numTotalFrames.Enabled = false;
                SetFrame(0);
            }
            else
            {
                int oldMax = _maxFrame;
                _maxFrame = _selectedAnim._numFrames;

                _updating = true;
                numTotalFrames.Enabled = true;
                numTotalFrames.Value = _maxFrame;
                _updating = false;

                if (_maxFrame < oldMax)
                {
                    SetFrame(1);
                    numFrameIndex.Maximum = _maxFrame;
                }
                else
                {
                    numFrameIndex.Maximum = _maxFrame;
                    SetFrame(1);
                }
            }
        }

        private void SetFrame(int index)
        {
            //if (_animFrame == index)
            //    return;

            _animFrame = _targetModel == null ? 0 : index;


            btnNextFrame.Enabled = _animFrame < _maxFrame;
            btnPrevFrame.Enabled = _animFrame > 0;

            numFrameIndex.Value = _animFrame;

            pnlAnim.CurrentFrame = _animFrame;

            modelPanel1.Invalidate();
        }

        private void PlayAnim()
        {
            if (_selectedAnim == null)
                return;

            pnlAnim.EnableTransformEdit = false;

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

            pnlAnim.EnableTransformEdit = true;
        }

        #endregion

        private void btnAssetToggle_Click(object sender, EventArgs e)
        {
            if (spltAssets.Visible = pnlAssets.Visible = !pnlAssets.Visible)
                btnAssetToggle.Text = "<";
            else
                btnAssetToggle.Text = ">";
        }
        private void btnOptionToggle_Click(object sender, EventArgs e) { pnlOptions.Visible = !pnlOptions.Visible; }
        private void btnPlaybackToggle_Click(object sender, EventArgs e) { pnlPlayback.Visible = !pnlPlayback.Visible; }

        private void btnAnimToggle_Click(object sender, EventArgs e)
        {
            if (pnlAnim.Visible = !pnlAnim.Visible)
                btnAnimToggle.Text = ">";
            else
                btnAnimToggle.Text = "<";
        }

        private void numFrameIndex_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)numFrameIndex.Value;
            if (val != _animFrame)
                SetFrame(val);
        }

        private void btnPrevFrame_Click(object sender, EventArgs e) { numFrameIndex.Value--; }
        private void btnNextFrame_Click(object sender, EventArgs e) { numFrameIndex.Value++; }
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

        private void RenderStateChanged(object sender, EventArgs e) { modelPanel1.Invalidate(); }

        private void pnlAssets_SelectedBoneChanged(object sender, EventArgs e) { pnlAnim.TransformObject = pnlAssets.SelectedBone; }

        private void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_updating))
                return;

            _maxFrame = (int)numTotalFrames.Value;

            _selectedAnim.FrameCount = _maxFrame;
            numFrameIndex.Maximum = _maxFrame;
        }

        private void pnlOptions_ClearColorChanged(object sender, EventArgs e) { modelPanel1.BackColor = pnlOptions.ClearColor; }

        private void pnlAnim_ReferenceLoaded(ResourceNode node) { modelPanel1.AddReference(node); }
        private void pnlAnim_ReferenceClosed(ResourceNode node) { modelPanel1.RemoveReference(node); }
        private void pnlAnim_SelectedAnimationChanged(object sender, EventArgs e) { _selectedAnim = pnlAnim.SelectedAnimation; AnimChanged(); }

        private void pnlOptions_CamResetClicked(object sender, EventArgs e) { modelPanel1.ResetCamera(); }

        private void pnlAnim_AnimStateChanged(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            if (_animFrame < _selectedAnim.FrameCount)
                SetFrame(_animFrame);
            numTotalFrames.Value = _selectedAnim.FrameCount;
        }
    }
}
