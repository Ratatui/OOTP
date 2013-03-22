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
	public class Agreement_ViewModel : BasicViewModel
	{
		#region Fields

		private Agreement currentAgreement { get; set; }

		private Filter_ViewModel filterViewModel { get; set; }

		private ObservableCollection<Agreement> agreements { get; set; }

		private ObservableCollection<Buyer> buyers { get; set; }

		private ObservableCollection<Controller> controllers { get; set; }

		private ObservableCollection<Organization> organizations { get; set; }

		private ObservableCollection<Organization> privatizedOrganizations { get; set; }

		public ObservableCollection<Agreement> Agreements
		{
			get { return agreements; }
			set
			{
				agreements = value;
				this.RaisePropertyChanged("Agreements");
			}
		}

		public ObservableCollection<Buyer> Buyers
		{
			get { return buyers; }
			set
			{
				buyers = value;
				this.RaisePropertyChanged("Buyers");
			}
		}

		public ObservableCollection<Controller> Controllers
		{
			get { return controllers; }
			set
			{
				this.controllers = value;
				this.RaisePropertyChanged("Controllers");
			}
		}

		public ObservableCollection<Organization> Organizations
		{
			get { return organizations; }
			set
			{
				organizations = value;
				this.RaisePropertyChanged("Organizations");
			}
		}

		public ObservableCollection<Organization> PrivatizedOrganizations
		{
			get { return privatizedOrganizations; }
			set
			{
				privatizedOrganizations = value;
				this.RaisePropertyChanged("PrivatizedOrganizations");
			}
		}

		public Agreement CurrentAgreement
		{
			get { return currentAgreement; }
			set
			{
				currentAgreement = value;
				this.RaisePropertyChanged("CurrentAgreement");
			}
		}

		#endregion // Fields

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
		public ICommand RefreshCommand
		{
			get
			{
				if (refreshCommand == null)
					refreshCommand = new RelayCommand(s => OnRefresh(), s => CanRefresh());
				return refreshCommand;
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
				{
					saveCommand = new RelayCommand(s => this.OnSave(), s => this.CanSave());
				}
				return saveCommand;
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

		#endregion

		#region Constructors

		public Agreement_ViewModel()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Organizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == false).OrderBy(s => s.Name));
				this.PrivatizedOrganizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == true));
				this.Controllers = new ObservableCollection<Controller>(uow.Controllers.OrderBy(s => s.Name));
				this.Buyers = new ObservableCollection<Buyer>(uow.Buyers.OrderBy(s => s.Passport));
				this.Agreements = new ObservableCollection<Agreement>(uow.Agreements.OrderBy(s => s.Number));

				this.filterViewModel = new Filter_ViewModel();

				#region Fake Data
				//Random rand = new Random();

				//for (int i = 0; i < 30; i++)
				//{
				//	var obj = new Organization()
				//	{
				//		Address = "г Донецк Артема "+rand.Next(1,300).ToString()+"/"+rand.Next(1,100).ToString(),
				//		Description = "Description organization #"+i.ToString(),
				//		LegalAddress = "г Донецк Артема " + rand.Next(1, 300).ToString() + "/" + rand.Next(1, 100).ToString(),
				//		Name = "Organization #" + i.ToString(),
				//		Profit = rand.Next(0,100000),
				//		Staff = rand.Next(0, 1000),
				//		Telephone = "+38(062)" + rand.Next(100, 999).ToString() + "-" + rand.Next(10, 99).ToString() + "-" + rand.Next(10, 99).ToString(),
				//		TotalArea = rand.Next(100,10000)
				//	};
				//	uow.Add(obj);
				//	uow.SaveChanges();
				//}
				#endregion // Fake Data
			}
		}

		#endregion

		#region Methods

		private void OnCreate()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				var newAgreement = new Agreement();
				this.Agreements.Add(newAgreement);
				this.CurrentAgreement = newAgreement;
			}
		}

		private bool CanCreate()
		{
			return true;
		}

		private void OnRefresh()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				this.Organizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == false).OrderBy(s => s.Name));
				this.PrivatizedOrganizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == true));
				this.Controllers = new ObservableCollection<Controller>(uow.Controllers.OrderBy(s => s.Name));
				this.Buyers = new ObservableCollection<Buyer>(uow.Buyers.OrderBy(s => s.Passport));
				this.Agreements = new ObservableCollection<Agreement>(uow.Agreements.OrderBy(s => s.Number));
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
				uow.Remove(CurrentAgreement);
				this.CurrentAgreement.Organization.IsPrivatized = false;
				this.Agreements.Remove(CurrentAgreement);
				uow.SaveChanges();
			}
		}

		private bool CanDelete()
		{
			if (this.CurrentAgreement != null)
				return true;
			return false;
		}

		private void OnSave()
		{
			using (var uow = context.CreateUnitOfWork())
			{
				if (CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.New)
				{
					uow.Add(CurrentAgreement);
					CurrentAgreement.Organization.IsPrivatized = true;
					uow.SaveChanges();
				}
				else
				{
					var obj = uow.Agreements.SingleOrDefault(s => s.Id == CurrentAgreement.Id);
					{
						obj.Organization.IsPrivatized = false;
						obj.Number = CurrentAgreement.Number;
						obj.OrganizationId = CurrentAgreement.OrganizationId;
						obj.BuyerId = CurrentAgreement.BuyerId;
						obj.ControllerId = CurrentAgreement.ControllerId;
						obj.Date = CurrentAgreement.Date;
						obj.Organization.IsPrivatized = true;
					}
					uow.SaveChanges();
				}
			}
		}

		private bool CanSave()
		{
			if (this.CurrentAgreement == null)
				return false;
			if ((this.CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.Modified ||
				this.CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentAgreement.IsValid)
				return true;
			return false;
		}

		private void OnFilter()
		{
			var wnd = new FilterOrganizationsDialog();
			wnd.ExternalViewModel = filterViewModel;
			wnd.ExternalAgreementViewModel = this;
			wnd.Closed += (sender, args) =>
				{
					if (wnd.DialogResult == true)
					{
						using (var uow = context.CreateUnitOfWork())
						{
							var query = from agreement in uow.Agreements
										join organization in uow.Organizations
										on agreement.OrganizationId equals organization.Id
										join buyer in uow.Buyers
										on agreement.BuyerId equals buyer.Id
										join controller in uow.Controllers
										on agreement.ControllerId equals controller.Id
										where agreement.Number.Contains(filterViewModel.Number ?? "")
											&& agreement.Date >= (filterViewModel.DateStart ?? agreement.Date)
											&& agreement.Date <= (filterViewModel.DateEnd ?? agreement.Date)
											&& organization.Name.Contains(filterViewModel.Name ?? "")
											&& organization.Profit >= (filterViewModel.ProfitMin ?? organization.Profit)
											&& organization.Profit <= (filterViewModel.ProfitMax ?? organization.Profit)
											&& organization.TotalArea >= (filterViewModel.AreaMin ?? organization.TotalArea)
											&& organization.TotalArea <= (filterViewModel.AreaMax ?? organization.TotalArea)
											&& organization.Staff >= (filterViewModel.StaffMin ?? organization.Staff)
											&& organization.Staff <= (filterViewModel.StaffMax ?? organization.Staff)
											&& buyer.Passport.Contains(filterViewModel.Passport ?? "")
											&& buyer.Inn.Contains(filterViewModel.Inn ?? "")
											&& controller.Id == (filterViewModel.Controller ?? controller.Id)
										select agreement;
							this.Organizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == false).OrderBy(s => s.Name));
							this.PrivatizedOrganizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == true));
							this.Controllers = new ObservableCollection<Controller>(uow.Controllers.OrderBy(s => s.Name));
							this.Buyers = new ObservableCollection<Buyer>(uow.Buyers.OrderBy(s => s.Passport));
							this.Agreements = new ObservableCollection<Agreement>(query);
						}
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
