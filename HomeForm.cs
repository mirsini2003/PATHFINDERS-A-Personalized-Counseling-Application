using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ptyxiaki
{
    public partial class HomeForm : Form
    {
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();
        public HomeForm(string username,string password,string theme,string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
            
        }

        private void HomeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            Application.ExitThread();
        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AboutForm(username,password,theme, test, notes, favoriteSchoolIds).Show();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SettingsForm(username,password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            string relativePath = "html_files\\online_help.html";
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Αποτυχής φόρτωση σελίδας: {ex.Message}");
            }           
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TestForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CareerForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            button2.Text = "       " + username;
          if (theme == "light")
          {
                pictureBox1.Image = Image.FromFile("pathFinders_light.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                this.BackColor = Color.White;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label12.ForeColor = Color.Black;
                label13.ForeColor = Color.Black;
                label14.ForeColor = Color.Black;
                flowLayoutPanel1.ForeColor = Color.Black;
            }
            else if (theme == "dark")
          {
                pictureBox1.Image = Image.FromFile("pathFinders_dark.png");
                pictureBox1.SizeMode=PictureBoxSizeMode.Zoom;
                pictureBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                this.BackColor = Color.FromArgb(64, 64, 64);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                label13.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                flowLayoutPanel1.ForeColor = Color.White;            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ProfileForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show(); 
        }
    }
}
