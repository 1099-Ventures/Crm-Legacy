function GetContactById(id)
{
	var sFetch = "<fetch mapping='logical' count='1'>" +
	"<entity name='contact'>" +
	"<attribute name='telephone1' />" +
	"<attribute name='telephone2' />" +
	"<attribute name='telephone3' />" +
	"<attribute name='mobilephone' />" +
	"<filter type='and'>" +
	"<condition attribute = 'contactid' operator='eq' value='" + id + "'/>" +
	"</filter>" +
	"</entity>" +
	"</fetch>";

	var fetchResult = _oService.Fetch(sFetch, null);
	var result = new Contact();

	if (fetchResult.results != null)
	{
		if (fetchResult.results[0].attributes["telephone1"] != null)
			result.telephone1 = fetchResult.results[0].attributes["telephone1"].value;

		if (fetchResult.results[0].attributes["telephone2"] != null)
			result.telephone2 = fetchResult.results[0].attributes["telephone2"].value;

		if (fetchResult.results[0].attributes["telephone3"] != null)
			result.telephone3 = fetchResult.results[0].attributes["telephone3"].value;

		if (fetchResult.results[0].attributes["mobilephone"] != null)
			result.mobilephone = fetchResult.results[0].attributes["mobilephone"].value;

		return result;
	}
}

function Contact()
{
	this.telephone1 = '';
	this.telephone2 = '';
	this.telephone3 = '';
	this.mobilephone = '';
}