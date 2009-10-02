namespace BrawlScape
{
    partial class ItemFrame
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._iconList = new System.Windows.Forms.ImageList(this.components);
            this._itemList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._textureList = new System.Windows.Forms.ListView();
            this._textureImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _iconList
            // 
            this._iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._iconList.ImageSize = new System.Drawing.Size(64, 64);
            this._iconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _itemList
            // 
            this._itemList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this._itemList.AutoArrange = false;
            this._itemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._itemList.Dock = System.Windows.Forms.DockStyle.Top;
            this._itemList.HideSelection = false;
            this._itemList.LargeImageList = this._iconList;
            this._itemList.Location = new System.Drawing.Point(0, 0);
            this._itemList.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this._itemList.MultiSelect = false;
            this._itemList.Name = "_itemList";
            this._itemList.ShowGroups = false;
            this._itemList.Size = new System.Drawing.Size(540, 108);
            this._itemList.SmallImageList = this._iconList;
            this._itemList.TabIndex = 0;
            this._itemList.TileSize = new System.Drawing.Size(100, 68);
            this._itemList.UseCompatibleStateImageBehavior = false;
            this._itemList.SelectedIndexChanged += new System.EventHandler(this._itemList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 344);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Models";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._textureList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(345, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 344);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Textures";
            // 
            // _textureList
            // 
            this._textureList.AutoArrange = false;
            this._textureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textureList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._textureList.HideSelection = false;
            this._textureList.LargeImageList = this._textureImageList;
            this._textureList.Location = new System.Drawing.Point(3, 16);
            this._textureList.MultiSelect = false;
            this._textureList.Name = "_textureList";
            this._textureList.ShowGroups = false;
            this._textureList.Size = new System.Drawing.Size(189, 325);
            this._textureList.TabIndex = 0;
            this._textureList.UseCompatibleStateImageBehavior = false;
            this._textureList.SelectedIndexChanged += new System.EventHandler(this._textureList_SelectedIndexChanged);
            // 
            // _textureImageList
            // 
            this._textureImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._textureImageList.ImageSize = new System.Drawing.Size(128, 128);
            this._textureImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ItemFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._itemList);
            this.Name = "ItemFrame";
            this.Size = new System.Drawing.Size(540, 452);
            this.Load += new System.EventHandler(this.ItemFrame_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList _iconList;
        private System.Windows.Forms.ListView _itemList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView _textureList;
        private System.Windows.Forms.ImageList _textureImageList;
    }
}
