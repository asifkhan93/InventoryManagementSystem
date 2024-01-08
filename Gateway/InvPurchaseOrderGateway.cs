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
    public class InvPurchaseOrderGateway :DbConnection
    {
        //public List<InvPurchaseOrderModel> GetRequisitionList(string param)
        //{
        //    try
        //    {
        //        var lists = new List<InvPurchaseOrderModel>();
        //        string query = @"SELECT * from  InvPurchaseRequisitionMain WHERE ReqNo='" + param + "'  ";
        //        Con.Open();
        //        var cmd = new SqlCommand(query, Con);
        //        var rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            lists.Add(new InvPurchaseOrderModel()
        //            {
        //                ReqNo = rdr["ReqNo"].ToString(),
        //                ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
        //                // ProductCode = rdr["ProductCode"].ToString(),
        //                // ProductName = rdr["ProductName"].ToString(),
        //                //Details = rdr["Details"].ToString(),
        //                //Unit = rdr["Unit"].ToString(),
        //                SupplierName = rdr["Supplier"].ToString(),
        //                ReqFrom = rdr["ReqFrom"].ToString(),
        //                //OurStock = rdr["OurStock"].ToString(),
        //                //BatchNo = rdr["BatchNo"].ToString(),
        //                //ReqInQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
        //                //ReqApprovedQty = Convert.ToInt32(rdr["ReqInQty"].ToString()),
        //            });
        //        }

        //        rdr.Close();
        //        Con.Close();
        //        return lists;

        //    }
        //    catch (Exception exception)
        //    {

        //        if (Con.State == ConnectionState.Open)
        //        {
        //            Con.Close();
        //        }
        //        return new List<InvPurchaseOrderModel>();
        //    }
        //}
        private SqlTransaction _transaction;
        public string Save(List<InvPurchaseOrderModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                string PoNo = GetMonthlyTrNo("ReqNo", "InvPurchaseOrderMain", "P", _transaction);

                string query = @"INSERT INTO InvPurchaseOrderMain (ReqNo, ReqDate, PoNo, PoDate, SupplierName, SupplierId, SupplierMobileNo, SupplierAddress, LastIssueFor, NextIssueFor, Note, TotalNet) OUTPUT INSERTED.ID VALUES (@ReqNo, @ReqDate, @PoNo, @PoDate, @SupplierName, @SupplierId, @SupplierMobileNo, @SupplierAddress, @LastIssueFor, @NextIssueFor, @Note, @TotalNet)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ReqNo", _aModel[0].ReqNo);
                cmd.Parameters.AddWithValue("@ReqDate", _aModel[0].ReqDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PoNo", PoNo);
                cmd.Parameters.AddWithValue("@PoDate", _aModel[0].PoDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SupplierName", _aModel[0].SupplierName);
                cmd.Parameters.AddWithValue("@SupplierId", _aModel[0].SupplierId);
                cmd.Parameters.AddWithValue("@SupplierMobileNo", _aModel[0].SupplierMobileNo);
                cmd.Parameters.AddWithValue("@SupplierAddress", _aModel[0].SupplierAddress);
                cmd.Parameters.AddWithValue("@LastIssueFor", _aModel[0].LastIssueFor);
                cmd.Parameters.AddWithValue("@NextIssueFor", _aModel[0].NextIssueFor);
                cmd.Parameters.AddWithValue("@TotalNet", _aModel[0].TotalNet);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvPurchaseOrderDetails (ReqNo, ReqDate, PoNo, PoDate, SupplierName, SupplierId, SupplierMobileNo, SupplierAddress, ProductCode, ProductName, Unit, CurrentQty, IssuedQty, ReqQty, EstReqQty, OrderdQty,LastIssueFor,NextIssueFor, Price, Total, TotalNet, Note, UserName) VALUES (@ReqNo, @ReqDate, @PoNo, @PoDate, @SupplierName, @SupplierId, @SupplierMobileNo, @SupplierAddress, @ProductCode, @ProductName, @Unit, @CurrentQty, @IssuedQty, @ReqQty, @EstReqQty, @OrderdQty,@LastIssueFor,@NextIssueFor, @Price, @Total,@TotalNet,@Note,@UserName )";
                    cmd = new SqlCommand(query, Con, _transaction); 
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PoNo",PoNo);
                    cmd.Parameters.AddWithValue("@PoDate", item.PoDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@SupplierId", item.SupplierId);
                    cmd.Parameters.AddWithValue("@SupplierMobileNo", item.SupplierMobileNo);
                    cmd.Parameters.AddWithValue("@SupplierAddress", item.SupplierAddress);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@CurrentQty", item.CurrentQty);
                    cmd.Parameters.AddWithValue("@IssuedQty", item.IssuedQty);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@EstReqQty", item.EstReqQty);
                    cmd.Parameters.AddWithValue("@OrderdQty", item.OrderdQty);
                    cmd.Parameters.AddWithValue("@LastIssueFor", item.LastIssueFor);
                    cmd.Parameters.AddWithValue("@NextIssueFor", item.NextIssueFor);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Total", item.Total);
                    cmd.Parameters.AddWithValue("@TotalNet", item.TotalNet);
                    cmd.Parameters.AddWithValue("@Note", item.Note);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }
                foreach (var item in _aModel)
                {
                    query = @"update InvItemRegistration set ApproverPrice=@ApproverPrice , lastPurchaseDate=@lastPurchaseDate where ProductCode=@ProductCode";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ApproverPrice", item.Price);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@lastPurchaseDate",GetBdTime());
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
        public List<InvPurchaseOrderModel> GetProductList(string param, string searchString, string lastIssue)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "ReqNo Like '%'+'" + searchString + "'+'%'";
                }
                string query = @"select aa.ProductCode, bb.productName,bb.Unit,bb.ApproverPrice, Sum(aa.ReqQty) as ReqQty, Sum(aa.CurrentQty) as CurrentQty , Sum(aa.IssuedQty) as IssuedQty , Sum(aa.EstQty) as EstQty from (select ProductCode , ReqQty , 0 as CurrentQty, 0 as IssuedQty, 0 as EstQty from InvPurchaseRequisitionDetails where " + cond + "and Valid=1 union all select ProductCode,0 ,Sum(InQty-OutQty) , 0, 0 from InvStockLedger where ProductCode in (select  ProductCode from InvPurchaseRequisitionDetails where " + cond + " and Valid=1)  group by ProductCode union all select ProductCode,0 ,0,Sum(OutQty),Sum(OutQty*" + param + "/" + lastIssue + ") from InvStockLedger where ProductCode in (select  ProductCode from InvPurchaseRequisitionDetails where " + cond + " and Valid=1)  and TrDate between GETDATE()-" + lastIssue + " and GETDATE() group by ProductCode) aa, InvItemRegistration bb where aa.ProductCode = bb.ProductCode group by aa.ProductCode, bb.productName,bb.Unit,bb.ApproverPrice order by aa.ProductCode";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"]),
                        CurrentQty = Convert.ToInt32(rdr["CurrentQty"]),
                        IssuedQty = Convert.ToInt32(rdr["IssuedQty"]),
                        EstQty = Convert.ToInt32(rdr["EstQty"]),
                        Price = Convert.ToDouble(rdr["ApproverPrice"]),



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
                return new List<InvPurchaseOrderModel>();
            }
        }
       

        public List<InvPurchaseOrderModel> GetRequisitionList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += " a.ReqNo Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT a.ReqNo,a.ReqDate, a.ReqFrom from InvPurchaseRequisitionMain a Left join  InvPurchaseOrderMain b on a.ReqNo = b.ReqNo WHERE  " + cond + " And b.ReqNo is null";
                //string query = @"SELECT * from  InvPurchaseRequisitionMain WHERE 1=1 " + cond + "  ";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        ReqFrom = rdr["ReqFrom"].ToString(),


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
                return new List<InvPurchaseOrderModel>();
            }
        }
        public List<InvPurchaseOrderModel> GetSupplierList(int param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND (SupplierId+Name) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT SupplierId,Name,MobileNo,Address from  InvSupplierRegisterMain WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        SupplierId = rdr["SupplierId"].ToString(),
                        SupplierName = rdr["Name"].ToString(),
                        SupplierMobileNo = rdr["MobileNo"].ToString(),
                        SupplierAddress = rdr["Address"].ToString(),


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
                return new List<InvPurchaseOrderModel>();
            }
        }
        public List<InvPurchaseOrderModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (PoNo+ReqNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select a.ReqNo, a.ReqDate, a.PoNo, a.PoDate, a.Note,a.SupplierName,Note, a.UserName from InvPurchaseOrderMain a WHERE 1=1 " + cond + "  ";
                
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        PoNo = rdr["PoNo"].ToString(),
                        PoDate = Convert.ToDateTime(rdr["PoDate"].ToString()),
                        SupplierName = rdr["SupplierName"].ToString(),
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
                return new List<InvPurchaseOrderModel>();
            }
        }
        public List<InvPurchaseOrderModel> GetSingleInvDetails(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND PoNo Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT * from InvPurchaseOrderMain   WHERE Valid=1 " + cond + " ";
                //string query = @"SELECT * from  InvPurchaseRequisitionMain WHERE 1=1 " + cond + "  ";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        ReqNo = rdr["ReqNo"].ToString(),
                        ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
                        PoNo = rdr["PoNo"].ToString(),
                        PoDate = Convert.ToDateTime(rdr["PoDate"].ToString()),
                        SupplierName = rdr["SupplierName"].ToString(),
                        SupplierAddress = rdr["SupplierAddress"].ToString(),
                        SupplierId = rdr["SupplierId"].ToString(),
                        SupplierMobileNo = rdr["SupplierMobileNo"].ToString(),
                        Note = rdr["Note"].ToString(),
                        LastIssueFor = Convert.ToInt32(rdr["LastIssueFor"].ToString()),
                        NextIssueFor = Convert.ToInt32(rdr["NextIssueFor"].ToString()),
                        TotalNet = Convert.ToInt32(rdr["TotalNet"].ToString()),



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
                return new List<InvPurchaseOrderModel>();
            }
        }
        public string Update(List<InvPurchaseOrderModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                DeleteInsertWithTransaction("Update InvPurchaseOrderDetails set Valid=0 WHERE PoNo='" + _aModel[0].PoNo + "' AND PoDate ='" + _aModel[0].PoDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                string query = @"UPDATE InvPurchaseOrderMain SET PoDate=@PoDate, SupplierName=@SupplierName, SupplierId=@SupplierId, SupplierMobileNo=@SupplierMobileNo, SupplierAddress=@SupplierAddress, LastIssueFor=@LastIssueFor, NextIssueFor=@NextIssueFor,Note=@Note,UserName=@UserName WHERE PoNo=@PoNo";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@PoNo", _aModel[0].PoNo);
                cmd.Parameters.AddWithValue("@PoDate", _aModel[0].PoDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SupplierName", _aModel[0].SupplierName);
                cmd.Parameters.AddWithValue("@SupplierId", _aModel[0].SupplierId);
                cmd.Parameters.AddWithValue("@SupplierMobileNo", _aModel[0].SupplierMobileNo);
                cmd.Parameters.AddWithValue("@SupplierAddress", _aModel[0].SupplierAddress);
                cmd.Parameters.AddWithValue("@NextIssueFor", _aModel[0].NextIssueFor);
                cmd.Parameters.AddWithValue("@LastIssueFor", _aModel[0].LastIssueFor);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteNonQuery();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvPurchaseOrderDetails (ReqNo, ReqDate, PoNo, PoDate, SupplierName, SupplierId, SupplierMobileNo, SupplierAddress, ProductCode, ProductName, Unit, CurrentQty, IssuedQty, ReqQty, EstReqQty, OrderdQty,LastIssueFor,NextIssueFor, Price, Total, TotalNet, Note, UserName) VALUES (@ReqNo, @ReqDate, @PoNo, @PoDate, @SupplierName, @SupplierId, @SupplierMobileNo, @SupplierAddress, @ProductCode, @ProductName, @Unit, @CurrentQty, @IssuedQty, @ReqQty, @EstReqQty, @OrderdQty,@LastIssueFor,@NextIssueFor, @Price, @Total,@TotalNet,@Note,@UserName )";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ReqNo", item.ReqNo);
                    cmd.Parameters.AddWithValue("@ReqDate", item.ReqDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@PoNo", item.PoNo);
                    cmd.Parameters.AddWithValue("@PoDate", item.PoDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@SupplierId", item.SupplierId);
                    cmd.Parameters.AddWithValue("@SupplierMobileNo", item.SupplierMobileNo);
                    cmd.Parameters.AddWithValue("@SupplierAddress", item.SupplierAddress);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@CurrentQty", item.CurrentQty);
                    cmd.Parameters.AddWithValue("@IssuedQty", item.IssuedQty);
                    cmd.Parameters.AddWithValue("@ReqQty", item.ReqQty);
                    cmd.Parameters.AddWithValue("@EstReqQty", item.EstReqQty);
                    cmd.Parameters.AddWithValue("@OrderdQty", item.OrderdQty);
                    cmd.Parameters.AddWithValue("@LastIssueFor", item.LastIssueFor);
                    cmd.Parameters.AddWithValue("@NextIssueFor", item.NextIssueFor);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Total", item.Total);
                    cmd.Parameters.AddWithValue("@TotalNet", item.TotalNet);
                    cmd.Parameters.AddWithValue("@Note", item.Note);
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }
                foreach (var item in _aModel)
                {
                    query = @"update InvItemRegistration set ApproverPrice=@ApproverPrice , lastPurchaseDate=@lastPurchaseDate where ProductCode=@ProductCode";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ApproverPrice", item.Price);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@lastPurchaseDate", GetBdTime());
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
        public List<InvPurchaseOrderModel> GetEditProductList(string param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseOrderModel>();
                string cond = "";

                if ((searchString != null) && (searchString != ""))
                {

                    cond += "And PoNo Like '%'+'" + searchString + "'+'%'";
                }
                string query = @"select  ProductCode, ProductName, Unit, CurrentQty, IssuedQty, ReqQty, EstReqQty, OrderdQty, Price, Total from InvPurchaseOrderDetails where Valid=1 "+cond+"";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseOrderModel()
                    {
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        ReqQty = Convert.ToInt32(rdr["ReqQty"]),
                        CurrentQty = Convert.ToInt32(rdr["CurrentQty"]),
                        IssuedQty = Convert.ToInt32(rdr["IssuedQty"]),
                        EstReqQty = Convert.ToInt32(rdr["EstReqQty"]),
                        Price = Convert.ToDouble(rdr["Price"]),
                        Total = Convert.ToDouble(rdr["Total"]),
                        OrderdQty = Convert.ToInt32(rdr["OrderdQty"]),



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
                return new List<InvPurchaseOrderModel>();
            }
        }
    }
}