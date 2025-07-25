using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using ClosedXML.Excel;

namespace ptyxiaki
{
    public partial class CareerForm : Form
    {
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();

        public CareerForm(string username,string password,string theme,string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
             this.test = test;
             this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
        }

        private void CareerForm_Load(object sender, EventArgs e)
        {
            button2.Text = "       "+username;
            //κάνω clear τα προηγούμενα δεδομένα
            panelContainer.Controls.Clear();
            panelContainer.AutoScroll = true;

            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

            // ορισμός αρχικής θέσης των panels
            int startX = 15;  // x απόσταση από την αριστερη πλευρά
            int startY = 0;  // y απόσταση από την κορυφή
            int currentY = startY;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                string query = "SELECT Id, Department, Foundation, Description FROM Schools";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                conn.Open();
                SQLiteDataReader reader = cmd.ExecuteReader();

                panelContainer.SuspendLayout();

                while (reader.Read())
                {
                    
                    string id = reader["Id"].ToString();
                    string department = reader["Department"].ToString();
                    string foundation = "Foundation: " + reader["Foundation"].ToString();
                    string description = "Description: " + reader["Description"].ToString();

                    // panel για καθε σχολή
                    Panel panel = new Panel
                    {
                        Width = 1300,
                        Height = 70,
                        BorderStyle = BorderStyle.FixedSingle,
                        Location = new Point(startX, currentY),
                        BackColor = Color.LightGray
                    };
                    
                    //δεξι κλικ==>προσθήκη της συγκεκριμένης σχολής στα αγαπημένα
                    panel.MouseDown += (s, args) =>
                    {
                        if (args.Button == MouseButtons.Right)
                        {
                            Panel clickedPanel = (Panel)s;
                            // menu με επιλογών
                            ContextMenu menu = new ContextMenu();
                           
                            menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                            {
                                if (!favoriteSchoolIds.Contains(id))
                                {
                                    favoriteSchoolIds.Add(id);
                                    MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                }
                                else
                                {
                                    //αν είναι ήδη στα αγαπημένα
                                    MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                }
                            });
                            menu.Show(clickedPanel, args.Location);
                        }
                    };
                    // φτιάχνω τα labels
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

                    // προσθέτω τα labes στα panels
                    panel.Controls.Add(lblDepartment);
                    panel.Controls.Add(lblFoundation);
                    panel.Controls.Add(lblDescription);

                    //προσθέτω το panel της κάθε σχολής στο κύριο panel
                    panelContainer.Controls.Add(panel);

                    // υπολογισμός θέσης για το επόμενο panel
                    currentY += panel.Height + 10;
                }
                reader.Close();
                panelContainer.ResumeLayout(); 
            }
        }       

        //μετάβαση σε άλλα Forms από το menu
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomeForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
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
            //άνοιγμα online help
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

        private void CareerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           // MessageBox.Show(theme);
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
          //  MessageBox.Show(result, "Save Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.ExitThread();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ProfileForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex == 0) // ΟΛΕΣ ΟΙ ΣΧΟΛΕΣ
            {
                // clear τα data
                panelContainer.Controls.Clear();
                panelContainer.AutoScroll = true;

                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

                // αρχική θέση
                int startX = 15;  // x αποσταση από τα αριστερά
                int startY = 0;  // y απόσταση από πάνω
                int currentY = startY;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    string query = "SELECT Id, Department, Foundation, Description FROM Schools";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    panelContainer.SuspendLayout(); 
                    while (reader.Read())
                    {
                        string id = reader["Id"].ToString();
                        string department = reader["Department"].ToString();
                        string foundation = "Foundation: " + reader["Foundation"].ToString();
                        string description = "Description: " + reader["Description"].ToString();

                        // panel για κάθε σχολή 
                        Panel panel = new Panel
                        {
                            Width = 1300,
                            Height = 70,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(startX, currentY),
                            BackColor = Color.LightGray
                        };

                        //προσθήκη στα αγαπημένα
                        panel.MouseDown += (s, args) =>
                        {
                            if (args.Button == MouseButtons.Right)
                            {
                                Panel clickedPanel = (Panel)s;

                                // menu επιλογών
                                ContextMenu menu = new ContextMenu();
                                menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                                {
                                    if (!favoriteSchoolIds.Contains(id))
                                    {
                                        favoriteSchoolIds.Add(id);
                                        MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                    }
                                    else
                                    {
                                        //αν είναι ήδη στα αγαπημένα
                                        MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                    }
                                });
                                menu.Show(clickedPanel, args.Location);
                            }
                        };

                        // δημιουργία labels
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

                        // βάζω τα labels στα panels
                        panel.Controls.Add(lblDepartment);
                        panel.Controls.Add(lblFoundation);
                        panel.Controls.Add(lblDescription);

                        // βάζω το panel της κάθε σχολής στο κύριο panel
                        panelContainer.Controls.Add(panel);

                        // θέση επόμενου panel
                        currentY += panel.Height + 10;
                    }

                    reader.Close();
                    panelContainer.ResumeLayout(); 
                }
            }
            else if (comboBox1.SelectedIndex == 1)  //ΘΕΩΡΗΤΙΚΗ
            {
                // clear τα data
                panelContainer.Controls.Clear();
                panelContainer.AutoScroll = true;

                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

                // αρχική θέση
                int startX = 15;  // x απόσταση από τα αριστερά
                int startY = 0;  // y απόσταση από πάνω
                int currentY = startY;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Theoritiki=1";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    panelContainer.SuspendLayout(); 

                    while (reader.Read())
                    {
                        string id = reader["Id"].ToString();
                        string department = reader["Department"].ToString();
                        string foundation = "Foundation: " + reader["Foundation"].ToString();
                        string description = "Description: " + reader["Description"].ToString();

                        // panel για κάθε σχολή
                        Panel panel = new Panel
                        {
                            Width = 1300,
                            Height = 70,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(startX, currentY),
                            BackColor = Color.LightGray
                        };

                        //προσθήκη στα αγαπημένα
                        panel.MouseDown += (s, args) =>
                        {
                            if (args.Button == MouseButtons.Right)
                            {
                                Panel clickedPanel = (Panel)s;
                                // menu επιλογών
                                ContextMenu menu = new ContextMenu();
                                menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                                {
                                    if (!favoriteSchoolIds.Contains(id))
                                    {
                                        favoriteSchoolIds.Add(id);
                                        MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                    }
                                    else
                                    {
                                       //αν είναι ήδη στα αγαπημένα
                                        MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                    }
                                });
                                menu.Show(clickedPanel, args.Location);
                            }
                        };
                        // labels
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

                        // βάζω labels στα panels
                        panel.Controls.Add(lblDepartment);
                        panel.Controls.Add(lblFoundation);
                        panel.Controls.Add(lblDescription);

                        // βάζω το κάθε panel στο κύριο panel
                        panelContainer.Controls.Add(panel);

                        // θέση επόμενου panel
                        currentY += panel.Height + 10;
                    }
                    reader.Close();
                    panelContainer.ResumeLayout(); 
                }
            }
            else if (comboBox1.SelectedIndex == 2)   //ΘΕΤΙΚΗ
            {
                // clear τα δεδομένα
                panelContainer.Controls.Clear();
                panelContainer.AutoScroll = true;

                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

                // αρχική θέση
                int startX = 15;  // x απόσταση από τα αριστερά
                int startY = 0;  // y απόσταση από πάνω
                int currentY = startY;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Thetiki=1";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    panelContainer.SuspendLayout(); 

                    while (reader.Read())
                    {
                        string id = reader["Id"].ToString();
                        string department = reader["Department"].ToString();
                        string foundation = "Foundation: " + reader["Foundation"].ToString();
                        string description = "Description: " + reader["Description"].ToString();

                        //panel για κάθε σχολή
                        Panel panel = new Panel
                        {
                            Width = 1300,
                            Height = 70,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(startX, currentY),
                            BackColor = Color.LightGray
                        };

                        //προσθήκη στα αγαπημένα
                        panel.MouseDown += (s, args) =>
                        {
                            if (args.Button == MouseButtons.Right)
                            {
                                Panel clickedPanel = (Panel)s;
                                //menu με επιλογές
                                ContextMenu menu = new ContextMenu();
                           
                                menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                                {
                                    if (!favoriteSchoolIds.Contains(id))
                                    {
                                        favoriteSchoolIds.Add(id);
                                        MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                    }
                                    else
                                    {
                                       //αν υπάρχει ήδη στα αγαπημένα
                                        MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                    }
                                });
                                menu.Show(clickedPanel, args.Location);
                            }
                        };

                        // labels
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

                        //βάζω τα labels στα panel
                        panel.Controls.Add(lblDepartment);
                        panel.Controls.Add(lblFoundation);
                        panel.Controls.Add(lblDescription);

                        // βάζω το κάθε panel στο κύριο panel
                        panelContainer.Controls.Add(panel);

                        // θέση καινούριου panel
                        currentY += panel.Height + 10;
                    }

                    reader.Close();
                    panelContainer.ResumeLayout(); 
                }
            }
            else if (comboBox1.SelectedIndex == 3)   //ΥΓΕΙΑΣ
            {
                // clear data
                panelContainer.Controls.Clear();
                panelContainer.AutoScroll = true;

                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

                //αρχική θέση
                int startX = 15;  // x απόσταση από αριστερά
                int startY = 0;  // y απόσταση από πάνω
                int currentY = startY;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Ygeias=1";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    panelContainer.SuspendLayout(); 

                    while (reader.Read())
                    {
                        string id = reader["Id"].ToString();
                        string department = reader["Department"].ToString();
                        string foundation = "Foundation: " + reader["Foundation"].ToString();
                        string description = "Description: " + reader["Description"].ToString();

                        // panel για σχολές
                        Panel panel = new Panel
                        {
                            Width = 1300,
                            Height = 70,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(startX, currentY),
                            BackColor = Color.LightGray
                        };


                        //μενού με επιλογές
                        panel.MouseDown += (s, args) =>
                        {
                            if (args.Button == MouseButtons.Right)
                            {
                                Panel clickedPanel = (Panel)s;
                                //menu με επιλογές
                                ContextMenu menu = new ContextMenu();
                                menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                                {
                                    if (!favoriteSchoolIds.Contains(id))
                                    {
                                        favoriteSchoolIds.Add(id);
                                        MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                    }
                                    else
                                    {
                                        //αν υπάρχει ήδη στα αγαπημένα
                                        MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                    }
                                });
                                menu.Show(clickedPanel, args.Location);
                            }
                        };
                        // labels
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

                        //labels στα panel
                        panel.Controls.Add(lblDepartment);
                        panel.Controls.Add(lblFoundation);
                        panel.Controls.Add(lblDescription);

                        // βάζω τα panel κάθε σχολής στο panelContainer
                        panelContainer.Controls.Add(panel);

                        // θέση για το καινούριο panel
                        currentY += panel.Height + 10;
                    }

                    reader.Close();
                    panelContainer.ResumeLayout(); 
                }
            }
            else if (comboBox1.SelectedIndex == 4)   //OIKONOMIKA
            {
                // clear
                panelContainer.Controls.Clear();
                panelContainer.AutoScroll = true;

                string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";

                // αρχική θέση
                int startX = 15;  // x απόσταση από τα αριστερά
                int startY = 0;  // y απόσταση από πάνω
                int currentY = startY;

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Oikonomika=1";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    conn.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    panelContainer.SuspendLayout(); 

                    while (reader.Read())
                    {
                        string id = reader["Id"].ToString();
                        string department = reader["Department"].ToString();
                        string foundation = "Foundation: " + reader["Foundation"].ToString();
                        string description = "Description: " + reader["Description"].ToString();

                        // panel για κάθε σχολή
                        Panel panel = new Panel
                        {
                            Width = 1300,
                            Height = 70,
                            BorderStyle = BorderStyle.FixedSingle,
                            Location = new Point(startX, currentY),
                            BackColor = Color.LightGray
                        };

                        //προσθήκη στα αγαπημένα
                        panel.MouseDown += (s, args) =>
                        {
                            if (args.Button == MouseButtons.Right)
                            {
                                Panel clickedPanel = (Panel)s;
                                // φτιάχνεις menu με επιλογές
                                ContextMenu menu = new ContextMenu();
                                
                                menu.MenuItems.Add("Προσθήκη στα Αγαπημένα", (sender3, e3) =>
                                {
                                    if (!favoriteSchoolIds.Contains(id))
                                    {
                                        favoriteSchoolIds.Add(id);
                                        MessageBox.Show($"Προστέθηκε στα Αγαπημένα!");
                                    }
                                    else
                                    {
                                        //αν υπάρχει ήδη στα αγαπημένα
                                        MessageBox.Show("Αυτή η σχολή είναι ήδη στα Αγαπημένα.");
                                    }
                                });
                                menu.Show(clickedPanel, args.Location);
                            }
                        };

                        // labels
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

                        // labels στα panel
                        panel.Controls.Add(lblDepartment);
                        panel.Controls.Add(lblFoundation);
                        panel.Controls.Add(lblDescription);

                        // προσθηκη panels στο κυριο panel
                        panelContainer.Controls.Add(panel);

                        // θέση για το επόμενο panel
                        currentY += panel.Height + 10;
                    }

                    reader.Close();
                    panelContainer.ResumeLayout(); 
                }
            }         
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void comboBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)  //όλες οι σχολές
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    // επικεφαλίδες
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";

                    // bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτους στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;

                    int row = 2; // τα δεδομένα ξεκινάνε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // save του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }
            else if (comboBox1.SelectedIndex == 1)  //θεωρητική
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    // επικεφαλίδες
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";

                    //bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτους στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;

                    int row = 2; // ξεκινάμε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Theoritiki=1";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // αποθήκευση του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }
            else if (comboBox1.SelectedIndex == 2) //θετική
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    // επικεφαλίδες
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";

                    //bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτους στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;

                    int row = 2; // τα δεδομένα ξεκινάνε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Thetiki=1";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // αποθήκευση του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }
            else if (comboBox1.SelectedIndex == 3) //υγείας
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    // επικεφαλίδες
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";

                    //bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτος στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;

                    int row = 2; // τα δεδομένα ξεκινάνε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Ygeias=1";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // αποθήκευση του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }
            else if (comboBox1.SelectedIndex == 4) //οικονομικά
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    //επικεφαλίδων
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";
                    // bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτους στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;


                    int row = 2; //τα δεδομένα ξεκινάνε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools WHERE Oikonomika=1";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // αποθήκευση του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }
            else  //περίπτωση που ο χρήστης δεν εχει επιλέξει κατι απτο comboBox
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Schools");

                    //επικεφαλίδες
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Department";
                    worksheet.Cell(1, 3).Value = "Foundation";
                    worksheet.Cell(1, 4).Value = "Description";

                    //bold τις επικεφαλίδες
                    for (int i = 1; i <= 4; i++)
                    {
                        worksheet.Cell(1, i).Style.Font.Bold = true;
                    }

                    //πλάτος στηλών
                    worksheet.Column(1).Width = 10;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    worksheet.Column(4).Width = 150;

                    int row = 2; // ξεκινάμε από την δεύτερη γραμμή

                    string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
                    using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                    {
                        string query = "SELECT Id, Department, Foundation, Description FROM Schools";
                        SQLiteCommand cmd = new SQLiteCommand(query, conn);
                        conn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            worksheet.Cell(row, 1).Value = reader["Id"].ToString();
                            worksheet.Cell(row, 2).Value = reader["Department"].ToString();
                            worksheet.Cell(row, 3).Value = reader["Foundation"].ToString();
                            worksheet.Cell(row, 4).Value = reader["Description"].ToString();
                            row++;
                        }
                        reader.Close();
                    }

                    // αποθήκευση του αρχείου Excel
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Οι σχολές αποθηκεύτηκαν σε Excel.");
                    }
                }
            }


        }

       
    }
}
