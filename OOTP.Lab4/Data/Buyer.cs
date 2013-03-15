using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOTP.Lab4.Data
{
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
		}

		#endregion // Consctructors

		#region IDataErrorInfo Members

		string IDataErrorInfo.Error
		{
			get { throw new NotImplementedException(); }
		}

		string IDataErrorInfo.this[string columnName]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion // IDataErrorInfo Members
	}
}
