using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using PointOfSalesApp.Gateway.DB_Helper;
using PointOfSalesApp.Models;
namespace PointOfSalesApp.Controllers
{
    class InvStockAdjustGateway : DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(List<InvStockAdjustModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string TrNo = GetMonthlyTrNo("ReqNo", "InvRequisitionMain", "T", _transaction);

                string query = @"INSERT INTO invStockUpdate (TrNo, TrDate, ProductCode, ProductName, Note, UserName )  VALUES (@TrNo, @TrDate, @ProductCode, @ProductName, @Note, @UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrNo", TrNo);
                cmd.Parameters.AddWithValue("@TrDate", _aModel[0].TrDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ProductCode", _aModel[0].ProductCode);
                cmd.Parameters.AddWithValue("@ProductName", _aModel[0].ProductName);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvStockLedger (TrNo, TrDate, ProductCode, ProductName, Unit, BatchNo, InQty, OutQty, UserName) VALUES (@TrNo, @TrDate, @ProductCode, @ProductName, @Unit, @BatchNo, @InQty, @OutQty, @UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", TrNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.TrDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@InQty", item.InQty);
                    cmd.Parameters.AddWithValue("@OutQty", item.OutQty);
                   // cmd.Parameters.AddWithValue("@CorrectedQty", item.CorrectedQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }
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
        public List<InvStockAdjustModel> GetProductList(string param, string searchString, string lastIssue)
        {
            try
            {
                var lists = new List<InvStockAdjustModel>();
                string cond = "";
                string last = "";

                if ((param != null) && (param != ""))
                {
                    last = "COALESCE(SUM(OutQty*" + param + "/" + lastIssue + "),0)";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "ProductCode Like '%'+'" + searchString + "'+'%'";
                }
                if ((lastIssue != null) && (lastIssue != ""))
                {

                    cond += "AND b.TrDate > DATEADD(DAY, -" + lastIssue + ", CAST( GETDATE() AS Date)) ";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
                string query = @"SELECT ProductCode, ProductName,BatchNo, Unit, COALESCE(SUM(InQty+RtnQty-OutQty),0) as CurrentQty from InvStockLedger where " + cond + " group by ProductCode, ProductName, Unit, BatchNo";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockAdjustModel()
                    {

                        //ProductCategory = rdr["ProductCategory"].ToString(),
                        ProductCode = rdr["ProductCode"].ToString(),
                        //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        //productcategory = rdr["productcategory"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        BatchNo = rdr["BatchNo"].ToString(),
                        CurrentQty = Convert.ToInt32(rdr["CurrentQty"]),



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
                return new List<InvStockAdjustModel>();
            }
        }
        public List<InvStockAdjustModel> GetProduct(string param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvStockAdjustModel>();
                string cond = "";
                string last = "";

                if ((param != null) && (param != ""))
                {
                    last = "COALESCE(SUM(OutQty*" + param + "/" + category + "),0)";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "(ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
                }
                if ((category != null) && (category != ""))
                {

                    cond += "AND b.TrDate > DATEADD(DAY, -" + category + ", CAST( GETDATE() AS Date)) ";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
                string query = @"SELECT Distinct ProductCode, ProductName from InvStockLedger where  " + cond + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvStockAdjustModel()
                    {

                        //ProductCategory = rdr["ProductCategory"].ToString(),
                        ProductCode = rdr["ProductCode"].ToString(),
                        //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),


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
                return new List<InvStockAdjustModel>();
            }
        }
    }
}
