using System;

using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace OOTP.Lab4.Data
{
  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table("Organizations", IdColumnName="OrganizationId", IdentityMethod=IdentityMethod.IdentityColumn)]
  public partial class Organization : Entity<int>
  {
    #region Fields
  
    [ValidateLength(0, 100)]
    private string _name;
    [ValidateLength(0, 200)]
    private string _description;
    [ValidateLength(0, 200)]
    private string _address;
    [ValidateLength(0, 200)]
    private string _legalAddress;
    [ValidateLength(17, 17)]
    private string _telephone;
    private int _profit;
    private int _staff;
    private double _totalArea;
    private bool _isPrivatized;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Name entity attribute.</summary>
    public const string NameField = "Name";
    /// <summary>Identifies the Description entity attribute.</summary>
    public const string DescriptionField = "Description";
    /// <summary>Identifies the Address entity attribute.</summary>
    public const string AddressField = "Address";
    /// <summary>Identifies the LegalAddress entity attribute.</summary>
    public const string LegalAddressField = "LegalAddress";
    /// <summary>Identifies the Telephone entity attribute.</summary>
    public const string TelephoneField = "Telephone";
    /// <summary>Identifies the Profit entity attribute.</summary>
    public const string ProfitField = "Profit";
    /// <summary>Identifies the Staff entity attribute.</summary>
    public const string StaffField = "Staff";
    /// <summary>Identifies the TotalArea entity attribute.</summary>
    public const string TotalAreaField = "TotalArea";
    /// <summary>Identifies the IsPrivatized entity attribute.</summary>
    public const string IsPrivatizedField = "IsPrivatized";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Organization")]
    private readonly EntityCollection<Agreement> _agreements = new EntityCollection<Agreement>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<Agreement> Agreements
    {
      get { return Get(_agreements); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public string Name
    {
      get { return Get(ref _name, "Name"); }
      set { Set(ref _name, value, "Name"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Description
    {
      get { return Get(ref _description, "Description"); }
      set { Set(ref _description, value, "Description"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Address
    {
      get { return Get(ref _address, "Address"); }
      set { Set(ref _address, value, "Address"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string LegalAddress
    {
      get { return Get(ref _legalAddress, "LegalAddress"); }
      set { Set(ref _legalAddress, value, "LegalAddress"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Telephone
    {
      get { return Get(ref _telephone, "Telephone"); }
      set { Set(ref _telephone, value, "Telephone"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public int Profit
    {
      get { return Get(ref _profit, "Profit"); }
      set { Set(ref _profit, value, "Profit"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public int Staff
    {
      get { return Get(ref _staff, "Staff"); }
      set { Set(ref _staff, value, "Staff"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public double TotalArea
    {
      get { return Get(ref _totalArea, "TotalArea"); }
      set { Set(ref _totalArea, value, "TotalArea"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public bool IsPrivatized
    {
      get { return Get(ref _isPrivatized, "IsPrivatized"); }
      set { Set(ref _isPrivatized, value, "IsPrivatized"); }
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table("Buyers", IdColumnName="BuyerId", IdentityMethod=IdentityMethod.IdentityColumn)]
  public partial class Buyer : Entity<int>
  {
    #region Fields
  
    [ValidateLength(0, 50)]
    private string _firstName;
    [ValidateLength(0, 50)]
    private string _lastName;
    [ValidateLength(0, 50)]
    private string _middleName;
    [ValidateLength(8, 8)]
    private string _passport;
    [ValidateLength(10, 10)]
    private string _inn;
    [ValidateLength(0, 200)]
    private string _address;
    [ValidateLength(17, 17)]
    private string _telephone;
    private System.DateTime _birthDay;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the FirstName entity attribute.</summary>
    public const string FirstNameField = "FirstName";
    /// <summary>Identifies the LastName entity attribute.</summary>
    public const string LastNameField = "LastName";
    /// <summary>Identifies the MiddleName entity attribute.</summary>
    public const string MiddleNameField = "MiddleName";
    /// <summary>Identifies the Passport entity attribute.</summary>
    public const string PassportField = "Passport";
    /// <summary>Identifies the Inn entity attribute.</summary>
    public const string InnField = "Inn";
    /// <summary>Identifies the Address entity attribute.</summary>
    public const string AddressField = "Address";
    /// <summary>Identifies the Telephone entity attribute.</summary>
    public const string TelephoneField = "Telephone";
    /// <summary>Identifies the BirthDay entity attribute.</summary>
    public const string BirthDayField = "BirthDay";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Buyer")]
    private readonly EntityCollection<Agreement> _agreements = new EntityCollection<Agreement>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<Agreement> Agreements
    {
      get { return Get(_agreements); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public string FirstName
    {
      get { return Get(ref _firstName, "FirstName"); }
      set { Set(ref _firstName, value, "FirstName"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string LastName
    {
      get { return Get(ref _lastName, "LastName"); }
      set { Set(ref _lastName, value, "LastName"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string MiddleName
    {
      get { return Get(ref _middleName, "MiddleName"); }
      set { Set(ref _middleName, value, "MiddleName"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Passport
    {
      get { return Get(ref _passport, "Passport"); }
      set { Set(ref _passport, value, "Passport"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Inn
    {
      get { return Get(ref _inn, "Inn"); }
      set { Set(ref _inn, value, "Inn"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Address
    {
      get { return Get(ref _address, "Address"); }
      set { Set(ref _address, value, "Address"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Telephone
    {
      get { return Get(ref _telephone, "Telephone"); }
      set { Set(ref _telephone, value, "Telephone"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime BirthDay
    {
      get { return Get(ref _birthDay, "BirthDay"); }
      set { Set(ref _birthDay, value, "BirthDay"); }
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table("Controllers", IdColumnName="ControllerId", IdentityMethod=IdentityMethod.IdentityColumn)]
  public partial class Controller : Entity<int>
  {
    #region Fields
  
    [ValidateLength(0, 100)]
    private string _name;
    [ValidateLength(10, 10)]
    private string _license;
    [ValidateLength(0, 200)]
    private string _address;
    [ValidateLength(0, 17)]
    private string _telephone;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Name entity attribute.</summary>
    public const string NameField = "Name";
    /// <summary>Identifies the License entity attribute.</summary>
    public const string LicenseField = "License";
    /// <summary>Identifies the Address entity attribute.</summary>
    public const string AddressField = "Address";
    /// <summary>Identifies the Telephone entity attribute.</summary>
    public const string TelephoneField = "Telephone";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Controller")]
    private readonly EntityCollection<Agreement> _agreements = new EntityCollection<Agreement>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public EntityCollection<Agreement> Agreements
    {
      get { return Get(_agreements); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public string Name
    {
      get { return Get(ref _name, "Name"); }
      set { Set(ref _name, value, "Name"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string License
    {
      get { return Get(ref _license, "License"); }
      set { Set(ref _license, value, "License"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Address
    {
      get { return Get(ref _address, "Address"); }
      set { Set(ref _address, value, "Address"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public string Telephone
    {
      get { return Get(ref _telephone, "Telephone"); }
      set { Set(ref _telephone, value, "Telephone"); }
    }

    #endregion
  }


  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [System.ComponentModel.DataObject]
  [Table("Agreements", IdColumnName="AgreementId", IdentityMethod=IdentityMethod.IdentityColumn)]
  public partial class Agreement : Entity<int>
  {
    #region Fields
  
    [ValidateLength(10, 10)]
    private string _number;
    private System.DateTime _date;
    private int _organizationId;
    private int _buyerId;
    private int _controllerId;

    #endregion
    
    #region Field attribute and view names
    
    /// <summary>Identifies the Number entity attribute.</summary>
    public const string NumberField = "Number";
    /// <summary>Identifies the Date entity attribute.</summary>
    public const string DateField = "Date";
    /// <summary>Identifies the OrganizationId entity attribute.</summary>
    public const string OrganizationIdField = "OrganizationId";
    /// <summary>Identifies the BuyerId entity attribute.</summary>
    public const string BuyerIdField = "BuyerId";
    /// <summary>Identifies the ControllerId entity attribute.</summary>
    public const string ControllerIdField = "ControllerId";


    #endregion
    
    #region Relationships

    [ReverseAssociation("Agreements")]
    private readonly EntityHolder<Organization> _organization = new EntityHolder<Organization>();
    [ReverseAssociation("Agreements")]
    private readonly EntityHolder<Buyer> _buyer = new EntityHolder<Buyer>();
    [ReverseAssociation("Agreements")]
    private readonly EntityHolder<Controller> _controller = new EntityHolder<Controller>();


    #endregion
    
    #region Properties

    [System.Diagnostics.DebuggerNonUserCode]
    public Organization Organization
    {
      get { return Get(_organization); }
      set { Set(_organization, value); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public Buyer Buyer
    {
      get { return Get(_buyer); }
      set { Set(_buyer, value); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public Controller Controller
    {
      get { return Get(_controller); }
      set { Set(_controller, value); }
    }


    [System.Diagnostics.DebuggerNonUserCode]
    public string Number
    {
      get { return Get(ref _number, "Number"); }
      set { Set(ref _number, value, "Number"); }
    }

    [System.Diagnostics.DebuggerNonUserCode]
    public System.DateTime Date
    {
      get { return Get(ref _date, "Date"); }
      set { Set(ref _date, value, "Date"); }
    }

    /// <summary>Gets or sets the ID for the <see cref="Organization" /> property.</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public int OrganizationId
    {
      get { return Get(ref _organizationId, "OrganizationId"); }
      set { Set(ref _organizationId, value, "OrganizationId"); }
    }

    /// <summary>Gets or sets the ID for the <see cref="Buyer" /> property.</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public int BuyerId
    {
      get { return Get(ref _buyerId, "BuyerId"); }
      set { Set(ref _buyerId, value, "BuyerId"); }
    }

    /// <summary>Gets or sets the ID for the <see cref="Controller" /> property.</summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public int ControllerId
    {
      get { return Get(ref _controllerId, "ControllerId"); }
      set { Set(ref _controllerId, value, "ControllerId"); }
    }

    #endregion
  }




  /// <summary>
  /// Provides a strong-typed unit of work for working with the Model model.
  /// </summary>
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  public partial class ModelUnitOfWork : Mindscape.LightSpeed.UnitOfWork
  {

    public System.Linq.IQueryable<Organization> Organizations
    {
      get { return this.Query<Organization>(); }
    }
    
    public System.Linq.IQueryable<Buyer> Buyers
    {
      get { return this.Query<Buyer>(); }
    }
    
    public System.Linq.IQueryable<Controller> Controllers
    {
      get { return this.Query<Controller>(); }
    }
    
    public System.Linq.IQueryable<Agreement> Agreements
    {
      get { return this.Query<Agreement>(); }
    }
    
  }

}
