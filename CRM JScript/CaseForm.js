//_oService = new FetchUtil(_sOrgName, _sServerUrl);

function OnLoad()
{
	//var reason = Xrm.Page.ui.controls.get("abs_reasonforassignment");
	//reason.setDisabled(true);

	//var reassignedby = Xrm.Page.ui.controls.get("azuro_reassignedby");
	//reassignedby.setDisabled(true);

	//var reassigneddate = Xrm.Page.ui.controls.get("azuro_reassigneddate");
	//reassigneddate.setDisabled(true);

	Xrm.Page.getAttribute("contractid").setRequiredLevel("required");
	Xrm.Page.getAttribute("contractdetailid").setRequiredLevel("required");
	Xrm.Page.getAttribute("productid").setRequiredLevel("required");

	//if (Xrm.Page.ui.getFormType() == 2)
	//{
	//	//Xrm.Page.getAttribute("abs_feedbacktoclient").setRequiredLevel("required");
	//	Xrm.Page.getAttribute("azuro_escalationdate").setRequiredLevel("recommended");
	//}

	//var feedback = Xrm.Page.ui.controls.get("abs_feedbacktoclient");
	//feedback.setDisabled(true);

	//var escalate = Xrm.Page.ui.controls.get("azuro_escalationdate");
	//escalate.setDisabled(true);
}

function OnChangeContractLine()
{
	_oService = new FetchUtil(_sOrgName, _sServerUrl);

	var contractdetail = Xrm.Page.getAttribute("contractdetailid").getValue();
	if (contractdetail != null)
	{
		lineItem = GetContractLineById(contractdetail[0].id);
		if (lineItem != null)
		{
			var lookupData = new Array();
			var lookupItem = new Object();
			lookupItem.id = lineItem.productid
			lookupItem.typename = 'product';
			lookupItem.name = lineItem.productname;
			lookupData[0] = lookupItem;

			Xrm.Page.getAttribute("productid").setValue(lookupData);
		}
	}
}