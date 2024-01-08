using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using PointOfSalesApp.Gateway.DB_Helper;
using PointOfSalesApp.Models;
using System.Threading.Tasks;


namespace PointOfSalesApp.Controllers
{
    class InvRequisitionApprovalGateway : DbConnection
    {

        private SqlTransaction _transaction;
        public Task<string> Save(List<InvRequisitionApprovalModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();

                string TrNo = GetMonthlyTrNo("TrNo", "InvReqLedger", "P", _transaction);

                string query = @"INSERT INTO InvRequisitionApprovalMain (TrNo, TrDate, PrevReqNo, PrevReqDate, ApprovedBy, ReqFrom, Department, UserName) OUTPUT INSERTED.ID VALUES ( @TrNo, @TrDate, @PrevReqNo, @PrevReqDate, @ApprovedBy, @ReqFrom, @Department, @UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrNo", TrNo);
                cmd.Parameters.AddWithValue("@TrDate", _aModel[0].ApprovalDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PrevReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@PrevReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ApprovedBy", _aModel[0].ApprovedBy);
                cmd.Parameters.AddWithValue("@ReqFrom", _aModel[0].ReqFrom);
                cmd.Parameters.AddWithValue("@Department", _aModel[0].Department);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();

                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvReqLedger (TrNo, TrDate, PrevReqNo, PrevReqDate, ApprovedBy, ProductCode, ProductName,Department, ReqFrom,ReqInQty, Unit, ReqApprovedQty, UiFrom, UserName) VALUES (@TrNo, @TrDate, @PrevReqNo, @PrevReqDate, @ApprovedBy, @ProductCode, @ProductName,@Department, @ReqFrom,@ReqInQty, @Unit, @ReqApprovedQty, @UiFrom, @UserName )";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", TrNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.ApprovalDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PrevReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@PrevReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ApprovedBy",item.ApprovedBy );
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    cmd.Parameters.AddWithValue("@ReqInQty", item.ReqInQty);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@UiFrom", item.Ui);
                    cmd.Parameters.AddWithValue("@ReqApprovedQty", item.ReqApprovedQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }

                _transaction.Commit();
                Con.Close();
                return Task.FromResult<string>(SaveMessage);
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
        public string Update(List<InvRequisitionApprovalModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                DeleteInsertWithTransaction("Update InvReqLedger set Valid=0 WHERE TrNo='" + _aModel[0].TrNo + "' AND TrDate ='" + _aModel[0].ApprovalDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                string query = @"UPDATE InvRequisitionApprovalMain SET TrDate=@TrDate,PrevReqNo=@PrevReqNo, PrevReqDate=@TrDate,ApprovedBy=@ApprovedBy, ReqFrom=@ReqFrom,Department=@Department,Note=@Note WHERE TrNo=@TrNo";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrNo", _aModel[0].TrNo);
                cmd.Parameters.AddWithValue("@TrDate", _aModel[0].ApprovalDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PrevReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@PrevReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ApprovedBy", _aModel[0].ApprovedBy);
                cmd.Parameters.AddWithValue("@ReqFrom", _aModel[0].ReqFrom);
                cmd.Parameters.AddWithValue("@Department", _aModel[0].Department);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteNonQuery();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvReqLedger (TrNo, TrDate, PrevReqNo, PrevReqDate, ApprovedBy, ProductCode, ProductName,Department, ReqFrom, ReqInQty, Unit, ReqApprovedQty, UiFrom, UserName) VALUES (@TrNo, @TrDate, @PrevReqNo, @PrevReqDate, @ApprovedBy, @ProductCode, @ProductName,@Department, @ReqFrom, @ReqInQty, @Unit, @ReqApprovedQty, @UiFrom, @UserName )";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", item.TrNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.ApprovalDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PrevReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@PrevReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ApprovedBy", item.ApprovedBy);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Department", item.Department);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    cmd.Parameters.AddWithValue("@ReqInQty", item.ReqInQty);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@UiFrom", item.Ui);
                    cmd.Parameters.AddWithValue("@ReqApprovedQty", item.ReqApprovedQty);
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
        public List<InvRequisitionApprovalModel> GetProductList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvRequisitionApprovalModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND (ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
                }
                if ((category != null) && (category != ""))
                {

                    cond += "AND ReqNo='" + category + "' ";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
                string query = @"SELECT a.ProductCode, a.ProductName, a.Unit, a.ReqQty, COALESCE(SUM(InQty-OutQty+RtnQty),0) AS OurStock  from  InvRequisutionDetails a left join InvStockLedger b on a.ProductCode=b.ProductCode WHERE a.valid = 1 " + cond + " group by a.ProductCode, a.ProductName, a.Unit, a.ReqQty ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionApprovalModel()
                    {
                        ProductCode = rdr["ProductCode"].ToString(),                        
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"]),
                        OurStock = Convert.ToInt32(rdr["OurStock"]),


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
                return new List<InvRequisitionApprovalModel>();
            }
        }
        public List<InvRequisitionApprovalModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvRequisitionApprovalModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (TrNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select a.TrNo, a.TrDate, a.ReqFrom, a.Department, a.ApprovedBy, a.Note, a.UserName from InvRequisitionApprovalMain a WHERE valid =1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionApprovalModel()
                    {
                        TrNo = rdr["TrNo"].ToString(),
                        ApprovalDate = Convert.ToDateTime(rdr["TrDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        ApprovedBy = rdr["ApprovedBy"].ToString(),
                        Department = rdr["Department"].ToString(),
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
                return new List<InvRequisitionApprovalModel>();
            }
        }
        public List<InvRequisitionApprovalModel> GetReqList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvRequisitionApprovalModel>();
                string cond = "";
                //if (param != 0)
                //{
                //    cond = "AND Id=" + param + "";
                //}

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND (a.Department+a.ReqNo+a.ReqFrom) Like '%'+'" + searchString + "'+'%'";
                }
                if ((category != null) && (category != ""))
                {

                    cond += "AND a.ReqNo='" + category + "' ";
                }


                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
                //string query = @"SELECT * from  InvRequisitionMain WHERE 1=1 " + cond + "  ";
                string query = @"SELECT a.ReqNo,a.ReqDate,a.ReqFrom,a.Department, b.PrevReqNo from  InvRequisitionMain a left join InvRequisitionApprovalMain b on a.ReqNo=b.PrevReqNo WHERE 1=1 "+ cond +"  AND b.PrevReqNo is null";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionApprovalModel()
                    {
                        //Id = Convert.ToInt32(rdr["Id"]),
                        //ProductCategory = rdr["ProductCategory"].ToString(),
                        ReqNo = rdr["ReqNo"].ToString(),
                        //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        //productcategory = rdr["productcategory"].ToString(),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        Department = rdr["Department"].ToString(),


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
                return new List<InvRequisitionApprovalModel>();
            }
        }
        public List<InvRequisitionApprovalModel> GetRequisitionList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvRequisitionApprovalModel>();
                string cond = "";


                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND a.TrNo ='" + searchString + "'";
                } if ((category != null) && (category != ""))
                {

                    cond += "AND a.TrDate ='" + category + "'";
                }
                string query = @"SELECT a.TrNo,a.TrDate,a.ReqFrom,a.Department, a.PrevReqNo,a.PrevReqDate,a.UserName,a.Note from  InvRequisitionApprovalMain a  WHERE Valid=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionApprovalModel()
                    {
                        TrNo = rdr["TrNo"].ToString(),
                        ReqNo = rdr["PrevReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["PrevReqDate"].ToString()),
                        ApprovalDate = Convert.ToDateTime(rdr["TrDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),
                        Department = rdr["Department"].ToString(),
                        Note = rdr["Note"].ToString(),
                        UserName = rdr["UserName"].ToString(),


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
                return new List<InvRequisitionApprovalModel>();
            }
        }
        public List<InvRequisitionApprovalModel> GetEditProductList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvRequisitionApprovalModel>();
                string cond = "";

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND a.TrNo='" + searchString + "' ";
                }
                string query = @"SELECT a.ProductCode, a.ProductName, a.Unit, a.ReqInQty,a.ReqApprovedQty, COALESCE(SUM(InQty-OutQty+RtnQty),0) AS OurStock  from  InvReqLedger a left join InvStockLedger b on a.ProductCode=b.ProductCode WHERE a.Valid = 1 And a.UiFrom='Req.Approval' " + cond + " group by a.ProductCode, a.ProductName, a.Unit, a.ReqInQty , a.ReqApprovedQty";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvRequisitionApprovalModel()
                    {
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ReqInQty = Convert.ToInt32(rdr["ReqInQty"]),
                        OurStock = Convert.ToInt32(rdr["OurStock"]),
                        ReqApprovedQty = Convert.ToInt32(rdr["ReqApprovedQty"]),


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
                return new List<InvRequisitionApprovalModel>();
            }
        }
       
    }
}
