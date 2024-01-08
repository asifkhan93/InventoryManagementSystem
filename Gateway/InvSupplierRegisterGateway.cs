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
    public class InvSupplierRegisterGateway : DbConnection
    {
        private SqlTransaction _transaction;
        public string Save(InvSupplierRegisterModel _aModel)
        {
            try
            {
                Thread.Sleep(5);

                Con.Open();
                _transaction = Con.BeginTransaction();
                string getId = ReturnFieldValueOpenCon("InvSupplierRegisterMain ORDER by id DESC", "", "TOP 1 SupplierId", _transaction);
                
                int lastId = Convert.ToInt32(getId.Substring(1, 4)) + 1;
                string lastIdConvert = lastId.ToString();
                var strpd = lastIdConvert.PadLeft(4, '0');
                string SupplierId = "S" + strpd;

                string query = @"INSERT INTO InvSupplierRegisterMain (SupplierId,Name, MobileNo, Address,BankName,BankAddress,AccountNo,SwiftCode,UserName) OUTPUT INSERTED.ID VALUES (@SupplierId , @Name, @MobileNo, @Address, @BankName, @BankAddress, @AccountNo, @SwiftCode, @UserName)";
                var cmd = new SqlCommand(query, Con, _transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SupplierId", SupplierId);
                cmd.Parameters.AddWithValue("@Name", _aModel.Name);
                cmd.Parameters.AddWithValue("@MobileNo", _aModel.MobileNo);
                cmd.Parameters.AddWithValue("@Address", _aModel.Address);
                cmd.Parameters.AddWithValue("@BankName", _aModel.BankName);
                cmd.Parameters.AddWithValue("@BankAddress", _aModel.BankAddress);
                cmd.Parameters.AddWithValue("@AccountNo", _aModel.AccountNo);
                cmd.Parameters.AddWithValue("@SwiftCode", _aModel.SwiftCode);
               // cmd.Parameters.AddWithValue("@Pno", _aModel[0].Pno);
                cmd.Parameters.AddWithValue("@UserName", HttpContext.Current.Session["UserName"]);
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
        public List<InvSupplierRegisterModel> GetSupplierList(int param, string searchString)
        {
            try
            {
                var lists = new List<InvSupplierRegisterModel>();
                string cond = "", topres = "";
                if (param != 0)
                {
                    cond = "AND Id=" + param + "";
                }

                if ((searchString != "0") && (searchString != ""))
                {
                    topres = "TOP 10 ";
                    cond += "AND (SupplierID+MobileNo) Like '%'+'" + searchString + "'+'%'";
                }

                //select a.*,b.Name As AreaName from tbl_customer a LEFT JOIN tbl_AREA_INFO b ON a.AreaId=b.Id

                string query = @"SELECT " + topres + "  * from InvSupplierRegisterMain WHERE 1=1 " + cond + "  ";
                Con.Open();
                var cmd = new SqlCommand(query, Con);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new InvSupplierRegisterModel()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        SupplierId = rdr["SupplierId"].ToString(),
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
        //public List<InvSupplierRegisterModel> GetInvList(string searchString)
        //{
        //    try
        //    {
        //        var lists = new List<InvSupplierRegisterModel>();
        //        string cond = "";
        //        if ((searchString != "0") && (searchString != ""))
        //        {
        //            cond += "AND (ReqNo+ReqDate) Like '%'+'" + searchString + "'+'%'";
        //        }

        //        string query = @"select a.ReqNo, a.ReqDate, a.ReqFrom, a.Department, a.TotalQty, a.Note, a.UserName from InvRequisitionMain a WHERE 1=1 " + cond + "  ";
        //        Con.Open();
        //        var cmd = new SqlCommand(query, Con);
        //        var rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            lists.Add(new InvSupplierRegisterModel()
        //            {
        //                ReqNo = rdr["ReqNo"].ToString(),
        //                ReqDate = Convert.ToDateTime(rdr["ReqDate"].ToString()),
        //                ReqFrom = rdr["ReqFrom"].ToString(),
        //                Department = rdr["Department"].ToString(),
        //                TotalQty = Convert.ToInt32(rdr["TotalQty"].ToString()),
        //                Note = rdr["Note"].ToString(),
        //                //UserName=rdr["UserName"].ToString(),
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
        //        return new List<InvRequisitionModel>();
        //    }
        //}
    }
}