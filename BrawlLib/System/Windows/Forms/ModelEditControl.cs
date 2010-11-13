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
        private const float _orbRadius = 1.0f;
        private const float _circRadius = 1.2f;
        private const float _axisSnapRange = 7.0f;
        private const float _selectRange = 0.03f; //Selection error range for orb and circ
        private const float _selectOrbScale = _selectRange / _orbRadius;
        private const float _circOrbScale = _circRadius / _orbRadius;

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
            this.components = new System.ComponentModel.Container();
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
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.spltAssets = new System.Windows.Forms.Splitter();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.pnlOptions = new System.Windows.Forms.ModelOptionPanel();
            this.pnlAssets = new System.Windows.Forms.ModelAssetPanel();
            this.pnlAnim = new System.Windows.Forms.ModelAnimPanel();
            this.pnlPlayback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameIndex)).BeginInit();
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
            this.numFPS.Location = new System.Drawing.Point(42, 10);
            this.numFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFPS.Name = "numFPS";
            this.numFPS.Size = new System.Drawing.Size(40, 20);
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
            this.chkLoop.Location = new System.Drawing.Point(85, 12);
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
            this.modelPanel1.PreRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PreRender);
            this.modelPanel1.PostRender += new System.Windows.Forms.GLRenderEventHandler(this.modelPanel1_PostRender);
            this.modelPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseDown);
            this.modelPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseMove);
            this.modelPanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.modelPanel1_MouseUp);
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
            this.pnlOptions.FloorRenderChanged += new System.EventHandler(this.pnlOptions_FloorRenderChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private int _animFrame, _maxFrame;
        private bool _updating, _loop;
        //private ResourceNode _externalNode;
        //private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        private CHR0Node _selectedAnim;

        private bool _rotating;
        private Vector3 _lastPoint, _oldAngles;
        private bool _snapX, _snapY, _snapZ, _snapCirc;

        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set { ModelChanged(value); }
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
                else if (key == Keys.Escape)
                {
                    if (_rotating)
                    {
                        //Undo rotation, make sure to reset keyframes
                        _rotating = false;
                        pnlAnim.numRotX.Value = _oldAngles._x;
                        pnlAnim.numRotY.Value = _oldAngles._y;
                        pnlAnim.numRotZ.Value = _oldAngles._z;
                        pnlAnim.BoxChanged(pnlAnim.numRotX, null);
                        pnlAnim.BoxChanged(pnlAnim.numRotY, null);
                        pnlAnim.BoxChanged(pnlAnim.numRotZ, null);
                    }
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        public bool CloseFiles()
        {
            return pnlAnim.CloseReferences();
        }

        private void ModelChanged(MDL0Node model)
        {
            //if (_externalNode != null)
            //{
            //    _externalNode.Dispose();
            //    _externalNode = null;
            //}

            if (_targetModel != null)
                modelPanel1.RemoveTarget(_targetModel);

            if ((_targetModel = model) != null)
                modelPanel1.AddTarget(_targetModel);

            modelPanel1.ResetCamera();

            //modelPanel1.TargetModel = _targetModel;

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

        private void pnlOptions_FloorRenderChanged(object sender, EventArgs e)
        {
            modelPanel1.Invalidate();
        }

        private void modelPanel1_PreRender(object sender, GLContext context)
        {
            if (pnlOptions.RenderFloor)
            {
                GLTexture _bgTex = context.FindOrCreate<GLTexture>("TexBG", GLTexturePanel.CreateBG);

                float s = 10.0f, t = 10.0f;
                float e = 30.0f;

                context.glDisable((uint)GLEnableCap.Lighting);
                context.glEnable(GLEnableCap.DepthTest);
                context.glPolygonMode(GLFace.Front, GLPolygonMode.Fill);
                context.glPolygonMode(GLFace.Back, GLPolygonMode.Line);

                context.glEnable(GLEnableCap.Texture2D);
                _bgTex.Bind();

                context.glColor(0.5f, 0.5f, 0.75f, 1.0f);
                context.glBegin(GLPrimitiveType.Quads);

                context.glTexCoord(0.0f, 0.0f);
                context.glVertex(-e, 0.0f, -e);
                context.glTexCoord(s, 0.0f);
                context.glVertex(e, 0.0f, -e);
                context.glTexCoord(s, t);
                context.glVertex(e, 0.0f, e);
                context.glTexCoord(0, t);
                context.glVertex(-e, 0.0f, e);

                context.glEnd();
            }
        }

        private unsafe void modelPanel1_PostRender(object sender, GLContext context)
        {
            context.glClear(GLClearMask.DepthBuffer);
            context.glEnable(GLEnableCap.DepthTest);

            if (pnlAssets.SelectedBone != null)
            {
                context.glDisable((uint)GLEnableCap.Lighting);

                //Render rotation orb
                MDL0BoneNode bone = pnlAssets.SelectedBone;
                GLDisplayList circle = context.GetRingList();
                GLDisplayList sphere = context.GetCircleList();
                Matrix m;

                //Prepare camera-facing matrix
                Vector3 center = bone._frameMatrix.GetPoint();
                Vector3 cam = modelPanel1._camera.GetPoint();
                float radius = center.TrueDistance(cam) / _orbRadius * 0.1f;

                m = Matrix.TransformMatrix(new Vector3(radius), center.LookatAngles(cam) * Maths._rad2degf, center);
                context.glPushMatrix();
                context.glMultMatrix((float*)&m);

                //Orb
                context.glColor(0.7f, 0.7f, 0.7f, 0.15f);
                sphere.Call();

                context.glDisable((uint)GLEnableCap.DepthTest);

                //Container
                context.glColor(0.4f, 0.4f, 0.4f, 1.0f);
                circle.Call();

                //Circ
                if (_snapCirc) context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else context.glColor(1.0f, 0.8f, 0.5f, 1.0f);
                context.glScale(_circOrbScale, _circOrbScale, _circOrbScale);
                circle.Call();

                //Pop
                context.glPopMatrix();

                context.glEnable(GLEnableCap.DepthTest);

                //Enter local space
                m = Matrix.TransformMatrix(new Vector3(radius), bone._frameMatrix.GetAngles(), center);
                context.glPushMatrix();
                context.glMultMatrix((float*)&m);

                //Z
                if (_snapZ) context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else context.glColor(0.0f, 0.0f, 1.0f, 1.0f);
                circle.Call();

                context.glRotate(90.0f, 0.0f, 1.0f, 0.0f);

                //X
                if (_snapX) context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else context.glColor(1.0f, 0.0f, 0.0f, 1.0f);
                circle.Call();

                context.glRotate(90.0f, 1.0f, 0.0f, 0.0f);

                //Y
                if (_snapY) context.glColor(1.0f, 1.0f, 0.0f, 1.0f);
                else context.glColor(0.0f, 1.0f, 0.0f, 1.0f);
                circle.Call();

                //Pop
                context.glPopMatrix();

                //Clear depth buffer for next operation
                context.glClear(GLClearMask.DepthBuffer);
            }

            //Render invisible depth orbs
            context.glColorMask(false, false, false, false);

            if (_targetModel != null && _targetModel._boneList != null)
            {
                GLDisplayList list = context.GetSphereList();
                foreach (MDL0BoneNode bone in _targetModel._boneList)
                    RenderOrbRecursive(bone, context, list);
            }

            context.glColorMask(true, true, true, true);
        }

        private unsafe void RenderOrbRecursive(MDL0BoneNode bone, GLContext ctx, GLDisplayList list)
        {
            Matrix m = Matrix.TransformMatrix(new Vector3(MDL0BoneNode._nodeRadius), new Vector3(), bone._frameMatrix.GetPoint());
            ctx.glPushMatrix();
            ctx.glMultMatrix((float*)&m);

            list.Call();
            ctx.glPopMatrix();

            foreach (MDL0BoneNode b in bone.Children)
                RenderOrbRecursive(b, ctx, list);
        }

        private void modelPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != Forms.MouseButtons.Left)
                return;

            //Reset snap flags
            _snapX = _snapY = _snapZ = _snapCirc = false;

            MDL0BoneNode bone = pnlAssets.SelectedBone;

            //Re-target selected bone
            if (bone != null)
            {
                //Try to re-target selected node
                Vector3 center = bone._frameMatrix.GetPoint();

                //Standard radius scaling snippet. This is used for orb scaling depending on camera distance.
                float radius = center.TrueDistance(modelPanel1._camera.GetPoint()) / _orbRadius * 0.1f;

                //Get point projected onto our orb.
                Vector3 point = modelPanel1.ProjectCameraSphere(new Vector2(e.X, e.Y), center, radius, false);

                //Check distances
                float distance = point.TrueDistance(center);


                if (Math.Abs(distance - radius) < (radius * _selectOrbScale)) //Point lies within orb radius
                {
                    //Determine axis snapping
                    Vector3 angles = (bone._inverseFrameMatrix * point).GetAngles() * Maths._rad2degf;
                    angles._x = (float)Math.Abs(angles._x);
                    angles._y = (float)Math.Abs(angles._y);
                    angles._z = (float)Math.Abs(angles._z);

                    if (Math.Abs(angles._y - 90.0f) <= _axisSnapRange)
                        _snapX = true;
                    else if (angles._x >= (180 - _axisSnapRange) || angles._x <= _axisSnapRange)
                        _snapY = true;
                    else if (angles._y >= (180 - _axisSnapRange) || angles._y <= _axisSnapRange)
                        _snapZ = true;
                }
                else if (Math.Abs(distance - (radius * _circOrbScale)) < (radius * _selectOrbScale)) //Point lies on circ line
                    _snapCirc = true;
                else
                {
                    //Orb selection missed. Assign bone and move to next step.
                    pnlAssets.SelectedBone = bone = null;
                    goto Next;
                }

                //Bone re-targeted. Get angles and local point (aligned to snapping plane).
                if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                {
                    _rotating = true;
                    _oldAngles = bone._frameState._rotate;
                    _lastPoint = bone._inverseFrameMatrix * point;
                }

                //Ensure a redraw so the snapping indicators are correct
                modelPanel1.Invalidate();
            }

        Next:

            //Try selecting new bone
            if (bone == null)
            {
                float depth = modelPanel1.GetDepth(e.X, e.Y);
                if ((depth < 1.0f) && (_targetModel != null) && (_targetModel._boneList != null))
                {
                    Vector3 point = modelPanel1.UnProject(e.X, e.Y, depth);

                    //Find orb near chosen point
                    foreach (MDL0BoneNode b in _targetModel._boneList)
                        if (CompareDistanceRecursive(b, point, ref bone))
                            break;

                    //Assign new bone
                    if (bone != null)
                        pnlAssets.SelectedBone = bone;

                    //No need to redraw.
                }
            }
        }
        private void modelPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == Forms.MouseButtons.Left)
                _rotating = false;
        }
        private unsafe void modelPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            MDL0BoneNode bone = pnlAssets.SelectedBone;
            if (_rotating && bone != null)
            {
                Vector3 point;
                if (GetOrbPoint(new Vector2(e.X, e.Y), out point))
                {
                    //Convert to local point
                    Vector3 lPoint = bone._inverseFrameMatrix * point;

                    //Check for change in selection.
                    if (_lastPoint != lPoint)
                    {
                        //Get matrix with new rotation applied
                        Matrix m = bone._frameState._transform * Matrix.AxisAngleMatrix(_lastPoint, lPoint);

                        //Derive angles from matrices, get difference
                        Vector3 angles = m.GetAngles() - bone._frameState._transform.GetAngles();

                        //Truncate (allows winding)
                        if (angles._x > 180.0f) angles._x -= 360.0f;
                        if (angles._y > 180.0f) angles._y -= 360.0f;
                        if (angles._z > 180.0f) angles._z -= 360.0f;
                        if (angles._x < -180.0f) angles._x += 360.0f;
                        if (angles._y < -180.0f) angles._y += 360.0f;
                        if (angles._z < -180.0f) angles._z += 360.0f;

                        //Apply difference to axes that have changed (pnlAnim should handle this so keyframes are created)
                        if (angles._x != 0.0f) ApplyAngle(0, angles._x);
                        if (angles._y != 0.0f) ApplyAngle(1, angles._y);
                        if (angles._z != 0.0f) ApplyAngle(2, angles._z);

                        //Find new local mouse-point (should be the same)
                        //_lastPoint = bone._inverseFrameMatrix * point;

                        //Redraw (taken care of by pnlAnim)
                        //modelPanel1.Invalidate();
                    }
                }
            }
        }

        //Updates specified angle by applying an offset.
        //Allows pnlAnim to handle the changes so keyframes are updated.
        private unsafe void ApplyAngle(int index, float offset)
        {
            NumericInputBox box = pnlAnim._transBoxes[index + 3];
            box.Value = (float)Math.Round(box._value + offset, 3);
            pnlAnim.BoxChanged(box, null);
        }

        //Gets world-point of specified mouse point projected onto the selected bone's local space.
        //Intersects the projected ray with the appropriate plane using the snap flags.
        private bool GetOrbPoint(Vector2 mousePoint, out Vector3 point)
        {
            MDL0BoneNode bone = pnlAssets.SelectedBone;
            if (bone == null)
            {
                point = new Vector3();
                return false;
            }

            Vector3 lineStart = modelPanel1.UnProject(mousePoint._x, mousePoint._y, 0.0f);
            Vector3 lineEnd = modelPanel1.UnProject(mousePoint._x, mousePoint._y, 1.0f);
            Vector3 center = bone._frameMatrix.GetPoint();
            Vector3 camera = modelPanel1._camera.GetPoint();
            Vector3 normal;
            float radius = center.TrueDistance(camera) / _orbRadius * 0.1f;

            if (_snapX)
                normal = (bone._frameMatrix * new Vector3(1.0f, 0.0f, 0.0f)).Normalize(center);
            else if (_snapY)
                normal = (bone._frameMatrix * new Vector3(0.0f, 1.0f, 0.0f)).Normalize(center);
            else if (_snapZ)
                normal = (bone._frameMatrix * new Vector3(0.0f, 0.0f, 1.0f)).Normalize(center);
            else if (_snapCirc)
            {
                radius *= _circOrbScale;
                normal = camera.Normalize(center);
            }
            else if (Maths.LineSphereIntersect(lineStart, lineEnd, center, radius, out point))
                return true;
            else
                normal = camera.Normalize(center);

            if (Maths.LinePlaneIntersect(lineStart, lineEnd, center, normal, out point))
            {
                point = Maths.PointAtLineDistance(center, point, radius);
                return true;
            }

            point = new Vector3();
            return false;
        }

        private bool CompareDistanceRecursive(MDL0BoneNode bone, Vector3 point, ref MDL0BoneNode match)
        {
            Vector3 center = bone._frameMatrix.GetPoint();
            float dist = center.TrueDistance(point);

            if (Math.Abs(dist - MDL0BoneNode._nodeRadius) < 0.01)
            {
                match = bone;
                return true;
            }

            foreach (MDL0BoneNode b in bone.Children)
                if (CompareDistanceRecursive(b, point, ref match))
                    return true;

            return false;
        }


    }
}
