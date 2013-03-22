using Mindscape.LightSpeed;
using OOTP.Lab4.Data;
using System.ComponentModel;

namespace OOTP.Lab4.ViewModels
{
	public class BasicViewModel : INotifyPropertyChanged 
	{
		protected static readonly LightSpeedContext<ModelUnitOfWork> context = new LightSpeedContext<ModelUnitOfWork>("Default");

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged(string propertyName)
		{
			var e = PropertyChanged;
			if (e != null)
				e(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
