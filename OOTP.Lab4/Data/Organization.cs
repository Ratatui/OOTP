using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OOTP.Lab4.Data
{
	/// <summary>
	/// Organization information
	/// Inplementation IDataErrorInfo
	/// </summary>
	public partial class Organization : IDataErrorInfo
	{
		#region Consctructors

		public Organization()
		{
			this.Id = -1;
			this.Name = "";
			this.Description = "";
			this.Telephone = "";
			this.Address = "";
			this.LegalAddress = "";
			this.Profit = 0;
			this.TotalArea = 0;
			this.Staff = 0;
		}

		#endregion // Consctructors

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { return null; }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				switch (columnName)
				{
					case "Name":
						if (this.Name.Length == 0)
							return "Required value";
						if (this.Address.Length > 100)
							return "The maximum length of 100 characters";
						break;
					case "Description":
						if (this.Description.Length == 0)
							return "Required value";
						if (this.Address.Length > 200)
							return "The maximum length of 200 characters";
						break;
					case "Telephone":
						if (this.Name.Length == 0)
							return "Required value";
						if (this.Telephone.Length > 0)
						{
							var match = Regex.Match(this.Telephone, @"^\+\d{2}\(\d{3}\)\d{3}-\d{2}-\d{2}$");
							if (!match.Success)
								return "Invalid phone number, example +38(044)555-55-55";
						}
						break;
					case "Address":
						if (this.Address.Length == 0)
							return "Required value";
						if (this.Address.Length > 200)
							return "The maximum length of 200 characters";
						break;
					case "LegalAddress":
						if (this.LegalAddress.Length == 0)
							return "Required value";
						if (this.Address.Length > 200)
							return "The maximum length of 200 characters";
						break;
					case "Staff":
						if (this.Staff < 0)
							return "The value can not be negative";
						break;
					case "TotalArea":
						if (this.TotalArea < 0)
							return "The value can not be negative";
						break;
					default:
						break;
				}
				return null;
			}
		}
		#endregion // IDataErrorInfo Members
	}
}
