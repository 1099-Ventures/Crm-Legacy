var Azuro = Azuro || {
    DynamicsCRM: {

        SDK: {

            _context: function () {
                if (typeof GetGlobalContext != "undefined")
                { return GetGlobalContext(); }
                else {
                    if (typeof Xrm != "undefined") {
                        return Xrm.Page.context;
                    }
                    else
                    { throw new Error("Context is not available."); }
                }
            },
            _getServerUrl: function () {
                var serverUrl = this._context().getServerUrl()
                if (serverUrl.match(/\/$/)) {
                    serverUrl = serverUrl.substring(0, serverUrl.length - 1);
                }
                return serverUrl;
            },
            _ODataPath: function () {
                return this._getServerUrl() + "/XRMServices/2011/OrganizationData.svc/";
            },
            _errorHandler: function (req) {
                return new Error("Error : " +
                    req.status + ": " +
                    req.statusText + ": " +
                    JSON.parse(req.responseText).error.message.value);
            },
            _dateReviver: function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a = /Date\(([-+]?\d+)\)/.exec(value);
                    if (a) {
                        return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
                    }
                }
                return value;
            },
            _parameterCheck: function (parameter, message) {
                if ((typeof parameter === "undefined") || parameter === null) {
                    throw new Error(message);
                }
            },
            _stringParameterCheck: function (parameter, message) {
                if (typeof parameter != "string") {
                    throw new Error(message);
                }
            },
            _callbackParameterCheck: function (callbackParameter, message) {
                if (typeof callbackParameter != "function") {
                    throw new Error(message);
                }
            },

            retrieveMultipleRecords: function (type, options, successCallback, errorCallback, OnComplete) {
                var optionsString = "";
                if (options != null) {
                    if (options.charAt(0) != "?") {
                        optionsString = "?" + options;
                    }
                    else
                    { optionsString = options; }
                }
                var req = new XMLHttpRequest();
                var s = this._ODataPath() + type + "Set" + optionsString;
                req.open("GET", s, true);
                req.setRequestHeader("Accept", "application/json");
                req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                req.onreadystatechange = function () {
                    if (this.readyState == 4 /* complete */) {
                        if (this.status == 200) {
                            var returned = JSON.parse(this.responseText, Azuro.DynamicsCRM.SDK._dateReviver).d;
                            successCallback(returned.results);
                            if (returned.__next != null) {
                                var queryOptions = returned.__next.substring((Azuro.DynamicsCRM.SDK._ODataPath() + type + "Set").length);
                                Azuro.DynamicsCRM.SDK.retrieveMultipleRecords(type, queryOptions, successCallback, errorCallback, OnComplete);
                            }
                            else
                            { OnComplete(); }
                        }
                        else {
                            errorCallback(Azuro.DynamicsCRM.SDK._errorHandler(this));
                        }
                    }
                };
                req.send();
            },

            retrieveRecord: function (id, type, select, expand, successCallback, errorCallback) {
                var systemQueryOptions = "";

                if (select != null || expand != null) {
                    systemQueryOptions = "?";
                    if (select != null) {
                        var selectString = "$select=" + select;
                        if (expand != null) {
                            selectString = selectString + "," + expand;
                        }
                        systemQueryOptions = systemQueryOptions + selectString;
                    }
                    if (expand != null) {
                        systemQueryOptions = systemQueryOptions + "&$expand=" + expand;
                    }
                }


                var req = new XMLHttpRequest();
                var url = this._ODataPath() + type + "Set(guid'" + id + "')" + systemQueryOptions;
                req.open("GET", url, true);
                req.setRequestHeader("Accept", "application/json");
                req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                //Has to be added for retrieve
                req.setRequestHeader("Pragma", "no-cache");
                req.onreadystatechange = function () {

                    if (this.readyState == 4 /* complete */) {
                        if (this.status == 200) {
                            successCallback(JSON.parse(this.responseText, Azuro.DynamicsCRM.SDK._dateReviver).d);
                        }
                        else {
                            errorCallback(Azuro.DynamicsCRM.SDK._errorHandler(this));
                        }
                    }
                };
                req.send();
            }
        },

        EntityWatcherOptions: {
            None: 0,
            Modified: 1,
            Deactivated: 2,
            Completed: 4,
            All: 7
        },


        FormType: {
            Undefined: 0,
            Create: 1,
            Update: 2,
            ReadOnly: 3,
            Disabled: 4,
            QuickCreate: 5,
            BulkEdit: 6
        },

        EntityUtils: {
            GetCurrentEntityName: function () {
                var logicalName = Xrm.Page.data.entity.getEntityName();
                return logicalName.charAt(0).toUpperCase() + logicalName.substring(1);
            },

            GetRESTId: function () {
                return Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");
            }
        },

        EntityWatcher: function (options, frequency) {

            if (typeof Azuro.DynamicsCRM.ClientHelper.EntityWatcher !== "undefined" &&
               Azuro.DynamicsCRM.ClientHelper.EntityWatcher != null) return Azuro.DynamicsCRM.ClientHelper.EntityWatcher;

            Azuro.DynamicsCRM.ClientHelper.EntityWatcher = this;

            var _instance = this;

            var _options = options;
            var _frequency = frequency;
            var _stopped = true;

            //var _uiLoadedCallbacks = new Array();
            //var _uiLoadedCallback = null;
            var _uiLoadedTimeInterval = null;

            var _modifiedOn = null;
            var _listeners = new Array();

            Azuro.DynamicsCRM.EntityWatcher.prototype.UILoaded = function () {
                var result = Xrm.Page.ui !== null;
                return result;
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.WaitUILoaded = function () {
                if (_uiLoadedTimeInterval === null) {
                    _uiLoadedTimeInterval = setInterval(function () { _instance.timerCheckUILoaded(); }, 1000);
                }
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.timerCheckUILoaded = function () {
                if (_instance.UILoaded()) {
                    clearInterval(_uiLoadedTimeInterval);
                    _uiLoadedTimeInterval = null;
                    _instance.OnUILoaded();
                }
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.AddListener = function (listener) {
                _listeners.push(listener);
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.Start = function () {
                _stopped = false;
                if (_instance.UILoaded()) {
                    _instance._start();
                }
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype._retrieveEntitySuccessCallback = function (entity) {
                if (_modifiedOn === null) _modifiedOn = entity.ModifiedOn;
                else {
                    if (_modifiedOn.getTime() != entity.ModifiedOn.getTime()) {
                        _modifiedOn = entity.ModifiedOn;
                        for (i in _listeners) {
                            _listeners[i](Azuro.DynamicsCRM.FormType.Update);
                        }

                        //_uiController.ShowNotification(_messages[Azuro.DynamicsCRM.FormType.Update], _actions[Azuro.DynamicsCRM.FormType.Update]);
                    }
                }
                _instance._start();
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype._retrieveEntityErrorCallback = function () {
                alert("Entity watcher error");
                //_uiController.ShowNotification("Error", null);
                _instance._start();
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.OnUILoaded = function () {
                //Read modified on
                Azuro.DynamicsCRM.SDK.retrieveRecord(
                       Azuro.DynamicsCRM.EntityUtils.GetRESTId(),
                       Azuro.DynamicsCRM.EntityUtils.GetCurrentEntityName(),
                       "ModifiedOn",
                       null,
                       _instance._retrieveEntitySuccessCallback,
                       _instance._retrieveEntityErrorCallback);
                if (!_stopped) _instance._start();
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.Stop = function () {
                _stopped = true;
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype.GetFormType = function () {
                return Xrm.Page.ui.getFormType();
            }

            //TODO
            //Check if timeout is set already(don't create another one then)
            Azuro.DynamicsCRM.EntityWatcher.prototype._start = function () {
                if (_instance.GetFormType() != Azuro.DynamicsCRM.FormType.Update) return;
                setTimeout(function () { _instance._timerTick(); }, _frequency);
            }

            Azuro.DynamicsCRM.EntityWatcher.prototype._timerTick = function () {
                if (_stopped) return;
                Azuro.DynamicsCRM.SDK.retrieveRecord(
                    Azuro.DynamicsCRM.EntityUtils.GetRESTId(),
                    Azuro.DynamicsCRM.EntityUtils.GetCurrentEntityName(),
                    "ModifiedOn",
                    null,
                    _instance._retrieveEntitySuccessCallback,
                    _instance._retrieveEntityErrorCallback);
            }
            _instance.WaitUILoaded();

        },

        HtmlNotification: function (backgroundCss, notificationCss, cssStyleClearAction, clearActionMsg, messages, actions) {

            if (typeof Azuro.DynamicsCRM.ClientHelper.HtmlNotification !== "undefined" &&
               Azuro.DynamicsCRM.ClientHelper.HtmlNotification != null) return Azuro.DynamicsCRM.ClientHelper.HtmlNotification;

            Azuro.DynamicsCRM.ClientHelper.HtmlNotification = this;

            var _messages = messages;
            var _actions = actions;
            if (_messages === null) {
                _messages = new Array();
                _messages[Azuro.DynamicsCRM.FormType.Update] = "This record has been updated. Click here to reload it.";
            }

            if (_actions === null) {
                _actions = new Array();
                _actions[Azuro.DynamicsCRM.FormType.Update] = function () { window.parent.location.reload(true); };
            }


            var _instance = this;
            var _notificationControl = null;
            var _cssStyle = "color:red";
            var _notificationElement = null;
            var _clearActionElement = null;

            var _notificationControlName = "crmFormHeader";
            var _cssStyleClearAction = "color:black";
            var _clearActionMsg = "Clear notification"
            var _backgroundCss = "background-color:white; height:25px; width:100%; border: 1px solid black";

            if (notificationCss !== null) _cssStyle = notificationCss;
            if (cssStyleClearAction !== null) _cssStyleClearAction = cssStyleClearAction;
            if (clearActionMsg !== null) _clearActionMsg = clearActionMsg;
            if (backgroundCss !== null) _backgroundCss = backgroundCss;


            Azuro.DynamicsCRM.HtmlNotification.prototype.getNotificationControl = function () {
                var _control = null;
                if (_notificationControl === null) {
                    _control = $('.ms-crm-Form-StandaloneSection');
                    _control.each(function () {
                        if ($(this).attr('id') == _notificationControlName) {
                            _notificationControl = $(this)[0];
                        }

                    });
                }
                return _notificationControl;
            }

            Azuro.DynamicsCRM.HtmlNotification.prototype.WatcherCallback = function (eventType) {
                _instance.ShowNotification(_messages[Azuro.DynamicsCRM.FormType.Update], _actions[Azuro.DynamicsCRM.FormType.Update]);
            }

            //ShowNotificationMessage function
            Azuro.DynamicsCRM.HtmlNotification.prototype.ShowNotification = function (msg, clickevent) {


                if (_notificationElement === null) {

                    _instance.getNotificationControl();
                    if (_notificationControl === null) return;
                    var colCount = 0;

                    $('#' + _notificationControlName + ' tr').each(function () {
                        var rowColCount = 0;
                        $(this).children('td').each(function () {
                            if ($(this).attr('colspan')) {
                                rowColCount += +$(this).attr('colspan');
                            } else {
                                rowColCount++;
                            }

                        });
                        if (rowColCount > colCount) colCount = rowColCount;
                    });

                    var rowCount = _notificationControl.rows.length;
                    var row = _notificationControl.insertRow(rowCount);
                    var _notificationCell = row.insertCell(0);
                    _notificationCell.colSpan = colCount;

                    _notificationElement = document.createElement('a');
                    _notificationCell.style.cssText = _backgroundCss;
                    _notificationElement.style.cssText = _cssStyle;
                    _notificationCell.appendChild(_notificationElement);
                    _clearActionElement = document.createElement('a');
                    _clearActionElement.style.cssText = _cssStyleClearAction;
                    _clearActionElement.title = _clearActionMsg;

                    _clearActionElement.innerHTML = _clearActionMsg;
                    _clearActionElement.attachEvent("onclick", function () { _instance.HideNotification(); });
                    _notificationCell.appendChild(_clearActionElement);


                    _notificationElement.title = msg;
                    _notificationElement.innerHTML = msg;
                    if (typeof clickevent === "undefined" || clickevent === null) {
                        _notificationElement.style.cursor = "auto";
                        //_notificationTextElement.nodeValue = msg;
                    }
                    else {
                        _notificationElement.style.cursor = "pointer";
                        _notificationElement.attachEvent("onclick", clickevent);
                    }
                }



            }

            //HideNotification function
            Azuro.DynamicsCRM.HtmlNotification.prototype.HideNotification = function () {
                if (_notificationControl === null) return;
                var rowCount = _notificationControl.rows.length;
                _notificationControl.deleteRow(rowCount - 1);
                _notificationElement = null;
            }



        },


        ClientHelper: {

            UINotifier: null,
            EntityWatcher: null,

            CreateHtmlHelper: function (startWatcher) {

                Azuro.DynamicsCRM.ClientHelper.UINotifier = new Azuro.DynamicsCRM.HtmlNotification("background-color:white;width:100%;padding:2px;height:25px;border: 1px solid black;",
                    "color:red;font-size:14px;font-weight:bold;",
                    "margin-left: 10px;cursor:pointer;text-decoration:underline;color:black;font-size:12px;font-weight:normal;background-color:white;",
                    "Clear notification..", null, null);
                Azuro.DynamicsCRM.ClientHelper.EntityWatcher = new Azuro.DynamicsCRM.EntityWatcher(Azuro.DynamicsCRM.EntityWatcherOptions.Modified,
                    10000);
                Azuro.DynamicsCRM.ClientHelper.EntityWatcher.AddListener(function () { Azuro.DynamicsCRM.ClientHelper.HtmlNotification.WatcherCallback(); });
                if (startWatcher) Azuro.DynamicsCRM.ClientHelper.Start();

                return Azuro.DynamicsCRM.ClientHelper;
            },


            Start: function () {
                Azuro.DynamicsCRM.ClientHelper.EntityWatcher.Start();
            },

            Controls: {

                DropDownControl: function (controlName, idField, valueField, entityName, refIdField, refValueField, masterControl, masterRefField) {

                    this._selectName = controlName + "ddn";
                    this._childControls = new Array();

                    this._idField = idField;
                    this._valueField = valueField;
                    this._refEntityName = entityName;
                    this._refIdField = refIdField;
                    this._refValueField = refValueField;
                    //var _selectedValue = refValueField;

                    this._ctrl = $("#" + controlName);
                    this._control = this._ctrl.get(0).parentNode;

                    this._control.innerHTML = "<select style='width:100%' id=" + this._selectName + "></select>";

                    this._controlSelect = $('#' + this._selectName);

                    this._masterControl = masterControl;
                    this._masterRefField = masterRefField;

                    var _instance = this;

                    if (typeof this._masterControl !== "undefined" && this._masterControl !== null) {
                        this._masterControl.AddChild(this);
                    }

                    this.getIdField = function () {
                        return this._idField;
                    }

                    //if (_masterControl === null) {

                    this._controlSelect.change(function () {
                        _instance.OnChange();
                    });
                    //}


                    this.AddChild = function (child) {
                        this._childControls.push(child);
                    }


                    this.OnChange = function () {
                        if (this._idField !== null) {
                            var value = new Array();
                            if (typeof this.getSelectedValue() !== "undefined" && this.getSelectedValue() !== null) {
                                value[0] = new Object();
                                value[0].id = this.getSelectedValue();
                                value[0].name = this.getSelectedText();
                                value[0].entityType = this._refEntityName;
                            }
                            Xrm.Page.getAttribute(this._idField).setValue(value);

                        }
                        if (this._valueField !== null) {
                            Xrm.Page.getAttribute(this._valueField).setValue(this.getSelectedText());
                        }

                        for (var c in this._childControls) {
                            this._childControls[c].Reload();
                        }
                    }


                    this.getSelectedValue = function () {
                        return this._controlSelect.val(); // $('#' + _selectName + " option:selected").val();
                        //return _selectedValue;
                    }

                    this.getSelectedText = function () {
                        return $('#' + this._selectName + " option:selected").text();
                    }

                    this._retrieveSuccessCallback = function (results) {
                        try {
                            var option = document.createElement("OPTION");
                            this._controlSelect.get(0).appendChild(option);
                            for (r in results) {
                                var option = document.createElement("OPTION");
                                this._controlSelect.get(0).appendChild(option);
                                option.text = results[r][this._refValueField];
                                option.value = results[r][this._refIdField];
                                if (this._idField !== null) {
                                    var idLookup = Xrm.Page.getAttribute(this._idField).getValue();
                                    if (typeof idLookup !== "undefined" && idLookup !== null && idLookup.length > 0 && ("{" + option.value.toUpperCase() + "}") === idLookup[0].id.toUpperCase()) {
                                        option.selected = true;
                                    }
                                }
                                else if (this._valueField !== null) {
                                    if (option.text === Xrm.Page.getAttribute(this._valueField).getValue()) {
                                        option.selected = true;
                                    }
                                }
                            }
                        }
                        catch (err) {
                            alert("Error in dropdowns:" + err.Message + ":" + this._idField + ":" + this._valueField);
                        }
                    }

                    this._retrieveErrorCallback = function (error) {
                        alert("Error in dropdowns:" + error.message);
                    }

                    this._retrieveCompleteCallback = function () {
                        this.OnChange();
                    }

                    this.Reload = function () {
												var options = "$orderby=" + this._refValueField;
												var hasMasterControl = false;
												var hasMasterControlValue = false;
                        this._controlSelect.empty();
                        if (typeof this._masterControl !== "undefined" && this._masterControl !== null) {
                        		hasMasterControl = true;
                            var masterId = this._masterControl.getSelectedValue();
														//alert("masterRefField: '"+this._masterRefField+"' | masterId: '"+masterId+"'");
                            if (typeof masterId !== "undefined" && masterId !== null && masterId != "") {
                        				options += "&$filter=" + this._masterRefField + "/Id eq (guid'" + masterId + "')";
                        				hasMasterControlValue = true;
                        		}
				                }

												if(!hasMasterControl || hasMasterControl && hasMasterControlValue) {
												Azuro.DynamicsCRM.SDK.retrieveMultipleRecords(
		                          this._refEntityName,
		                          options,
		                          function (param) { _instance._retrieveSuccessCallback(param); },
		                          function (param) { _instance._retrieveErrorCallback(param); },
		                          function () { _instance._retrieveCompleteCallback(); }
		                    );
		                  }
                    }

                    //this.Reload();
                }

            }
        }
    }
}

//Azuro.DynamicsCRM.ClientHelper.CreateHtmlHelper(true);