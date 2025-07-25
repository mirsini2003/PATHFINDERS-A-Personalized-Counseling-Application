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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Media;
using System.Data.SQLite;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Presentation;
using static System.Windows.Forms.AxHost;
using Font = System.Drawing.Font;

namespace ptyxiaki
{
    public partial class ProfileForm : Form
    {
        private SoundPlayer soundPlayer;
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();

        public ProfileForm(string username,string password,string theme,string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
            soundPlayer = new SoundPlayer("notification_sound.wav");
            
        }
        private void ProfileForm_Load(object sender, EventArgs e)
        {
            button4.Text = "       " + username;
            label8.Text = username + "'s Notes:";
            richTextBox1.Text = notes;
            if(test=="no")
            {
                label1.Show();
            }
            else if(test=="yes")
            {
                label1.Hide();
            }
            if (theme=="light")
            {
                this.BackColor = Color.White;
                richTextBox1.BackColor = Color.Silver;
                richTextBox1.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                flowLayoutPanel1.ForeColor = Color.Black;
                label1.ForeColor= Color.Black;
            }
            else if (theme=="dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                richTextBox1.BackColor = Color.Gray;
                richTextBox1.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                flowLayoutPanel1.ForeColor = Color.White;
                label1.ForeColor= Color.White;
            }

            // clear τα data
            panelContainer.Controls.Clear();
            panelContainer.AutoScroll = true;

            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

            // αρχική θέση
            int startX = 15;  // x αποσταση από τα αριστερά
            int startY = 0;  // y απόσταση από πάνω
            int currentY = startY;


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (string id in favoriteSchoolIds)
                {
                    string query = "SELECT * FROM Schools WHERE Id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string id_rec = reader["Id"].ToString();
                                string department = reader["Department"].ToString();
                                string foundation = "Foundation: " + reader["Foundation"].ToString();
                                string description = "Description: " + reader["Description"].ToString();

                                // panel για κάθε σχολή 
                                Panel panel = new Panel
                                {
                                    Width = 1250,
                                    Height = 70,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Location = new Point(startX, currentY),
                                    BackColor = Color.LightGray
                                };
                                

                                // δημιουργία labels
                                Label lblDepartment = new Label
                                {
                                    Text =  department,
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    AutoSize = true,
                                    Location = new Point(10, 8)
                                };

                                Label lblFoundation = new Label
                                {
                                    Text = foundation,
                                    AutoSize = true,
                                    Location = new Point(10, 35)
                                };

                                Label lblDescription = new Label
                                {
                                    Text = description,
                                    AutoSize = true,
                                    Location = new Point(10, 50)
                                };

                                // βάζω τα labels στα panels
                                panel.Controls.Add(lblDepartment);
                                panel.Controls.Add(lblFoundation);
                                panel.Controls.Add(lblDescription);

                                // βάζω το panel της κάθε σχολής στο κύριο panel
                                panelContainer.Controls.Add(panel);
                                currentY += panel.Height + 10;

                                // δημιουργώ context menu
                                ContextMenuStrip contextMenu = new ContextMenuStrip();
                                ToolStripMenuItem removeItem = new ToolStripMenuItem("Αφαίρεση από τα αγαπημένα");

                                // event handler
                                removeItem.Click += (sender4, e4) =>
                                {
                                    favoriteSchoolIds.Remove(id);
                                    LoadPanels();

                                };

                                // σύνδεση του μενού με το panel
                                contextMenu.Items.Add(removeItem);
                                panel.ContextMenuStrip = contextMenu;
                            }
                        }
                    }
                }
            }


        }
       
       

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomeForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void CareersBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CareerForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TestForm(username, password, theme, test, notes, favoriteSchoolIds).Show();   
        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AboutForm(username, password, theme, test, notes, favoriteSchoolIds).Show();   
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SettingsForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
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

        private void ProfileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notes=richTextBox1.Text;
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            Application.ExitThread();
        }

        

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
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

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void clearTextBox_btn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

       

        private void saveTextBox_btn_Click_1(object sender, EventArgs e)
        {
            notes = richTextBox1.Text;
            MessageBox.Show("Notes Saved Succesfully!");
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
           
        }
        private void LoadPanels()
        {
            panelContainer.Controls.Clear();
            panelContainer.AutoScroll = true;

            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
            int startX = 15;
            int currentY = 0;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (string id in favoriteSchoolIds.ToList()) 
                {
                    string query = "SELECT * FROM Schools WHERE Id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string department = reader["Department"].ToString();
                                string foundation = "Foundation: " + reader["Foundation"].ToString();
                                string description = "Description: " + reader["Description"].ToString();

                                Panel panel = new Panel
                                {
                                    Width = 1250,
                                    Height = 70,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Location = new Point(startX, currentY),
                                    BackColor = Color.LightGray
                                };

                                Label lblDepartment = new Label
                                {
                                    Text = department,
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    AutoSize = true,
                                    Location = new Point(10, 8)
                                };

                                Label lblFoundation = new Label
                                {
                                    Text = foundation,
                                    AutoSize = true,
                                    Location = new Point(10, 35)
                                };

                                Label lblDescription = new Label
                                {
                                    Text = description,
                                    AutoSize = true,
                                    Location = new Point(10, 50)
                                };

                                panel.Controls.Add(lblDepartment);
                                panel.Controls.Add(lblFoundation);
                                panel.Controls.Add(lblDescription);

                                // context menu
                                ContextMenuStrip contextMenu = new ContextMenuStrip();
                                ToolStripMenuItem removeItem = new ToolStripMenuItem("Αφαίρεση από τα αγαπημένα");

                                // event handler για αφαίρεση
                                removeItem.Click += (s, e) =>
                                {
                                    favoriteSchoolIds.Remove(id); // αφαίρεση από τη λίστα
                                    LoadPanels();  // επαναφόρτωση όλων των panels
                                };

                                contextMenu.Items.Add(removeItem);
                                panel.ContextMenuStrip = contextMenu;

                                panelContainer.Controls.Add(panel);
                                currentY += panel.Height + 10;
                            }
                        }
                    }
                }
            }
        }

    }
}
