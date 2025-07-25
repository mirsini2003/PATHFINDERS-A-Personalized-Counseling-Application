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

namespace ptyxiaki
{
    public partial class LogInForm : Form
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User user = new User();

            string status = user.login(textUsername.Text, textPassword.Text);
            string theme=user.Theme;
            string language = user.Test;
            string notes = user.Notes;
            List<string> favouriteSchoolId = user.FavoriteSchoolIds;
            // Έλεγχος αν η είσοδος ήταν επιτυχής
            if (status == "Successful log-in")
            {
                this.Hide();
                new HomeForm(textUsername.Text, textPassword.Text,user.Theme,user.Test, user.Notes,user.FavoriteSchoolIds).Show();
            }
            else
            {
                MessageBox.Show(status);
            }
            //κάνω clear τα πεδία
            textUsername.Text = "";
            textPassword.Text = "";
            textUsername.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //κάνω clear τα πεδία
            textUsername.Text = "";
            textPassword.Text = "";
            textUsername.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new RegisterForm().Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textPassword.PasswordChar = '\0';               
            }
            else
            {
                textPassword.PasswordChar = '*';
            }
        }

        private void LogInForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void label6_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

      
        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor= Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void checkBox1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void checkBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

       
    }
}
