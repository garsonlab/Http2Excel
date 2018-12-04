namespace HttpClient
{
    partial class HttpTool
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HttpTool));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.配置相关ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上传配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.配置相关ToolStripMenuItem,
            this.上传配置ToolStripMenuItem,
            this.查看列表ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(314, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 配置相关ToolStripMenuItem
            // 
            this.配置相关ToolStripMenuItem.Name = "配置相关ToolStripMenuItem";
            this.配置相关ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.配置相关ToolStripMenuItem.Text = "刷新配置";
            this.配置相关ToolStripMenuItem.Click += new System.EventHandler(this.配置相关ToolStripMenuItem_Click);
            // 
            // 上传配置ToolStripMenuItem
            // 
            this.上传配置ToolStripMenuItem.Name = "上传配置ToolStripMenuItem";
            this.上传配置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.上传配置ToolStripMenuItem.Text = "上传配置";
            this.上传配置ToolStripMenuItem.Click += new System.EventHandler(this.上传配置ToolStripMenuItem_Click);
            // 
            // 查看列表ToolStripMenuItem
            // 
            this.查看列表ToolStripMenuItem.Name = "查看列表ToolStripMenuItem";
            this.查看列表ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.查看列表ToolStripMenuItem.Text = "用户列表";
            this.查看列表ToolStripMenuItem.Click += new System.EventHandler(this.查看列表ToolStripMenuItem_Click);
            // 
            // gridView
            // 
            this.gridView.AllowUserToAddRows = false;
            this.gridView.AllowUserToDeleteRows = false;
            this.gridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView.Location = new System.Drawing.Point(12, 24);
            this.gridView.Name = "gridView";
            this.gridView.ReadOnly = true;
            this.gridView.RowTemplate.Height = 23;
            this.gridView.Size = new System.Drawing.Size(290, 340);
            this.gridView.TabIndex = 3;
            this.gridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridView_MouseDoubleClick);
            this.gridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.gridView_RowPostPaint);
            // 
            // HttpTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 376);
            this.Controls.Add(this.gridView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HttpTool";
            this.Text = "信息转换";
            this.Load += new System.EventHandler(this.HttpTool_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 配置相关ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上传配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看列表ToolStripMenuItem;
        private System.Windows.Forms.DataGridView gridView;
    }
}

