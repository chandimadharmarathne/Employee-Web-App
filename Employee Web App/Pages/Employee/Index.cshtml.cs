using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Employee_Web_App.Pages.Employee
{
    public class IndexModel : PageModel
    {
		public List<EmployeeInfo> listEmployee = new List<EmployeeInfo>();


		public void OnGet()
		{
			try
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM Employee";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								EmployeeInfo employeeInfo = new EmployeeInfo();
								employeeInfo.employee_Id = "" + reader.GetInt32(0);
								employeeInfo.first_name = reader.GetString(1);
								employeeInfo.last_name = reader.GetString(2);
								employeeInfo.email = reader.GetString(3);
								employeeInfo.salary = reader.GetDecimal(4).ToString();
								

								listEmployee.Add(employeeInfo);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString());
			}
		}
	}
	public class EmployeeInfo
	{
		public string employee_Id;
		public string first_name;
		public string last_name;
		public string email;
		public string salary;
	}
}
