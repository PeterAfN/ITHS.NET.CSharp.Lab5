namespace ITHS.NET.Peter.Palosaari.Lab5
{
    partial class FormMain
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
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.textBoxImageLinks = new System.Windows.Forms.TextBox();
            this.labelImagesFound = new System.Windows.Forms.Label();
            this.buttonExtract = new System.Windows.Forms.Button();
            this.buttonSaveImages = new System.Windows.Forms.Button();
            this.tableLayoutPanelInput = new System.Windows.Forms.TableLayoutPanel();
            this.labelFault = new System.Windows.Forms.Label();
            this.tableLayoutPanelInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxURL
            // 
            this.textBoxURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxURL.Location = new System.Drawing.Point(3, 5);
            this.textBoxURL.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(923, 20);
            this.textBoxURL.TabIndex = 0;
            // 
            // textBoxImageLinks
            // 
            this.textBoxImageLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxImageLinks.Location = new System.Drawing.Point(10, 40);
            this.textBoxImageLinks.Multiline = true;
            this.textBoxImageLinks.Name = "textBoxImageLinks";
            this.textBoxImageLinks.ReadOnly = true;
            this.textBoxImageLinks.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxImageLinks.Size = new System.Drawing.Size(1006, 437);
            this.textBoxImageLinks.TabIndex = 1;
            this.textBoxImageLinks.WordWrap = false;
            // 
            // labelImagesFound
            // 
            this.labelImagesFound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelImagesFound.AutoSize = true;
            this.labelImagesFound.Location = new System.Drawing.Point(11, 505);
            this.labelImagesFound.Name = "labelImagesFound";
            this.labelImagesFound.Size = new System.Drawing.Size(95, 15);
            this.labelImagesFound.TabIndex = 2;
            this.labelImagesFound.Text = "Images found: 0";
            // 
            // buttonExtract
            // 
            this.buttonExtract.Location = new System.Drawing.Point(932, 3);
            this.buttonExtract.MinimumSize = new System.Drawing.Size(77, 23);
            this.buttonExtract.Name = "buttonExtract";
            this.buttonExtract.Size = new System.Drawing.Size(77, 23);
            this.buttonExtract.TabIndex = 3;
            this.buttonExtract.Text = "Extract";
            this.buttonExtract.UseVisualStyleBackColor = true;
            this.buttonExtract.Click += new System.EventHandler(this.ButtonExtract_Click);
            // 
            // buttonSaveImages
            // 
            this.buttonSaveImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveImages.Location = new System.Drawing.Point(938, 494);
            this.buttonSaveImages.MinimumSize = new System.Drawing.Size(77, 23);
            this.buttonSaveImages.Name = "buttonSaveImages";
            this.buttonSaveImages.Size = new System.Drawing.Size(80, 23);
            this.buttonSaveImages.TabIndex = 4;
            this.buttonSaveImages.Text = "Save images";
            this.buttonSaveImages.UseVisualStyleBackColor = true;
            this.buttonSaveImages.Click += new System.EventHandler(this.ButtonSaveImages_Click);
            // 
            // tableLayoutPanelInput
            // 
            this.tableLayoutPanelInput.ColumnCount = 2;
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanelInput.Controls.Add(this.buttonExtract, 1, 0);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxURL, 0, 0);
            this.tableLayoutPanelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelInput.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanelInput.Name = "tableLayoutPanelInput";
            this.tableLayoutPanelInput.RowCount = 1;
            this.tableLayoutPanelInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanelInput.Size = new System.Drawing.Size(1015, 27);
            this.tableLayoutPanelInput.TabIndex = 5;
            // 
            // labelFault
            // 
            this.labelFault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFault.AutoSize = true;
            this.labelFault.Location = new System.Drawing.Point(10, 486);
            this.labelFault.Name = "labelFault";
            this.labelFault.Size = new System.Drawing.Size(0, 15);
            this.labelFault.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 533);
            this.Controls.Add(this.labelFault);
            this.Controls.Add(this.tableLayoutPanelInput);
            this.Controls.Add(this.buttonSaveImages);
            this.Controls.Add(this.labelImagesFound);
            this.Controls.Add(this.textBoxImageLinks);
            this.DoubleBuffered = true;
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Scraper";
            this.tableLayoutPanelInput.ResumeLayout(false);
            this.tableLayoutPanelInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.TextBox textBoxImageLinks;
        private System.Windows.Forms.Label labelImagesFound;
        private System.Windows.Forms.Button buttonExtract;
        private System.Windows.Forms.Button buttonSaveImages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInput;
        private System.Windows.Forms.Label labelFault;
    }
}

