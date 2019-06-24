using Library_Management_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Library_Management_System.Repo
{
    public class UtilityFunctions
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["LMS"].ToString();
            con = new SqlConnection(constr);

        }

        public List<BookSearch> BookSearch(String text)
        {
            if (text.Equals(""))
            {
                return new List<BookSearch>();
            }
            connection();
            List<BookSearch> BookList = new List<BookSearch>();

            SqlCommand command = new SqlCommand("SearchBook", con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@text",text);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach(DataRow dr in dt.Rows)
            {
                BookList.Add(
                    new BookSearch
                    {
                        ISBN = Convert.ToString(dr["isbn"]),
                        Title = Convert.ToString(dr["title"]),
                        Authors = Convert.ToString(dr["authors"]),
                        availability = !Convert.ToBoolean(dr["Availablilty"])
                    });
            }

            return BookList;
        }

        public void Checkin(string loan_id)
        {
            connection();
            SqlCommand check = new SqlCommand("Update book_loans set date_in = getdate() where loan_id ='"+loan_id+"';", con);
            con.Open();
            check.ExecuteNonQuery();
            con.Close();

        }

        public bool CreateBorrower(Borrower borrower)
        {
            connection();
            SqlCommand check = new SqlCommand("Select count(*) from borrower where ssn ='" + borrower.SSN + "';", con);
            con.Open();
            SqlDataReader rdr = check.ExecuteReader();
            int num = 0;
            while (rdr.Read())
            {
                num = Convert.ToInt32(rdr[0]);
            }
            con.Close();

            if (num == 0)
            {
                connection();
                SqlCommand command = new SqlCommand("CreateBorrower", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ssn", borrower.SSN);
                command.Parameters.AddWithValue("@bname", borrower.Name);
                command.Parameters.AddWithValue("@address", borrower.Address);
                command.Parameters.AddWithValue("@phone", borrower.Phone);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return true;
            }
            else
            {
                return false;
            }

        }

        public bool LoanBook(string cardid,string isbn)
        {
            connection();
            SqlCommand check= new SqlCommand("Select count(*) from book_loans where card_id ='"+cardid+"' and date_in is null", con);
            con.Open();
            SqlDataReader rdr = check.ExecuteReader();
            int num = 0;
            while (rdr.Read())
            {
                num = Convert.ToInt32(rdr[0]);
            }
            con.Close();

            if (num < 3)
            {

                SqlCommand com = new SqlCommand("LoanBook", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@isbn", isbn);
                com.Parameters.AddWithValue("@cardid", cardid);

                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return true;

                }
                else
                {

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<BookLoan> GetBookLoans(String text)
        {
            if (text.Equals(""))
            {
                return new List<BookLoan>();
            }
            connection();
            List<BookLoan> BookList = new List<BookLoan>();

            SqlCommand command = new SqlCommand("GetBookLoans", con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@text", text);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                BookList.Add(
                    new BookLoan
                    {
                        Loan_ID = Convert.ToString(dr["loan_id"]),
                        ISBN = Convert.ToString(dr["isbn"]),
                        Borrower_Name = Convert.ToString(dr["bname"]),
                        Card_ID = Convert.ToString(dr["card_id"]),
                        Date_Out = Convert.ToString(dr["date_out"]),
                        Due_Date = Convert.ToString(dr["due_date"])
                    });
            }

            return BookList;
        }

        public void UpdateFines()
        {
            connection();
            List<Loan> loans = new List<Loan>();
            List<Fines> fines = new List<Fines>();
            SqlCommand command = new SqlCommand("Select * from book_loans", con);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                if ((dr["date_in"] is DBNull))
                {
                    dr["date_in"] = DateTime.MinValue;
                }

                    loans.Add(
                        new Loan
                        {
                            loan_id = Convert.ToString(dr["loan_id"]),
                            isbn = Convert.ToString(dr["isbn"]),
                            card_id = Convert.ToString(dr["card_id"]),
                            date_out = Convert.ToDateTime(dr["date_out"]),
                            due_date = Convert.ToDateTime(dr["due_date"]),
                            date_in = Convert.ToDateTime(dr["date_in"])

                        });
            }


            command = new SqlCommand("Select * from fines", con);
            da = new SqlDataAdapter(command);
            dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                fines.Add(
                        new Fines
                        {
                            loan_id = Convert.ToString(dr["loan_id"]),
                            fine_amt = Convert.ToDouble(dr["fine_amt"]),
                            paid = Convert.ToBoolean(dr["paid"])
                        });
            }

            foreach(Loan loan in loans)
            {
                double fine_amt = 0;
                bool flag = false;
                if(!loan.date_in.Equals(DateTime.MinValue) && loan.date_in > loan.due_date)
                {
                    fine_amt = (loan.date_in - loan.due_date).Days * 0.25;
                    flag = true;
                }
                else if(loan.date_in.Equals(DateTime.MinValue) &&  DateTime.Now > loan.due_date)
                {
                    fine_amt = (DateTime.Now - loan.due_date).Days * 0.25;
                    flag = true;
                }
                if (flag)
                {
                    
                Fines fine_row = fines.Find(x => x.loan_id == loan.loan_id);
                if (fine_row != null)
                {
                    if (!fine_row.paid)
                    {
                        fine_row.fine_amt = fine_amt;
                    }
                }
                else
                {
                    fines.Add(
                        new Fines
                        {
                            loan_id = loan.loan_id,
                            fine_amt = fine_amt,
                            paid = false
                        } 
                        );
                }
                }
            }

            SqlCommand update = new SqlCommand("delete from fines", con);
            con.Open();
            update.ExecuteNonQuery();
            con.Close();

            DataTable updateDt = new DataTable();
            updateDt = ConvertToDataTable(fines);

            con.Open();
            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(con))
            {
                bulkcopy.BulkCopyTimeout = 660;
                bulkcopy.DestinationTableName = "Fines";
                bulkcopy.WriteToServer(updateDt);
                bulkcopy.Close();
            }

            con.Close();
        }

        public List<FineDue> GetFineDue()
        {
            connection();
            List<FineDue> FineList = new List<FineDue>();

            SqlCommand command = new SqlCommand("GetFines", con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                FineList.Add(
                    new FineDue
                    {
                        card_id = Convert.ToString(dr["card_id"]),
                        bname = Convert.ToString(dr["bname"]),
                        fine_due = Convert.ToDouble(dr["fine_due"])
                    });
            }

            return FineList;
        }

        public void Pay(string loan_id)
        {
            connection();
            SqlCommand check = new SqlCommand("Update fines set paid = 1 where loan_id ='" + loan_id + "';", con);
            con.Open();
            check.ExecuteNonQuery();
            con.Close();
        }

        public List<Fines> GetAllFines()
        {
            connection();
            List<Fines> FineList = new List<Fines>();

            SqlCommand command = new SqlCommand("GetAllFines", con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();

            con.Open();
            da.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                FineList.Add(
                    new Fines
                    {
                        loan_id = Convert.ToString(dr["loan_id"]),
                        fine_amt = Convert.ToDouble(dr["fine_amt"]),
                        paid = Convert.ToBoolean(dr["paid"])
                    });
            }

            return FineList;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}