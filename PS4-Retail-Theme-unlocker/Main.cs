﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PS4_Retail_Theme_unlocker
{
    public partial class Main : Form
    {
        private List<string> filePaths = new List<string>();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            /// thse are what you see when you hover over the buttons
            toolTip1.SetToolTip(this.btadd, "This is to add them one by one");
            toolTip1.SetToolTip(this.btadddir, "This is for adding a dirtory");
            toolTip1.SetToolTip(this.btClear, "this will purge the list");
            toolTip1.SetToolTip(this.credit, "This will open the credit");
            toolTip1.SetToolTip(this.btstart, "this will start the ");
            Directory.CreateDirectory("pkg");
        }

        private void btadd_Click(object sender, EventArgs e)
        {
            /// this is going to add files via the add file button to the listbox
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.ShowDialog();
            filePaths.AddRange(OFD.FileNames);
            for (int i = 0; i < filePaths.Count; i++)
            {
                lboxFiles.Items.Add(OFD.SafeFileNames[i]);
            }
            /// this is going to tell the system that label is the count of the listbox and the " To be processed "
            /// IE, "5 To be processed "
            label1.Text = lboxFiles.Items.Count + " To be processed ";
        }

        private void btadddir_Click(object sender, EventArgs e)
        {
            /// this is going to add all files via the add dir to the listbox
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.Description = "Select The Folder With the Package Files";
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(FBD.SelectedPath, "*.pkg", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (!filePaths.Contains(file))
                    {
                        lboxFiles.Items.Add(Path.GetFileName(file));
                        filePaths.Add(file);
                        label1.Text = lboxFiles.Items.Count + " To be processed ";
                    }
                }
            }
            /// this is going to tell the system that label is the count of the listbox and the " To be processed "
            /// IE, "5 To be processed "
            label1.Text = lboxFiles.Items.Count + " To be processed ";
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            /// this is going to clear the list box
            lboxFiles.Items.Clear();
            /// this is going to refresh the listbox
            lboxFiles.Refresh();
            /// this is going to clear the list of file that where in the listbox
            filePaths.Clear();
            /// this is going to make the progress bar know how many files there are so it can move the bar acordingly
            progressBar1.Maximum = lboxFiles.Items.Count;
            /// this is going to tell the system that label is the count of the listbox and the " To be processed "
            /// IE, "5 To be processed "
            label1.Text = lboxFiles.Items.Count + " To be processed ";
        }

        private void lboxFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string text in array)
            {
                if ((Path.GetExtension(text) == ".pkg" || Path.GetExtension(text) == ".PKG") && !filePaths.Contains(text))
                {
                    lboxFiles.Items.Add(Path.GetFileName(text));
                    filePaths.Add(text);
                    label1.Text = lboxFiles.Items.Count + " To be processed ";
                }
                if (Directory.Exists(text))
                {
                    string[] files = Directory.GetFiles(text, "*.pkg", SearchOption.AllDirectories);
                    foreach (string text2 in files)
                    {
                        if (!filePaths.Contains(text2))
                        {
                            lboxFiles.Items.Add(Path.GetFileName(text2));
                            filePaths.Add(text2);
                            label1.Text = lboxFiles.Items.Count + " To be processed ";
                        }
                    }
                }
            }
        }

        private void lboxFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void credit_Click(object sender, EventArgs e)
        {
            /// this is going to open the credits window
            if (Application.OpenForms["Credits"] == null)
            {
                Credits form = new Credits();
                form.Show();
            }
        }

        private string getFileName(string path)
        {
            path = path.Replace("\\", ",");
            string[] pathSplit = path.Split(',');
            return pathSplit[pathSplit.Length - 1];
        }

        private void btstart_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("pkg");
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath + "\\pkg\\");
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            for (int i = 0; i < filePaths.Count; i++)
            {
                if (filePaths[i].Contains(".pkg"))
                {
                    string CUSA = Path.GetFileNameWithoutExtension(filePaths[i]).Substring(7, 9);
                    string EPID = Path.GetFileNameWithoutExtension(filePaths[i]);
                    Directory.CreateDirectory("fake_dlc_pkg");
                    Directory.CreateDirectory("fake_dlc_temp");
                    Directory.CreateDirectory("fake_dlc_temp\\sce_sys");
                    if (File.Exists("fake_dlc_temp\\param_template.sfx") == true)
                    {
                        File.Delete("fake_dlc_temp\\param_template.sfx");
                    }
                    File.Copy("Data\\param_template.sfx", "fake_dlc_temp\\param_template.sfx");
                    string text = File.ReadAllText("fake_dlc_temp\\param_template.sfx");
                    string exportinfo = File.ReadAllText("fake_dlc_temp\\param_template.sfx");
                    text = text.Replace("%1", EPID);
                    exportinfo = text.Replace("%2", CUSA);
                    File.WriteAllText("fake_dlc_temp\\param_template.sfx", text);
                    File.WriteAllText("fake_dlc_temp\\param_template.sfx", exportinfo);
                    Process process1 = new Process();
                    process1.StartInfo.FileName = "cmd.exe";
                    process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process1.StartInfo.Arguments = "/c data\\orbis-pub-cmd.exe sfo_create \"" + "fake_dlc_temp\\param_template.sfx" + "\" \"" + "fake_dlc_temp\\sce_sys\\param.sfo";
                    process1.Start();
                    process1.WaitForExit();
                    if (File.Exists("fake_dlc_temp\\fake_dlc_project.gp4") == true)
                    {
                        File.Delete("fake_dlc_temp\\fake_dlc_project.gp4");
                    }
                    File.Copy("Data\\fake_dlc_project.gp4", "fake_dlc_temp\\fake_dlc_project.gp4");
                    string time = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                    time = time.Replace("TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", time);
                    string id = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                    id = id.Replace("ID", EPID);
                    File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", id);
                    string dir = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                    dir = dir.Replace("DIR", AppDomain.CurrentDomain.BaseDirectory);
                    File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", dir);
                    Process process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    process2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    string s = AppDomain.CurrentDomain.BaseDirectory + "fake_dlc_pkg";
                    process2.StartInfo.Arguments = "/c data\\orbis-pub-cmd.exe img_create \"" + AppDomain.CurrentDomain.BaseDirectory + "fake_dlc_temp\\fake_dlc_project.gp4" + "\" \"" + AppDomain.CurrentDomain.BaseDirectory + "\\fake_dlc_pkg\\" + EPID + "-A0000-V0100.pkg";
                    process2.Start();
                    process2.WaitForExit();
                    File.Delete("fake_dlc_temp\\param_template.sfx");
                    File.Delete("fake_dlc_temp\\sce_sys\\param.sfo");
                    File.Delete("fake_dlc_temp\\fake_dlc_project.gp4");
                    Directory.Delete("fake_dlc_temp\\sce_sys");
                    Directory.Delete("fake_dlc_temp");
                    progressBar1.Maximum = lboxFiles.Items.Count;
                    /// this is going to make the progress bar know how many files there are so it can move the bar acordingly
                    System.GC.Collect();
                    /// This is going to make it so you can see the bar move
                    progressBar1.Value++;
                    /// this is going to make it move
                }

                /// this checks to see if they are all done converting
                if (progressBar1.Value == progressBar1.Maximum)
                {
                    /// this is going to show the messege box
                    MessageBox.Show("Your Playstation 4 Package File processed!", "Finished!");
                    filePaths.Clear();
                    lboxFiles.Items.Clear();
                    label1.Text = "Press convert to start";
                    progressBar1.Value = 0;
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Directory.CreateDirectory("pkg");
            Directory.Delete("pkg");
        }
    }
}
