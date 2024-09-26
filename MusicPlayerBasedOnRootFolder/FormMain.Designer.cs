namespace MusicManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonClose = new System.Windows.Forms.Button();
            this.backgroundWorkerLoadTree = new System.ComponentModel.BackgroundWorker();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonProcess = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorkerPlayList = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playAlbumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(592, 306);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(56, 29);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // backgroundWorkerLoadTree
            // 
            this.backgroundWorkerLoadTree.WorkerReportsProgress = true;
            this.backgroundWorkerLoadTree.WorkerSupportsCancellation = true;
            this.backgroundWorkerLoadTree.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerTree_DoWork);
            this.backgroundWorkerLoadTree.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerTree_ProgressChanged);
            this.backgroundWorkerLoadTree.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerTree_RunWorkerCompleted);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(593, 50);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 29);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.MarqueeAnimationSpeed = 5;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(660, 10);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Visible = false;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(6, 45);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(580, 290);
            this.treeView1.TabIndex = 11;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // buttonProcess
            // 
            this.buttonProcess.Location = new System.Drawing.Point(593, 15);
            this.buttonProcess.Name = "buttonProcess";
            this.buttonProcess.Size = new System.Drawing.Size(56, 29);
            this.buttonProcess.TabIndex = 12;
            this.buttonProcess.Text = "Process";
            this.buttonProcess.UseVisualStyleBackColor = true;
            this.buttonProcess.Click += new System.EventHandler(this.buttonProcess_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(592, 120);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(57, 29);
            this.buttonPlay.TabIndex = 13;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFolder.Location = new System.Drawing.Point(48, 18);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.Size = new System.Drawing.Size(539, 21);
            this.textBoxFolder.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Folder";
            // 
            // backgroundWorkerPlayList
            // 
            this.backgroundWorkerPlayList.WorkerReportsProgress = true;
            this.backgroundWorkerPlayList.WorkerSupportsCancellation = true;
            this.backgroundWorkerPlayList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerPlayList_DoWork);
            this.backgroundWorkerPlayList.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerPlayList_ProgressChanged);
            this.backgroundWorkerPlayList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerPlayList_RunWorkerCompleted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playOneToolStripMenuItem,
            this.playAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(122, 48);
            // 
            // playOneToolStripMenuItem
            // 
            this.playOneToolStripMenuItem.Name = "playOneToolStripMenuItem";
            this.playOneToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.playOneToolStripMenuItem.Text = "Play One";
            this.playOneToolStripMenuItem.Click += new System.EventHandler(this.playOneToolStripMenuItem_Click);
            // 
            // playAllToolStripMenuItem
            // 
            this.playAllToolStripMenuItem.Name = "playAllToolStripMenuItem";
            this.playAllToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.playAllToolStripMenuItem.Text = "Play All";
            this.playAllToolStripMenuItem.Click += new System.EventHandler(this.playAllToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playAlbumToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(136, 26);
            // 
            // playAlbumToolStripMenuItem
            // 
            this.playAlbumToolStripMenuItem.Name = "playAlbumToolStripMenuItem";
            this.playAlbumToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.playAlbumToolStripMenuItem.Text = "Play Album";
            this.playAlbumToolStripMenuItem.Click += new System.EventHandler(this.playAlbumToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(660, 345);
            this.Controls.Add(this.textBoxFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.buttonProcess);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music Player Based On Root Folder";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonClose;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadTree;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button buttonProcess;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorkerPlayList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem playOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem playAlbumToolStripMenuItem;
    }
}

