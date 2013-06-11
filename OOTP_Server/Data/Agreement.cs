using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OOTP.Lab4.Data
{
	/// <summary>
	/// Agreement information
	/// Inplementation IDataErrorInfo
	/// </summary>
	public partial class Agreement : IDataErrorInfo
	{
		#region Constructors

		public Agreement()
		{
			this.BuyerId = -1;
			this.ControllerId = -1;
			this.Date = DateTime.Today;
			this.Number = "";
			this.OrganizationId = -1;
		}

		#endregion // Constructors

		#region IDataErrorInfo members

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
					case "BuyerId":
						if (this.BuyerId <= 0)
							return "Required value";
						break;
					case "ControllerId":
						if (this.ControllerId <= 0)
						return "Required value";
						break;
					case "Number":
						if (this.Number.Length == 0)
							return "Required value";
						else
						{
							var match = Regex.Match(this.Number, @"^\d{10}$");
							if (!match.Success)
								return "Must be 10 numbers";
						}
						break;
					case "OrganizationId":
						if (this.OrganizationId <= 0)
							return "Required value";
						break;
				}
				return null;
			}
		}

		#endregion // IDataErrorInfo members
	}
}
