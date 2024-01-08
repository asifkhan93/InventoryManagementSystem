using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using PointOfSalesApp.Gateway.DB_Helper;
using PointOfSalesApp.Models;

namespace PointOfSalesApp.Gateway
{
    public class InvItemRegistrationGateway:DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(InvItemRegistrationModel _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();


                const string query = @"INSERT INTO InvItemRegistration (ProductCategory, ProductCode, ProductName,Unit,ApproverPrice,RackName, Row, StoreName, InventoryType, ReminderStock,ReOrderOnly,AcCode,CostCenter,Note) OUTPUT INSERTED.ID VALUES (@ProductCategory, @ProductCode, @ProductName,@Unit,@ApproverPrice,@RackName, @Row, @StoreName,@InventoryType,@ReminderStock,@ReOrderOnly,@AcCode,@CostCenter,@Note)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@ProductCategory", _aModel.ProductCategory);
                cmd.Parameters.AddWithValue("@ProductCode", _aModel.ProductCode);
              //  cmd.Parameters.AddWithValue("@RegistrationGroup", _aModel.RegistrationGroup);
                cmd.Parameters.AddWithValue("@ProductName", _aModel.ProductName);
                cmd.Parameters.AddWithValue("@Unit", _aModel.Unit);
                cmd.Parameters.AddWithValue("@ApproverPrice", _aModel.ApproverPrice);
                cmd.Parameters.AddWithValue("@RackName", _aModel.RackName);
                cmd.Parameters.AddWithValue("@Row", _aModel.Row);
                cmd.Parameters.AddWithValue("@StoreName", _aModel.StoreName);
                cmd.Parameters.AddWithValue("@InventoryType", _aModel.InventoryType);
                cmd.Parameters.AddWithValue("@ReminderStock", _aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@ReOrderOnly", _aModel.ReOrderOnly);
                cmd.Parameters.AddWithValue("@AcCode", _aModel.AcCode);
                //cmd.Parameters.AddWithValue("@Description", _aModel.Description);
                cmd.Parameters.AddWithValue("@CostCenter", 'U');
                cmd.Parameters.AddWithValue("@Note", _aModel.Note);
                cmd.ExecuteScalar();

                _transaction.Commit();
                Con.Close();
                return SaveMessage;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _transaction.Rollback();
                    Con.Close();
                }
                return exception.Message;
            }

        }
        public List<InvItemRegistrationModel> GetProductList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvItemRegistrationModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {

                    cond += "AND (ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
                }
                if ((category != "0") && (category != ""))
                {

                    cond += "AND ProductCategory='" + category + "' ";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT * from InvItemRegistration WHERE 1=1 "+ cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvItemRegistrationModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductCategory = rdr["ProductCategory"].ToString(),
                        ProductCode = rdr["ProductCode"].ToString(),
                       // RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ApproverPrice = Convert.ToDecimal(rdr["ApproverPrice"].ToString()),
                        RackName = rdr["RackName"].ToString(),
                        Row = rdr["Row"].ToString(),
                        StoreName = rdr["StoreName"].ToString(),
                        InventoryType = rdr["InventoryType"].ToString(),
                        ReminderStock = rdr["ReminderStock"].ToString(),
                        ReOrderOnly = rdr["ReOrderOnly"].ToString(),
                        AcCode = rdr["AcCode"].ToString(),
                        //Description = rdr["Description"].ToString(),
                        CostCenter = rdr["CostCenter"].ToString(),
                        Note = rdr["Note"].ToString(),

                        
                    });
                }

                rdr.Close();
                Con.Close();
                return lists;

            }
            catch (Exception exception)
            {

                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<InvItemRegistrationModel>();
            }
        }

        public string Update(InvItemRegistrationModel _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();

                const string query = @"UPDATE InvItemRegistration SET ProductCategory=@ProductCategory, ProductCode=@ProductCode,ProductName=@ProductName,Unit=@Unit,ApproverPrice=@ApproverPrice,RackName=@RackName, Row=@Row, StoreName=@StoreName,InventoryType=@InventoryType,ReOrderOnly=@ReOrderOnly,AcCode=@AcCode,Description=@Description,Note=@Note WHERE Id=@Id";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductCategory", _aModel.ProductCategory);
                cmd.Parameters.AddWithValue("@ProductCode", _aModel.ProductCode);
            //    cmd.Parameters.AddWithValue("@RegistrationGroup", _aModel.RegistrationGroup);
                cmd.Parameters.AddWithValue("@ProductName", _aModel.ProductName);
                cmd.Parameters.AddWithValue("@Unit", _aModel.Unit);
                cmd.Parameters.AddWithValue("@ApproverPrice", _aModel.ApproverPrice);
                cmd.Parameters.AddWithValue("@RackName", _aModel.RackName);
                cmd.Parameters.AddWithValue("@Row", _aModel.Row);
                cmd.Parameters.AddWithValue("@StoreName", _aModel.StoreName);
                cmd.Parameters.AddWithValue("@InventoryType", _aModel.InventoryType);
                cmd.Parameters.AddWithValue("@ReminderStock", _aModel.ReminderStock);
                cmd.Parameters.AddWithValue("@ReOrderOnly", _aModel.ReOrderOnly);
                cmd.Parameters.AddWithValue("@AcCode", _aModel.AcCode);
                cmd.Parameters.AddWithValue("@Description", _aModel.Description);
                cmd.Parameters.AddWithValue("@Note", _aModel.Note);
                cmd.Parameters.AddWithValue("@Id", _aModel.Id);
                cmd.ExecuteNonQuery();
                _transaction.Commit();
                Con.Close();
                return UpdateMessage;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _transaction.Rollback();

                    Con.Close();
                }

                throw;
            }
        }
        public List<InvItemRegistrationModel> GetProductCategory(string searchString)
        {
            try
            {
                var lists = new List<InvItemRegistrationModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "ProductCategory Like '%'+'" + searchString + "'+'%'"; 
                }

                string query = @"SELECT distinct ProductCategory  FROM InvItemCategory where " + cond + "  ORDER BY ProductCategory";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvItemRegistrationModel()
                    {
                        ProductCategory = rdr["ProductCategory"].ToString(),
                        //UserName=rdr["UserName"].ToString(),
                    });
                }

                rdr.Close();
                Con.Close();
                return lists;

            }
            catch (Exception exception)
            {

                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<InvItemRegistrationModel>();
            }
        }
        public List<InvItemRegistrationModel> GetAcCode(string searchString)
        {
            try
            {
                var lists = new List<InvItemRegistrationModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "(Code+Description) like  '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT  Code,Description  FROM tbl_CHART_OF_ACCOUNTS where " + cond + "  and RIGHT(code,4)<>'0000' ORDER BY Code";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvItemRegistrationModel()
                    {
                        AcCode = rdr["Code"].ToString(),
                        Description = rdr["Description"].ToString(),
                    });
                }

                rdr.Close();
                Con.Close();
                return lists;

            }
            catch (Exception exception)
            {

                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                return new List<InvItemRegistrationModel>();
            }
        }
        public string SaveCategory(InvItemRegistrationModel _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string query = @"INSERT INTO InvItemCategory (ProductCategory)  VALUES (@ProductCategory)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductCategory", _aModel.ProductCategory);
                cmd.ExecuteScalar();
                _transaction.Commit();
                Con.Close();
                return SaveMessage;
            }
            catch (Exception exception)
            {
                if (Con.State == ConnectionState.Open)
                {
                    _transaction.Rollback();
                    Con.Close();
                }
                return exception.Message;
            }

        }

    }
}