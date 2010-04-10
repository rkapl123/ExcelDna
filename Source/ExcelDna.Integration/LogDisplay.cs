using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ExcelDna.Integration;
using System.IO;

namespace ExcelDna.Logging
{

    public partial class LogDisplayForm : Form
    {

        public LogDisplayForm()
        {
            InitializeComponent();
            Text = DnaLibrary.CurrentLibraryName + " - Log Display";
        }

        public void SetText(string message)
        {
            string[] messageLines = message.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            listBoxErrors.Items.Clear();
            foreach (string line in messageLines)
            {
                listBoxErrors.Items.Add(line);
            }
        }

        public void AppendText(string message)
        {
            string[] messageLines = message.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in messageLines)
            {
                listBoxErrors.Items.Add(line);
            }
            // Select last item ... and clear.
            listBoxErrors.SelectedItems.Clear();
            listBoxErrors.SelectedItem = listBoxErrors.Items[listBoxErrors.Items.Count - 1];
            listBoxErrors.SelectedItems.Clear();
        }

		private void btnSaveErrors_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = "txt";
			sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
			sfd.Title = "Save Error List As";
			DialogResult result = sfd.ShowDialog();
			if (result == DialogResult.OK)
			{
				using (StreamWriter w =  new StreamWriter(sfd.FileName))
				{
					foreach (object item in listBoxErrors.Items)
					{
						w.WriteLine(item.ToString());
					}
				}
			}
		}
    }
    
    public class LogDisplay
    {
        static LogDisplayForm logDisplayForm;

        static public LogDisplayForm LogDisplayForm
        {
            get
            {
                if (logDisplayForm == null)
                {
                    logDisplayForm = new LogDisplayForm();
                    logDisplayForm.FormClosed += logDisplayForm_FormClosed;
                }
                return logDisplayForm;
            }
        }

        static void logDisplayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            logDisplayForm = null;
        }

        public static void SetText(string message)
        {
            try
            {
                LogDisplayForm.SetText(message);
                if (!LogDisplayForm.Visible)
                    LogDisplayForm.Show( NativeWindow.FromHandle(ExcelDnaUtil.WindowHandle) );
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public static void WriteLine(string message)
        {
            try
            {
                LogDisplayForm.AppendText(message);
                if (!LogDisplayForm.Visible)
                    LogDisplayForm.Show(NativeWindow.FromHandle(ExcelDnaUtil.WindowHandle));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }

}