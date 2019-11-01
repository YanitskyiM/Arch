using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using WinForms = System.Windows.Forms;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System;

namespace YanitskyiGLTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FolderBrowDir_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select your path";
            if (fbd.ShowDialog() == WinForms.DialogResult.OK)
                DirectoryPath.Text = fbd.SelectedPath;

        }

        private void FielBrowDir_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "All files|*.*", ValidateNames = true, Multiselect = false })
            {

                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                    SavePath.Text = ofd.FileName;
            }

        }

        private void ZipFolder_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(DirectoryPath.Text))
            {
                WinForms.MessageBox.Show("Please select your folder.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DirectoryPath.Focus();
                return;
            }

            string path = DirectoryPath.Text;

            Thread thread = new Thread(t =>
            {

                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    zip.AddDirectory(path);
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                    zip.SaveProgress += Zip_SaveProgress;
                    zip.Save(string.Format($"{di.Parent.FullName}{di.Name}.zip"));

                }

            })
            { IsBackground = true };
            thread.Start();
        }

        private void Zip_SaveProgress(object sender, Ionic.Zip.SaveProgressEventArgs e)
        {
            if (e.EventType == Ionic.Zip.ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                WinForms.MessageBox.Show("Succsess", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }


        private void Zip_SaveFileProgress(object sender, Ionic.Zip.SaveProgressEventArgs e)
        {
            if (e.EventType == Ionic.Zip.ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                WinForms.MessageBox.Show("Succsess", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
        }

        private void FileZip_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(SavePath.Text))
            {
                WinForms.MessageBox.Show("Please select your file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SavePath.Focus();
                return;
            }

            string filename = SavePath.Text;

            Thread thread = new Thread(t =>
            {

                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    FileInfo fi = new FileInfo(filename);
                    zip.AddFile(filename);
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filename);
                    zip.SaveProgress += Zip_SaveFileProgress;
                    zip.Save(string.Format($"{di.Parent.FullName}/{di.Name}.zip"));

                }

            })
            { IsBackground = true };
            thread.Start();
        }

      
    }
}
