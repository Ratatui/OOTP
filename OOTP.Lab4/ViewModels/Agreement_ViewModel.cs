﻿using OOTP.Lab4.Data;
using OOTP.Lab4.Screens;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OOTP.Lab4.ViewModels
{
	/// <summary>
	/// Agreement Screen View Model
	/// </summary>
	public class Agreement_ViewModel : BasicViewModel
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

		private ObservableCollection<Organization> privatizedOrganizations;
		public ObservableCollection<Organization> PrivatizedOrganizations
		{
			get { return privatizedOrganizations; }
			set
			{
				privatizedOrganizations = value;
				this.RaisePropertyChanged("PrivatizedOrganizations");
			}
		}

		private Agreement currentAgreement;
		public Agreement CurrentAgreement
		{
			get { return currentAgreement; }
			set
			{
				if (CanSaveCommand())
				{
					if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
					{
						OnSaveCommand();
					}
					else
						currentAgreement.CancelEdit();
				}
				currentAgreement = value;
				currentAgreement.BeginEdit();
				this.RaisePropertyChanged("CurrentAgreement");
			}
		}

		private Filter_ViewModel filterViewModel { get; set; }

		#endregion // Fields

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
				{
					saveCommand = new RelayCommand(s => this.OnSaveCommand(), s => this.CanSaveCommand());
				}
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

		#endregion

		#region Constructors

		public Agreement_ViewModel()
		{
			this.filterViewModel = new Filter_ViewModel();

			this.Organizations = new ObservableCollection<Organization>(context.Organizations);
			this.PrivatizedOrganizations = new ObservableCollection<Organization>(context.Organizations);
			this.Controllers = new ObservableCollection<Controller>(context.Controllers);
			this.Buyers = new ObservableCollection<Buyer>(context.Buyers);
			this.Agreements = new ObservableCollection<Agreement>(context.Agreements);
		}

		#endregion

		#region Methods

		private void OnCreateCommand()
		{
			var newAgreement = new Agreement();
			this.Agreements.Add(newAgreement);
			this.CurrentAgreement = newAgreement;
		}

		private void OnRefreshCommand()
		{
			//using (var uow = context.CreateUnitOfWork())
			//{
			//	this.Organizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == false).OrderBy(s => s.Name));
			//	this.PrivatizedOrganizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == true));
			//	this.Controllers = new ObservableCollection<Controller>(uow.Controllers.OrderBy(s => s.Name));
			//	this.Buyers = new ObservableCollection<Buyer>(uow.Buyers.OrderBy(s => s.Passport));
			//	this.Agreements = new ObservableCollection<Agreement>(uow.Agreements.OrderBy(s => s.Number));
			//}
		}

		private void OnDeleteCommand()
		{
			//using (var uow = context.CreateUnitOfWork())
			//{
			//	uow.Remove(CurrentAgreement);
			//	this.CurrentAgreement.Organization.IsPrivatized = false;
			//	this.Agreements.Remove(CurrentAgreement);
			//	uow.SaveChanges();
			//}
		}

		private void OnSaveCommand()
		{
			var uow = context;
			{
				this.currentAgreement.EndEdit();
				if (CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.New)
				{
					uow.AddAgreement(CurrentAgreement);
					//CurrentAgreement.Organization.IsPrivatized = true;
					//uow.SaveChanges();
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
					//uow.SaveChanges();
				}
			}
		}

		private void OnFilterCommand()
		{
			var wnd = new FilterAgreementsDialog();
			wnd.ExternalViewModel = filterViewModel;
			wnd.ExternalAgreementViewModel = this;
			wnd.Closed += (sender, args) =>
			{
				if (wnd.DialogResult == true)
				{
					var uow = context;
					{
						if (filterViewModel.IsOR)
						{
							var query = from agreement in uow.Agreements
										join organization in uow.Organizations
										on agreement.OrganizationId equals organization.Id
										join buyer in uow.Buyers
										on agreement.BuyerId equals buyer.Id
										join controller in uow.Controllers
										on agreement.ControllerId equals controller.Id
										where organization.Profit >= (filterViewModel.ProfitMin ?? organization.Profit)
										&& organization.Profit <= (filterViewModel.ProfitMax ?? organization.Profit)
										|| organization.TotalArea >= (filterViewModel.AreaMin ?? organization.TotalArea)
										&& organization.TotalArea <= (filterViewModel.AreaMax ?? organization.TotalArea)
										|| organization.Staff >= (filterViewModel.StaffMin ?? organization.Staff)
										&& organization.Staff <= (filterViewModel.StaffMax ?? organization.Staff)
										select agreement;
							this.Organizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == false).OrderBy(s => s.Name));
							this.PrivatizedOrganizations = new ObservableCollection<Organization>(uow.Organizations.Where(s => s.IsPrivatized == true));
							this.Controllers = new ObservableCollection<Controller>(uow.Controllers.OrderBy(s => s.Name));
							this.Buyers = new ObservableCollection<Buyer>(uow.Buyers.OrderBy(s => s.Passport));
							this.Agreements = new ObservableCollection<Agreement>(query);

						}
						else
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
			if (this.CurrentAgreement != null)
				return true;
			return false;
		}

		private bool CanSaveCommand()
		{
			if (this.CurrentAgreement == null)
				return false;
			if ((this.CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.Modified ||
				this.CurrentAgreement.EntityState == Mindscape.LightSpeed.EntityState.New)
				&& CurrentAgreement.IsValid)
				return true;
			return false;
		}

		private bool CanFilterCommand()
		{
			return true;
		}

		#endregion // Methods
	}
}
