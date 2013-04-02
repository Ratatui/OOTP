using OOTP.Lab4.Data;
using OOTP.Lab4.Screens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OOTP.Lab4.ViewModels
{
	/// <summary>
	/// Organization Screen View Model
	/// </summary>
	public class Organization_ViewModel : BasicViewModel
	{
		#region Fields

		private Filter_ViewModel filterViewModel { get; set; }

		private ObservableCollection<Organization> organizations;
		public ObservableCollection<Organization> Organizations
		{
			get { return organizations; }
			set
			{
				organizations = value;
				this.RaisePropertyChanged("Organizations");
			}
		}

		private Organization currentOrganization;
		public Organization CurrentOrganization
		{
			get { return currentOrganization; }
			set
			{
				if (CanSaveCommand())
				{
					if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
					{
						OnSaveCommand();
					}
					else
						currentOrganization.CancelEdit();
				}
				currentOrganization = value;
				currentOrganization.BeginEdit();
				this.RaisePropertyChanged("CurrentOrganization");
			}
		}

		#endregion // Fields

		#region Constructors

		public Organization_ViewModel()
		{
			this.filterViewModel = new Filter_ViewModel();
			using (var uow = context.CreateUnitOfWork())
			{
				this.Organizations = new ObservableCollection<Organization>(uow.Organizations);
			}
		}

		#endregion // Constructors

		#region Commands

		private RelayCommand createCommand;
		public ICommand CreateCommand
		{
			get
			{
				if (createCommand == null)
					createCommand = new RelayCommand(s => OnCreateCommand(), s => CanCreateCommand());
				return createCommand;
			}
		}

		private RelayCommand refreshCommand;
		public ICommand RefreshCommand
		{
			get
			{
				if (refreshCommand == null)
					refreshCommand = new RelayCommand(s => OnRefreshCommand(), s => CanRefreshCommand());
				return refreshCommand;
			}
		}

		private RelayCommand deleteCommand;
		public ICommand DeleteCommand
		{
			get
			{
				if (deleteCommand == null)
					deleteCommand = new RelayCommand(s => OnDeleteCommand(), s => CanDeleteCommand());
				return deleteCommand;
			}
		}

		private RelayCommand saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new RelayCommand(s => OnSaveCommand(), s => CanSaveCommand());
				return saveCommand;
			}
		}

		private RelayCommand filterCommand;
		public ICommand FilterCommand
		{
			get
			{
				if (filterCommand == null)
					filterCommand = new RelayCommand(s => OnFilterCommand(), s => CanFilterCommand());
				return filterCommand;
			}
		}

		#endregion // Commands

		#region Methods

		private void OnCreateCommand()
		{
			var newOrganization = new Organization();
			this.CurrentOrganization = newOrganization;
			this.Organizations.Add(newOrganization);
		}

		private void OnSaveCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.currentOrganization.EndEdit();
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

		private void OnRefreshCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Organizations = new ObservableCollection<Organization>(uow.Organizations);
			}
		}

		private void OnDeleteCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentOrganization);
				this.Organizations.Remove(CurrentOrganization);
				uow.SaveChanges();
			}
		}

		private void OnFilterCommand()
		{
			var wnd = new FilterOrganizationsDialog();
			wnd.ExternalViewModel = filterViewModel;
			wnd.ExternalOrganizationViewModel = this;
			wnd.Closed += (s, args) =>
				{
					using (var uow = context.CreateUnitOfWork())
					{
						var query = from organization in uow.Organizations
									where organization.Name.Contains(filterViewModel.Name ?? "")
										&& organization.TotalArea >= (filterViewModel.AreaMin ?? organization.TotalArea)
										&& organization.TotalArea <= (filterViewModel.AreaMax ?? organization.TotalArea)
										&& organization.Staff >= (filterViewModel.StaffMin ?? organization.Staff)
										&& organization.Staff <= (filterViewModel.StaffMax ?? organization.Staff)
										&& organization.Profit >= (filterViewModel.ProfitMin ?? organization.Profit)
										&& organization.Profit <= (filterViewModel.ProfitMax ?? organization.Profit)
									select organization;
						this.Organizations = new ObservableCollection<Organization>(query);
					}
				};
			wnd.ShowDialog();
		}


		private bool CanCreateCommand()
		{
			return true;
		}

		private bool CanSaveCommand()
		{
			if (CurrentOrganization == null)
				return false;
			if ((CurrentOrganization.EntityState == Mindscape.LightSpeed.EntityState.Modified ||
				CurrentOrganization.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentOrganization.IsValid)
				return true;
			return false;
		}

		private bool CanRefreshCommand()
		{
			return true;
		}

		private bool CanDeleteCommand()
		{
			if (CurrentOrganization == null)
				return false;
			return true;
		}

		private bool CanFilterCommand()
		{
			return true;
		}

		#endregion // Methods
	}
}
