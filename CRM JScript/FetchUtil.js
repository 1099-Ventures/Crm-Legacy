// Company: Azuro Business Solutions : http://www.azuro.co.za/ - support@azuro.co.za
// Support IE 6, 7, 8, 9, 10 and Chrome 25+, Mozilla firefox 19+, Safari 5+ and Opera ?
/* Changes to FetchXml after RU12 Solaris went Multi-browser and 
	stopped using XMLDomCollections and Now uses HTMLCollections on xmlhttp.responseXML
*/

var XMLHTTPSUCCESS = 200;
var XMLHTTPREADY = 4;
function FetchUtil(sOrg, sServer)
{
	this.org = sOrg;
	this.server = sServer;
	if (sOrg == null)
	{
		if (typeof (ORG_UNIQUE_NAME) != "undefined")
		{
			this.org = ORG_UNIQUE_NAME;
		}
	}
	if (sServer == null)
	{
		this.server = window.location.protocol + "//" + window.location.host;
	}
}

FetchUtil.prototype._ExecuteRequest = function (sXml, sMessage, fInternalCallback, fUserCallback)
{
	var xmlhttp = new XMLHttpRequest();
	xmlhttp.open("POST", this.server + "/XRMServices/2011/Organization.svc/web", (fUserCallback != null));
	xmlhttp.setRequestHeader("Accept", "application/xml, text/xml, */*");
	xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
	xmlhttp.setRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");

	if (fUserCallback != null)
	{
		//asynchronous: register callback function, then send the request.
		var crmServiceObject = this;
		xmlhttp.onreadystatechange = function ()
		{
			fInternalCallback.call(crmServiceObject, xmlhttp, fUserCallback)
		};
		xmlhttp.send(sXml)
	}
	else
	{
		//synchronous: send request, then call the callback function directly
		xmlhttp.send(sXml);
		return fInternalCallback.call(this, xmlhttp, null);
	}
}

FetchUtil.prototype._HandleErrors = function (xmlhttp)
{
	/// <summary>(private) Handles xmlhttp errors</summary>
	if (xmlhttp.status != XMLHTTPSUCCESS)
	{
		var sError = "Error: " + xmlhttp.responseText + " " + xmlhttp.statusText;
		alert(sError);
		return true;
	} else
	{
		return false;
	}
}

FetchUtil.prototype.Fetch = function (sFetchXml, fCallback)
{
	/// <summary>Execute a FetchXml request. (result is the response XML)</summary>
	/// <param name="sFetchXml">fetchxml string</param>
	/// <param name="fCallback" optional="true" type="function">(Optional) Async callback function if specified. If left null, function is synchronous </param>

	var request = "<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>";
	request += "<s:Body>";
	request += "<Execute xmlns='http://schemas.microsoft.com/xrm/2011/Contracts/Services'>" +
					"<request i:type='b:RetrieveMultipleRequest' " +
					" xmlns:b='http://schemas.microsoft.com/xrm/2011/Contracts' " +
					" xmlns:i='http://www.w3.org/2001/XMLSchema-instance'>" +
						"<b:Parameters xmlns:c='http://schemas.datacontract.org/2004/07/System.Collections.Generic'>" +
							"<b:KeyValuePairOfstringanyType>" +
								"<c:key>Query</c:key>" +
								"<c:value i:type='b:FetchExpression'>" +
								   "<b:Query>";

	request += CrmEncodeDecode.CrmXmlEncode(sFetchXml);
	request += "</b:Query>" +
			"</c:value>" +
		"</b:KeyValuePairOfstringanyType>" +
	"</b:Parameters>" +
	"<b:RequestId i:nil='true'/>" +
	"<b:RequestName>RetrieveMultiple</b:RequestName>" +
"</request>" +
"</Execute>";
	request += "</s:Body></s:Envelope>";
	return this._ExecuteRequest(request, "Fetch", this._FetchCallback, fCallback);
}

FetchUtil.prototype._FetchCallback = function (xmlhttp, callback)
{
	///<summary>(private) Fetch message callback.</summary>
	//xmlhttp must be completed
	if (xmlhttp.readyState != XMLHTTPREADY)
	{
		return;
	}
	//check for server errors 
	if (this._HandleErrors(xmlhttp))
	{
		return;
	}
	var fetchResult;
	//WhichVersionAmI
	ieBrowserVersion = WhichVersionAmI();
	if (ieBrowserVersion > 0 && ieBrowserVersion < 10) fetchResult = getOldResponseXML(xmlhttp);
	else fetchResult = getNewResponseXML(xmlhttp);

	//return entity id if sync, or call user callback func if async
	if (callback != null) callback(fetchResult);
	else return fetchResult;
}

// use new HTMLCollection
function getNewResponseXML(xmlhttp)
{
	//var xmlReturn = List of returned Entities 
	var xmlReturn = xmlhttp.responseXML.getElementsByTagName("a:Entity");
	//xmlhttp.responseXML.getElementsByTagName("a:Entity")
	var count = xmlhttp.responseXML.getElementsByTagName("a:TotalRecordCount")[0].textContent;

	//parse result xml into array of jsDynamicEntity objects
	var results = new Array(xmlReturn.length);
	var fetchResult = new FetchResult(xmlReturn.length);
	for (var i = 0; i < xmlReturn.length; i++)
	{
		//inside Entity, there's =  "Attributes","EntityState","FormattedValues","Id","LogicalName","RelatedEntities".
		var oResultNode = xmlReturn[i];
		var jDE = new jsDynamicEntity();
		var obj = new Object();

		for (var j = 0; j < oResultNode.childElementCount; j++)
		{
			switch (oResultNode.childNodes[j].localName)
			{
				case "Attributes":
					{
						//inside Attributes, there's =  "KeyValuePairOfstringanyType" only.
						var attr = oResultNode.childNodes[j];

						for (var k = 0; k < attr.childElementCount; k++)
						{
							//Check the node: it should be KeyValuePairOfstringanyType
							if (attr.childNodes[k].localName != "KeyValuePairOfstringanyType")
							{
								alert("attr.childNodes[k].localName: " + attr.childNodes[k].localName);
								return;
							}
							//Inside KeyValuePairOfstringanyType, there's =  "key","value".
							var KeyValuePairOfstringanyType = attr.childNodes[k];
							var sType = "";

							// Determine the Type of Attribute value we should expect
							for (var l = 0; l < attr.childNodes[k].childElementCount; l++) //KeyValuePairOfstringanyType
							{
								var sType = KeyValuePairOfstringanyType.childNodes[l].localName;

								switch (sType)
								{
									case "key":
										{
											sKey = KeyValuePairOfstringanyType.childNodes[l].childNodes[0].textContent;
											break;
										}
									case "value":
										{
											var nodeOfValue = KeyValuePairOfstringanyType.childNodes[l];
											var typeOfValue = null;
											//Multiple attributes
											for (var atribLength = 0; atribLength < nodeOfValue.attributes.length; atribLength++)
											{
												if (nodeOfValue.attributes[atribLength].localName == "type")
													typeOfValue = nodeOfValue.attributes[atribLength].value;
											}
											//if not attribute then do not save: Very harsh
											if (typeOfValue == null || typeOfValue == '') break;


											switch (typeOfValue) // Check the type of value this is...
											{
												case "a:OptionSetValue":
													{
														var entOSV = new jsOptionSetValue();
														entOSV.type = sType;
														entOSV.value = nodeOfValue.childNodes[0].textContent;
														obj[sKey] = entOSV;
														break;
													}
												case "a:EntityReference":
													{
														var entRef = new jsEntityReference();
														entRef.type = sType;
														entRef.guid = nodeOfValue.childNodes[0].textContent;
														entRef.logicalName = nodeOfValue.childNodes[1].textContent;
														entRef.name = nodeOfValue.childNodes[2].textContent;
														obj[sKey] = entRef;
														break;
													}
												default:
													{
														var entCV = new jsCrmValue();
														entCV.type = sType;
														entCV.value = nodeOfValue.textContent;
														obj[sKey] = entCV;
														break;
													}
											}
											break;
										}
									default:
										{
											var entCV = null;

											break;
										}
								}
							}
						}

						jDE.attributes = obj;
						break;
					}
				case "Id":
					{
						jDE.guid = oResultNode.childNodes[j].textContent;
						break;
					}
				case "LogicalName":
					{
						jDE.logicalName = oResultNode.childNodes[j].textContent;
						break;
					}
				case "FormattedValues": // Needs Testing ??
					{
						var foVal = oResultNode.childNodes[j];

						for (var k = 0; k < foVal.childElementCount; k++)
						{
							// Establish the Key, we are going to fill in the formatted value of the already found attribute
							var sKey = foVal.childNodes[k].firstChild.textContent;
							jDE.attributes[sKey].formattedValue = foVal.childNodes[k].childNodes[1].textContent;
						}
						break;
					}
			}
		}
		jDE.count = count;
		fetchResult.results[i] = jDE;
	}
	//Formalities for supporting's sake
	fetchResult.count = results.length;
	//return entity
	return fetchResult;
}

//Uses ActiveXObject with IXMLDOMCollection
function getOldResponseXML(xmlhttp)
{
	var sFetchResult = xmlhttp.responseXML.selectSingleNode("//a:Entities").xml;
	var resultDoc = new ActiveXObject("Microsoft.XMLDOM");
	resultDoc.async = false;
	resultDoc.loadXML(sFetchResult);
	//parse result xml into array of jsDynamicEntity objects
	var fetchResult = new FetchResult(resultDoc.firstChild.childNodes.length);

	for (var i = 0; i < resultDoc.firstChild.childNodes.length; i++)
	{
		var oResultNode = resultDoc.firstChild.childNodes[i];
		var jDE = new jsDynamicEntity();
		var obj = new Object();
		for (var j = 0; j < oResultNode.childNodes.length; j++)
		{

			switch (oResultNode.childNodes[j].baseName)
			{
				case "Attributes":
					var attr = oResultNode.childNodes[j];
					for (var k = 0; k < attr.childNodes.length; k++)
					{
						// Establish the Key for the Attribute
						var sKey = attr.childNodes[k].firstChild.text;
						var sType = "";
						// Determine the Type of Attribute value we should expect
						for (var l = 0; l < attr.childNodes[k].childNodes[1].attributes.length; l++)
						{
							if (attr.childNodes[k].childNodes[1].attributes[l].baseName == 'type')
							{
								sType = attr.childNodes[k].childNodes[1].attributes[l].text;
							}
						}

						switch (sType)
						{
							case "a:OptionSetValue":
								var entOSV = new jsOptionSetValue();
								entOSV.type = sType;
								entOSV.value = attr.childNodes[k].childNodes[1].text;
								obj[sKey] = entOSV;
								break;
							case "a:EntityReference":
								var entRef = new jsEntityReference();
								entRef.type = sType;
								entRef.guid = attr.childNodes[k].childNodes[1].childNodes[0].text;
								entRef.logicalName = attr.childNodes[k].childNodes[1].childNodes[1].text;
								entRef.name = attr.childNodes[k].childNodes[1].childNodes[2].text;
								obj[sKey] = entRef;
								break;
							default:
								var entCV = new jsCrmValue();
								entCV.type = sType;
								entCV.value = attr.childNodes[k].childNodes[1].text;
								obj[sKey] = entCV;
								break;
						}
					}

					jDE.attributes = obj;
					jDE.count = xmlhttp.responseXML.selectSingleNode("//a:TotalRecordCount").text;
					fetchResult.count = jDE.count;

					break;
				case "Id":
					jDE.guid = oResultNode.childNodes[j].text;
					break;
				case "LogicalName":
					jDE.logicalName = oResultNode.childNodes[j].text;
					break;
				case "FormattedValues":
					var foVal = oResultNode.childNodes[j];
					for (var k = 0; k < foVal.childNodes.length; k++)
					{
						// Establish the Key, we are going to fill in the formatted value of the already found attribute
						var sKey = foVal.childNodes[k].firstChild.text;
						jDE.attributes[sKey].formattedValue = foVal.childNodes[k].childNodes[1].text;
					}
					break;
			}
		}
		fetchResult.results[i] = jDE;
	}
	//return entity
	return fetchResult;
}

function jsDynamicEntity(gID, sLogicalName)
{
	this.guid = gID;
	this.logicalName = sLogicalName;
	this.attributes = new Object();
	this.count = 0;
}

// This function returns Internet Explorer's major version number,
// or 0 for others. It works by finding the "MSIE " string and
// extracting the version number following the space, up to the decimal
// point, ignoring the minor version number
function WhichVersionAmI()
{
	var ua = window.navigator.userAgent
	var msie = ua.indexOf("MSIE ")

	if (msie > 0)      // If Internet Explorer, return version number
		return parseInt(ua.substring(msie + 5, ua.indexOf(".", msie)))
	else                 // If another browser, return 0
		return 0

}

function jsCrmValue(sType, sValue)
{
	this.type = sType;
	this.value = sValue;
}

function jsEntityReference(gID, sLogicalName, sName)
{
	this.guid = gID;
	this.logicalName = sLogicalName;
	this.name = sName;
	this.type = "EntityReference";
}

function FetchResult(resultsLength)
{
	this.results = new Array(resultsLength);
	this.count = 0;
}

function jsOptionSetValue(iValue, sFormattedValue)
{
	this.value = iValue;
	this.formattedValue = sFormattedValue;
	this.type = "OptionSetValue";
}