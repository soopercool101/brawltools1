using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class AnimEditControl : UserControl
    {
        private int _numFrames;
        private bool _updating = false;

        private int _currentPage = 1;
        private AnimationKeyframe _currentFrame = AnimationKeyframe.Neutral;
        private ListBox listKeyframes;
        private Button btnAddKf;
        private Button btnDelKf;
        private GroupBox groupBox1;

        private CHR0EntryNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0EntryNode TargetSequence
        {
            get { return _target; }
            set
            {
                if (_target == value)
                    return;


                //_updating = true;

                if ((_target != null) && (_target.FrameCount > 0))
                {
                    _target = value;

                    _numFrames = _target.FrameCount;
                    numFrame.Value = 1;
                    numFrame.Maximum = _numFrames;
                    lblFrameCount.Text = String.Format("/ {0}", _numFrames);
                }
                else
                {
                    _target = null;
                    numFrame.Value = 1;
                }

                //_updating = false;

                RefreshPage();
            }
        }

        public AnimEditControl() { InitializeComponent(); }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!_updating)
                RefreshPage();
        }

        private void RefreshPage()
        {
            if (_target == null)
            {
            }
            else
            {
                _currentPage = (int)numFrame.Value;

                _currentFrame = _target[_currentPage - 1];

                _updating = true;

                numScaleX.Value = _currentFrame.Scale._x;
                numScaleY.Value = _currentFrame.Scale._y;
                numScaleZ.Value = _currentFrame.Scale._z;

                numRotX.Value = _currentFrame.Rotation._x;
                numRotY.Value = _currentFrame.Rotation._y;
                numRotZ.Value = _currentFrame.Rotation._z;

                numTransX.Value = _currentFrame.Translation._x;
                numTransY.Value = _currentFrame.Translation._y;
                numTransZ.Value = _currentFrame.Translation._z;

                _updating = false;

                btnPrev.Enabled = _currentPage > 1;
                btnNext.Enabled = _currentPage < _numFrames;
            }
        }

        private void numScaleX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numScaleX.Value;
            if (val != _currentFrame.Scale._x)
            {
                _currentFrame.Scale._x = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numScaleY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numScaleY.Value;
            if (val != _currentFrame.Scale._y)
            {
                _currentFrame.Scale._y = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numScaleZ_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numScaleZ.Value;
            if (val != _currentFrame.Scale._z)
            {
                _currentFrame.Scale._z = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }

        private void numRotX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numRotX.Value;
            if (val != _currentFrame.Rotation._x)
            {
                _currentFrame.Rotation._x = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numRotY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numRotY.Value;
            if (val != _currentFrame.Rotation._y)
            {
                _currentFrame.Rotation._y = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numRotZ_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numRotZ.Value;
            if (val != _currentFrame.Rotation._z)
            {
                _currentFrame.Rotation._z = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }

        private void numTransX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numTransX.Value;
            if (val != _currentFrame.Translation._x)
            {
                _currentFrame.Translation._x = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numTransY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numTransY.Value;
            if (val != _currentFrame.Translation._y)
            {
                _currentFrame.Translation._y = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }
        private void numTransZ_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;

            float val = (float)numTransZ.Value;
            if (val != _currentFrame.Translation._z)
            {
                _currentFrame.Translation._z = val;
                _target[_currentPage - 1] = _currentFrame;
            }
        }


        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value = numFrame.Value - 1; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value = numFrame.Value + 1; }

        #region Designer

        private Label label1;
        private Label label2;
        private Label label3;
        private NumericInputBox numScaleX;
        private Label label4;
        private NumericInputBox numRotX;
        private NumericInputBox numTransX;
        private Label label5;
        private Label label6;
        private NumericInputBox numScaleY;
        private NumericInputBox numTransY;
        private NumericInputBox numRotY;
        private NumericInputBox numScaleZ;
        private NumericInputBox numTransZ;
        private Label label7;
        private NumericUpDown numFrame;
        private Label lblFrameCount;
        private Button btnPrev;
        private Button btnNext;
        private NumericInputBox numRotZ;

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numScaleX = new NumericInputBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.listKeyframes = new System.Windows.Forms.ListBox();
            this.btnAddKf = new System.Windows.Forms.Button();
            this.btnDelKf = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 161);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scale";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(12, 199);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Translation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(12, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Rotation";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numScaleX
            // 
            this.numScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleX.Location = new System.Drawing.Point(81, 161);
            this.numScaleX.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(70, 20);
            this.numScaleX.TabIndex = 3;
            this.numScaleX.ValueChanged += new System.EventHandler(this.numScaleX_ValueChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(81, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "X";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRotX
            // 
            this.numRotX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotX.Location = new System.Drawing.Point(81, 180);
            this.numRotX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(70, 20);
            this.numRotX.TabIndex = 6;
            this.numRotX.ValueChanged += new System.EventHandler(this.numRotX_ValueChanged);
            // 
            // numTransX
            // 
            this.numTransX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransX.Location = new System.Drawing.Point(81, 199);
            this.numTransX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(70, 20);
            this.numTransX.TabIndex = 9;
            this.numTransX.ValueChanged += new System.EventHandler(this.numTransX_ValueChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(150, 142);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Y";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(219, 142);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Z";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numScaleY
            // 
            this.numScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleY.Location = new System.Drawing.Point(150, 161);
            this.numScaleY.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(70, 20);
            this.numScaleY.TabIndex = 4;
            this.numScaleY.ValueChanged += new System.EventHandler(this.numScaleY_ValueChanged);
            // 
            // numTransY
            // 
            this.numTransY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransY.Location = new System.Drawing.Point(150, 199);
            this.numTransY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(70, 20);
            this.numTransY.TabIndex = 10;
            this.numTransY.ValueChanged += new System.EventHandler(this.numTransY_ValueChanged);
            // 
            // numRotY
            // 
            this.numRotY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotY.Location = new System.Drawing.Point(150, 180);
            this.numRotY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(70, 20);
            this.numRotY.TabIndex = 7;
            this.numRotY.ValueChanged += new System.EventHandler(this.numRotY_ValueChanged);
            // 
            // numScaleZ
            // 
            this.numScaleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numScaleZ.Location = new System.Drawing.Point(219, 161);
            this.numScaleZ.Margin = new System.Windows.Forms.Padding(0);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(70, 20);
            this.numScaleZ.TabIndex = 5;
            this.numScaleZ.ValueChanged += new System.EventHandler(this.numScaleZ_ValueChanged);
            // 
            // numTransZ
            // 
            this.numTransZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTransZ.Location = new System.Drawing.Point(219, 199);
            this.numTransZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(70, 20);
            this.numTransZ.TabIndex = 11;
            this.numTransZ.ValueChanged += new System.EventHandler(this.numTransZ_ValueChanged);
            // 
            // numRotZ
            // 
            this.numRotZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numRotZ.Location = new System.Drawing.Point(219, 180);
            this.numRotZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(70, 20);
            this.numRotZ.TabIndex = 8;
            this.numRotZ.ValueChanged += new System.EventHandler(this.numRotZ_ValueChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(39, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Frame:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            this.numFrame.Location = new System.Drawing.Point(106, 107);
            this.numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(58, 20);
            this.numFrame.TabIndex = 0;
            this.numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Location = new System.Drawing.Point(170, 107);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(45, 20);
            this.lblFrameCount.TabIndex = 17;
            this.lblFrameCount.Text = "/ 10";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.Location = new System.Drawing.Point(221, 106);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 23);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "<";
            this.btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(246, 106);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = ">";
            this.btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // listKeyframes
            // 
            this.listKeyframes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listKeyframes.FormattingEnabled = true;
            this.listKeyframes.IntegralHeight = false;
            this.listKeyframes.Location = new System.Drawing.Point(6, 19);
            this.listKeyframes.Name = "listKeyframes";
            this.listKeyframes.Size = new System.Drawing.Size(263, 52);
            this.listKeyframes.TabIndex = 18;
            // 
            // btnAddKf
            // 
            this.btnAddKf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddKf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddKf.Location = new System.Drawing.Point(275, 19);
            this.btnAddKf.Name = "btnAddKf";
            this.btnAddKf.Size = new System.Drawing.Size(15, 23);
            this.btnAddKf.TabIndex = 19;
            this.btnAddKf.Text = "+";
            this.btnAddKf.UseVisualStyleBackColor = true;
            // 
            // btnDelKf
            // 
            this.btnDelKf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelKf.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelKf.Location = new System.Drawing.Point(275, 48);
            this.btnDelKf.Name = "btnDelKf";
            this.btnDelKf.Size = new System.Drawing.Size(15, 23);
            this.btnDelKf.TabIndex = 20;
            this.btnDelKf.Text = "-";
            this.btnDelKf.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listKeyframes);
            this.groupBox1.Controls.Add(this.btnDelKf);
            this.groupBox1.Controls.Add(this.btnAddKf);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 78);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyframes";
            // 
            // AnimEditControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblFrameCount);
            this.Controls.Add(this.numFrame);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numScaleZ);
            this.Controls.Add(this.numTransZ);
            this.Controls.Add(this.numRotZ);
            this.Controls.Add(this.numScaleY);
            this.Controls.Add(this.numTransY);
            this.Controls.Add(this.numRotY);
            this.Controls.Add(this.numScaleX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numTransX);
            this.Controls.Add(this.numRotX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AnimEditControl";
            this.Size = new System.Drawing.Size(302, 258);
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
