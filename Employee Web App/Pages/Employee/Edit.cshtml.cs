using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee_Web_App.Pages.Employee
{
    public class EditModel : PageModel
    {
		public EmployeeInfo employeeInfo = new EmployeeInfo();
		public String errorMessage = "";
		public String successMessage = "";
		public void OnGet()
		{
			String employee_Id = Request.Query["employee_Id"];

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM Employee WHERE employee_Id=@employee_Id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@employee_Id", employee_Id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								employeeInfo.employee_Id = "" + reader.GetInt32(0);
								employeeInfo.first_name = reader.GetString(1);
								employeeInfo.last_name = reader.GetString(2);
								employeeInfo.email = reader.GetString(3);
								employeeInfo.salary = reader.GetDecimal(4).ToString();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}

		public void OnPost()
		{
			employeeInfo.employee_Id = Request.Form["employee_Id"];
			employeeInfo.first_name = Request.Form["first_name"];
			employeeInfo.last_name = Request.Form["last_name"];
			employeeInfo.email = Request.Form["email"];
			employeeInfo.salary = Request.Form["salary"];

			if (employeeInfo.employee_Id.Length == 0 || employeeInfo.first_name.Length == 0 ||
				employeeInfo.last_name.Length == 0 || employeeInfo.email.Length == 0 || employeeInfo.salary.Length == 0)
			{
				errorMessage = "All field are requred";
				return;
			}
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE Employee " +
						"SET first_name=@first_name, last_name=@last_name, email=@email, salary=@salary" +
						" WHERE employee_Id=@employee_Id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@first_name", employeeInfo.first_name);
						command.Parameters.AddWithValue("@last_name", employeeInfo.last_name);
						command.Parameters.AddWithValue("@email", employeeInfo.email);
						command.Parameters.AddWithValue("@salary", employeeInfo.salary);
						command.Parameters.AddWithValue("@employee_Id", employeeInfo.employee_Id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			Response.Redirect("/Employee/Index");
		}

	}
}