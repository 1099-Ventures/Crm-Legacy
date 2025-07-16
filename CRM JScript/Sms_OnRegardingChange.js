function onRegardingChange()
{
	console.log('entering onRegardingChange');
	var regarding = Xrm.Page.getAttribute('regardingobjectid').getValue();
	console.log(regarding);

	if (regarding != null)
	{
		//		_oService = new FetchUtil(_sOrgName, _sServerUrl);

		if (Xrm.Page.getAttribute('regardingobjectid').getValue() != null)
		{
			var regardingId = Xrm.Page.getAttribute('regardingobjectid').getValue()[0].id;

			if (regarding[0].entityType == "lead")
			{
				console.log('Lead');
				GetEntityById("Lead", regardingId, function (regObj)
				{
					Xrm.Page.getAttribute("azuro_mobilephone").setValue(regObj.MobilePhone);
				});
			}
			else if (regarding[0].entityType == "account")
			{
				console.log('account');
				GetEntityById("Account", regardingId, function (regObj)
				{
					Xrm.Page.getAttribute("azuro_mobilephone").setValue(regObj.Telephone1);
				});
			}
			else if (regarding[0].entityType == "contact")
			{
				console.log('contact');
				GetEntityById("Contact", regardingId, function (regObj)
				{
					Xrm.Page.getAttribute("azuro_mobilephone").setValue(regObj.MobilePhone);
				});
			}
		}
	}

}