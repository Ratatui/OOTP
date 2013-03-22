using OOTP.Lab4.Data;
using OOTP.Lab4.Screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
					createCommand = new RelayCommand(s => OnCreate(), s => CanCreate());
				return createCommand;
			}
		}

		private RelayCommand refreshCommand;
		public ICommand RefreshCommand
		{
			get
			{
				if (refreshCommand == null)
					refreshCommand = new RelayCommand(s => OnRefresh(), s => CanRefresh());
				return refreshCommand;
			}
		}

		private RelayCommand saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
					saveCommand = new RelayCommand(s => OnSave(), s => CanSave());
				return saveCommand;
			}
		}

		private RelayCommand deleteCommand;
		public ICommand DeleteCommand
		{
			get
			{
				if (deleteCommand == null)
					deleteCommand = new RelayCommand(s => OnDelete(), s => CanDelete());
				return deleteCommand;
			}
		}

		private RelayCommand filterCommand;
		public ICommand FilterCommand
		{
			get
			{
				if (filterCommand == null)
					filterCommand = new RelayCommand(s => OnFilter(), s => CanFilter());
				return filterCommand;
			}
		}

		#endregion // Commands

		#region Methods

		private void OnCreate()
		{
			var newBuyer = new Buyer();
			this.Buyers.Add(newBuyer);
			this.CurrentBuyer = newBuyer;
		}

		private bool CanCreate()
		{
			return true;
		}

		private void OnRefresh()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Buyers = new ObservableCollection<Buyer>(uow.Buyers);
			}
		}

		private bool CanRefresh()
		{
			return true;
		}

		private void OnDelete()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				uow.Remove(CurrentBuyer);
				this.Buyers.Remove(CurrentBuyer);
				uow.SaveChanges();
			}
		}

		private bool CanDelete()
		{
			if (CurrentBuyer != null)
				return true;
			return false;
		}

		private void OnSave()
		{
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

		private bool CanSave()
		{
			if (CurrentBuyer == null)
				return false;
			if ((CurrentBuyer.EntityState == Mindscape.LightSpeed.EntityState.New
				|| CurrentBuyer.EntityState == Mindscape.LightSpeed.EntityState.Modified)
				&& CurrentBuyer.IsValid)
				return true;
			return false;
		}

		private void OnFilter()
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

		private bool CanFilter()
		{
			return true;
		}

		#endregion // methods
	}
}
