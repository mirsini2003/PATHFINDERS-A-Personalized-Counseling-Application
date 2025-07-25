using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ptyxiaki
{
    public partial class RegisterForm : Form
    {
        private string username;
        private string password;
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "" || txtConPassword.Text == "")
            {
                MessageBox.Show("There are empty fields!", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPassword.Text == txtConPassword.Text)
            {
                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // έλεγχος αν υπάρχει ήδη το username
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @name";
                        using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("@name", txtUsername.Text);
                            long userExists = (long)checkCmd.ExecuteScalar();

                            if (userExists > 0)
                            {
                                MessageBox.Show("This username is already taken. Please choose another one.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtUsername.Text = "";
                                txtPassword.Text = "";
                                txtConPassword.Text = "";
                                return;
                            }
                        }

                        // αν δεν υπάρχει, κάνουμε κανονικά την εισαγωγή
                        string insertQuery = "INSERT INTO Users (Username, Password, Theme, Test, Notes, FavoriteSchoolIds) VALUES (@name, @password, @theme, @test, @notes, @favoriteIds)" +
                            "";

                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@name", txtUsername.Text);
                            insertCmd.Parameters.AddWithValue("@password", txtPassword.Text);
                            insertCmd.Parameters.AddWithValue("@theme", "light");     // default theme
                            insertCmd.Parameters.AddWithValue("@test", "no");  // default test
                            insertCmd.Parameters.AddWithValue("@notes", "");  // empty notes
                            insertCmd.Parameters.AddWithValue("@favoriteIds", "");   // empty list

                            int rowsAffected = insertCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Your account has been successfully created!", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // καθαρισμός πεδίων και μετάβαση στο home
                                string username = txtUsername.Text;
                                string password = txtPassword.Text;
                                string theme = "light";
                                string test = "no";
                                string notes = "";
                                List<string> favouriteSchoolIds=new List<string>();
                                txtUsername.Text = "";
                                txtPassword.Text = "";
                                txtConPassword.Text = "";

                                this.Hide();
                                new HomeForm(username, password,theme, test, notes,favouriteSchoolIds).Show();
                            }
                            else
                            {
                                MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please re-enter.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtConPassword.Text = "";
                txtPassword.Focus();
            }
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtConPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtConPassword.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //κάνω clear τα πεδία
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConPassword.Text = "";
            txtUsername.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LogInForm().Show();
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
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

        private void button2_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void label6_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
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
            this.Cursor=Cursors.Default;
        }

        private void checkBoxShowPassword_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void checkBoxShowPassword_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

       
    }
}
