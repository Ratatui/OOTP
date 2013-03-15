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
	class Buyer_ViewModel : BasicViewModel
	{
		#region Fields

		private ObservableCollection<Buyer> _buyers;

		public ObservableCollection<Buyer> Buyers
		{
			get { return _buyers; }
			set
			{
				_buyers = value;
				this.RaisePropertyChanged("Buyers");
			}
		}

		private Buyer _currentBuyer;

		public Buyer CurrentBuyer
		{
			get { return _currentBuyer; }
			set
			{
				_currentBuyer = value;
				this.RaisePropertyChanged("CurrentBuyer");
			}
		}


		#endregion // Fields

		#region Constructors

		public Buyer_ViewModel()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				Buyers = new ObservableCollection<Buyer>(uow.Buyers);
			}
		}

		#endregion // Constructors

		#region Commands

		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
					_createCommand = new RelayCommand(s => Create(), s => CanCreate());
				return _createCommand;
			}
		}

		private RelayCommand _createCommand;

		public ICommand RefreshCommand{
			get
			{
				if (_refreshCommand == null)
					_refreshCommand = new RelayCommand(s => Refresh(), s => CanRefresh());
				return _refreshCommand;
			}
		}

		private RelayCommand _refreshCommand;

		public ICommand SaveCommand{
			get
			{
				if (_saveCommand == null)
					_saveCommand = new RelayCommand(s => Save(), s => CanSave());
				return _saveCommand;
			}
		}

		private RelayCommand _saveCommand;

		public ICommand DeleteCommand{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(s => Delete(), s => CanDelete());
				return DeleteCommand;
			}
		}

		private RelayCommand _deleteCommand;

		#endregion // Commands

		#region Methods

		private void Create()
		{
		}

		private bool CanCreate()
		{
			return true;
		}

		private void Refresh()
		{
		}

		private bool CanRefresh()
		{
			return true;
		}

		private void Delete()
		{

		}

		private bool CanDelete()
		{
			return false;
		}

		private void Save()
		{
		}

		private bool CanSave()
		{
			return true;
		}

		#endregion // methods
	}
}
