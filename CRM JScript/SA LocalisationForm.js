function InitForm()
{
  var countryCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Country", "azuro_country", null, "azuro_country", "azuro_countryId", "azuro_country_name", null, null);
  var provinceCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Province", "azuro_province", null, "azuro_province", "azuro_provinceId", "azuro_province_name", countryCtrl, "azuro_countryId");
  var cityCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_City", "azuro_city", null, "azuro_city", "azuro_cityId", "azuro_city_name", provinceCtrl, "azuro_provinceId");
  var suburbCtrl = new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Suburb", "azuro_suburb", null, "azuro_suburb", "azuro_suburbId", "azuro_suburb_name", cityCtrl, "azuro_cityId");
  var postalCodeCtrl = new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_PostalCode", "azuro_suburb", null, "azuro_suburb", "azuro_suburbId", "azuro_postalCode", cityCtrl, "azuro_cityId");
  countryCtrl.Reload();
}

function InitFormValues()
{
  var countryCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Country", null, "address1_country", "azuro_country", "azuro_countryId", "azuro_country_name", null, null);
  var provinceCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Province", null, "address1_stateorprovince", "azuro_province", "azuro_provinceId", "azuro_province_name", countryCtrl, "azuro_country");
  var cityCtrl =  new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_City", null, "address1_city", "azuro_city", "azuro_cityId", "azuro_city_name", provinceCtrl, "azuro_province");
  var suburbCtrl = new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_Suburb", null, "address1_line3", "azuro_suburb", "azuro_suburbId", "azuro_suburb_name", cityCtrl, "azuro_city");
  var postalCodeCtrl = new Azuro.DynamicsCRM.ClientHelper.Controls.DropDownControl("WebResource_PostalCode", null, "address1_postalcode", "azuro_suburb", "azuro_suburbId", "azuro_postalCode", cityCtrl, "azuro_city");
  countryCtrl.Reload();
}