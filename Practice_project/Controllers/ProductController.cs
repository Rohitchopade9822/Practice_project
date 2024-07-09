using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Practice_project.Model;
using System.Collections.Immutable;
using System.Data;

namespace Practice_project.Controllers
{

    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("Getproduct")]
        [HttpGet]

        public async Task<IActionResult> GetAllDetails()
        {
            List<int> ints = new List<int>();

            

           

            List<ProductModel> productsmodel = new List<ProductModel>();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("defaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from TblProduct", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductModel productmodel = new ProductModel();
                productmodel.id = Convert.ToInt32(dt.Rows[i]["id"]);
                productmodel.PName = dt.Rows[i]["PName"].ToString();
                productmodel.PPrice = Convert.ToInt32(dt.Rows[i]["PPrice"]);
                productmodel.PEntryData = Convert.ToDateTime(dt.Rows[i]["PEntrydate"]);
                productsmodel.Add(productmodel);
            }

            return Ok(productsmodel);

        }
        [Route("Postproduct")]
        [HttpPost]
        public async Task<IActionResult> PostDetail(ProductModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("defaultConnection"));
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into TblProduct values('" + obj.id + "','" + obj.PName + "','" + obj.PPrice + "',getdate())", con);

                cmd.ExecuteNonQuery();
                return Ok(obj);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Route("Updateproduct")]
        [HttpPost]
        public async Task<IActionResult> updatedetails(ProductModel obj)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("defaultConnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("update TblProduct set PName='" + obj.PName + "',PPrice='" + obj.PPrice + "'where id='" + obj.id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return Ok(obj);
        }

        [Route("deleteproduct")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("defaultConnection"));
            con.Open();

            string sql = "DELETE FROM TblProduct WHERE Id = @Id";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();


            if (rowsAffected == 0)
            {
                return NotFound(); // HTTP 404
            }

            return Ok(); // HTTP 200
        }
    } 


    
}
