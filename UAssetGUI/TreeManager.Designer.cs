namespace UAssetGUI
{
    partial class TreeManager
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
            tabControl1 = new System.Windows.Forms.TabControl();
            ImportTab = new System.Windows.Forms.TabPage();
            ImportTree = new System.Windows.Forms.TreeView();
            ExportTab = new System.Windows.Forms.TabPage();
            ExportTree = new System.Windows.Forms.TreeView();
            tabControl1.SuspendLayout();
            ImportTab.SuspendLayout();
            ExportTab.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(ImportTab);
            tabControl1.Controls.Add(ExportTab);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // ImportTab
            // 
            ImportTab.Controls.Add(ImportTree);
            ImportTab.Location = new System.Drawing.Point(4, 24);
            ImportTab.Name = "ImportTab";
            ImportTab.Padding = new System.Windows.Forms.Padding(3);
            ImportTab.Size = new System.Drawing.Size(792, 422);
            ImportTab.TabIndex = 0;
            ImportTab.Text = "Imports";
            ImportTab.UseVisualStyleBackColor = true;
            // 
            // ImportTree
            // 
            ImportTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ImportTree.Dock = System.Windows.Forms.DockStyle.Fill;
            ImportTree.Indent = 19;
            ImportTree.Location = new System.Drawing.Point(3, 3);
            ImportTree.Name = "ImportTree";
            ImportTree.Size = new System.Drawing.Size(786, 416);
            ImportTree.TabIndex = 0;
            // 
            // ExportTab
            // 
            ExportTab.Controls.Add(ExportTree);
            ExportTab.Location = new System.Drawing.Point(4, 24);
            ExportTab.Name = "ExportTab";
            ExportTab.Padding = new System.Windows.Forms.Padding(3);
            ExportTab.Size = new System.Drawing.Size(792, 422);
            ExportTab.TabIndex = 1;
            ExportTab.Text = "Exports";
            ExportTab.UseVisualStyleBackColor = true;
            // 
            // ExportTree
            // 
            ExportTree.Dock = System.Windows.Forms.DockStyle.Fill;
            ExportTree.Indent = 19;
            ExportTree.Location = new System.Drawing.Point(3, 3);
            ExportTree.Name = "ExportTree";
            ExportTree.Size = new System.Drawing.Size(786, 416);
            ExportTree.TabIndex = 0;
            ExportTree.NodeMouseDoubleClick += ExportTree_NodeMouseDoubleClick;
            // 
            // TreeManager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(tabControl1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Name = "TreeManager";
            Text = "TreeManager";
            Load += TreeManager_Load;
            tabControl1.ResumeLayout(false);
            ImportTab.ResumeLayout(false);
            ExportTab.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage ImportTab;
        public System.Windows.Forms.TreeView ImportTree;
        public System.Windows.Forms.TabPage ExportTab;
        public System.Windows.Forms.TreeView ExportTree;
    }
}