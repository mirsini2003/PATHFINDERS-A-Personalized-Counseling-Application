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
    public partial class TestForm : Form

    {
        private SQLiteConnection conn;
        private SQLiteCommand cmd;
        private SQLiteDataReader reader;
        private int currentQuestionId_1 = 1;
        private int currentQuestionId_2 = 1;
        public int s1=0;  
        public int s2=0;
        public int s3 = 0;
        public int s4 = 0;
        int quizSelected = 0;

        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();


        //αθτο το διψτιοναρυ πρέπει όταν πατάω το κουμπί για το τεστ κατευθύνσεων να ξανα μηδενίσω τις τιμές
        //γιατι κρατάει αυτές από το τεστ
        Dictionary<string, int> jobs = new Dictionary<string, int>  //dictionary της μορφής επάγγελμα και σκορ
        {
            { "Δάσκαλος/Καθηγητής", 0 },
            { "Γιατρός", 0 },
            { "Ψυχολόγος", 0 },
            { "Μηχανικός", 0 },
            { "Ερευνητής", 0 },
            { "Αρχιτέκτονας", 0 },
            { "Προγραμματιστής", 0 },
            { "Ηλεκτρολόγος Μηχανικός", 0 },
            { "Νοσηλευτής", 0 },
            { "Κοινωνικός Λειτουργός", 0 },
            { "Αναλυτής Δεδομένων", 0 },
            { "Συγγραφέας", 0 },
            { "Σκηνοθέτης",0 },
            { "Γραφίστας", 0 },
            { "Ηθοποιός", 0 },
            { "Μουσικός", 0 },
            { "Λογιστής", 0 },
            { "Οικονομολόγος", 0 },
            { "Δικηγόρος", 0 },
            { "Αστυνομικός", 0 },
            { "Φωτογράφος", 0 },
            { "Δημοσιογράφος", 0 },
            { "Διαφημιστής", 0 },
            { "Σύμβουλος Επιχειρήσεων", 0 },
            { "Επιχειρηματίας", 0 },
            { "Manager", 0 },
            { "Φυσικοθεραπευτής", 0 },
            { "Διαιτολόγος/Διατροφολόγος", 0 },
            { "Γεωπόνος", 0 },
            { "Αρχαιολόγος", 0 },
            { "Μάγειρας", 0 },
            { "Πωλητής", 0 },
            { "Γεωλόγος", 0 },
            { "Βιολόγος", 0 }
        };    
        

        public TestForm(string username,string password,string theme, string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
        }
        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show(theme);
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            MessageBox.Show(result, "Save Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.ExitThread();
        }

        private void AboutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AboutForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }
        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SettingsForm(username,password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomeForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
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

        private void TestForm_Load(object sender, EventArgs e)
        {
            button2.Text = "       " + username;
            lblQuestion.Hide();  //τα κουιζ είναι hide και όταν πατήσει ο χρήστης το αντίστοιχο κουμπί γίνονται show
            rbAns1.Hide();
            rbAns2.Hide();
            rbAns3.Hide();
            rbAns4.Hide();
            btnNext.Hide();
            btnExit.Hide();
            if (theme=="light")
            {
                this.BackColor = Color.White;
                lblQuestion.ForeColor = Color.Black;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                button3.ForeColor = Color.Black;
                button4.ForeColor = Color.Black;
                rbAns1.ForeColor = Color.Black;
                rbAns2.ForeColor = Color.Black;
                rbAns3.ForeColor = Color.Black;
                rbAns4.ForeColor = Color.Black;
                flowLayoutPanel1.ForeColor = Color.Black;
            }
            else if (theme == "dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                lblQuestion.ForeColor = Color.White;
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                button3.ForeColor = Color.White;
                button4.ForeColor = Color.White;
                rbAns1.ForeColor = Color.White;
                rbAns2.ForeColor = Color.White;
                rbAns3.ForeColor = Color.White;
                rbAns4.ForeColor = Color.White;
                flowLayoutPanel1.ForeColor = Color.White;
            }
        }        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CareerForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ProfileForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            s1 = 0; 
            s2 = 0; 
            s3 = 0;
            s4 = 0;
            button3.Hide();
            button4.Hide();
            lblQuestion.Show();
            rbAns1.Show();
            rbAns2.Show();
            rbAns3.Show();
            rbAns4.Show();
            btnNext.Show();
            btnExit.Show();
            quizSelected = 1;
            currentQuestionId_1 = 1;
            LoadQuestion1(currentQuestionId_1);
            

        }
        private void LoadQuestion1(int id)
        {       
            string connectionString = "Data Source= db\\ptyxiaki.db;Version=3;";
            conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM DirectionQuiz WHERE Id = @id";
            cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
               
                lblQuestion.Text = $"{reader["Id"]}. {reader["Question"]}";
                rbAns1.Text = reader["Ans1"].ToString();
                rbAns2.Text = reader["Ans2"].ToString();
                rbAns3.Text = reader["Ans3"].ToString();
                rbAns4.Text = reader["Ans4"].ToString();
                btnNext.Text = "ΕΠΟΜΕΝΗ ΕΡΩΤΗΣΗ";

                rbAns1.Checked = rbAns2.Checked = rbAns3.Checked = rbAns4.Checked = false;
                if (id==11)
                {
                    btnNext.Text = "Τέλος QUIZ";
                }
            }
            else
            {
                MessageBox.Show("Δεν βρέθηκαν δεδομένα για το id.");
                MessageBox.Show($"Τιμή του id: {id}");
            }
            if (id==12) // αφού ο χρήστης έχει απαντήσει όλες τις ερωτήσεις
            {
                test = "yes";
                MessageBox.Show($"s1={s1}, s2={s2}, s3={s3}, s4={s4}");

                // αποθήκευση των τιμών σε dictionary
                Dictionary<string, int> directions = new Dictionary<string, int>()
                {
                     { "ΘΕΩΡΗΤΙΚΗ", s1 },
                     { "ΘΕΤΙΚΗ", s2 },
                     { "ΟΙΚΟΝΟΜΙΚΑ", s3 },
                     { "ΥΓΕΙΑΣ", s4 }
                };

                // μέγιστο score
                int maxScore = directions.Values.Max();

                //βρίσκω όλες τις κατευθύνσεις με αυτό το μέγιστο
                var bestMatches = directions
                    .Where(pair => pair.Value == maxScore)
                    .Select(pair => pair.Key)
                    .ToList();

                // εμφάνιση αποτελέσματος
                if (bestMatches.Count == 1)
                {
                    MessageBox.Show($"Η κατεύθυνση που σου ταιριάζει καλύτερα είναι η {bestMatches[0]}", "Αποτέλεσμα");
                }
                else
                {
                    string joined = string.Join(" και η ", bestMatches[1]);
                    MessageBox.Show($"Οι κατευθύνσεις που σου ταιριάζουν εξίσου είναι η {joined}", "Αποτέλεσμα");
                }

                lblQuestion.Hide();
                rbAns1.Hide();
                rbAns2.Hide();
                rbAns3.Hide();
                rbAns4.Hide();
                btnNext.Hide();
                btnExit.Hide();
                button3.Show();  //κουμπί για κουιζ κατευθυνσεων
                button4.Show();
                currentQuestionId_1 = 1;

                new DirectionResult(username,password, theme, test, notes, favoriteSchoolIds,s1, s2,s3,s4).Show();
            }

            reader.Close();
            conn.Close();
            

        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!rbAns1.Checked && !rbAns2.Checked && !rbAns3.Checked && !rbAns4.Checked)
            {
                MessageBox.Show("Παρακαλώ επιλέξτε μια απάντηση!");
            }
            else
            { //για τις κατευθύνσεις
                if (quizSelected == 1)
                {
                    currentQuestionId_1++;
                    LoadQuestion1(currentQuestionId_1);
                }
                else if (quizSelected == 2)
                {//για επαγγέλματα
                    switch (currentQuestionId_2)
                    {
                        case 1:     // ΕΡΩΤΗΣΗ 1
                            if (rbAns1.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Γιατρός"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                
                            }
                            else if (rbAns2.Checked) 
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Συγγραφέας"] += 1;
                                jobs["Φωτογράφος"] += 1;
                                
                            }
                            else if (rbAns3.Checked) 
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Ερευνητής"] += 1;
                                jobs["Αρχιτέκτονας"] += 1;
                               
                            }
                            else if (rbAns4.Checked) 
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                            }
                            break;

                        case 2: // ΕΡΩΤΗΣΗ  2
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 2;
                                jobs["Νοσηλευτής"] += 2;
                                jobs["Φυσικοθεραπευτής"] += 2;
                                jobs["Κοινωνικός Λειτουργός"] += 2;
                                jobs["Ψυχολόγος"] += 2;
                                
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;                               
                            }
                            break;

                        case 3: // ΕΡΩΤΗΣΗ 3
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Δικηγόρος"] += 1;                             
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Ερευνητής"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Αστυνομικός"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            break;
                        case 4: // ΕΡΩΤΗΣΗ 4
                            if (rbAns1.Checked)
                            {
                                jobs["Λογιστής"] += 2;
                                jobs["Αναλυτής Δεδομένων"] += 2;
                                jobs["Οικονομολόγος"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Δικηγόρος"] += 2;
                                jobs["Δημοσιογράφος"] += 2;
                                jobs["Αρχαιολόγος"] += 2;
                                jobs["Ψυχολόγος"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] += 2;
                                jobs["Ερευνητής"] += 2;
                                jobs["Βιολόγος"] += 2;
                                jobs["Γεωπόνος"] += 2;
                                jobs["Γεωλόγος"] += 2;
                            }
                            break;
                        case 5: // ΕΡΩΤΗΣΗ 5
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 2;
                                jobs["Δημοσιογράφος"] += 2;
                                jobs["Αστυνομικός"] += 2;
                                jobs["Δικηγόρος"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Ερευνητής"] += 2;
                                jobs["Λογιστής"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Μηχανικός"] += 2;
                                jobs["Προγραμματιστής"] += 2;
                                jobs["Αναλυτής Δεδομένων"] += 2;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 2;
                            }
                            break;
                        case 6: // ΕΡΩΤΗΣΗ 6
                            if (rbAns1.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;                             
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Δικηγόρος"] += 1;
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                            }
                            break;
                        case 7: // ΕΡΩΤΗΣΗ 7
                            if (rbAns1.Checked)
                            {
                                jobs["Μηχανικός"] += 2;
                                jobs["Προγραμματιστής"] += 2;
                                jobs["Ερευνητής"] += 2;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 2;
                                jobs["Κοινωνικός Λειτουργός"] += 2;
                                jobs["Γιατρός"] += 2;
                                jobs["Ψυχολόγος"] += 2;
                                jobs["Νοσηλευτής"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Οικονομολόγος"] += 2;
                                jobs["Διαφημιστής"] += 2;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 2;
                                jobs["Επιχειρηματίας"] += 2;
                                jobs["Πωλητής"] += 2;
                            }
                            break;
                        case 8: // ΕΡΩΤΗΣΗ 8
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 2;
                                jobs["Νοσηλευτής"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Φυσικοθεραπευτής"] += 2;
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Ψυχολόγος"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] -= 2;
                                jobs["Νοσηλευτής"] -= 2;
                                jobs["Φυσικοθεραπευτής"] -= 2;
                                jobs["Διαιτολόγος/Διατροφολόγος"] -= 2;
                                jobs["Ψυχολόγος"] -= 2;
                            }
                            break;
                        case 9: // ΕΡΩΤΗΣΗ 9
                            if (rbAns1.Checked)
                            {
                                jobs["Δικηγόρος"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Ερευνητής"] += 1;                                
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Διαφημιστής"] += 1;
                            }
                            break;
                        case 10:    // ΕΡΩΤΗΣΗ 10
                            if (rbAns1.Checked)
                            {
                                jobs["Δικηγόρος"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Δημοσιογράφος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Ερευνητής"] += 1;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Πωλητής"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;                                
                            }
                            break;
                        case 11:    // ΕΡΩΤΗΣΗ 11
                            if (rbAns1.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Μηχανικός"] += 2;
                                jobs["Προγραμματιστής"] += 2;
                                jobs["Αναλυτής Δεδομένων"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 2;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 2;
                                jobs["Manager"] += 2;
                            }
                            break;
                        case 12:    // ΕΡΩΤΗΣΗ 12
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                                jobs["Νοσηλευτής"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Ερευνητής"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Διαφημιστής"] += 1;
                                jobs["Πωλητής"] += 1;
                                jobs["Manager"] += 1;
                            }
                            break;
                        case 13:    // ΕΡΩΤΗΣΗ 13 
                            if (rbAns1.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Δικηγόρος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Δημοσιογράφος"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {                                
                                jobs["Γραφίστας"] += 1;                               
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;                                
                            }
                            break;
                        case 14:    // ΕΡΩΤΗΣΗ 14
                            if (rbAns1.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Ερευνητής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Γιατρός"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                                jobs["Νοσηλευτής"] += 1;
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Manager"] += 1;
                            }
                            break;
                        case 15:    // ΕΡΩΤΗΣΗ 15
                            if (rbAns1.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Δικηγόρος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;                                
                            }
                            break;
                        case 16:    // ΕΡΩΤΗΣΗ 16
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Ερευνητής"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 1;                               
                            }
                            break;
                        case 17:    // ΕΡΩΤΗΣΗ 17
                            if (rbAns1.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Γιατρός"] += 1;
                                jobs["Δικηγόρος"] += 1;
                                jobs["Αστυνομικός"] += 1;                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Γραφίστας"] += 1;
                                jobs["Συγγραφέας"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γεωπόνος"] += 1;
                                jobs["Γεωλόγος"] += 1;
                                jobs["Αρχαιολόγος"] += 1;
                            }
                            break;
                        case 18:    // ΕΡΩΤΗΣΗ 18
                            if (rbAns1.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                                jobs["Μηχανικός"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Γραφίστας"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Νοσηλευτής"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                            }
                            break;
                        case 19:    // ΕΡΩΤΗΣΗ 19
                            if (rbAns1.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Αρχιτέκτονας"] += 1;
                                jobs["Γιατρός"] += 1;                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Δικηγόρος"] += 1;
                                jobs["Δημοσιογράφος"] += 1;
                            }
                            break;
                        case 20:    // ΕΡΩΤΗΣΗ 20
                            if (rbAns1.Checked)
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {

                                jobs["Μάγειρας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Αρχιτέκτονας"] += 1;         
                            }
                            break;
                        case 21:    // ΕΡΩΤΗΣΗ 21
                            if (rbAns1.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Επιχειρηματίας"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Σύμβουλος Επιχειρήσεων"] += 1;
                                jobs["Διαφημιστής"] += 1;
                                jobs["Πωλητής"] += 1;
                                jobs["Manager"] += 1; 
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Δικηγόρος"] += 1;                       
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Οικονομολόγος"] += 1;                      
                            }
                            break;
                        case 22:    // ΕΡΩΤΗΣΗ 22
                            if (rbAns1.Checked)
                            {
                                jobs["Αναλυτής Δεδομένων"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                                jobs["Διαφημιστής"] += 1;
                                jobs["Μάγειρας"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δικηγόρος"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Μηχανικός"] += 1;
                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Ψυχολόγος"] += 2;
                                jobs["Κοινωνικός Λειτουργός"] += 2;
                                
                            }
                            break;
                        case 23:    // ΕΡΩΤΗΣΗ 23
                            if (rbAns1.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Δικηγόρος"] += 1;
                                jobs["Ερευνητής"] += 1;
                                jobs["Νοσηλευτής"] += 1;
                                
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Γραφίστας"] += 1;
                                jobs["Αρχιτέκτονας"] += 1;
                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Επιχειρηματίας"] += 1;
                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;
                               
                            }
                            break;
                        case 24:    // ΕΡΩΤΗΣΗ 24
                            if (rbAns1.Checked)
                            {
                                jobs["Ερευνητής"] += 1;
                                jobs["Γιατρός"] += 1;
                                
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                                jobs["Αρχιτέκτονας"] += 1;
                               
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Επιχειρηματίας"] += 1;
                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Οικονομολόγος"] += 1;                               
                            }
                            break;
                        case 25:    // ΕΡΩΤΗΣΗ 25
                            if (rbAns1.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 2;
                                jobs["Manager"] += 2;
                                jobs["Σύμβουλος Επιχειρήσεων"] += 2;                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Αρχιτέκτονας"] += 2;
                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Προγραμματιστής"] += 2;
                                jobs["Λογιστής"] += 2;
                                jobs["Δικηγόρος"] += 2;
                                
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 2;
                                jobs["Φωτογράφος"] += 2;                               
                            }
                            break;
                        case 26:    // ΕΡΩΤΗΣΗ 26
                            if (rbAns1.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                                jobs["Μηχανικός"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Λογιστής"] += 1;
                                jobs["Ερευνητής"] += 1;
                               
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Διαφημιστής"] += 1;
                                jobs["Manager"] += 1;
                                jobs["Πωλητής"] += 1;                                
                            }
                            break;
                        case 27:    // ΕΡΩΤΗΣΗ 27
                            if (rbAns1.Checked)
                            {
                                jobs["Δικηγόρος"] += 1;
                                jobs["Οικονομολόγος"] += 1;                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;
                                jobs["Γραφίστας"] += 1;
                                jobs["Ηθοποιός"] += 1;
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Φυσικοθεραπευτής"] += 1;
                                jobs["Νοσηλευτής"] += 1;
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 1;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Επιχειρηματίας"] += 1;
                                jobs["Ερευνητής"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                               
                            }
                            break;
                        case 28:    // ΕΡΩΤΗΣΗ 28
                            if (rbAns1.Checked)
                            {
                                jobs["Προγραμματιστής"] += 2;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 2;
                                jobs["Μηχανικός"] += 2;
                                jobs["Αναλυτής Δεδομένων"] += 2;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 2;
                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Συγγραφέας"] += 2;
                                jobs["Γραφίστας"] += 2;
                                jobs["Ηθοποιός"] += 2;
                                jobs["Μουσικός"] += 2;
                                jobs["Σκηνοθέτης"] += 2;
                                jobs["Φωτογράφος"] += 2;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] += 2;
                                jobs["Νοσηλευτής"] += 2;
                                jobs["Ψυχολόγος"] += 2;
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 2;
                                
                            }
                            break;
                        case 29:    // ΕΡΩΤΗΣΗ 29
                            if (rbAns1.Checked)
                            {
                                jobs["Δημοσιογράφος"] += 1;
                                jobs["Manager"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Μηχανικός"] += 1;
                                jobs["Προγραμματιστής"] += 1;
                                jobs["Αναλυτής Δεδομένων"] += 1;                               
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Επιχειρηματίας"] += 1;
                                
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Αρχιτέκτονας"] += 1;
                                jobs["Οικονομολόγος"] += 1;
                              
                            }
                            break;
                        case 30:    // ΕΡΩΤΗΣΗ 30
                            if (rbAns1.Checked)
                            {
                                jobs["Συγγραφέας"] += 1;                           
                                jobs["Μουσικός"] += 1;
                                jobs["Σκηνοθέτης"] += 1;
                                jobs["Φωτογράφος"] += 1;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Γιατρός"] += 1;
                                jobs["Ψυχολόγος"] += 1;
                                jobs["Κοινωνικός Λειτουργός"] += 1;
                                jobs["Δάσκαλος/Καθηγητής"] += 1;
                                
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Ερευνητής"] += 1;
                                jobs["Ηλεκτρολόγος Μηχανικός"] += 1;
                                jobs["Μηχανικός"] += 1;
                              
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Σύμβουλος Επιχειρήσεων"] += 1;
                                jobs["Manager"] += 1;
                                
                            }
                            break;
                        case 31:    // ΕΡΩΤΗΣΗ 31
                            if (rbAns1.Checked)
                            {
                                jobs["Βιολόγος"] += 4;
                                jobs["Γεωλόγος"] += 4;
                               
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Γεωπόνος"] += 4;
                               
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Φωτογράφος"] += 2;
                                
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Βιολόγος"] -= 4;
                                jobs["Γεωλόγος"] -= 4;
                                jobs["Γεωπόνος"] -= 4;
                                jobs["Φωτογράφος"] -= 4;
                            }
                            break;
                        case 32:    // ΕΡΩΤΗΣΗ 32
                            if (rbAns1.Checked)
                            {

                                jobs["Αστυνομικός"] += 3;
                            }
                            else if (rbAns2.Checked)
                            {

                                jobs["Manager"] += 3;
                            }
                            else if (rbAns3.Checked)
                            {

                                jobs["Φυσικοθεραπευτής"] += 3;
                            }
                            else if (rbAns4.Checked)
                            {

                                jobs["Διαιτολόγος/Διατροφολόγος"] += 3;
                            }
                            break;
                        case 33:    // ΕΡΩΤΗΣΗ 33
                            if (rbAns1.Checked)
                            {

                                jobs["Γεωπόνος"] += 3;
                            }
                            else if (rbAns2.Checked)
                            {

                                jobs["Αρχαιολόγος"] += 3;
                            }
                            else if (rbAns3.Checked)
                            {

                                jobs["Μάγειρας"] += 3;
                            }
                            else if (rbAns4.Checked)
                            {

                                jobs["Πωλητής"] += 3;
                            }
                            break;
                        case 34:    // ΕΡΩΤΗΣΗ 34
                            if (rbAns1.Checked)
                            {

                                jobs["Γεωλόγος"] += 3;
                            }
                            else if (rbAns2.Checked)
                            {

                                jobs["Βιολόγος"] += 3;
                            }
                            else if (rbAns3.Checked)
                            {

                                jobs["Αστυνομικός"] += 3;
                            }
                            else if (rbAns4.Checked)
                            {

                                jobs["Manager"] += 3;
                            }
                            break;
                        case 35:    // ΕΡΩΤΗΣΗ 35
                            if (rbAns1.Checked)
                            {
                                jobs["Βιολόγος"] += 3;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Μάγειρας"] += 3;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Αρχαιολόγος"] += 3;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Γεωπόνος"] += 3;
                            }
                            break;
                        case 36:    // ΕΡΩΤΗΣΗ 36
                            if (rbAns1.Checked)
                            {
                                jobs["Διαιτολόγος/Διατροφολόγος"] += 3;
                            }
                            else if (rbAns2.Checked)
                            {
                                jobs["Φυσικοθεραπευτής"] += 3;
                            }
                            else if (rbAns3.Checked)
                            {
                                jobs["Manager"] += 3;
                            }
                            else if (rbAns4.Checked)
                            {
                                jobs["Πωλητής"] += 3;
                            }
                            break;
                    }
                 
                            currentQuestionId_2++;
                    LoadQuestion2(currentQuestionId_2);
                    
                }


            }

        }

        private void rbAns1_CheckedChanged(object sender, EventArgs e)
        {
            s1++; //για το πρώτο τεστ
        }

        private void rbAns2_CheckedChanged(object sender, EventArgs e)
        {
            s2++; //για το πρώτο τεστ
        }

        private void rbAns3_CheckedChanged(object sender, EventArgs e)
        {
            s3++;  //για το πρώτο τεστ
        }

        private void rbAns4_CheckedChanged(object sender, EventArgs e)
        {
            s4++;  //για το πρώτο τεστ
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            jobs["Δάσκαλος/Καθηγητής"] = 0;
            jobs["Γιατρός"] = 0;
            jobs["Ψυχολόγος"] = 0;
            jobs["Μηχανικός"] = 0;
            jobs["Ερευνητής"] = 0;
            jobs["Αρχιτέκτονας"] = 0;
            jobs["Προγραμματιστής"] = 0;
            jobs["Ηλεκτρολόγος Μηχανικός"] = 0;
            jobs["Νοσηλευτής"] = 0;
            jobs["Κοινωνικός Λειτουργός"] = 0;
            jobs["Αναλυτής Δεδομένων"] = 0;
            jobs["Συγγραφέας"] = 0;
            jobs["Σκηνοθέτης"] = 0;
            jobs["Γραφίστας"] = 0;
            jobs["Ηθοποιός"] = 0;
            jobs["Μουσικός"] = 0;
            jobs["Λογιστής"] = 0;
            jobs["Οικονομολόγος"] = 0;
            jobs["Δικηγόρος"] = 0;
            jobs["Αστυνομικός"] = 0;
            jobs["Φωτογράφος"] = 0;
            jobs["Δημοσιογράφος"] = 0;
            jobs["Διαφημιστής"] = 0;
            jobs["Σύμβουλος Επιχειρήσεων"] = 0;
            jobs["Επιχειρηματίας"] = 0;
            jobs["Manager"] = 0;
            jobs["Φυσικοθεραπευτής"] = 0;
            jobs["Διαιτολόγος/Διατροφολόγος"] = 0;
            jobs["Γεωπόνος"] = 0;
            jobs["Αρχαιολόγος"] = 0;
            jobs["Μάγειρας"] = 0;
            jobs["Πωλητής"] = 0;
            jobs["Γεωλόγος"] = 0;
            jobs["Βιολόγος"] = 0;


            button3.Hide();
            button4.Hide();
            lblQuestion.Show();
            rbAns1.Show();
            rbAns2.Show();
            rbAns3.Show();
            rbAns4.Show();
            btnNext.Show();
            btnExit.Show();
            quizSelected = 2;
            currentQuestionId_2 = 1;
            LoadQuestion2(currentQuestionId_2);
            
        }
        private void LoadQuestion2(int id)
        {
            string connectionString = "Data Source= db\\ptyxiaki.db;Version=3;";
            conn = new SQLiteConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM Questions WHERE Id = @id";
            cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblQuestion.Text = $"{reader["Id"]}. {reader["Question"]}";
                rbAns1.Text = reader["Answer1"].ToString();
                rbAns2.Text = reader["Answer2"].ToString();
                rbAns3.Text = reader["Answer3"].ToString();
                rbAns4.Text = reader["Answer4"].ToString();
                btnNext.Text = "ΕΠΟΜΕΝΗ ΕΡΩΤΗΣΗ";

                rbAns1.Checked = rbAns2.Checked = rbAns3.Checked = rbAns4.Checked = false;
                if (id == 36)
                {
                    btnNext.Text = "Τέλος QUIZ";
                }
            }
            if (id == 37) // αφού ο χρήστης έχει απαντήσει όλες τις ερωτήσεις
            {
                test = "yes";
                lblQuestion.Hide();
                rbAns1.Hide();
                rbAns2.Hide();
                rbAns3.Hide();
                rbAns4.Hide();
                btnNext.Hide();
                btnExit.Hide();
                button3.Show();  //κουμπί για κουιζ κατευθυνσεων
                button4.Show();
                currentQuestionId_2 = 1;

                new QuizResults(username,password,theme,test,jobs).Show();
            }
            reader.Close();
            conn.Close();
        }

        private void btnNext_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnNext_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor= Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            lblQuestion.Hide();
            rbAns1.Hide();
            rbAns2.Hide();
            rbAns3.Hide();
            rbAns4.Hide();
            btnNext.Hide();
            btnExit.Hide();
            button3.Show();
            button4.Show();
            currentQuestionId_1 = 1; //όταν ο χρήστης πάει να ξανακάνει τα τεστ, θα κααναξεκινήσει από την αρχή
            currentQuestionId_2= 1;
        }

        private void btnExit_MouseHover(object sender, EventArgs e)
        {
            this.Cursor=Cursors.Hand;
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor=Cursors.Default;
        }

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }
    }
}
