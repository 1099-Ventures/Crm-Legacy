var messagesAllowed = 0;
var charsAllowed = 0;

function onIframeReady(iframe, callback) {
	if (iframe.contentWindow && iframe.contentWindow.document.readyState === 'complete' && iframe.contentWindow.document.getElementById('smsmessagecounterlabel') != null) {
		console.log('iframeDoc is ready');
		callback();
	} else {
		console.log('adding event listener');
		iframe.addEventListener('load', callback);
	}
}

function OnLoad() {
	if (Xrm.Page.ui.getFormType() == formtype_create) {
		console.log('on create');
		onRegardingChange();
	}
	initFields();
}

function initFields() {
	fetchOnLoad();
}

function setLabelText(iframe, message) {
	console.log('entering setLabelText');
	if (iframe == null) {
		alert('frame is null');
		return false;
	}
	if (message == null) {
		alert('message is null');
		return false;
	}

	onIframeReady(iframe, function () {
		console.log('on ready');
		console.log(iframe);
		var iframeDocument = iframe.contentDocument ? iframe.contentDocument : iframe.contentWindow.document;
		console.log(iframeDocument);
		var label = iframeDocument.getElementById('smsmessagecounterlabel');
		console.log(label);
		if (label == null) {
			console.log('label is null');
			return false;
		}
		var charCount = message.title.length;
		var messageCount = Math.floor(charCount / 160) + 1;
		label.innerHTML = "Characters:&nbsp;&nbsp;" + charCount + "/ " + charsAllowed + "&nbsp;&nbsp;Messages:&nbsp;&nbsp;" + messageCount + "/" + messagesAllowed;
		label.style.color = charCount > charsAllowed ? "#ff0000" : "";
	});

	return true;
}

function fetchOnLoad() {
	SDK.REST.retrieveMultipleRecords(
	   'azuro_smsconfiguration',
	   '$select=azuro_smsconfigurationId,azuro_AllowMultiPartMessages,azuro_SmsTriggerEventQueue,CreatedOn&$top=1',
	   function (results) {
	   	console.log(results);
	   	if (results != null && results[0] != null && results[0].azuro_AllowMultiPartMessages != null) {
	   		messagesAllowed = results[0].azuro_AllowMultiPartMessages.Value;
	   		if (messagesAllowed != "None") {
	   			messagesAllowed -= 100000000;
	   			console.log('messagesAllowed: ' + messagesAllowed);
	   			charsAllowed = (messagesAllowed * 160);
	   		} else
	   			charsAllowed = (1 * 160);
	   	}
	   	else {
	   		charsAllowed = (1 * 160);
	   		messagesAllowed = 1;
	   	}
	   	console.log('charsAllowed: ' + charsAllowed);

	   	var messageOuter = window.parent.document.getElementById('azuro_message');
	   	var message = window.parent.document.getElementById('Message_label');
	   	var frame = window.parent.document.getElementById('WebResource_smsmessagecounters');
	   	if (!setLabelText(frame, message)) {
	   		console.log('backup');
	   		frame.addEventListener("load", function () { setLabelText(frame, message); });
	   	}
	   },
		errorHandler,
     function () {
     	//OnComplete handler
     }
	 );
}

//onChange event being called by form events because CRM
function MessageChange() {
	var message = window.parent.document.getElementById('Message_label');
	var frame = window.parent.document.getElementById('WebResource_smsmessagecounters');
	setLabelText(frame, message);
}

function DoSend() {
	Xrm.Page.getAttribute("azuro_status").setValue(100000003);
	Xrm.Page.data.entity.save("saveandclose");
}

