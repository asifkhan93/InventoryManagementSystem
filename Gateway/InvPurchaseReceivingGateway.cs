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
    public class InvPurchaseReceivingGateway : DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(List<InvPurchaseReceivingModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
               // string ReqNo = GetMonthlyTrNo("ReqNo", "InvProductIssueFormMain", "R", _transaction);
                string GrnNo = GetMonthlyTrNo("GrnNo", "InvPurchaseReceivingMain", "GR", _transaction);
                string InvoiceNo = GetMonthlyTrNo("InvoiceNo", "InvPurchaseReceivingMain", "I", _transaction);

                string query = @"INSERT INTO InvPurchaseReceivingMain (GrnNo, GrnDate, InvoiceNo, InvoiceDate, SupplierName,PoNo,PoDate, Note, TotalQty, TotalPrice, TotalVat, TotalDiscount,TotalNetPrice, UserName) OUTPUT INSERTED.ID VALUES (@GrnNo,@GrnDate, @InvoiceNo, @InvoiceDate,@SupplierName,@PoNo,@PoDate,@Note,@TotalQty,@TotalPrice,@TotalVat,@TotalDiscount,@TotalNetPrice,@UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@GrnNo", GrnNo);
                cmd.Parameters.AddWithValue("@GrnDate", _aModel[0].GrnDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", _aModel[0].InvoiceDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SupplierName", _aModel[0].SupplierName);
                cmd.Parameters.AddWithValue("@PoNo", _aModel[0].PoNo);
                cmd.Parameters.AddWithValue("@PoDate", _aModel[0].PoDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@TotalPrice", _aModel[0].TotalPrice);
                cmd.Parameters.AddWithValue("@TotalVAt", _aModel[0].TotalVat);
                cmd.Parameters.AddWithValue("@TotalDiscount", _aModel[0].TotalDiscount);
                cmd.Parameters.AddWithValue("@TotalNetPrice", _aModel[0].TotalNetPrice);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteScalar();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvPurchaseReceivingDetails (GrnNo,GrnDate,InvoiceNo,InvoiceDate,SupplierName,PoNo,PoDate,ProductCode,ProductName,Unit,ProductCategory,Price,Qty,Total,Vat,VatType,VatAmount,Discount,DiscountType,DiscountAmount,NetPrice,BatchNo,NetUnitPrice,ExpDate,UserName) VALUES (@GrnNo,@GrnDate,@InvoiceNo,@InvoiceDate,@SupplierName,@PoNo,@PoDate,@ProductCode,@ProductName,@Unit,@ProductCategory,@Price,@Qty,@Total,@Vat,@VatType,@VatAmount,@Discount,@DiscountType,@DiscountAmount,@NetPrice,@BatchNo,@NetUnitPrice,@ExpDate,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@GrnNo", GrnNo);
                    cmd.Parameters.AddWithValue("@GrnDate", item.GrnDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    cmd.Parameters.AddWithValue("@InvoiceDate", item.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@PoNo", item.PoNo);
                    cmd.Parameters.AddWithValue("@PoDate", item.PoDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@ProductCategory", item.ProductCategory);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);
                    cmd.Parameters.AddWithValue("@Total", item.Total);
                    cmd.Parameters.AddWithValue("@Vat", item.Vat);
                    cmd.Parameters.AddWithValue("@VatType", item.VatType);
                    cmd.Parameters.AddWithValue("@VatAmount", item.VatAmount);
                    cmd.Parameters.AddWithValue("@Discount", item.Discount);
                    cmd.Parameters.AddWithValue("@DiscountType", item.DiscountType);
                    cmd.Parameters.AddWithValue("@DiscountAmount", item.DiscountAmount);
                    cmd.Parameters.AddWithValue("@NetPrice", item.NetPrice);
                    cmd.Parameters.AddWithValue("@NetUnitPrice", item.NetPrice / item.Qty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.ExpDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                } 
                foreach (var item in _aModel)
                {
                    query = @"UPDATE InvItemRegistration SET LastPP = @LastPP , lastPurchaseDate = (getdate()) WHERE ProductCode = @ProductCode";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@LastPP", item.TotalNetPrice / item.Qty);
                    //cmd.Parameters.AddWithValue("@lastPurchaseDate", GetBdTime().ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.ExecuteNonQuery();
                }
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvStockLedger (TrNo,TrDate,InvoiceNo,InvoiceDate,SupplierName,ProductCode,ProductName,Unit,InQty,PPrice,TotalPPrice,BatchNo,ExpDate,UserName) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@SupplierName,@ProductCode,@ProductName,@Unit,@InQty,@PPrice,@TotalPPrice,@BatchNo,@ExpDate,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", GrnNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.GrnDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    cmd.Parameters.AddWithValue("@InvoiceDate", item.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@InQty", item.Qty);
                    cmd.Parameters.AddWithValue("@TotalPPrice", item.NetPrice);
                    cmd.Parameters.AddWithValue("@PPrice", item.NetPrice / item.Qty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.ExpDate.ToString("yyyy-MM-dd"));
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
        public string Update(List<InvPurchaseReceivingModel> _aModel)
        {
            try
            {
                Thread.Sleep(5);
                Con.Open();
                _transaction = Con.BeginTransaction();
                DeleteInsertWithTransaction("Update InvPurchaseReceivingDetails set Valid=0 WHERE GrnNo='" + _aModel[0].GrnNo + "' AND GrnDate ='" + _aModel[0].GrnDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                DeleteInsertWithTransaction("Update InvStockLedger set Valid=0 WHERE TrNo='" + _aModel[0].GrnNo + "' AND TrDate ='" + _aModel[0].GrnDate.ToString("yyyy-MM-dd") + "' ", _transaction);
                string query = @"UPDATE InvPurchaseReceivingMain SET  TotalQty=@TotalQty, TotalPrice=@TotalPrice, TotalVAt=@TotalVAt, TotalDiscount=@TotalDiscount, TotalNetPrice=@TotalNetPrice, Note=@Note,UserName=@UserName WHERE GrnNo=@GrnNo";

                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@GrnNo", _aModel[0].GrnNo);
                cmd.Parameters.AddWithValue("@TotalQty", _aModel[0].TotalQty);
                cmd.Parameters.AddWithValue("@TotalPrice", _aModel[0].TotalPrice);
                cmd.Parameters.AddWithValue("@TotalVAt", _aModel[0].TotalVat);
                cmd.Parameters.AddWithValue("@TotalDiscount", _aModel[0].TotalDiscount);
                cmd.Parameters.AddWithValue("@TotalNetPrice", _aModel[0].TotalNetPrice);
                cmd.Parameters.AddWithValue("@Note", _aModel[0].Note);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                cmd.ExecuteNonQuery();
                foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvPurchaseReceivingDetails (GrnNo,GrnDate,InvoiceNo,InvoiceDate,SupplierName,PoNo,PoDate,ProductCode,ProductName,Unit,ProductCategory,Price,Qty,Total,Vat,VatType,VatAmount,Discount,DiscountType,DiscountAmount,NetPrice,BatchNo,NetUnitPrice,ExpDate,UserName) VALUES (@GrnNo,@GrnDate,@InvoiceNo,@InvoiceDate,@SupplierName,@PoNo,@PoDate,@ProductCode,@ProductName,@Unit,@ProductCategory,@Price,@Qty,@Total,@Vat,@VatType,@VatAmount,@Discount,@DiscountType,@DiscountAmount,@NetPrice,@BatchNo,@NetUnitPrice,@ExpDate,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@GrnNo", item.GrnNo);
                    cmd.Parameters.AddWithValue("@GrnDate", item.GrnDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                    cmd.Parameters.AddWithValue("@InvoiceDate", item.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@PoNo", item.PoNo);
                    cmd.Parameters.AddWithValue("@PoDate", item.PoDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@ProductCategory", item.ProductCategory);
                    cmd.Parameters.AddWithValue("@Price", item.Price);
                    cmd.Parameters.AddWithValue("@Qty", item.Qty);
                    cmd.Parameters.AddWithValue("@Total", item.Total);
                    cmd.Parameters.AddWithValue("@Vat", item.Vat);
                    cmd.Parameters.AddWithValue("@VatType", item.VatType);
                    cmd.Parameters.AddWithValue("@VatAmount", item.VatAmount);
                    cmd.Parameters.AddWithValue("@Discount", item.Discount);
                    cmd.Parameters.AddWithValue("@DiscountType", item.DiscountType);
                    cmd.Parameters.AddWithValue("@DiscountAmount", item.DiscountAmount);
                    cmd.Parameters.AddWithValue("@NetPrice", item.NetPrice);
                    cmd.Parameters.AddWithValue("@NetUnitPrice", item.NetPrice / item.Qty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.ExpDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
                    cmd.ExecuteNonQuery();
                } foreach (var item in _aModel)
                {
                    query = @"INSERT INTO InvStockLedger (TrNo,TrDate,InvoiceNo,InvoiceDate,SupplierName,ProductCode,ProductName,Unit,InQty,PPrice,TotalPPrice,BatchNo,ExpDate,UserName) VALUES (@TrNo,@TrDate,@InvoiceNo,@InvoiceDate,@SupplierName,@ProductCode,@ProductName,@Unit,@InQty,@PPrice,@TotalPPrice,@BatchNo,@ExpDate,@UserName)";
                    cmd = new SqlCommand(query, Con, _transaction);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TrNo", item.GrnNo);
                    cmd.Parameters.AddWithValue("@TrDate", item.GrnDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@InvoiceNo", item.InvoiceNo);
                    cmd.Parameters.AddWithValue("@InvoiceDate", item.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@SupplierName", item.SupplierName);
                    cmd.Parameters.AddWithValue("@ProductCode", item.ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@InQty", item.Qty);
                    cmd.Parameters.AddWithValue("@TotalPPrice", item.NetPrice);
                    cmd.Parameters.AddWithValue("@PPrice", item.NetPrice / item.Qty);
                    cmd.Parameters.AddWithValue("@BatchNo", item.BatchNo);
                    cmd.Parameters.AddWithValue("@ExpDate", item.ExpDate.ToString("yyyy-MM-dd"));
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
        public List<InvPurchaseReceivingModel> GetProductList(string param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";
                if (param != null)
                {
                    cond = "AND PoNo='" + param + "'";
                }

                if ((searchString != "0") && (searchString != ""))
                {
                  
                    cond += " AND (a.ProductCode+a.ProductName) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

               // string query = @"SELECT * from InvItemRegistration WHERE 1=1 " + cond + "  ";
                string query = @"select a.ProductCode,a.ProductName,a.Unit,a.OrderdQty ,a.Price,a.OrderdQty,b.ProductCategory from InvPurchaseOrderDetails a, InvItemRegistration b where a.Valid=1 and  a.ProductCode=b.ProductCode " + cond + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        //Id = Convert.ToInt32(rdr["Id"]),
                        ProductCategory = rdr["ProductCategory"].ToString(),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Price = Convert.ToDouble(rdr["Price"].ToString()),
                        Qty = Convert.ToInt32(rdr["OrderdQty"]),
                        //Qty = rdr["Row"].ToString(),
                        //StoreName = rdr["StoreName"].ToString(),
                        //ReminderStock = rdr["ReminderStock"].ToString(),
                        //ReOrderOnly = rdr["ReOrderOnly"].ToString(),
                        //AcCode = rdr["AcCode"].ToString(),
                        //CostCenter = rdr["CostCenter"].ToString(),
                        //Note = rdr["Note"].ToString(),


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
                return new List<InvPurchaseReceivingModel>();
            }
        }
        public List<InvPurchaseReceivingModel> GetPoList(string param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";
                if (param != null)
                {
                    cond = "AND a.SupplierName='" + param + "'";
                }

                if ((searchString != "0") && (searchString != ""))
                {

                    cond += "AND (a.SupplierName+a.PoNo) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                //string query = @"SELECT distinct SupplierName,PoNo from InvPurchaseOrderDetails WHERE 1=1 " + cond + "  ";
                string query = @"SELECT a.SupplierName,a.PoNo,a.PoDate from InvPurchaseOrdermain a WHERE Valid=1 "+cond+" And  NOT EXISTS (SELECT *  FROM InvPurchaseReceivingmain b  WHERE a.PoNo= b.PoNo)";

                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        PoNo = rdr["PoNo"].ToString(),
                        SupplierName = rdr["SupplierName"].ToString(),
                        PoDate = Convert.ToDateTime(rdr["PoDate"].ToString()),



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
                return new List<InvPurchaseReceivingModel>();
            }
        }

        public List<InvSupplierRegisterModel> GetSupplierList(int param, string searchString)
        {
            try
            {
                var lists = new List<InvSupplierRegisterModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (Name+MobileNo) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT  * from InvSupplierRegisterMain WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvSupplierRegisterModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        MobileNo = rdr["MobileNo"].ToString(),
                        Address = rdr["Address"].ToString(),
                        BankName = rdr["BankName"].ToString(),
                        BankAddress = rdr["BankAddress"].ToString(),
                        AccountNo = rdr["AccountNo"].ToString(),
                        SwiftCode = rdr["SwiftCode"].ToString(),


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
                return new List<InvSupplierRegisterModel>();
            }
        }

        public List<InvPurchaseReceivingModel> GetBatchList(int param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (BatchNo+ProductCode) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT  * from InvStockLedger WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        BatchNo = rdr["BatchNo"].ToString(),
                        OurStock = Convert.ToInt32(rdr["InQty"]),
                        //ExpDate = rdr["ExpDate"].ToString(),


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
                return new List<InvPurchaseReceivingModel>();
            }
        }
        public List<InvPurchaseReceivingModel> GetInvList(string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";
                if ((searchString != "0") && (searchString != ""))
                {
                    cond += "AND (GrnNo) Like '%'+'" + searchString + "'+'%'";
                }

                string query = @"select GrnNo, GrnDate, InvoiceNo, InvoiceDate, SupplierName, PoNo , PoDate, UserName from InvPurchaseReceivingMain a WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        GrnNo = rdr["GrnNo"].ToString(),
                        GrnDate = Convert.ToDateTime(rdr["GrnDate"].ToString()),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"].ToString()),
                        SupplierName = rdr["SupplierName"].ToString(),
                        PoNo = rdr["PoNo"].ToString(),
                        PoDate = Convert.ToDateTime(rdr["PoDate"].ToString()),
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
                return new List<InvPurchaseReceivingModel>();
            }
        }
        public List<InvPurchaseReceivingModel> GetGrnList(int param, string searchString, string category)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";


                if ((searchString != null) && (searchString != ""))
                {

                    cond += "AND a.GrnNo ='" + searchString + "'";
                } if ((category != null) && (category != ""))
                {

                    cond += "AND a.GrnDate ='" + category + "'";
                }
                string query = @"SELECT a.GrnNo,a.GrnDate,a.InvoiceNo,a.InvoiceDate, a.PoNo,a.PoDate,a.SupplierName,a.TotalQty,a.TotalPrice,a.TotalVat,a.TotalDiscount,a.TotalNetPrice,a.UserName,a.Note from  InvPurchaseReceivingMain a  WHERE Valid=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        GrnNo = rdr["GrnNo"].ToString(),
                        PoNo = rdr["PoNo"].ToString(),
                        InvoiceNo = rdr["InvoiceNo"].ToString(),
                        SupplierName = rdr["SupplierName"].ToString(),
                        GrnDate = Convert.ToDateTime(rdr["GrnDate"].ToString()),
                        InvoiceDate = Convert.ToDateTime(rdr["InvoiceDate"].ToString()),
                        PoDate = Convert.ToDateTime(rdr["PoDate"].ToString()),
                        TotalQty = Convert.ToDouble(rdr["TotalQty"].ToString()),
                        TotalPrice = Convert.ToDouble(rdr["TotalPrice"].ToString()),
                        TotalVat = Convert.ToDouble(rdr["TotalVat"].ToString()),
                        TotalDiscount = Convert.ToDouble(rdr["TotalDiscount"].ToString()),
                        TotalNetPrice = Convert.ToDouble(rdr["TotalNetPrice"].ToString()),
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
                return new List<InvPurchaseReceivingModel>();
            }
        }
        public List<InvPurchaseReceivingModel> GetEditProductList(string param, string searchString)
        {
            try
            {
                var lists = new List<InvPurchaseReceivingModel>();
                string cond = "";

                if ((searchString != "0") && (searchString != ""))
                {

                    cond = "AND GrnNo='" + searchString + "'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                // string query = @"SELECT * from InvItemRegistration WHERE 1=1 " + cond + "  ";
                string query = @"select  ProductCode, ProductName,Unit, ProductCategory, Price, Qty, Total, Vat, VatType, VatAmount, Discount, DiscountType, DiscountAmount, NetPrice, BatchNo, ExpDate from InvPurchaseReceivingDetails where Valid =1 " + cond + "";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvPurchaseReceivingModel()
                    {
                        //Id = Convert.ToInt32(rdr["Id"]),
                        ProductCategory = rdr["ProductCategory"].ToString(),
                        ProductCode = rdr["ProductCode"].ToString(),
                        ProductName = rdr["ProductName"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        Price = Convert.ToDouble(rdr["Price"].ToString()),
                        Qty = Convert.ToInt32(rdr["Qty"]),
                        Total = Convert.ToDouble(rdr["Total"]),
                        Vat = Convert.ToDouble(rdr["Vat"]),
                        VatType = rdr["VatType"].ToString(),
                        DiscountType = rdr["DiscountType"].ToString(),
                        VatAmount = Convert.ToDouble(rdr["VatAmount"]),
                        Discount = Convert.ToDouble(rdr["Discount"]),
                        DiscountAmount = Convert.ToDouble(rdr["DiscountAmount"]),
                        NetPrice = Convert.ToDouble(rdr["NetPrice"]),
                        BatchNo = rdr["BatchNo"].ToString(),
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
                return new List<InvPurchaseReceivingModel>();
            }
        }

    }
}