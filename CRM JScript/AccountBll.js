function GetAccountById(id)
{
	var sFetch = "<fetch mapping='logical' count='1'>" +
	"<entity name='account'>" +
	"<attribute name='telephone1' />" +
	"<attribute name='telephone2' />" +
	"<attribute name='telephone3' />" +
	"<filter type='and'>" +
	"<condition attribute = 'accountid' operator='eq' value='" + id + "'/>" +
	"</filter>" +
	"</entity>" +
	"</fetch>";

	var fetchResult = _oService.Fetch(sFetch, null);
	var result = new Account();

	if (fetchResult.results != null)
	{
		//mainphone
		if (fetchResult.results[0].attributes["telephone1"] != null)
			result.telephone1 = fetchResult.results[0].attributes["telephone1"].value;
		//otherphone
		if (fetchResult.results[0].attributes["telephone2"] != null)
			result.telephone2 = fetchResult.results[0].attributes["telephone2"].value;

		if (fetchResult.results[0].attributes["telephone3"] != null)
			result.telephone3 = fetchResult.results[0].attributes["telephone3"].value;

		return result;
	}
}

function Account()
{
	this.telephone1 = '';
	this.telephone2 = '';
	this.telephone3 = '';
}