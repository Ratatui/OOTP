using OOTP.Lab4.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOTP.Lab4.ViewModels
{
	class Agreement_ViewModel : BasicViewModel
	{
		#region Fields

		private ObservableCollection<Agreement> agreements;

		public ObservableCollection<Agreement> Agreements
		{
			get { return agreements; }
			set
			{
				agreements = value;
				this.RaisePropertyChanged("Agreements");
			}
		}
		
		#endregion // Fields

		#region Commands

		private RelayCommand _saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(param => this.Save(),
						param => this.CanSave());
				}
				return _saveCommand;
			}
		}
		

		#endregion

		#region Constructors

		public Agreement_ViewModel()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				Agreements = new ObservableCollection<Agreement>(uow.Agreements);
			}
		}

		#endregion

		#region Methods

		private void Save()
		{
			System.Windows.MessageBox.Show("Test derive");
		}

		private bool CanSave()
		{
			return false;
		}

		#endregion // Methods
	}
}
