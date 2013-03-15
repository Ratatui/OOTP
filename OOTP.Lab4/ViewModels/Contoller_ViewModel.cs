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

namespace OOTP.Lab4.ViewModels
{
	class Controller_ViewModel : BasicViewModel
	{
		#region Fields

		private ObservableCollection<Controller> _controllers;

		public ObservableCollection<Controller> Controllers
		{
			get { return _controllers; }
			set
			{
				this._controllers = value;
				this.RaisePropertyChanged("Controllers");
			}
		}

		private Controller _currentController;

		public Controller CurrentController
		{
			get { return this._currentController; }
			set
			{
				this._currentController = value;
				this.RaisePropertyChanged("CurrentController");
			}
		}

		#endregion // Fields

		#region Constructors

		public Controller_ViewModel()
		{
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
				if (_createCommand == null)
					_createCommand = new RelayCommand(s => Create(), s => CanCreate());
				return _createCommand;
			}
		}
		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
					_deleteCommand = new RelayCommand(s => Delete(), s => CanDelete());
				return _deleteCommand;
			}
		}
		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
					_saveCommand = new RelayCommand(s => Save(), s => CanSave());
				return _saveCommand;
			}
		}
		public ICommand RefreshCommand
		{
			get
			{
				if (_refreshCommand == null)
					_refreshCommand = new RelayCommand(s => Refresh(), s => CanRefresh());
				return _refreshCommand;
			}
		}

		private RelayCommand _createCommand;
		private RelayCommand _deleteCommand;
		private RelayCommand _saveCommand;
		private RelayCommand _refreshCommand;

		#endregion // Commands

		#region Methods

		private void Create()
		{
			var newController = new Controller();
			this.Controllers.Add(newController);
			this.CurrentController = newController;
		}

		private bool CanCreate()
		{
			return true;
		}

		private void Delete()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentController);
				Controllers.Remove(CurrentController);
				uow.SaveChanges();
			}
		}

		private bool CanDelete()
		{
			if (CurrentController != null)
				return true;
			return false;
		}

		private void Refresh()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				Controllers = new ObservableCollection<Controller>(uow.Controllers);
			}
		}

		private bool CanRefresh()
		{
			return true;
		}

		private void Save()
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

		#endregion // Methods
	}
}
