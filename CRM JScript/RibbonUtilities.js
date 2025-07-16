function LaunchModalDialog(dialogID, typeName, recordId, reload){
//Load Modal
var serverUri = Mscrm.CrmUri.create('/cs/dialog/rundialog.aspx');

window.showModalDialog(serverUri + '?DialogId=' + dialogID + '&EntityName=' + typeName + '&ObjectId=' + recordId, null, 'width=615,height=480, resizable=1, status=1, scrollbars=1');

//Reload form
if (reload == true)
{
window.location.reload(true);
}
else
{
     DoExtraFormWork();
}
}

function DoExtraFormWork()
{}