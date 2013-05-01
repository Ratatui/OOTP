using OOTP.Lab4.Data;
using OOTP.Lab4.Screens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OOTP.Lab4.ViewModels
{

	/// <summary>
	/// Controller page View Model
	/// </summary>
	public class Controller_ViewModel : BasicViewModel
	{
		#region Fields

		private Filter_ViewModel filterViewModel { get; set; }

		private ObservableCollection<Controller> controllers;
		public ObservableCollection<Controller> Controllers
		{
			get { return controllers; }
			set
			{
				this.controllers = value;
				this.RaisePropertyChanged("Controllers");
			}
		}

		private Controller currentController;
		public Controller CurrentController
		{
			get { return this.currentController; }
			set
			{
				if (CanSaveCommand())
				{
					if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
					{
						OnSaveCommand();
					}
					else
						currentController.CancelEdit();
				}
				this.currentController = value;
				if (currentController != null)
                    this.currentController.BeginEdit();
				this.RaisePropertyChanged("CurrentController");
			}
		}

		#endregion // Fields

		#region Constructors

		public Controller_ViewModel()
		{
			this.filterViewModel = new Filter_ViewModel();
			using (var uow = context.CreateUnitOfWork())
			{
				this.Controllers = new ObservableCollection<Controller>(uow.Controllers);
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

		private RelayCommand filterCommand;
		public ICommand FilterCommand
		{
			get
			{
				if (filterCommand == null)
					filterCommand = new RelayCommand(s => OnFilterCommand(), s => CanFilter());
				return filterCommand;
			}
		}

		#endregion // Commands

		#region Methods

		private void OnCreateCommand()
		{
			var newController = new Controller();
			this.Controllers.Add(newController);
			this.CurrentController = newController;
		}

		private void OnDeleteCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentController);
				this.Controllers.Remove(CurrentController);
				uow.SaveChanges();
			}
		}

		private void OnRefreshCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Controllers = new ObservableCollection<Controller>(uow.Controllers);
			}
		}

		private void OnSaveCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.currentController.EndEdit();
				if (CurrentController.EntityState == Mindscape.LightSpeed.EntityState.New)
				{
					uow.Add(CurrentController);
					uow.SaveChanges();
				}
				else
				{
					var obj = uow.Controllers.SingleOrDefault(s => s.Id == CurrentController.Id);
					{
						obj.Name = CurrentController.Name;
						obj.Address = CurrentController.Address;
						obj.Telephone = CurrentController.Telephone;
						obj.License = CurrentController.License;
					}
					uow.SaveChanges();
				}
			}
		}

		private void OnFilterCommand()
		{

			var wnd = new FilterControllersDialog();
			wnd.ExternalControllerViewModel = this;
			wnd.ExternalViewModel = filterViewModel;
			wnd.Closed += (sender, args) =>
				{
					using (var uow = context.CreateUnitOfWork())
					{
						var query = from controller in uow.Controllers
									where controller.Id == (filterViewModel.Controller ?? controller.Id)
										&& controller.Name.Contains(filterViewModel.Name ?? "")
									select controller;
						this.Controllers = new ObservableCollection<Controller>(query);
					}
				};
			wnd.ShowDialog();
		}


		private bool CanCreateCommand()
		{
			return true;
		}

		private bool CanDeleteCommand()
		{
			if (CurrentController != null)
				return true;
			return false;
		}

		private bool CanRefreshCommand()
		{
			return true;
		}

		private bool CanSaveCommand()
		{
			if (CurrentController == null)
				return false;
			if ((CurrentController.EntityState == Mindscape.LightSpeed.EntityState.Modified
				|| CurrentController.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentController.IsValid)
				return true;
			return false;
		}

		private bool CanFilter()
		{
			return true;
		}

		#endregion // Methods
	}
}
