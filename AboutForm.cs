using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;
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
    public partial class AboutForm : Form
    {
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();
        public AboutForm(string username,string password,string theme,string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            InitializeMap();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
        }
        private void InitializeMap()
        {
            //επιλογή του Provider για τον χάρτη
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            //ορισμός συντεταγμένων του σημείου στον χάρτη 
            double lat = 37.9416;
            double lng = 23.6529;
            gMapControl1.Position = new PointLatLng(lat, lng);
            //zoom
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 18;
            gMapControl1.Zoom = 14;

            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.ShowCenter = false;

            //προσθήκη Marker
            GMapOverlay markers = new GMapOverlay("markers");
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(lat, lng),
                                      GMarkerGoogleType.red);
            markers.Markers.Add(marker);
            gMapControl1.Overlays.Add(markers);
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show(theme);
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            MessageBox.Show(result, "Save Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.ExitThread();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            button2.Text = "       " + username;
            dateTimePicker1.MinDate = DateTime.Today;
            dateTimePicker1.Hide();
            button4.Hide();
            if (theme=="light")
            {
                this.BackColor = Color.White;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                linkLabel1.LinkColor = Color.Blue;
                flowLayoutPanel1.ForeColor = Color.Black;
            }
            else if (theme == "dark")
            {
                this.BackColor = Color.FromArgb(64, 64, 64);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                linkLabel1.LinkColor = Color.LightBlue;
                flowLayoutPanel1.ForeColor = Color.White;
            }
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new SettingsForm(username,password,theme, test, notes, favoriteSchoolIds).Show();
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new HomeForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TestForm(username,password, theme, test, notes, favoriteSchoolIds).Show();
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
            button3.Hide();
            dateTimePicker1.Show();
            button4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Checked==true){
                MessageBox.Show("Η επιβεβαίωση του ραντεβού έγινε επιτυχώς!\n" +
                 "Ραντεβού: " + dateTimePicker1.Value.ToString("dd/MM/yyyy"));
                button3.Show();
                button4.Hide(); 
                dateTimePicker1.Hide();
            }
            else
            {
                MessageBox.Show("Παρακαλώ επιλέξτε ημερομηνία για το ραντεβού σας!");
            }      
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void ExploreBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ExploreForm(username, password, theme, test, notes, favoriteSchoolIds).Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string email = "unipi@gmail.com";
                Process.Start(new ProcessStartInfo($"mailto:{email}") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Δεν ήταν δυνατή η εκκίνηση του προγράμματος του Ηλεκτρονικού Ταχυδρομείου.\n" + ex.Message);
            }
        }
    }
}
