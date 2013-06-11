using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OOTP.Lab4.Data
{
	/// <summary>
	/// Controller information
	/// Inplementation IDataErrorInfo
	/// </summary>
	public partial class Controller : IDataErrorInfo
	{
		#region Constructors

		public Controller()
		{
			this.Id = -1;
			this.License = "";
			this.Name = "";
			this.Telephone = "";
			this.Address = "";
		}

		#endregion // Constructors

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { return null; }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get
			{
				Match match;
				switch (columnName)
				{
					case "Name":
						if (this.Name.Length == 0)
							return "Required value";
						if (this.Name.Length > 100)
							return "The maximum length of 100 characters";
						break;
					case "Address":
						if (this.Address.Length == 0)
							return "Required value";
						if (this.Address.Length > 200)
							return "The maximum length of 200 characters";
						break;
					case "Telephone":
						if (this.Telephone.Length > 0)
						{
							match = Regex.Match(this.Telephone, @"^\+\d{2}\(\d{3}\)\d{3}-\d{2}-\d{2}$");
							if (!match.Success)
								return "Invalid phone number, example +38(044)555-55-55";
						}
						break;
					case "License":
						if (this.License.Length == 0)
							return "Required value";
						match = Regex.Match(this.License, @"^[0-9]{10}$");
						if (!match.Success)
							return "License number must be 10 digits";
						break;
					default:
						return "Unknown exception";
				}
				return null;
			}
		}

		#endregion // IDataErrorInfo Members
	}
}
