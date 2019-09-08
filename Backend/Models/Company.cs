using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Models
{
	public class Company
	{
		public Company(int id, string companyName)
		{
			Id = id;
			CompanyName = companyName;
		}
		public int Id { get; set; }
		public string CompanyName { get; set; }
	}
}