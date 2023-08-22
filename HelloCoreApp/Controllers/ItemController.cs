using HelloCoreApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HelloCoreApp.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        private readonly IConfiguration _configuraton;

        public ItemController(IConfiguration configuraton)
        {
            _configuraton = configuraton;
        }

        [HttpGet]
        [Route("GetItems")]
        public List<ItemModel> GetItems()
        {
            //return LoadList();
            return LoadListFromDB();
        }

        [HttpGet]
        [Route("GetItemByID")]
        public List<ItemModel> GetItemByID(string ItemId)
        {
            //return LoadList().Where(e => e.ItemCode == ItemId).ToList();
            return LoadListFromDB().Where(e => e.ItemCode == ItemId).ToList();
        }

        [HttpPost]
        [Route("PostItem")]
        public string PostItem(ItemModel Obj)
        {
            //List<ItemModel> lstmain = new List<ItemModel>();
            //lstmain.Add(Obj);

            SqlConnection con = new SqlConnection(_configuraton.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Insert into Item values ('" + Obj.ItemCode + "','"+Obj.ItemName+"','"+Obj.Price+"')", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return "Item Added Successfully";
        }

        [HttpPost]
        [Route("DeleteItem")]
        public string DeleteItem(string ItemID)
        {
            //List<ItemModel> lstmain = new List<ItemModel>();
            //lstmain.Add(Obj);

            SqlConnection con = new SqlConnection(_configuraton.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Delete from Item where ItemCode= '" + ItemID + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return "Item Deleted Successfully";
        }

        private List<ItemModel> LoadListFromDB()
        {
            List<ItemModel> lstmain = new List<ItemModel>();

            SqlConnection con = new SqlConnection(_configuraton.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from Item", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for(int i=0; i<dt.Rows.Count;i++)
            {
                ItemModel obj = new ItemModel();
                obj.ItemCode = dt.Rows[i]["ItemCode"].ToString();
                obj.ItemName = dt.Rows[i]["ItemName"].ToString();
                obj.Price = decimal.Parse(dt.Rows[i]["Price"].ToString());
                lstmain.Add(obj);
            }

            return lstmain;
        }
        

        private List<ItemModel> LoadList()
        {
            List<ItemModel> lstmain = new List<ItemModel>();

            ItemModel obj = new ItemModel();
            obj.ItemCode = "I001";
            obj.ItemName = "Lenovo Wireless Mouse";
            obj.Price = 5000;
            lstmain.Add(obj);

            obj = new ItemModel();
            obj.ItemCode = "I002";
            obj.ItemName = "Lenovo Wireless Keyboard";
            obj.Price = 5000;
            lstmain.Add(obj);

            obj = new ItemModel();
            obj.ItemCode = "I003";
            obj.ItemName = "Lenovo Laptop";
            obj.Price = 500000;
            lstmain.Add(obj);

            obj = new ItemModel();
            obj.ItemCode = "I004";
            obj.ItemName = "Lenovo UPS";
            obj.Price = 5000;
            lstmain.Add(obj);

            return lstmain;
        }
    }
}
