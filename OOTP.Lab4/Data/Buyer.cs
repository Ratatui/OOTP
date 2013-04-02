using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OOTP.Lab4.Data
{
	/// <summary>
	/// Buyer information
	/// Inplementation IDataErrorInfo
	/// </summary>
	public partial class Buyer : IDataErrorInfo
	{
		#region Consctructors

		public Buyer()
		{
			this.Id = -1;
			this.Inn = "";
			this.LastName = "";
			this.MiddleName = "";
			this.Passport = "";
			this.Telephone = "";
			this.BirthDay = DateTime.Today;
			this.FirstName = "";
			this.Address = "";
		}

		#endregion // Consctructors

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { throw new NotImplementedException(); }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				Match match;
				switch (columnName)
				{
					case "LastName":
						if (this.LastName.Length == 0)
							return "Required value";
						if (this.LastName.Length >= 50)
							return "Mas length 50";
						break;
					case "FirstName":
						if (this.FirstName.Length == 0)
							return "Required value";
						if (this.FirstName.Length >= 50)
							return "Mas length 50";
						break;
					case "MiddleName":
						if (this.MiddleName.Length == 0)
							return "Required value";
						if (this.MiddleName.Length >= 50)
							return "Mas length 50";
						break;
					case "Passport":
						if (this.Passport.Length == 0)
							return "Required value";
						if (this.Passport.Length != 8)
							return "Invalid serial & number of passport, example ВК502460";
						match = Regex.Match(this.Passport, @"^[А-Я]{2}\d{6}$");
						if (!match.Success)
							return "Invalid serial & number of passport, example ВК502460";
						break;
					case "Inn":
						if (this.Inn.Length == 0)
							return "Required value";
						if (this.Inn.Length != 10)
							return "Inn must have 10 numbers";
						match = Regex.Match(this.Inn, @"^\d{10}$");
						if (!match.Success)
							return "Inn must have 10 numbers";
						break;
					case "Telephone":
						if (this.Telephone.Length == 0)
							return "Required value";
						match = Regex.Match(this.Telephone, @"^\+\d{2}\(\d{3}\)\d{3}-\d{2}-\d{2}$");
						if (!match.Success)
							return "Invalid phone number, example +38(044)555-55-55";
						break;
					case "Address":
						if (this.Address.Length == 0)
							return "Required value";
						if (this.Address.Length >= 200)
							return "The maximum length of 200 characters";
						break;
				}
				return null;
			}
		}

		#endregion // IDataErrorInfo Members
	}
}
