using OOTP.Lab4.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace OOTP.Lab4.ViewModels
{
	class Organization_ViewModel : BasicViewModel
	{
		#region Fields

		private ObservableCollection<Organization> _organizations;

		public ObservableCollection<Organization> Organizations
		{
			get { return _organizations; }
			set
			{
				_organizations = value;
				this.RaisePropertyChanged("Organizations");
			}
		}

		private Organization _currentOrganization;

		public Organization CurrentOrganization
		{
			get { return _currentOrganization; }
			set
			{
				_currentOrganization = value;
				this.RaisePropertyChanged("CurrentOrganization");
			}
		}


		#endregion // Fields

		#region Constructors

		public Organization_ViewModel()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				Organizations = new ObservableCollection<Organization>(uow.Organizations);
			}
		}

		#endregion // Constructors

		#region Commands

		private RelayCommand _createCommand;

		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
					_createCommand = new RelayCommand(s => Create(), s => CanCreate());
				return _createCommand;
			}
		}

		private RelayCommand _refreshCommand;

		public ICommand RefreshCommand
		{
			get
			{
				if (_refreshCommand == null)
					_refreshCommand = new RelayCommand(s => Refresh(), s => CanRefresh());
				return _refreshCommand;
			}
		}

		private RelayCommand _deleteCommand;

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(s => Delete(), s => CanDelete());
				return _deleteCommand;
			}
		}

		private RelayCommand _saveCommand;

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
					_saveCommand = new RelayCommand(s => Save(), s => CanSave());
				return _saveCommand;
			}
		}

		#endregion // Commands

		#region Methods

		private void Create()
		{
			var newOrganization = new Organization();
			CurrentOrganization = newOrganization;
			Organizations.Add(newOrganization);
		}

		private bool CanCreate()
		{
			return true;
		}

		private void Save()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				if (CurrentOrganization.EntityState == Mindscape.LightSpeed.EntityState.New)
				{
					uow.Add(CurrentOrganization);
					uow.SaveChanges();
				}
				else
				{
					var obj = uow.Organizations.SingleOrDefault(s => s.Id == CurrentOrganization.Id);
					{
						obj.Address = CurrentOrganization.Address;
						obj.LegalAddress = CurrentOrganization.LegalAddress;
						obj.Name = CurrentOrganization.Name;
						obj.Description = CurrentOrganization.Description;
						obj.Telephone = CurrentOrganization.Telephone;
						obj.Staff = CurrentOrganization.Staff;
						obj.TotalArea = CurrentOrganization.TotalArea;
						obj.Profit = CurrentOrganization.Profit;
					}
					uow.SaveChanges();
				}

			}

		}

		private bool CanSave()
		{
			if (CurrentOrganization == null)
				return false;
			if ((CurrentOrganization.EntityState == Mindscape.LightSpeed.EntityState.Modified ||
				CurrentOrganization.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentOrganization.IsValid)
				return true;
			return false;
		}

		private void Refresh()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				Organizations = new ObservableCollection<Organization>(uow.Organizations);
			}
		}

		private bool CanRefresh()
		{
			return true;
		}

		private void Delete()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentOrganization);
				Organizations.Remove(CurrentOrganization);
				uow.SaveChanges();
			}
		}

		private bool CanDelete()
		{
			if (CurrentOrganization == null)
				return false;
			return true;
		}

		#endregion // Methods
	}
}
