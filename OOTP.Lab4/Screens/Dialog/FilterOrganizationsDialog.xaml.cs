﻿using OOTP.Lab4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOTP.Lab4.Screens
{
	/// <summary>
	/// Interaction logic for FilterOrganizationsDialog.xaml
	/// </summary>
	public partial class FilterOrganizationsDialog : Window
	{
		public Filter_ViewModel ExternalViewModel { get; set; }
		public Organization_ViewModel ExternalOrganizationViewModel { get; set; }

		public FilterOrganizationsDialog()
		{
			InitializeComponent();
			this.Loaded += (s, args) =>
				{
					this.DataContext = ExternalViewModel;
				};
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ExternalViewModel.Clear();
			ExternalOrganizationViewModel.RefreshCommand.Execute(null);
			this.DialogResult = false;
		}
	}
}
