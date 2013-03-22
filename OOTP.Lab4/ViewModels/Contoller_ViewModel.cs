using OOTP.Lab4.Data;
using System;
using Mindscape.LightSpeed.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OOTP.Lab4.Screens;

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
				this.currentController = value;
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

		public ICommand CreateCommand
		{
			get
			{
				if (createCommand == null)
					createCommand = new RelayCommand(s => OnCreate(), s => CanCreate());
				return createCommand;
			}
		}
		public ICommand DeleteCommand
		{
			get
			{
				if (deleteCommand == null)
					deleteCommand = new RelayCommand(s => OnDelete(), s => CanDelete());
				return deleteCommand;
			}
		}
		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new RelayCommand(s => OnSave(), s => CanSave());
				return saveCommand;
			}
		}
		public ICommand RefreshCommand
		{
			get
			{
				if (refreshCommand == null)
					refreshCommand = new RelayCommand(s => OnRefresh(), s => CanRefresh());
				return refreshCommand;
			}
		}
		public ICommand FilterCommand
		{
			get
			{
				if (filterCommand == null)
					filterCommand = new RelayCommand(s => OnFilter(), s => CanFilter());
				return filterCommand;
			}
		}

		private RelayCommand createCommand { get; set; }
		private RelayCommand deleteCommand { get; set; }
		private RelayCommand saveCommand { get; set; }
		private RelayCommand refreshCommand { get; set; }
		private RelayCommand filterCommand { get; set; }

		#endregion // Commands

		#region Methods

		private void OnCreate()
		{
			var newController = new Controller();
			this.Controllers.Add(newController);
			this.CurrentController = newController;
		}

		private bool CanCreate()
		{
			return true;
		}

		private void OnDelete()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentController);
				this.Controllers.Remove(CurrentController);
				uow.SaveChanges();
			}
		}

		private bool CanDelete()
		{
			if (CurrentController != null)
				return true;
			return false;
		}

		private void OnRefresh()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Controllers = new ObservableCollection<Controller>(uow.Controllers);
			}
		}

		private bool CanRefresh()
		{
			return true;
		}

		private void OnSave()
		{
			using (var uow = context.CreateUnitOfWork())
			{
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

		private bool CanSave()
		{
			if (CurrentController == null)
				return false;
			if ((CurrentController.EntityState == Mindscape.LightSpeed.EntityState.Modified
				|| CurrentController.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentController.IsValid)
				return true;
			return false;
		}

		private void OnFilter()
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

		private bool CanFilter()
		{
			return true;
		}

		#endregion // Methods
	}
}
