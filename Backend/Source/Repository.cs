using Backend.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Backend.Source
{
	public class Repository
	{
		private SqlConnection connection;
		private readonly string connectionString;

		public Repository()
		{
			connectionString = ConfigurationManager.ConnectionStrings["Sublime"].ConnectionString; ;
		}

		/// <summary>
		/// Get a list of all Contacts
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Contact> GetContacts()
		{
			using (connection = new SqlConnection(connectionString))
			{

				connection.Open();

				SqlCommand command = new SqlCommand
				{
					CommandText = @"
						SELECT Con.*, CompanyName FROM Contact Con
						INNER JOIN Company Com
						ON Con.CompanyId = Com.Id
					",
					CommandType = System.Data.CommandType.Text,
					Connection = connection
				};

				IAsyncResult result = command.BeginExecuteReader();

				int count = 0;
				while (!result.IsCompleted)
				{
					count += 1;
					Console.WriteLine("Waiting ({0})", count);
					System.Threading.Thread.Sleep(100);
				}

				var contacts = new List<Contact>();

				using (SqlDataReader reader = command.EndExecuteReader(result))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							var contact = new Contact(
								reader.GetInt32(reader.GetOrdinal("Id")),
								reader.GetString(reader.GetOrdinal("FirstName")),
								reader.GetString(reader.GetOrdinal("LastName")),
								reader.GetString(reader.GetOrdinal("Email")),
								reader.GetString(reader.GetOrdinal("PhoneNumber")),
								reader.GetInt32(reader.GetOrdinal("CompanyId")),
								reader.GetString(reader.GetOrdinal("CompanyName"))
							);

							contacts.Add(contact);
						}
					}
				}

				return contacts;
			}
		}

		/// <summary>
		/// Get one Contact by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Contact GetContact(int id)
		{
			using (connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand command = new SqlCommand
				{
					CommandText = @"
						SELECT Con.*, CompanyName FROM Contact Con
						INNER JOIN Company Com
						ON Con.CompanyId = Com.Id
						WHERE Con.Id = @id
					",
					CommandType = System.Data.CommandType.Text,
					Connection = connection
				};

				command.Parameters.AddWithValue("id", id);

				IAsyncResult result = command.BeginExecuteReader();

				int count = 0;
				while (!result.IsCompleted)
				{
					count += 1;
					Console.WriteLine("Waiting ({0})", count);
					System.Threading.Thread.Sleep(100);
				}

				Contact contact = null;

				using (SqlDataReader reader = command.EndExecuteReader(result))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							contact = new Contact(
								reader.GetInt32(reader.GetOrdinal("Id")),
								reader.GetString(reader.GetOrdinal("FirstName")),
								reader.GetString(reader.GetOrdinal("LastName")),
								reader.GetString(reader.GetOrdinal("Email")),
								reader.GetString(reader.GetOrdinal("PhoneNumber")),
								reader.GetInt32(reader.GetOrdinal("CompanyId")),
								reader.GetString(reader.GetOrdinal("CompanyName"))
							);
						}
					}
				}

				return contact;
			}
		}

		/// <summary>
		/// Creates a new Contact
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="email"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="companyId"></param>
		/// <returns></returns>
		public int CreateContact(string firstName, string lastName, string email, string phoneNumber, int companyId) {

			using (connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand command = new SqlCommand
				{
					Connection = connection,

					CommandText = @"
						INSERT INTO Contact(FirstName, LastName, Email, PhoneNumber, CompanyId) 
						VALUES (@firstname, @lastname, @email, @phonenumber, @companyid);

						SELECT SCOPE_IDENTITY() id
					"
				};

				command.Parameters.AddWithValue("firstname", firstName);
				command.Parameters.AddWithValue("lastname", lastName);
				command.Parameters.AddWithValue("email", email);
				command.Parameters.AddWithValue("phonenumber", phoneNumber);
				command.Parameters.AddWithValue("companyid", companyId);

				var result = command.ExecuteScalar();

				int returnValue = Decimal.ToInt32((decimal)result);

				return returnValue;
			}
		}

		/// <summary>
		/// Updates a contact with new values
		/// </summary>
		/// <param name="id"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="email"></param>
		/// <param name="phoneNumber"></param>
		/// <param name="companyId"></param>
		/// <returns></returns>
		public int UpdateContact(int id, string firstName, string lastName, string email, string phoneNumber, int companyId)
		{
			using (connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand command = new SqlCommand
				{
					Connection = connection,
					CommandText = @"
						UPDATE Contact
						SET	FirstName = @firstname,
							LastName = @lastname,
							Email = @email,
							PhoneNumber = @phonenumber,
							CompanyId = @companyid
						OUTPUT INSERTED.Id
						WHERE Id = @id
					"
				};

				command.Parameters.AddWithValue("id", id);
				command.Parameters.AddWithValue("firstname", firstName);
				command.Parameters.AddWithValue("lastname", lastName);
				command.Parameters.AddWithValue("email", email);
				command.Parameters.AddWithValue("phonenumber", phoneNumber);
				command.Parameters.AddWithValue("companyid", companyId);

				var result = command.ExecuteScalar();

				int returnVal = Decimal.ToInt32((int)result);

				return returnVal;
			}
		}
		
		/// <summary>
		/// Gets a list of all Companies
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Company> GetCompanies()
		{
			using (connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand command = new SqlCommand
				{
					CommandText = @"
						SELECT * FROM Company
					",

					CommandType = System.Data.CommandType.Text,

					Connection = connection
				};

				IAsyncResult result = command.BeginExecuteReader();


				int count = 0;
				while (!result.IsCompleted)
				{
					count += 1;
					Console.WriteLine("Waiting ({0})", count);
					System.Threading.Thread.Sleep(100);
				}

				var companies = new List<Company>();

				using (SqlDataReader reader = command.EndExecuteReader(result))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							var contact = new Company(
								reader.GetInt32(reader.GetOrdinal("Id")),
								reader.GetString(reader.GetOrdinal("CompanyName"))
							);

							companies.Add(contact);
						}
					}
				}

				return companies;
			}
		}
	}
}