
// REMARQUE : Le code généré peut nécessiter au moins .NET Framework 4.5 ou .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public class LinedataServicesLicenseInformation
{

    private string clientStringField;

    private LinedataServicesLicenseInformationProductKeyTouple[] productKeyCollectionField;

    /// <remarks/>
    public string ClientString
    {
        get
        {
            return this.clientStringField;
        }
        set
        {
            this.clientStringField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("ProductKeyTouple", IsNullable = false)]
    public LinedataServicesLicenseInformationProductKeyTouple[] ProductKeyCollection
    {
        get
        {
            return this.productKeyCollectionField;
        }
        set
        {
            this.productKeyCollectionField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public class LinedataServicesLicenseInformationProductKeyTouple
{

    private string productField;

    private string keyField;

    /// <remarks/>
    public string Product
    {
        get
        {
            return this.productField;
        }
        set
        {
            this.productField = value;
        }
    }

    /// <remarks/>
    public string Key
    {
        get
        {
            return this.keyField;
        }
        set
        {
            this.keyField = value;
        }
    }
}

