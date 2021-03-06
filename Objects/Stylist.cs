using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HairSalon
{
    public class Stylist
    {
        private int _id;
        private string _name;
        private string _phone;
        private string _email;

        public Stylist(string name, string phone, string email, int id = 0)
        {
            _name = name;
            _phone = phone;
            _email = email;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }
        public string GetName()
        {
            return _name;
        }

        public string GetPhone()
        {
            return _phone;
        }

        public string GetEmail()
        {
            return _email;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM stylists;", conn);
            cmd.ExecuteNonQuery();
        }

        public static List<Stylist> GetAll()
        {
            List<Stylist> allStylists = new List<Stylist>{};

            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM stylists;", conn);
            rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int newStylistId = rdr.GetInt32(0);
                string newStylistName = rdr.GetString(1);
                string newStylistPhone = rdr.GetString(2);
                string newStylistEmail = rdr.GetString(3);

                Stylist newStylist = new Stylist(newStylistName, newStylistPhone, newStylistEmail, newStylistId);
                allStylists.Add(newStylist);
            }

            if(rdr != null) rdr.Close();
            if(conn != null) conn.Close();

            return allStylists;
        }

        public override bool Equals(System.Object otherStylist)
        {
            if (!(otherStylist is Stylist))
            {
                return false;
            }
            else
            {
                Stylist newStylist = (Stylist) otherStylist;
                bool idEquality = (_id == newStylist.GetId());
                bool nameEquality = (_name == newStylist.GetName());
                bool phoneEquality = (_phone == newStylist.GetPhone());
                bool emailEquality = (_email == newStylist.GetEmail());
                return (idEquality && nameEquality && phoneEquality && emailEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO stylists (name, phone, email) OUTPUT INSERTED.id VALUES(@StylistName, @StylistPhone, @StylistEmail);", conn);

            SqlParameter stylistNameParameter = new SqlParameter();
            stylistNameParameter.ParameterName = "@StylistName";
            stylistNameParameter.Value = this.GetName();
            cmd.Parameters.Add(stylistNameParameter);

            SqlParameter stylistPhoneParameter = new SqlParameter();
            stylistPhoneParameter.ParameterName = "@StylistPhone";
            stylistPhoneParameter.Value = this.GetPhone();
            cmd.Parameters.Add(stylistPhoneParameter);

            SqlParameter stylistEmailParameter = new SqlParameter();
            stylistEmailParameter.ParameterName = "@StylistEmail";
            stylistEmailParameter.Value = this.GetEmail();
            cmd.Parameters.Add(stylistEmailParameter);

            rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            if(rdr != null) rdr.Close();
            if(conn != null) conn.Close();
        }

        public static Stylist Find(int id)
        {
            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM stylists WHERE id = @StylistId;", conn);
            SqlParameter stylistIdParameter = new SqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = id.ToString();
            cmd.Parameters.Add(stylistIdParameter);

            rdr = cmd.ExecuteReader();

            int foundStylistId = 0;
            string foundStylistName = null;
            string foundStylistPhone = null;
            string foundStylistEmail = null;

            while(rdr.Read())
            {
                foundStylistId = rdr.GetInt32(0);
                foundStylistName = rdr.GetString(1);
                foundStylistPhone = rdr.GetString(2);
                foundStylistEmail = rdr.GetString(3);
            }
            Stylist newStylist = new Stylist(foundStylistName, foundStylistPhone, foundStylistEmail, foundStylistId);

            if(rdr != null) rdr.Close();
            if(conn != null) conn.Close();

            return newStylist;
        }

        public void Update(string newName, string newPhone, string newEmail)
        {
            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE stylists SET name = @NewName, phone = @NewPhone, email = @NewEmail OUTPUT INSERTED.name, INSERTED.phone, INSERTED.email WHERE id = @StylistId;", conn);

            SqlParameter newNameParameter = new SqlParameter();
            newNameParameter.ParameterName = "@NewName";
            newNameParameter.Value = newName;
            cmd.Parameters.Add(newNameParameter);

            SqlParameter newPhoneParameter = new SqlParameter();
            newPhoneParameter.ParameterName = "@NewPhone";
            newPhoneParameter.Value = newPhone;
            cmd.Parameters.Add(newPhoneParameter);

            SqlParameter newEmailParameter = new SqlParameter();
            newEmailParameter.ParameterName = "@NewEmail";
            newEmailParameter.Value = newEmail;
            cmd.Parameters.Add(newEmailParameter);

            SqlParameter stylistIdParameter = new SqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = this.GetId();
            cmd.Parameters.Add(stylistIdParameter);

            rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(0);
                this._phone = rdr.GetString(1);
                this._email = rdr.GetString(2);
            }

            if(rdr != null) rdr.Close();
            if(conn != null) conn.Close();
        }
        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM stylists WHERE id = @StylistId;", conn);

            SqlParameter stylistIdParameter = new SqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = this.GetId();
            cmd.Parameters.Add(stylistIdParameter);

            cmd.ExecuteNonQuery();

            if(conn != null) conn.Close();
        }

        public List<Client> GetClients()
        {
            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE stylist_id = @StylistId;", conn);
            SqlParameter stylistIdParameter = new SqlParameter();
            stylistIdParameter.ParameterName = "@StylistId";
            stylistIdParameter.Value = this.GetId();
            cmd.Parameters.Add(stylistIdParameter);

            rdr = cmd.ExecuteReader();
            List<Client> allClients = new List<Client>{};

            while(rdr.Read())
            {
                int clientId = rdr.GetInt32(0);
                string clientName = rdr.GetString(1);
                string clientPhone = rdr.GetString(2);
                string clientEmail = rdr.GetString(3);
                int stylistId = rdr.GetInt32(4);
                Client newClient = new Client(clientName, clientPhone, clientEmail, stylistId, clientId);
                allClients.Add(newClient);
            }

            if(rdr != null) rdr.Close();
            if(conn != null) conn.Close();

            return allClients;
        }
    }
}
