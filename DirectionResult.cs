using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ptyxiaki
{
    public partial class DirectionResult : Form
    {
        private string username, password;
        private int s1, s2, s3, s4;
        private string theme;
        private string test;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new CareerForm(username,password,theme, test, notes,favoriteSchoolIds).Show();
        }


        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();

        //constructor
        public DirectionResult(string username,string password, string theme, string test, string notes, 
            List<string> favoriteSchoolIds,int s1,int s2,int s3, int s4)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
            this.s1 = s1;
            this.s2 = s2;
            this.s3 = s3;
            this.s4 = s4;
        }

        private void DirectionResult_Load(object sender, EventArgs e)
        {
            if (theme == "light")
            {
                this.BackColor = Color.White;
                label1.ForeColor = Color.Black;
                button1.BackColor = Color.Silver;
                button1.ForeColor = Color.Black;
            }
            else if (theme == "dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                label1.ForeColor = Color.White;
                button1.BackColor = Color.DimGray;
                button1.ForeColor = Color.White;
            }
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();

            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            //αποθηκεύω τις τιμές σε dictionary
            Dictionary<string, int> directions = new Dictionary<string, int>()
                {
                     { "ΘΕΩΡΗΤΙΚΗ", s1 },
                     { "ΘΕΤΙΚΗ", s2 },
                     { "ΟΙΚΟΝΟΜΙΚΗ", s3 },
                     { "ΥΓΕΙΑΣ", s4 }
                };

            //μέγιστο score
            int maxScore = directions.Values.Max();

            //βρίσκω όλες τις κατευθύνσεις με αυτό το μέγιστο
            var bestMatches = directions
                .Where(pair => pair.Value == maxScore)
                .Select(pair => pair.Key)
                .ToList();

            //εμφάνιση αποτελέσματος
            if (bestMatches.Count == 1)
            {
                label1.Text= "Η κατεύθυνση που σου ταιριάζει καλύτερα είναι η " + bestMatches[0] ;
                if (bestMatches[0] == "ΘΕΩΡΗΤΙΚΗ")
                {
                    pictureBox1.Show();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Hide();

                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();


                }
                else if (bestMatches[0]== "ΘΕΤΙΚΗ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Show();
                    pictureBox3.Hide();
                    pictureBox4.Hide();

                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();

                }
                else if (bestMatches[0] == "ΟΙΚΟΝΟΜΙΚΗ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    pictureBox3.Show();
                    pictureBox4.Hide();

                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();

                }
                else if (bestMatches[0] == "ΥΓΕΙΑΣ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Show();

                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();

                }
            }
            else
            {
                string joined = string.Join(" και η ", bestMatches[1]);
                label1.Text="Οι κατευθύνσεις που σου ταιριάζουν καλύτερα είναι η " + joined +" και η "+ bestMatches[0];
                if (bestMatches[1] == "ΘΕΩΡΗΤΙΚΗ")
                {
                    pictureBox5.Show();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Hide();
                }
                else if (bestMatches[1] == "ΘΕΤΙΚΗ")
                {
                    pictureBox5.Hide();
                    pictureBox6.Show();
                    pictureBox7.Hide();
                    pictureBox8.Hide();
                }
                else if (bestMatches[1] == "ΟΙΚΟΝΟΜΙΚΗ")
                {
                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Show();
                    pictureBox8.Hide();
                }
                else if (bestMatches[1] == "ΥΓΕΙΑΣ")
                {
                    pictureBox5.Hide();
                    pictureBox6.Hide();
                    pictureBox7.Hide();
                    pictureBox8.Show();
                }
                if (bestMatches[0] == "ΘΕΩΡΗΤΙΚΗ")
                {
                    pictureBox1.Show();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Hide();

                }
                else if (bestMatches[0] == "ΘΕΤΙΚΗ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Show();
                    pictureBox3.Hide();
                    pictureBox4.Hide();


                }
                else if (bestMatches[0] == "ΟΙΚΟΝΟΜΙΚΗ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    pictureBox3.Show();
                    pictureBox4.Hide();


                }
                else if (bestMatches[0] == "ΥΓΕΙΑΣ")
                {
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    pictureBox3.Hide();
                    pictureBox4.Show();


                }
            }
        }
    }
}
