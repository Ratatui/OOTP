using Mindscape.LightSpeed;
using OOTP.Lab4.Data;
using System.ComponentModel;

namespace OOTP.Lab4.ViewModels
{
	/// <summary>
	/// Basic view model
	/// </summary>
	public class BasicViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Property for connect DataBase with LightSpeed
		/// </summary>
		protected static Client context = new Client("127.0.0.1");

		/// <summary>
		/// Event Property Changed
		/// INotifyPropertyChanged
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Inplementation INotifyPropertyChanged
		/// </summary>
		/// <param name="propertyName">Name of property</param>
		/// <example><code>RaisePropertyChanged("CurrentObject")</code></example>
		public void RaisePropertyChanged(string propertyName)
		{
			var e = PropertyChanged;
			if (e != null)
				e(this, new PropertyChangedEventArgs(propertyName));
		}
		
	}
}
