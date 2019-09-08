using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
	public class Contact
	{
		public Contact() { }
		public Contact(int id, string firstName, string lastName, string email, string phoneNumber, int companyId, string companyName)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			PhoneNumber = phoneNumber;
			CompanyId = companyId;
			CompanyName = companyName;
		}

		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
	}
}
