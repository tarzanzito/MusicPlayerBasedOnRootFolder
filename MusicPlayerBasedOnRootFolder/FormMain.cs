using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MusicManager
{
    internal partial class FormMain : Form
    {
        #region Fields

        private readonly AppConfigInfo _appConfigInfo;
        private string _rootName;
        private string _rootPath;

        #endregion

        #region Constructors

        public FormMain(AppConfigInfo appConfigInfo)
        {
            _appConfigInfo = appConfigInfo;   

            InitializeComponent();
        }

        #endregion

        #region backgroundWorkerTree events

        // e.Argument - deve receber object class com toda a info necessaria.
        // Não devem ser usados directamente os conteudos que estão nos componentes UI
        private void backgroundWorkerTree_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //get Argument
                WorkerTreeArgument argument = e.Argument as WorkerTreeArgument;

                if (Directory.Exists(argument.FolderRoot))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(argument.FolderRoot);
                    string rootName = directoryInfo.Name;

                    TreeNode treeNode = ListDirectory(argument.FolderRoot, e);

                    WorkerTreeResult result = new WorkerTreeResult(treeNode, rootName, argument.FolderRoot);
                    e.Result = result;
                }
                else
                    throw new Exception($"Directory not exits. [{argument.FolderRoot}]");
            }
            catch (Exception ex)
            {
                backgroundWorkerLoadTree.CancelAsync();
                //e.Cancel = true;  // nao pode ser colocado a true porque result fica a null
                e.Result = ex;
            }
        }

        private void backgroundWorkerTree_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (e.UserState != null)
            //{
                //WorkerProcessState WorkerProcessState = e.UserState as WorkerProcessState;
                // listBox1.Items.Add(WorkerProcessState.Collection.ToString() + " : " + WorkerProcessState.Artist);
                //_folderList.Add(WorkerProcessState.Folder);
            //}
        }

        private void backgroundWorkerTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                    return;

                if (e.Result == null)
                    return;

                if (e.Result.GetType() == typeof(System.Exception))
                {
                    Exception ex = e.Result as Exception;
                    throw ex;
                }

                //bool isException = typeof(System.Exception).IsAssignableFrom(e.Result.GetType());

                if (e.Result.GetType().IsSubclassOf(typeof(System.Exception)))
                {
                    Exception ex = e.Result as Exception;
                    throw ex;
                }

                if (e.Result.GetType() != typeof(WorkerTreeResult))
                    throw new Exception("backgroundWorkerTree.Result is not an 'WorkerTreeResult'");

                WorkerTreeResult result = (WorkerTreeResult)e.Result;

                if (result.TreeNodeRoot != null)
                {
                    treeView1.Nodes.Add(result.TreeNodeRoot);

                    if (treeView1.Nodes.Count > 0)
                    {
                        treeView1.Nodes[0].Expand();
                        //_lastFullPath = result.RootFolder;
                        _rootPath = result.RootPath;
                        _rootName = result.RootName;
                    }
                }
            }
            catch (Exception ex)
            {
                ChangeFormStatus(true, true);
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ChangeFormStatus(true, false);
        }

        #endregion

        #region backgroundWorkerTree Methods

        private TreeNode ListDirectory(string path, DoWorkEventArgs e)
        {
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            TreeNode treeNodeRoot = CreateDirectoryNode(rootDirectoryInfo, e);

            RemoveDirectoryNode(treeNodeRoot.Nodes, e);

            return treeNodeRoot;
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
                if (backgroundWorkerLoadTree.CancellationPending)
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

                //Thread.Sleep(10);
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                if (e.Cancel)
                    break;

                TreeNode treeNode = CreateDirectoryNode(directory, e);
                directoryNode.Nodes.Add(treeNode);
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

                if (backgroundWorkerLoadTree.CancellationPending == true)
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

        #region BackgroundWorkerPlayList events

        // e.Argument - deve receber object class com toda a info necessaria.
        // Não devem ser usados directamente os conteudos que estão nos componentes
        private void backgroundWorkerPlayList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //get Argument
                WorkerPlayListArgument argument = e.Argument as WorkerPlayListArgument;
                
                string files = ProcessFilesFromTree(argument, e);

                WorkerPlayListResult result = new WorkerPlayListResult(files);
                e.Result = result;
            }
            catch (Exception ex) 
            {
                backgroundWorkerLoadTree.CancelAsync();
                // e.Cancel = true;  // nao pode ser colocado a true porque result fica a null
                e.Result = ex;
            }
        }

        private void backgroundWorkerPlayList_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorkerPlayList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled) 
                {
                    if (e.Result != null)
                    {
                        if (e.Result.GetType() == typeof(System.Exception))
                        {
                            Exception ex = e.Result as Exception;
                            throw ex;
                        }

                        //bool isEception = typeof(System.Exception).IsAssignableFrom(e.Result.GetType());
                        if (e.Result.GetType().IsSubclassOf(typeof(System.Exception)))
                        {
                            Exception ex = e.Result as Exception;
                            throw ex;
                        }

                        if (e.Result.GetType() == typeof(WorkerPlayListResult))
                        {
                            WorkerPlayListResult result = (WorkerPlayListResult)e.Result;
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
                ChangeFormStatus(true, false);
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ChangeFormStatus(true, false);
        }

        #endregion

        #region BackgroundWorkerPlayList Methods

        private string ProcessFilesFromTree(WorkerPlayListArgument arguments, DoWorkEventArgs e)
        {
            List<string> audioFileArray = new List<string>();

            if (arguments.PlayAll)
                GetFilesFromTree(arguments.TreeNodeRoot, audioFileArray, arguments.FullPathRoot, e);
            else
            {
                //testar se é um ficheiro audio
                audioFileArray.Add(arguments.FullPathRoot);
            }
            string files = PrepareToFoobar2000(audioFileArray, e);

            return files;
        }

        private string PrepareToFoobar2000(List<string> audioFileArray, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return null;

            StringBuilder sb = new StringBuilder();

            foreach(string item in audioFileArray)
            {
                sb.Append("\"");
                sb.Append(item);
                sb.Append("\" ");
            }
            return sb.ToString();
        }

        private void GetFilesFromTree(TreeNode treeNode, List<string> audioFileArray, string fullPathRoot, DoWorkEventArgs e)
        {
            if (e.Cancel)
                return;
            
            string extension;
            bool isSound;

            foreach (TreeNode node in treeNode.Nodes)
            {
                if (backgroundWorkerLoadTree.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                if (node.Nodes.Count > 0)
                {
                    string newPath = fullPathRoot + Path.DirectorySeparatorChar + node.Text;
                    GetFilesFromTree(node, audioFileArray, newPath, e);
                    continue;
                }

                //add file
                extension = Path.GetExtension(node.Text).ToUpper();
                isSound = _appConfigInfo.AudioFileExtensions.Contains(extension);

                if (isSound)
                {
                    string newPath = fullPathRoot + Path.DirectorySeparatorChar + node.Text;
                    audioFileArray.Add(newPath);
                }

                //Thread.Sleep(10);
            }
        }

        private void MusicPlay(bool playAll)
        {
                if (backgroundWorkerPlayList.IsBusy)
                    return;

                if (treeView1.SelectedNode == null)
                    return;

                TreeNode tempNode = treeView1.SelectedNode;

                string tempPath = tempNode.FullPath.Substring(_rootName.Length);
                string fullPathRoot = _rootPath + tempPath;


                ChangeFormStatus(false, false);

                // set arguments to worker
                // Não devem ser usados directamente os conteudos que estão nos componentes dentro do RunWorkerAsync
                // deve receber object class com toda a info necessaria como arguments.
                WorkerPlayListArgument argument = new WorkerPlayListArgument(tempNode, fullPathRoot, playAll);

                backgroundWorkerPlayList.RunWorkerAsync(argument);
        }

        #endregion

        #region Private Methods

        private void ChangeFormStatus(bool enabled, bool cleartreeView)
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

            textBoxFolder.ReadOnly = _appConfigInfo.FolderRoot != null;
            textBoxFolder.Enabled = true;

            buttonPlay.Enabled = enabled && (treeView1.Nodes.Count > 0); 
            buttonProcess.Enabled = enabled;
            buttonCancel.Enabled = !enabled;
            treeView1.Enabled = enabled && (treeView1.Nodes.Count > 0);
            progressBar1.Visible = !enabled;

            if (!enabled)
            {
                if (cleartreeView)
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
            this.backgroundWorkerLoadTree.CancelAsync();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerLoadTree.IsBusy)
            {
                if (backgroundWorkerLoadTree.WorkerSupportsCancellation)
                    backgroundWorkerLoadTree.CancelAsync();
            }

            if (backgroundWorkerPlayList.IsBusy)
            {
                if (backgroundWorkerPlayList.WorkerSupportsCancellation)
                    backgroundWorkerPlayList.CancelAsync();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                MusicPlay(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true, false);
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

             buttonProcess_Click(sender, e);
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            try
            {
                _rootName = null;
                _rootPath = null;

                if (textBoxFolder.Text == null)
                    return;

                textBoxFolder.Text = textBoxFolder.Text.Trim();

                if (textBoxFolder.Text == "")
                    return;

                if (backgroundWorkerLoadTree.IsBusy == true)
                    return;

                ChangeFormStatus(false, true);

                // set arguments to worker
                // Não devem ser usados directamente os conteudos que estão nos componentes dentro do RunWorkerAsync
                // deve receber object class com toda a info necessaria como arguments.
                WorkerTreeArgument argument = new WorkerTreeArgument(textBoxFolder.Text);

                backgroundWorkerLoadTree.RunWorkerAsync(argument);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true, true);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string fullPath = e.Node.FullPath.Trim().ToUpper();

                // Item Folder
                bool isValid = fullPath.EndsWith("@MP3") || fullPath.EndsWith("@FLAC"); //melhorar

                //ListViewItem focusedItem = listView1.FocusedItem;
                //if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                //{
                //    contextMenuStrip1.Show(Cursor.Position);
                //}
                if (isValid)
                {
                    contextMenuStrip2.Show(Cursor.Position);
                    return;
                }

                //File
                isValid = fullPath.EndsWith(".MP3") || fullPath.EndsWith(".FLAC") || fullPath.EndsWith(".APE"); //melhorar
                //    AudioFileExtensions

                if (isValid)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                    return;
                }
            }
        }

        private void playAlbumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MusicPlay(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true, false);
            }
        }

        private void playOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MusicPlay(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true, false);
            }
        }

        private void playAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MusicPlay(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "App ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ChangeFormStatus(true, false);
            }
        }

        #endregion
    }
}
