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
    public partial class ExploreForm : Form
    {
        private string username;
        private string password;
        private string theme;
        private string test;
        private string notes;
        private List<string> favoriteSchoolIds = new List<string>();
        public ExploreForm(string username,string password,string theme,string test, string notes, List<string> favoriteSchoolIds)
        {
            InitializeComponent();
            this.username = username;
            this.password = password;
            this.theme = theme;
            this.test = test;
            this.notes = notes;
            this.favoriteSchoolIds = favoriteSchoolIds;
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ProfileForm(username, password, theme, test, notes, favoriteSchoolIds).Show(); 
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

        private void ExploreForm_Load(object sender, EventArgs e)
        {
            button4.Text = "       " + username;
        }
               

        private void ExploreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            User user = new User();
            string result = user.saveChanges(username, password, theme, test, notes, favoriteSchoolIds);
            Application.ExitThread();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.sciencedirect.com/science/article/abs/pii/S0001879121000178?";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.tandfonline.com/doi/full/10.1080/14330237.2023.2240107?";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.kathimerini.gr/k/k-magazine/563128891/telika-leitoyrgei-o-epaggelmatikos-prosanatolismos-i-ochi/";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://gnosis.library.ucy.ac.cy/handle/7/66310?";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.didaktorika.gr/eadd/handle/10442/3140";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://youtu.be/fZi1f2OS7nU?si=7ixmhULjVmBXLc_8";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://youtu.be/QkzD5zQabfc?si=osqmEYUgbsb4IT-P";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://youtu.be/aIDeGNuokqA?si=IHWV1Wmq-YjM3sAn";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        
            string url = "https://youtu.be/LNryDrollbg?si=yPeHAxgkDXNFCQU2";
            Uri uriResult;

            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (validUrl)
            {
                Process.Start(new ProcessStartInfo(uriResult.ToString()) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("Η διεύθυνση URL δεν είναι έγκυρη.");
            }

        }

        private void linkLabel1_MouseHover(object sender, EventArgs e)
        {
            this.Cursor= Cursors.Hand;
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor=Cursors.Default;
        }

        private void linkLabel2_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel2_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel3_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel3_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel4_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel4_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel5_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel5_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel6_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel6_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel7_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel7_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel8_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel8_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void linkLabel9_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void linkLabel9_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
    }
}
