using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        #region BackgroundWorker1 events

        // e.Argument - deve receber object class com toda a info necessaria.
        // Não devem ser usados directamente os conteudos que estão nos componentes
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //get Argument
                WorkerArguments1 arguments = e.Argument as WorkerArguments1;

                if (Directory.Exists(arguments.FolderRoot))
                    ListDirectory(arguments.FolderRoot, e);
                else
                    throw new Exception($"Directory not exits. [{arguments.FolderRoot}]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (e.UserState != null)
            //{
            //    WorkerProcessState WorkerProcessState = e.UserState as WorkerProcessState;
            //   // listBox1.Items.Add(WorkerProcessState.Collection.ToString() + " : " + WorkerProcessState.Artist);
            //    _folderList.Add(WorkerProcessState.Folder);
            //}
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                _treeNodeRoot = null;

            if (_treeNodeRoot != null)
            {
                treeView1.Nodes.Add(_treeNodeRoot);

                if (treeView1.Nodes.Count > 0)
                    treeView1.Nodes[0].Expand();
            }

            ChangeFormStatus(true);
        }

        #endregion

        #region BackgroundWorker1 Methods

        private void ListDirectory(string path, DoWorkEventArgs e)
        {
            treeView1.Nodes.Clear();

            var rootDirectoryInfo = new DirectoryInfo(path);

            _treeNodeRoot = CreateDirectoryNode(rootDirectoryInfo, e);

            RemoveDirectoryNode(_treeNodeRoot.Nodes, e);
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return null;

            TreeNode directoryNode = new TreeNode(directoryInfo.Name);

            string ext = "";
            bool isSound = false;
            bool hasSounds = false;

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (backgroundWorker1.CancellationPending)
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

                Thread.Sleep(10);
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                if (e.Cancel)
                    break;

                directoryNode.Nodes.Add(CreateDirectoryNode(directory, e));
            }

            return directoryNode;
        }

        private void RemoveDirectoryNode(TreeNodeCollection nodeCollection, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;

            foreach (TreeNode node in nodeCollection)
            {
                 if (e.Cancel)
                    break;

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

        #region BackgroundWorker2 events

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //get Argument
                WorkerArguments2 arguments = e.Argument as WorkerArguments2;

                string files = ProcessFilesFromTree(arguments.TreeNodeRoot, arguments.NodeRootTextLenght, e);
                WorkerResult2 result = new WorkerResult2(files);
                e.Result = result;
            }
            catch (Exception ex) 
            {
              //  e.Cancel = true;
                e.Result = ex;
                
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    if (e.Result != null)
                    {
                        //aaaaaaaaaaaaa fazer em todos

                        //bool isEception = typeof(System.Exception).IsAssignableFrom(e.Result.GetType());
                        if (e.Result.GetType().IsSubclassOf(typeof(System.Exception)))
                        {
                            Exception ex = e.Result as Exception;
                            throw ex;
                        }
                        
                        

                        if (e.Result.GetType() == typeof(WorkerResult2))
                        {
                            WorkerResult2 result = (WorkerResult2)e.Result;
                            if (result.Files != null)
                            {
                                Process.Start(_appConfigInfo.MusicPlayerApplication, result.Files);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ChangeFormStatus(true);
        }

        #endregion

        #region BackgroundWorker2 Methods

        private string ProcessFilesFromTree(TreeNode treeNode, int NodeRootTextLenght, DoWorkEventArgs e)
        {
            List<string> audioFileArray = new List<string>();

            GetFilesFromTree(treeNode, audioFileArray, NodeRootTextLenght, e);

            if (e.Cancel)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (string audioFile in audioFileArray)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                   break;
                }

                sb.Append("\"" + this._appConfigInfo.FolderRoot + audioFile + "\" ");
                Thread.Sleep(100);
            }

            if (e.Cancel)
                return null;
            
            return sb.ToString();
        }

        private void GetFilesFromTree(TreeNode treeNode, List<string> audioFileArray, int NodeRootTextLenght, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;
            
            string ext;
            bool isSound;

            foreach (TreeNode node in treeNode.Nodes)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (node.Nodes.Count > 0)
                {
                    GetFilesFromTree(node, audioFileArray, NodeRootTextLenght, e);
                    continue;
                }

                ext = Path.GetExtension(node.Text).ToUpper();
                isSound = _appConfigInfo.AudioFileExtensions.Contains(ext);
                if (isSound)
                    audioFileArray.Add(node.FullPath.Substring(NodeRootTextLenght));

                Thread.Sleep(100);
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
            if (backgroundWorker1.IsBusy)
            {
                if (backgroundWorker1.WorkerSupportsCancellation)
                    backgroundWorker1.CancelAsync();
            }

            if (backgroundWorker2.IsBusy)
            {
                if (backgroundWorker2.WorkerSupportsCancellation)
                    backgroundWorker2.CancelAsync();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (backgroundWorker2.IsBusy)
                    return;

                if (treeView1.SelectedNode == null)
                    return;

                TreeNode tempNode = treeView1.SelectedNode;
                int nodeRootTextLenght = treeView1.Nodes[0].Text.Length;

                ChangeFormStatus(false);

                // set arguments to worker
                // Não devem ser usados directamente os conteudos que estão nos componentes dentro do RunWorkerAsync
                // deve receber object class com toda a info necessaria como arguments.
                WorkerArguments2 arguments = new WorkerArguments2(tempNode, nodeRootTextLenght);

                backgroundWorker2.RunWorkerAsync(arguments);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (buttonPlay.Enabled == false)
                buttonPlay.Enabled = true;
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
                WorkerArguments1 arguments = new WorkerArguments1(textBoxFolder.Text);

                backgroundWorker1.RunWorkerAsync(arguments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true);
            }
        }

        #endregion

    }
}
