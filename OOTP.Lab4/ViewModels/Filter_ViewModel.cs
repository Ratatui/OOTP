using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOTP.Lab4.ViewModels
{
	public class Filter_ViewModel : BasicViewModel
	{
		public string Number { get; set; }
		public string Name { get; set; }
		public string Passport { get; set; }
		public string Inn { get; set; }
		public int? Controller { get; set; }
		public int? AreaMax { get; set; }
		public int? AreaMin { get; set; }
		public int? StaffMax { get; set; }
		public int? StaffMin { get; set; }
		public double? ProfitMax { get; set; }
		public double? ProfitMin { get; set; }
		public DateTime? DateStart { get; set; }
		public DateTime? DateEnd { get; set; }

		public void Clear()
		{
			this.AreaMax = null;
			this.AreaMin = null;
			this.Controller = null;
			this.DateEnd = null;
			this.DateStart = null;
			this.Inn = null;
			this.Name = null;
			this.Number = null;
			this.Passport = null;
			this.ProfitMax = null;
			this.ProfitMin = null;
			this.StaffMax = null;
			this.StaffMin = null;
		}
	}
}
