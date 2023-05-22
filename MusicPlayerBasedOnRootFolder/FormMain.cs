using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MusicManager
{
    internal partial class FormMain : Form
    {
        #region Fields

        private readonly AppConfigInfo _appConfigInfo;
        private TreeNode _treeNodeRoot;
        
        #endregion

        #region Constructors

        public FormMain(AppConfigInfo appConfigInfo)
        {
            _appConfigInfo = appConfigInfo;   

            InitializeComponent();
        }

        #endregion

        #region UI Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = $"{this.Text} - Version: {System.Windows.Forms.Application.ProductVersion}";
            this.textBoxFolder.Text = _appConfigInfo.FolderRoot;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
                return;

            if (backgroundWorker1.WorkerSupportsCancellation == true)
                backgroundWorker1.CancelAsync();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView1.SelectedNode;
                string files = Xpto(treeView1.SelectedNode);
                Process.Start(_appConfigInfo.MusicPlayerApplication, files);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            //https://learn.microsoft.com/en-us/dotnet/desktop/winforms/order-of-events-in-windows-forms?view=netframeworkdesktop-4.8

            this.buttonProcess_Click(sender, e);
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxFolder.Text == null)
                    return;

                if (textBoxFolder.Text == "")
                    return;

                if (backgroundWorker1.IsBusy == true)
                    return;

                ChangeFormStatus(false);


                // set arguments to worker
                // Não devem ser usados directamente os conteudos que estão nos componentes dentro do RunWorkerAsync
                // deve receber object class com toda a info necessaria como arguments.
                WorkerArguments arguments = new WorkerArguments(textBoxFolder.Text);

                backgroundWorker1.RunWorkerAsync(arguments);
            }
            catch (Exception ex)
            {
                //listBox1.Items.Add(ex.Message);
                ChangeFormStatus(true);
            }
        }

        #endregion

        #region BackgroundWorker events

        // e.Argument - deve receber object class com toda a info necessaria.
        // Não devem ser usados directamente os conteudos que estão nos componentes
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //get Argument
                WorkerArguments arguments = e.Argument as WorkerArguments;

                if (Directory.Exists(arguments.FolderRoot))
                    ListDirectory(arguments.FolderRoot, e);
                else
                    throw new Exception($"Directory not exits. [{arguments.FolderRoot}]");
            }
            catch (Exception ex)
            {
              //  listBox1.Items.Add(ex.Message); //review throw in thread
                ChangeFormStatus(true);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (_isMarquee)
            //{
            //    progressBar1.Style = ProgressBarStyle.Marquee;
            //    progressBar1.Value = 0;
            //    _isMarquee = false;
            //    return;
            //}

            //if (_isFirstItem)
            //{
            //    progressBar1.Style = ProgressBarStyle.Blocks;
            //    progressBar1.Maximum = _totalFolders;
            //    progressBar1.Value = 0;
            //    _isFirstItem = false;
            //    return;
            //}

            //// Não devem ser usados directamente os conteudos que estão nos componentes ou em fields da classe
            //// deve receber object class com toda a info necessaria no "e.UserState".

            //if (e.UserState != null)
            //{
            //    WorkerProcessState WorkerProcessState = e.UserState as WorkerProcessState;
            //   // listBox1.Items.Add(WorkerProcessState.Collection.ToString() + " : " + WorkerProcessState.Artist);
            //    _folderList.Add(WorkerProcessState.Folder);
            //}
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_treeNodeRoot != null)
            {
                treeView1.Nodes.Add(_treeNodeRoot);

                if (treeView1.Nodes.Count > 0)
                    treeView1.Nodes[0].Expand();
            }

            ChangeFormStatus(true);
        }

        #endregion

        #region BackgroundWorker Methods

        private void ListDirectory(string path, DoWorkEventArgs e)
        {
            treeView1.Nodes.Clear();

            var rootDirectoryInfo = new DirectoryInfo(path);

            _treeNodeRoot = CreateDirectoryNode(rootDirectoryInfo, e);

            RemoveDirectoryNode(_treeNodeRoot.Nodes, e);
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, DoWorkEventArgs e)
        {
            if (e.Cancel == true)
                return null;

            TreeNode directoryNode = new TreeNode(directoryInfo.Name);

            string ext = "";
            bool isSound = false;
            bool hasSounds = false;

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (backgroundWorker1.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                ext = file.Extension.ToUpper();
                isSound = _appConfigInfo.AudioFileExtensions.Contains(ext);
                if (!hasSounds)
                    if (isSound)
                        hasSounds = true;

                if (isSound)
                {
                    TreeNode treeNode = new TreeNode(file.Name);
                    treeNode.Tag = "F";
                    
                    directoryNode.Nodes.Add(treeNode);
                }

                //Thread.Sleep(100);
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory, e));
            }

            return directoryNode;
        }

        private void RemoveDirectoryNode(TreeNodeCollection nodeCollection, DoWorkEventArgs e)
        {
            foreach (TreeNode node in nodeCollection)
            {
                if (backgroundWorker1.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                if (node == null)
                    continue;

                if ((node.Nodes.Count == 0) && (node.Tag == null))
                    node.Remove();
                else
                    RemoveDirectoryNode(node.Nodes, e);
            }
        }

        #endregion

        #region Private Methods

        private void ChangeFormStatus(bool enabled)
        {
            if (enabled)
            {
                Cursor = Cursors.Default;
                buttonCancel.Cursor = Cursors.Default;
                buttonClose.Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                buttonCancel.Cursor = Cursors.AppStarting;
                buttonClose.Cursor = Cursors.AppStarting;
            }

            textBoxFolder.Enabled = enabled;
            buttonPlay.Enabled = false;
            buttonProcess.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            treeView1.Enabled = enabled && (treeView1.Nodes.Count > 0);
            progressBar1.Visible = !enabled;

            if (!enabled)
            {
                treeView1.Nodes.Clear();
                progressBar1.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
            }

            System.Windows.Forms.Application.DoEvents();
        }




        #endregion

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (buttonPlay.Enabled == false)
                buttonPlay.Enabled = true;
        }

        private string Xpto(TreeNode treeNode)
        {
            List<string> audioFileArray = new List<string>();
            int rootLen = treeView1.Nodes[0].Text.Length;

            GetFilesFromTree(treeNode, audioFileArray, rootLen);

            StringBuilder sb = new StringBuilder();
            foreach (string audioFile in audioFileArray)
            {
                sb.Append("\"" + this._appConfigInfo.FolderRoot + audioFile + "\" ");
            }

            return sb.ToString();
        }

        private void GetFilesFromTree(TreeNode treeNode, List<string> audioFileArray, int rootLen)
        {
            string ext;
            bool isSound;

            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    GetFilesFromTree(node, audioFileArray, rootLen);
                    continue;
                }

                ext = Path.GetExtension(node.Text).ToUpper();
                isSound = _appConfigInfo.AudioFileExtensions.Contains(ext);
                if (isSound)
                    audioFileArray.Add(node.FullPath.Substring(rootLen));

                //Thread.Sleep(100);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
          
        }
    }
}
