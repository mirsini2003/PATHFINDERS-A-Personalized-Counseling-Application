using AudioSwitcher.AudioApi.CoreAudio;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;
using GMap.NET;
using System.Media;

namespace ptyxiaki
{
    public partial class SettingsForm : Form
    {
        private SoundPlayer soundPlayer;
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private CoreAudioDevice defaultPlaybackDevice;
        private List<string> favoriteSchoolIds = new List<string>();
        public SettingsForm(string username, string password, string theme, string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
            soundPlayer = new SoundPlayer("notification_sound.wav");

            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            trackBar1.Minimum = 0;   // ελάχιστη τιμή
            trackBar1.Maximum = 100; // μέγιστη τιμή
            trackBar1.TickFrequency = 10; // βήματα εμφάνισης

            // θέτω την αρχική τιμή ίση με την τρέχουσα ένταση του υπολογιστή
            trackBar1.Value = (int)defaultPlaybackDevice.Volume;
            volumelbl.Text = $"{trackBar1.Value}%";
        }



        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            Application.ExitThread();
        }


        private void lightTheme_btn_Click(object sender, EventArgs e)
        {
            theme = "light";
            this.BackColor = Color.White;
            label1.ForeColor = Color.Black;
            label2.ForeColor = Color.Black;
            volumelbl.ForeColor = Color.Black;
            flowLayoutPanel1.ForeColor = Color.Black;
            groupBox1.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            label6.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
            groupBox2.ForeColor = Color.Black;
            label9.ForeColor = Color.Black;
            label10.ForeColor = Color.Black;
        }

        private void darkTheme_btn_Click(object sender, EventArgs e)
        {
            theme = "dark";
            this.BackColor = Color.FromArgb(64, 64, 64);
            label1.ForeColor = Color.White;
            label2.ForeColor = Color.White;
            volumelbl.ForeColor = Color.White;
            flowLayoutPanel1.ForeColor = Color.White;
            groupBox1.ForeColor = Color.White;
            label7.ForeColor = Color.White;
            label4.ForeColor = Color.White;
            label6.ForeColor = Color.White;
            label8.ForeColor = Color.White;
            groupBox2.ForeColor = Color.White;
            label9.ForeColor = Color.White;
            label10.ForeColor = Color.White;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            groupBox2.Hide();
            label8.Text = username;
            label6.Text = password;
            button4.Text = "       " + username;
            if (theme == "light")
            {
                this.BackColor = Color.White;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                volumelbl.ForeColor = Color.Black;
                flowLayoutPanel1.ForeColor = Color.Black;
                groupBox1.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;

            }
            else if (theme == "dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                volumelbl.ForeColor = Color.White;
                flowLayoutPanel1.ForeColor = Color.White;
                groupBox1.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
            }
        }


        private void volume_upPictureBox_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void volume_upPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void volume_downPictureBox_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void volume_downPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void CareersBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CareerForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void AboutBtn_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new AboutForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HomeBtn_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new HomeForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void TestBtn_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new TestForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HelpBtn_Click_1(object sender, EventArgs e)
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new ProfileForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

      

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            defaultPlaybackDevice.Volume = trackBar1.Value;
            volumelbl.Text = $"{trackBar1.Value}%";
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string currentUsername = username; // το παλιό username
            string newUsername = textBox1.Text.Trim();// νέο username
            string newPassword = textBox2.Text.Trim(); // νέο password  

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            soundPlayer.Play();

            User user = new User();
            string result = user.UpdateUser(currentUsername, newUsername, newPassword);
            password = newPassword;
            label8.Text = newUsername;  //username
            label6.Text = newPassword; //password

            textBox1.Clear();
            textBox2.Clear();
            groupBox2.Hide();

            MessageBox.Show(result, "Update Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox2.Hide();
        }
    }
}
