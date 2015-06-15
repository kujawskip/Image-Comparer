namespace SimilarityFinder
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pictureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPictureLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondPictureLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comparePicturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPicturePanel = new System.Windows.Forms.Panel();
            this.secondPicturePanel = new System.Windows.Forms.Panel();
            this.picturesPanel = new System.Windows.Forms.Panel();
            this.geometricSimilarityLabel = new System.Windows.Forms.Label();
            this.colorSimilarityLabel = new System.Windows.Forms.Label();
            this.smoothnessSimilarityLabel = new System.Windows.Forms.Label();
            this.preparePicturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstPictureThresholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondPictureThresholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.similarityCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geometricalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smoothnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.picturesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pictureMenuItem,
            this.comparePicturesToolStripMenuItem,
            this.preparePicturesToolStripMenuItem,
            this.similarityCheckToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(654, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pictureMenuItem
            // 
            this.pictureMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstPictureLoadMenuItem,
            this.secondPictureLoadMenuItem,
            this.clearImagesToolStripMenuItem});
            this.pictureMenuItem.Name = "pictureMenuItem";
            this.pictureMenuItem.Size = new System.Drawing.Size(56, 20);
            this.pictureMenuItem.Text = "Picture";
            // 
            // firstPictureLoadMenuItem
            // 
            this.firstPictureLoadMenuItem.Name = "firstPictureLoadMenuItem";
            this.firstPictureLoadMenuItem.Size = new System.Drawing.Size(181, 22);
            this.firstPictureLoadMenuItem.Text = "Load first picture";
            this.firstPictureLoadMenuItem.Click += new System.EventHandler(this.firstPictureLoadMenuItem_Click);
            // 
            // secondPictureLoadMenuItem
            // 
            this.secondPictureLoadMenuItem.Name = "secondPictureLoadMenuItem";
            this.secondPictureLoadMenuItem.Size = new System.Drawing.Size(181, 22);
            this.secondPictureLoadMenuItem.Text = "Load second picture";
            this.secondPictureLoadMenuItem.Click += new System.EventHandler(this.secondPictureLoadMenuItem_Click);
            // 
            // clearImagesToolStripMenuItem
            // 
            this.clearImagesToolStripMenuItem.Name = "clearImagesToolStripMenuItem";
            this.clearImagesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.clearImagesToolStripMenuItem.Text = "Clear Images";
            this.clearImagesToolStripMenuItem.Click += new System.EventHandler(this.clearImagesToolStripMenuItem_Click);
            // 
            // comparePicturesToolStripMenuItem
            // 
            this.comparePicturesToolStripMenuItem.Name = "comparePicturesToolStripMenuItem";
            this.comparePicturesToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.comparePicturesToolStripMenuItem.Text = "Compare Pictures";
            this.comparePicturesToolStripMenuItem.Click += new System.EventHandler(this.comparePicturesToolStripMenuItem_Click);
            // 
            // firstPicturePanel
            // 
            this.firstPicturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.firstPicturePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.firstPicturePanel.Location = new System.Drawing.Point(0, 0);
            this.firstPicturePanel.Name = "firstPicturePanel";
            this.firstPicturePanel.Size = new System.Drawing.Size(313, 308);
            this.firstPicturePanel.TabIndex = 1;
            // 
            // secondPicturePanel
            // 
            this.secondPicturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.secondPicturePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.secondPicturePanel.Location = new System.Drawing.Point(322, 0);
            this.secondPicturePanel.Name = "secondPicturePanel";
            this.secondPicturePanel.Size = new System.Drawing.Size(332, 308);
            this.secondPicturePanel.TabIndex = 2;
            // 
            // picturesPanel
            // 
            this.picturesPanel.Controls.Add(this.secondPicturePanel);
            this.picturesPanel.Controls.Add(this.firstPicturePanel);
            this.picturesPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picturesPanel.Location = new System.Drawing.Point(0, 45);
            this.picturesPanel.Name = "picturesPanel";
            this.picturesPanel.Size = new System.Drawing.Size(654, 308);
            this.picturesPanel.TabIndex = 1;
            // 
            // geometricSimilarityLabel
            // 
            this.geometricSimilarityLabel.AutoSize = true;
            this.geometricSimilarityLabel.Location = new System.Drawing.Point(12, 29);
            this.geometricSimilarityLabel.Name = "geometricSimilarityLabel";
            this.geometricSimilarityLabel.Size = new System.Drawing.Size(132, 13);
            this.geometricSimilarityLabel.TabIndex = 2;
            this.geometricSimilarityLabel.Text = "Geometrical similarity: NaN";
            // 
            // colorSimilarityLabel
            // 
            this.colorSimilarityLabel.AutoSize = true;
            this.colorSimilarityLabel.Location = new System.Drawing.Point(150, 29);
            this.colorSimilarityLabel.Name = "colorSimilarityLabel";
            this.colorSimilarityLabel.Size = new System.Drawing.Size(100, 13);
            this.colorSimilarityLabel.TabIndex = 3;
            this.colorSimilarityLabel.Text = "Color similarity: NaN";
            // 
            // smoothnessSimilarityLabel
            // 
            this.smoothnessSimilarityLabel.AutoSize = true;
            this.smoothnessSimilarityLabel.Location = new System.Drawing.Point(256, 29);
            this.smoothnessSimilarityLabel.Name = "smoothnessSimilarityLabel";
            this.smoothnessSimilarityLabel.Size = new System.Drawing.Size(134, 13);
            this.smoothnessSimilarityLabel.TabIndex = 4;
            this.smoothnessSimilarityLabel.Text = "Smoothness similarity: NaN";
            // 
            // preparePicturesToolStripMenuItem
            // 
            this.preparePicturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstPictureThresholdToolStripMenuItem,
            this.secondPictureThresholdToolStripMenuItem});
            this.preparePicturesToolStripMenuItem.Name = "preparePicturesToolStripMenuItem";
            this.preparePicturesToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.preparePicturesToolStripMenuItem.Text = "Prepare pictures";
            // 
            // firstPictureThresholdToolStripMenuItem
            // 
            this.firstPictureThresholdToolStripMenuItem.Name = "firstPictureThresholdToolStripMenuItem";
            this.firstPictureThresholdToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.firstPictureThresholdToolStripMenuItem.Text = "First picture threshold";
            this.firstPictureThresholdToolStripMenuItem.Click += new System.EventHandler(this.firstPictureThresholdToolStripMenuItem_Click);
            // 
            // secondPictureThresholdToolStripMenuItem
            // 
            this.secondPictureThresholdToolStripMenuItem.Name = "secondPictureThresholdToolStripMenuItem";
            this.secondPictureThresholdToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.secondPictureThresholdToolStripMenuItem.Text = "Second picture threshold";
            this.secondPictureThresholdToolStripMenuItem.Click += new System.EventHandler(this.secondPictureThresholdToolStripMenuItem_Click);
            // 
            // similarityCheckToolStripMenuItem
            // 
            this.similarityCheckToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geometricalToolStripMenuItem,
            this.colorToolStripMenuItem,
            this.smoothnessToolStripMenuItem});
            this.similarityCheckToolStripMenuItem.Name = "similarityCheckToolStripMenuItem";
            this.similarityCheckToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.similarityCheckToolStripMenuItem.Text = "SimilarityCheck";
            // 
            // geometricalToolStripMenuItem
            // 
            this.geometricalToolStripMenuItem.Checked = true;
            this.geometricalToolStripMenuItem.CheckOnClick = true;
            this.geometricalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.geometricalToolStripMenuItem.Name = "geometricalToolStripMenuItem";
            this.geometricalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.geometricalToolStripMenuItem.Text = "Geometrical";
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Checked = true;
            this.colorToolStripMenuItem.CheckOnClick = true;
            this.colorToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.colorToolStripMenuItem.Text = "Color";
            // 
            // smoothnessToolStripMenuItem
            // 
            this.smoothnessToolStripMenuItem.Checked = true;
            this.smoothnessToolStripMenuItem.CheckOnClick = true;
            this.smoothnessToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.smoothnessToolStripMenuItem.Name = "smoothnessToolStripMenuItem";
            this.smoothnessToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.smoothnessToolStripMenuItem.Text = "Smoothness";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 353);
            this.Controls.Add(this.smoothnessSimilarityLabel);
            this.Controls.Add(this.colorSimilarityLabel);
            this.Controls.Add(this.geometricSimilarityLabel);
            this.Controls.Add(this.picturesPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.picturesPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pictureMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstPictureLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondPictureLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comparePicturesToolStripMenuItem;
        private System.Windows.Forms.Panel firstPicturePanel;
        private System.Windows.Forms.Panel secondPicturePanel;
        private System.Windows.Forms.Panel picturesPanel;
        private System.Windows.Forms.Label geometricSimilarityLabel;
        private System.Windows.Forms.Label colorSimilarityLabel;
        private System.Windows.Forms.Label smoothnessSimilarityLabel;
        private System.Windows.Forms.ToolStripMenuItem preparePicturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem firstPictureThresholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem secondPictureThresholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem similarityCheckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geometricalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smoothnessToolStripMenuItem;

    }
}

