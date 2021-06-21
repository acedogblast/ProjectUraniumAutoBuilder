using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolderSelect;
using System.IO;

namespace ProjectUraniumAutoBuilder
{
    public partial class Form1 : Form
    {
        public String OriginalGameFolder, ProjectUraniumGodotFolder, CustomFFmpegExe, ExportPath;
        private bool running = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CustomFFmpegExe = Directory.GetCurrentDirectory() + @"\ffmpeg.exe";
            textBox3.Text = CustomFFmpegExe;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e) // Open Original Pokemon Uranium Folder
        {
            var fsd = new FolderSelectDialog();
            fsd.Title = "Open Original Pokemon Uranium Folder";
            fsd.InitialDirectory = @"c:\";
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                textBox1.Text = fsd.FileName;
                OriginalGameFolder = fsd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e) // Open Project Uranium Godot Folder
        {
            var fsd = new FolderSelectDialog();
            fsd.Title = "Open Project Uranium Godot Folder";
            fsd.InitialDirectory = @"c:\";
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                textBox2.Text = fsd.FileName;
                ProjectUraniumGodotFolder = fsd.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button3.Enabled = false;
            }
            else
            {
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Open Custom FFmpeg
        {
            // Check if using custom ffmpeg
            if (!checkBox1.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select FFmpeg Exe";
                ofd.Filter = "FFmpeg executable|*.exe";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = ofd.FileName;
                    CustomFFmpegExe = ofd.FileName;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // Open Export Folder
        {
            var fsd = new FolderSelectDialog();
            fsd.Title = "Open Export Folder";
            fsd.InitialDirectory = @"c:\";
            if (fsd.ShowDialog(IntPtr.Zero))
            {
                textBox4.Text = fsd.FileName;
                ExportPath = fsd.FileName;
            }
        }

        
        private async void button4_Click(object sender, EventArgs e) // Build Button
        {
            if (running) // Check if we are already running
            {
                return;
            }

            // Check if correctly filled out
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please set the folders.");
                return;
            }

            running = true;
            button4.Enabled = false;
            button4.Text = "Building";
            Progress<int> progress = new Progress<int>( value =>
                {
                    progressBar1.Value = value;
                }
                );

            switch (comboBox1.Text)
            {
                case "Only setup assets (For developers and testers)": // Ignore Export
                    Console.WriteLine("Running as setup");
                    Builder builder = new Builder();
                    await Task.Run( () => builder.setupAssets(progress, OriginalGameFolder, ProjectUraniumGodotFolder, CustomFFmpegExe));
                    MessageBox.Show("Finished setting up the project.");
                    break;
                case "Build for Windows":
                    break; // TO BE IMPLEMENTED
                default:
                    break;
            }
            button4.Enabled = true;
            button4.Text = "Build";
            running = false;

            
        }
    }
}
