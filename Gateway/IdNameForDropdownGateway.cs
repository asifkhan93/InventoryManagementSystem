using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PointOfSalesApp.Gateway.DB_Helper;
using PointOfSalesApp.Models;
using System.Data.SqlClient;

namespace PointOfSalesApp.Gateway
{
    public class IdNameForDropdownGateway:DbConnection
    {
        public List<IdNameForDropdownModel> GetIdNameForDropDownBox(string query)
        {
            var lists = new List<IdNameForDropdownModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add( new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State==ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }
        public List<IdNameForDropdownModel> GetIdCasCadeDropDown(int query)
        {
            var lists = new List<IdNameForDropdownModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(@"SELECT ID,Cat_Id,NAME FROM tbl_BRAND WHERE Cat_Id="+ query +" ORDER BY ID", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new IdNameForDropdownModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        CatId = Convert.ToInt32(rdr["Cat_Id"]),
                        Name = rdr["Name"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }

        internal List<UserLogModel> GetLog(string userName)
        {
            var lists = new List<UserLogModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(@"SELECT * from tbl_User_LOG WHERE UserName='"+ userName  +"'", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new UserLogModel()
                    {
                        Description = rdr["Description"].ToString(),
                        Date=Convert.ToDateTime(rdr["DateTime"].ToString()),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }

        internal List<UserLogModel> GetUserName()
        {
           
            var lists = new List<UserLogModel>();
            try
            {
                Con.Open();
                var cmd = new SqlCommand(@"SELECT Distinct UserName FROM tbl_User_LOG", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new UserLogModel()
                    {
                        UserName = rdr["UserName"].ToString(),
                    });
                }
                rdr.Close();
                Con.Close();
                return lists;
            }
            catch (Exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                throw;
            }
        }

        internal int SaveSubItemCategory(string param)
        {
            throw new NotImplementedException();
        }

        public double GetBalanceQty(int itemId,int pno)
        {
            double qty = 0;
            string query = @"EXEC SP_GET_CURRENT_QTY_BY_ITEM_ID "+ itemId +","+ pno +"";
            var cmd = new SqlCommand(query, Con);
            Con.Open();
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                qty = Convert.ToDouble(rdr["BalQty"]);
            }
            rdr.Close();
            Con.Close();
            return qty;
        }
    }
}