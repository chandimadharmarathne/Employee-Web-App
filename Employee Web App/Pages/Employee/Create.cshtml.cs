using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee_Web_App.Pages.Employee
{
    public class CreateModel : PageModel
    {
		public EmployeeInfo employeeInfo = new EmployeeInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
		}

		public void OnPost()
		{
			employeeInfo.first_name = Request.Form["first_name"];
			employeeInfo.last_name = Request.Form["last_name"];
			employeeInfo.email = Request.Form["email"];
			employeeInfo.salary = Request.Form["salary"];

			if (employeeInfo.first_name.Length == 0 || employeeInfo.last_name.Length == 0 ||
				employeeInfo.email.Length == 0 || employeeInfo.salary.Length == 0)
			{
				errorMessage = "All the Fielda are required";
				return;
			}

			// save the new client data in database
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "INSERT INTO Employee " +
						"(first_name, last_name, email, salary) VALUES " +
						"(@first_name, @last_name, @email,@salary);";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@first_name", employeeInfo.first_name);
						command.Parameters.AddWithValue("@last_name", employeeInfo.last_name);
						command.Parameters.AddWithValue("@email", employeeInfo.email);
						command.Parameters.AddWithValue("@salary", employeeInfo.salary);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			employeeInfo.first_name = "";
			employeeInfo.last_name = "";
			employeeInfo.email = "";
			employeeInfo.salary = "";
			successMessage = "New Employee added Correctly";

			Response.Redirect("/Employee/Index");
		}
	}
}

