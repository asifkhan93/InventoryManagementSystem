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
    public class InvPurchaseRequisitionGateway : DbConnection
    {
        private SqlTransaction _transaction;

        public string Save(List<InvPurchaseRequisitionModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string ReqNo = GetMonthlyTrNo("ReqNo", "InvPurchaseRequisitionMain", "R", _transaction);

                string query = @"INSERT INTO InvPurchaseRequisitionMain (ReqNo, ReqDate, ReqFrom,Note,TotalQty,UserName) OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqDate, @ReqFrom,@Note,@TotalQty,@UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqFrom", HttpContext.Current.Session["UserName"]);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {

                    //string OurStock = ReturnFieldValueOpenCon("InvStockLedger", "ProductCode = '" + item.ProductCode + "'", "Min(InQty)", _transaction);
                    //string BatchNo = ReturnFieldValueOpenCon("InvStockLedger", "ProductCode = '" + item.ProductCode + "' order by InQty asc", "top 1 BatchNo", _transaction);
                    query = @"INSERT INTO InvPurchaseRequisitionDetails (ReqNo,ReqDate,Details,ProductCode,ProductName,Unit,ReqQty,EDD,UserName) VALUES (@ReqNo,@ReqDate,@Details,@ProductCode,@ProductName,@Unit,@ReqQty,@EDD,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Details", item.Details);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    //cmd.Parameters.AddWithValue("@BatchNo", BatchNo);
                    cmd.Parameters.AddWithValue("@EDD", item.ExDlDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteScalar();
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
        public List<InvPurchaseRequisitionModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseRequisitionModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (ReqNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select a.ReqNo, a.ReqDate, a.ReqFrom, a.TotalQty, a.Note, a.UserName from InvPurchaseRequisitionMain a WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseRequisitionModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        TotalQty = Convert.ToInt32(rdr["TotalQty"].ToString()),
                        Note = rdr["Note"].ToString(),
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
                return new List<InvPurchaseRequisitionModel>();
            }
        }
        public string Update(List<InvPurchaseRequisitionModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                DeleteInsertWithTransaction("Update InvPurchaseRequisitionDetails set Valid=0 WHERE ReqNo='" + _aModel[0].ReqNo + "' AND ReqDate ='" + _aModel[0].ReqDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                // DeleteInsertWithTransaction("DELETE FROM InvReqStockLedger WHERE ReqNo='" + _aModel[0].ReqNo + "' AND ReqDate ='" + _aModel[0].ReqDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                string query = @"UPDATE InvPurchaseRequisitionMain SET ReqNo=@ReqNo, ReqDate=@ReqDate, ReqFrom=@ReqFrom,TotalQty=@TotalQty,Note=@Note WHERE Id=@Id";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate);
                cmd.Parameters.AddWithValue("@ReqFrom", _aModel[0].ReqFrom);
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@Id", _aModel[0].Id);
                cmd.ExecuteNonQuery();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvPurchaseRequisitionDetails (ReqNo,ReqDate,Details,ProductCode,ProductName,Unit,ReqQty,EDD,UserName) VALUES (@ReqNo,@ReqDate,@Details,@ProductCode,@ProductName,@Unit,@ReqQty,@EDD,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Details", item.Details);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@EDD", item.ExDlDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                } 
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
        public List<InvPurchaseRequisitionModel> GetRequisitionList(string param)
        {
            try
            {
                var lists = new List<InvPurchaseRequisitionModel>();
                string query = @"SELECT * from  InvPurchaseRequisitionMain WHERE ReqNo='" + param + "'  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseRequisitionModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        UserName = rdr["UserName"].ToString(),
                        Note = rdr["note"].ToString(),
                        TotalQty = Convert.ToInt32(rdr["TotalQty"].ToString()),
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
                return new List<InvPurchaseRequisitionModel>();
            }
        }
        public List<InvPurchaseRequisitionModel> GetEditProductList(int param, string searchString, string productcode)
        {
            try
            {
                var lists = new List<InvPurchaseRequisitionModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {

                    cond += "AND reqNo ='" + searchString + "'";
                }
                if ((productcode != "0") && (productcode != ""))
                {

                    cond += "AND (productcode+productName) Like '%'+'" + productcode + "'+'%'";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT  * from InvPurchaseRequisitionDetails WHERE Valid=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Details = rdr["Details"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"]),
                        ExDlDate = Convert.ToDateTime(rdr["EDD"].ToString()),

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
                return new List<InvPurchaseRequisitionModel>();
            }
        }
    }
}