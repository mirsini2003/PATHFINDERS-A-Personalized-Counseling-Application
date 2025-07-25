using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ptyxiaki
{
    class User
    {       
        public string Username { get; set; }
        public string Password { get; set; }
        public string Theme { get; set; }
        public string Test { get; set; }
        public string Notes { get; set; }
        public List<string> FavoriteSchoolIds { get; set; } = new List<string>();

        public string login(string temp_user ,string temp_password )
        {            
            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
            string status = "temp";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = $"SELECT Username, Password, Theme, Test, Notes,FavoriteSchoolIds FROM Users WHERE Username = @username;";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", temp_user);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                this.Username = reader["Username"].ToString();
                                this.Password = reader["Password"].ToString();
                                this.Theme = reader["Theme"]?.ToString();
                                this.Test = reader["Test"]?.ToString();
                                this.Notes = reader["Notes"]?.ToString();

                                
                                string idsString = reader["FavoriteSchoolIds"].ToString();                                
                                this.FavoriteSchoolIds = idsString
                                     .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .ToList();
                            }

                            if (this.Username == null)
                            {
                                status = "The username does not exist!";
                            }
                            else if (this.Username.Equals(temp_user) && !temp_password.Trim().Equals(this.Password.Trim()))
                            {
                                status = "The password is wrong!";
                            }
                            else if (this.Username.Equals(temp_user) && temp_password.Trim().Equals(this.Password.Trim()))
                            {
                                status = "Successful log-in";
                            }
                            else
                            {
                                status = "Error";
                            }
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    status = "Database Error: " + ex.Message;
                }
            }

            return status;
        }
        public string saveChanges(string currentUsername, string currentPassword, string currentTheme, string currentTest, string currentNotes, List<string> favoriteSchoolIds)
        {
            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
            string status = "Changes saved successfully.";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show($"Saving for save Changes {currentUsername}, Theme: {currentTheme}");

                    // Μετατροπή της λίστας σε string
                    string favoriteIdsString = string.Join(",", favoriteSchoolIds);

                    string query = @"UPDATE Users
                             SET Password = @password,
                                 Theme = @theme,
                                 Test = @test,
                                 Notes = @notes,
                                 FavoriteSchoolIds = @favoriteIds
                             WHERE Username = @username;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", currentUsername);
                        command.Parameters.AddWithValue("@password", currentPassword);
                        command.Parameters.AddWithValue("@theme", currentTheme);
                        command.Parameters.AddWithValue("@test", currentTest);
                        command.Parameters.AddWithValue("@notes", currentNotes);
                        command.Parameters.AddWithValue("@favoriteIds", favoriteIdsString);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            status = "No user found with that username.";
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    status = "Error while saving changes: " + ex.Message;
                }
            }

            return status;
        }


        public string UpdateUser(string currentUsername, string newUsername, string newPassword)
        {
            string connectionString = "Data Source=db\\ptyxiaki.db;Version=3;";
            string status = "Error updating user.";
            int userId = -1;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //βρίσκω το ID του χρήστη με βάση το τρέχον username
                    string getIdQuery = "SELECT Id FROM Users WHERE Username = @currentUsername;";
                    using (SQLiteCommand getIdCmd = new SQLiteCommand(getIdQuery, connection))
                    {
                        getIdCmd.Parameters.AddWithValue("@currentUsername", currentUsername);
                        object result = getIdCmd.ExecuteScalar();

                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                        }
                        else
                        {
                            return "User not found.";
                        }
                    }

                    //ελέγχω αν υπάρχει ήδη το νέο username
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @newUsername AND Id != @userId;";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@newUsername", newUsername);
                        checkCmd.Parameters.AddWithValue("@userId", userId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            return "This username is already taken.";
                        }
                    }

                    // ενημερώνω την εγγραφή του χρήστη στη βάση
                    string updateQuery = "UPDATE Users SET Username = @newUsername, Password = @newPassword WHERE Id = @userId;";
                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@newUsername", newUsername);
                        command.Parameters.AddWithValue("@newPassword", newPassword);
                        command.Parameters.AddWithValue("@userId", userId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            status = "User updated successfully.";
                        }
                        else
                        {
                            status = "User update failed.";
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    status = "Database Error: " + ex.Message;
                }
            }
            return status;
        }


    }
}
