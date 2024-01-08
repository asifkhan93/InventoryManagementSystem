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
    public class InvProductIssueFormGateway:DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(List<InvProductIssueFormModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string GinNo = GetMonthlyTrNo("GinNo", "InvProductIssueFormMain", "GI", _transaction);

                string query = @"INSERT INTO InvProductIssueFormMain (ReqNo, ReqDate, GinNo, GinDate, IssueTo, Note, UserName) OUTPUT INSERTED.ID VALUES (@ReqNo,@ReqDate, @GinNo, @GinDate, @IssueTo, @Note, @UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@GinNo", GinNo);
                cmd.Parameters.AddWithValue("@GinDate", _aModel[0].GinDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@IssueTo", _aModel[0].IssueTo);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvProductIssueFormDetails (ReqNo, ReqDate, GinNo, GinDate,ProductCode, ProductName, IssueTo,  Unit, ReqQty, OutQty, InQty, BatchNo, ExpDate, ChkDone, UserName) VALUES (@ReqNo, @ReqDate, @GinNo, @GinDate, @ProductCode, @ProductName, @IssueTo, @Unit, @ReqQty, @OutQty, @InQty, @BatchNo, @ExpDate, @ChkDone, @UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", _aModel[0].ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@GinNo", GinNo);
                    cmd.Parameters.AddWithValue("@GinDate", item.GinDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@IssueTo", item.IssueTo);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@OutQty", item.OutQty);
                    cmd.Parameters.AddWithValue("@InQty", item.InQty);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.GinDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ChkDone", item.ChkDone);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvReqLedger (TrNo, TrDate, PrevReqNo, PrevReqDate, ProductCode, ProductName,Department,ReqFrom, Unit, UiFrom, ReqOutQty, UserName) VALUES (@TrNo, @TrDate, @PrevReqNo, @PrevReqDate, @ProductCode, @ProductName,@Department, @ReqFrom, @Unit, @UiFrom, @ReqOutQty, @UserName )";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", GinNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.GinDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PrevReqNo", item.TrNo);
                    cmd.Parameters.AddWithValue("@PrevReqDate", item.TrDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Department", item.IssueTo);
                    cmd.Parameters.AddWithValue("@ReqFrom", item.ReqFrom);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@UiFrom", item.Ui);
                    cmd.Parameters.AddWithValue("@ReqOutQty", item.IssueQty);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                } foreach (var item in _aModel)
                {


                    query = @"INSERT INTO InvStockLedger (TrNo,TrDate,InvoiceNo,InvoiceDate,ProductCode,ProductName,OutQty,BatchNo,ExpDate,ReqQty,SalesPrice,PPrice,TotalPPrice,UserName) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@ProductCode,@ProductName,@OutQty,@BatchNo,@ExpDate,@ReqQty,@SalesPrice,@PPrice,@TotalPPrice,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", GinNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.GinDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@InvoiceNo", item.TrNo);
                    cmd.Parameters.AddWithValue("@InvoiceDate", item.TrDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@OutQty", item.IssueQty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.ExpDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@SalesPrice", item.PerUnitPrice);
                    cmd.Parameters.AddWithValue("@TotalPPrice", item.PerUnitPrice * item.IssueQty);
                    cmd.Parameters.AddWithValue("@PPrice", item.PerUnitPrice );
                  
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
    public List<InvProductIssueFormModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvProductIssueFormModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (ReqNo+GinNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select ReqNo, ReqDate, GinNo, GinDate, Note, UserName from InvProductIssueFormMain a WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvProductIssueFormModel()
                    {
                        ReqNo=rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        GinNo = rdr["GinNo"].ToString(),
                        GinDate = Convert.ToDateTime(rdr["GinDate"].ToString()),
                        Note=rdr["Note"].ToString(),
                      //  UserName=rdr["UserName"].ToString(),
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
                return new List<InvProductIssueFormModel>();
            }
        }
    public List<InvProductIssueFormModel> GetBatchList(int param, string searchString, string category)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string cond = "";
            if (param != 0)
            {
                cond = "AND Id=" + param + "";
            }

            if ((searchString != "0") && (searchString != ""))
            {

                cond += "AND (ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
            }

            //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

            //string query = @"SELECT  * from InvStockLedger WHERE 1=1 " + cond + " AND InQty > 0 ";
            string query = @"SELECT  a.BatchNo,a.ProductCode,a.ProductName,a.ExpDate,a.PPrice, (select SUM(InQty-OutQty) as OurStock from InvStockLedger b where a.BatchNo = b.batchNo and a.ProductName = b.ProductName )AS OurStock from InvStockLedger  a  WHERE 1=1 " + cond + " AND InQty > 0 Group by BatchNo,ProductCode,ProductName,ExpDate,PPrice";

            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    //Id = Convert.ToInt32(rdr["Id"]),
                    ProductCode = rdr["ProductCode"].ToString(),
                    ProductName = rdr["ProductName"].ToString(),
                    InQty = Convert.ToInt32(rdr["OurStock"]),
                    BatchNo = rdr["BatchNo"].ToString(),
                    ExpDate = Convert.ToDateTime(rdr["ExpDate"].ToString()),
                    PerUnitPrice = Convert.ToDouble(rdr["PPrice"]),
                    

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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> GetProductList(int param, string searchString, string category)
     {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string cond = "";
            if (param != 0)
            {
                cond = "AND Id=" + param + "";
            }

            if ((searchString != null) && (searchString != ""))
            {

                //cond += "AND (ProductCode+ProductName) Like '%'+'" + searchString + "'+'%'";
                cond += "AND (ProductCode + ProductName) Like '%'+'" + searchString + "'+'%'";
               
            }
            if ((category != null) && (category != ""))
            {

                //  cond += " AND TrNo='" + category + "' ";
                cond += " AND PrevReqNo= '" + category + "'";
            }


            //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

           // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
           // string query = @"SELECT * from  InvReqLedger WHERE 1=1 " + cond + "  ";
            string query = @"select PrevReqDate AS ReqDate ,PrevReqNo AS ReqNo ,ProductCode , Productname,Unit, ReqInQty,SUM(ReqApprovedQty-ReqOutQty) as ReqApprovedQty  from InvReqLedger where valid = 1 " + cond + " GROUP BY PrevReqDate ,PrevReqNo ,ProductCode , Productname,ReqInQty,Unit HAVING SUM(ReqApprovedQty-ReqOutQty) <> 0 Order BY ProductCode";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    
                    //ProductCategory = rdr["ProductCategory"].ToString(),
                    ProductCode = rdr["ProductCode"].ToString(),
                    //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                    ProductName = rdr["ProductName"].ToString(),
                    //productcategory = rdr["productcategory"].ToString(),
                    Unit = rdr["Unit"].ToString(),
                    ReqInQty = Convert.ToInt32(rdr["ReqInQty"]),
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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> GetRequisitionList(string param)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string query = @"SELECT * from  InvRequisitionApprovalMain WHERE TrNo='" + param + "'  ";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    TrNo = rdr["TrNo"].ToString(),
                    TrDate = Convert.ToDateTime(rdr["TrDate"].ToString()),
                   // ProductCode = rdr["ProductCode"].ToString(),
                   // ProductName = rdr["ProductName"].ToString(),
                    //Details = rdr["Details"].ToString(),
                    //Unit = rdr["Unit"].ToString(),
                    Department = rdr["Department"].ToString(),
                    //OurStock = rdr["OurStock"].ToString(),
                    //BatchNo = rdr["BatchNo"].ToString(),
                    //ReqInQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
                    //ReqApprovedQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> GetReqList(int param, string searchString, string category)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string cond = "";
            if (param != 0)
            {
                cond = "AND Id=" + param + "";
            }

            if ((searchString != null) && (searchString != ""))
            {


                cond += "AND (TrNo+PrevReqNo+Department) Like '%'+'" + searchString + "'+'%'";
            }
            if ((category != null) && (category != ""))
            {

                cond += "AND ReqNo='" + category + "' ";
            }


            //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

            // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
            string query = @"select PrevReqDate,PrevReqNo,Department,ReqFrom  from InvReqLedger where valid = 1 " + cond + " GROUP BY PrevReqDate ,PrevReqNo,Department,ReqFrom HAVING SUM(ReqApprovedQty-ReqOutQty) <> 0 Order BY PrevReqNo";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    
                    //ProductCategory = rdr["ProductCategory"].ToString(),
                    PrevReqNo = rdr["PrevReqNo"].ToString(),
                    //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                    PrevReqDate = Convert.ToDateTime(rdr["PrevReqDate"].ToString()),
                    //productcategory = rdr["productcategory"].ToString(),
                    ReqFrom = rdr["ReqFrom"].ToString(),
                    Department = rdr["Department"].ToString(),
                   // ApprovedBy = rdr["ApprovedBy"].ToString(),


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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> GetApprovedQty(int param, string searchString, string category)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
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

                cond += "AND TrNo='" + category + "' ";
            }


            //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

            // string query = @"select a.ProductCode,b.ProductName,SUM(a.InQty-a.OutQty+a.RtnQty) AS Balance , c.ReqQty  from InvStockLedger a , InvItemRegistration b , InvRequisutionDetails c where a.ProductCode = b.ProductCode and a.valid = 1 and a.ProductCode = c.ProductCode  and a.ProductCode = " + cond + " GROUP BY a.ProductCode,b.ProductName,c.ReqQty";
            string query = @"SELECT * from  InvReqLedger WHERE 1=1 " + cond + "  ";
            // string query = @"SELECT a.ReqInQty,b.ReqApprovedQty from  InvReqLedger a , InvReqLedger b WHERE 1=1 AND (a.ProductCode+a.ProductName) Like '%01246%'AND b.PrevReqNo='R2108030001' AND a.TrNo = 'R2108030001'";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    Id = Convert.ToInt32(rdr["Id"]),
                    //ProductCategory = rdr["ProductCategory"].ToString(),
                    ProductCode = rdr["ProductCode"].ToString(),
                    //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                   // ProductName = rdr["ProductName"].ToString(),
                    //productcategory = rdr["productcategory"].ToString(),
                   // Unit = rdr["Unit"].ToString(),
                    // ReqInQty = Convert.ToInt32(rdr["ReqInQty"]),
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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> EditIssueReqList(int param, string searchString, string category)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string cond = "";
            if ((searchString != null) && (searchString != ""))
            {

                cond += "AND ReqNo ='" + searchString + "'";
            } if ((category != null) && (category != ""))
            {

                cond += "AND ReqDate ='" + category + "'";
            }
            string query = @"SELECT * from  InvProductIssueFormMain WHERE Valid=1 " + cond + "  ";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {
                    ReqNo = rdr["ReqNo"].ToString(),
                    ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                    GinNo = rdr["GinNo"].ToString(),
                    GinDate = Convert.ToDateTime(rdr["GinDate"].ToString()),
                    Department = rdr["IssueTo"].ToString(),
                    Note = rdr["Note"].ToString(),
                    UserName = rdr["UserName"].ToString(),
                    //BatchNo = rdr["BatchNo"].ToString(),
                    //ReqInQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
                    //ReqApprovedQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
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
            return new List<InvProductIssueFormModel>();
        }
    }
    public List<InvProductIssueFormModel> GetEditProductList(int param, string searchString, string category)
    {
        try
        {
            var lists = new List<InvProductIssueFormModel>();
            string cond = "";
            if (param != 0)
            {
                cond = "AND Id=" + param + "";
            }

            if ((searchString != null) && (searchString != ""))
            {


                cond += "AND ReqNo= '" + searchString + "'";

            }
            string query = @"select ProductCode, Productname,Unit,OutQty,InQty,BatchNo,ExpDate  from InvProductIssueFormDetails where valid = 1 " + cond + " ";
            Con.Open();
            var cmd = new SqlCommand(query, Con);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new InvProductIssueFormModel()
                {

                    //ProductCategory = rdr["ProductCategory"].ToString(),
                    ProductCode = rdr["ProductCode"].ToString(),
                    //RegistrationGroup = rdr["RegistrationGroup"].ToString(),
                    ProductName = rdr["ProductName"].ToString(),
                    //productcategory = rdr["productcategory"].ToString(),
                    Unit = rdr["Unit"].ToString(),
                    BatchNo = rdr["BatchNo"].ToString(),
                    InQty = Convert.ToInt32(rdr["InQty"]),
                    OutQty = Convert.ToInt32(rdr["OutQty"]),
                    ExpDate = Convert.ToDateTime(rdr["ExpDate"].ToString()),


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
            return new List<InvProductIssueFormModel>();
        }
    }
     }
    
}