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
    public class InvRequisitionGateway : DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(List<InvRequisitionModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string ReqNo = GetMonthlyTrNo("ReqNo", "InvRequisitionMain", "R", _transaction);

                string query = @"INSERT INTO InvRequisitionMain (ReqNo, ReqDate, ReqFrom, Department,Note,TotalQty,UserName) OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqDate, @ReqFrom, @Department,@Note,@TotalQty,@UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReqFrom", HttpContext.Current.Session["UserName"]);
                cmd.Parameters.AddWithValue("@Department", _aModel[0].Department);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {

                    //string OurStock = ReturnFieldValueOpenCon("InvStockLedger", "ProductCode = '" + item.ProductCode + "'", "Min(InQty)", _transaction);
                    //string BatchNo = ReturnFieldValueOpenCon("InvStockLedger", "ProductCode = '" + item.ProductCode + "' order by InQty asc", "top 1 BatchNo", _transaction);
                    query = @"INSERT INTO InvRequisutionDetails (ReqNo,ReqDate,Details,ProductCode,ProductName,Unit, Department, ReqQty,UserName) VALUES (@ReqNo,@ReqDate,@Details,@ProductCode,@ProductName,@Unit,@Department,@ReqQty,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Details", item.Details);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    //cmd.Parameters.AddWithValue("@BatchNo", BatchNo);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteScalar();
                } foreach (var item in _aModel)
                {

                    query = @"INSERT INTO InvReqLedger ( TrNo,TrDate, PrevReqNo, PrevReqDate, ProductCode, ProductName, Unit, ReqInQty,Department,ReqFrom,UiFrom, UserName) VALUES (@ReqNo,@ReqDate, @PrevReqNo, @PrevReqDate,@ProductCode,@ProductName,@Unit,@ReqInQty,@Department,@ReqFrom,@UiFrom,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PrevReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@PrevReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@ReqInQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    cmd.Parameters.AddWithValue("@UiFrom", item.Ui);
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
        public List<InvItemRegistrationModel> GetProductList(int param, string searchString)
        {
            try
            {
                var lists = new List<InvItemRegistrationModel>();
                string cond = "", topres = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {
                    topres = "TOP 10 ";
                    cond += "AND (ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT " + topres + "  * from InvItemRegistration WHERE 1=1 " + cond + "  ";
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
                        RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ApproverPrice = Convert.ToDecimal(rdr["ApproverPrice"].ToString()),
                        RackName = rdr["RackName"].ToString(),
                        Row = rdr["Row"].ToString(),
                        StoreName = rdr["StoreName"].ToString(),
                        ReminderStock = rdr["ReminderStock"].ToString(),
                        ReOrderOnly = rdr["ReOrderOnly"].ToString(),
                        AcCode = rdr["AcCode"].ToString(),
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

        public List<InvRequisitionModel> GetRequisitionList(string param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvRequisitionModel>();
                string query = @"SELECT * from  InvRequisutionDetails WHERE ReqNo='" + searchString + "'  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Details = rdr["Details"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Department = rdr["Department"].ToString(),
                        ReqFrom = rdr["UserName"].ToString(),
                        //OurStock = rdr["OurStock"].ToString(),
                        //BatchNo = rdr["BatchNo"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"].ToString()),
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
                return new List<InvRequisitionModel>();
            }
        }
        public List<InvRequisitionModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvRequisitionModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (ReqNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select a.ReqNo, a.ReqDate, a.ReqFrom, a.Department, a.TotalQty, a.Note, a.UserName from InvRequisitionMain a WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        Department = rdr["Department"].ToString(),
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
                return new List<InvRequisitionModel>();
            }
        }
        public string Update(List<InvRequisitionModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                DeleteInsertWithTransaction("Update InvRequisutionDetails set Valid=0 WHERE ReqNo='" + _aModel[0].ReqNo + "' AND ReqDate ='" + _aModel[0].ReqDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                DeleteInsertWithTransaction("Update InvReqLedger set Valid=0 WHERE TrNo='" + _aModel[0].ReqNo + "' AND TrDate ='" + _aModel[0].ReqDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                string query = @"UPDATE InvRequisitionMain SET ReqNo=@ReqNo, ReqDate=@ReqDate, ReqFrom=@ReqFrom,Department=@Department,TotalQty=@TotalQty,Note=@Note WHERE Id=@Id";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate);
                cmd.Parameters.AddWithValue("@ReqFrom", _aModel[0].ReqFrom);
                cmd.Parameters.AddWithValue("@Department", _aModel[0].Department);
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@Id", _aModel[0].Id);
                cmd.ExecuteNonQuery();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvRequisutionDetails (ReqNo,ReqDate,Details,Department,ProductCode,ProductName,Unit,ReqQty,UserName) VALUES (@ReqNo,@ReqDate,@Details,@Department,@ProductCode,@ProductName,@Unit,@ReqQty,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Details", item.Details);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                } foreach (var item in _aModel)
                {

                    query = @"INSERT INTO InvReqLedger ( TrNo,TrDate, PrevReqNo, PrevReqDate, ProductCode, ProductName, Unit, ReqInQty,Department,ReqFrom, UiFrom, UserName) VALUES (@ReqNo,@ReqDate, @ReqNo, @ReqDate,@ProductCode,@ProductName,@Unit,@InQty,@Department,@ReqFrom,@UiFrom,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@InQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    cmd.Parameters.AddWithValue("@UiFrom", item.Ui);
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
        public List<InvRequisitionModel> GetEditProductList(int param, string searchString, string productcode)
        {
            try
            {
                var lists = new List<InvRequisitionModel>();
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

                string query = @"SELECT  * from InvRequisutionDetails WHERE Valid=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Details = rdr["Details"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"]),

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
                return new List<InvRequisitionModel>();
            }
        }
    }
}