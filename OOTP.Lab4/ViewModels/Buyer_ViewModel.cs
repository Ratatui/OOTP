using OOTP.Lab4.Data;
using OOTP.Lab4.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OOTP.Lab4.ViewModels
{
	public class Buyer_ViewModel : BasicViewModel
	{
		#region Fields

		private Filter_ViewModel filterViewModel { get; set; }

		private ObservableCollection<Buyer> buyers;
		public ObservableCollection<Buyer> Buyers
		{
			get { return buyers; }
			set
			{
				buyers = value;
				this.RaisePropertyChanged("Buyers");
			}
		}

		private Buyer currentBuyer;
		public Buyer CurrentBuyer
		{
			get { return currentBuyer; }
			set
			{
				if (CanSaveCommand())
				{
					if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
					{
						OnSaveCommand();
					}
					else
						currentBuyer.CancelEdit();
				}
				currentBuyer = value;
				this.RaisePropertyChanged("CurrentBuyer");
			}
		}

		#endregion // Fields

		#region Constructors

		public Buyer_ViewModel()
		{
			this.filterViewModel = new Filter_ViewModel();
			using (var uow = context.CreateUnitOfWork())
			{
				this.Buyers = new ObservableCollection<Buyer>(uow.Buyers);
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
			var newBuyer = new Buyer();
			this.Buyers.Add(newBuyer);
			this.CurrentBuyer = newBuyer;
		}

		private void OnRefreshCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Buyers = new ObservableCollection<Buyer>(uow.Buyers);
			}
		}

		private void OnDeleteCommand()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentBuyer);
				this.Buyers.Remove(CurrentBuyer);
				uow.SaveChanges();
			}
		}

		private void OnSaveCommand()
		{
			this.currentBuyer.EndEdit();
			using (var uow = context.CreateUnitOfWork())
			{
				if (CurrentBuyer.EntityState == Mindscape.LightSpeed.EntityState.New)
				{
					uow.Add(CurrentBuyer);
					uow.SaveChanges();
				}
				else
				{
					var obj = uow.Buyers.SingleOrDefault(s => s.Id == CurrentBuyer.Id);
					{
						obj.Inn = CurrentBuyer.Inn;
						obj.LastName = CurrentBuyer.LastName;
						obj.MiddleName = CurrentBuyer.LastName;
						obj.Passport = CurrentBuyer.Passport;
						obj.Telephone = CurrentBuyer.Telephone;
						obj.Address = CurrentBuyer.Address;
						obj.BirthDay = CurrentBuyer.BirthDay;
						obj.FirstName = CurrentBuyer.FirstName;
					}
					uow.SaveChanges();
				}
			}
		}

		private void OnFilterCommand()
		{
			var wnd = new FilterBuyersDialog();
			wnd.ExternalBuyerViewModel = this;
			wnd.ExternalViewModel = filterViewModel;
			wnd.Closed += (sender, args) =>
			{
				using (var uow = context.CreateUnitOfWork())
				{
					var query = from buyer in uow.Buyers
								where buyer.LastName.Contains(filterViewModel.Name ?? "")
									&& buyer.BirthDay == (filterViewModel.DateStart ?? buyer.BirthDay)
									&& buyer.Passport.Contains(filterViewModel.Passport ?? "")
									&& buyer.Inn.Contains(filterViewModel.Inn ?? "")
								select buyer;
					this.Buyers = new ObservableCollection<Buyer>(query);
				}
			};
			wnd.ShowDialog();
		}


		private bool CanCreateCommand()
		{
			return true;
		}

		private bool CanRefreshCommand()
		{
			return true;
		}

		private bool CanDeleteCommand()
		{
			if (CurrentBuyer != null)
				return true;
			return false;
		}

		private bool CanSaveCommand()
		{
			if (CurrentBuyer == null)
				return false;
			if ((CurrentBuyer.EntityState == Mindscape.LightSpeed.EntityState.New
				|| CurrentBuyer.EntityState == Mindscape.LightSpeed.EntityState.Modified)
				&& CurrentBuyer.IsValid)
				return true;
			return false;
		}

		private bool CanFilterCommand()
		{
			return true;
		}

		#endregion // methods
	}
}
