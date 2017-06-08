webpackJsonp([50],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(235);


/***/ },

/***/ 235:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(236)], __WEBPACK_AMD_DEFINE_RESULT__ = function (CustomerCommGroupList) {
	    "use strict";
	
	    $(CustomerCommGroupList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ },

/***/ 236:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(9)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup) {
	        "use strict";
	
	        var FORMSELECTOR = "#CommissionGroupForm";
	        var EXCELBUTTONSELECTOR = "#ExportToExcel";
	
	        var optionDeletePopup = null;
	        var deleteGroupData = {
	            groupId: 0,
	            isRemovePlayerComm: false,
	            isResetDefault: false
	        };
	
	        function initialize() {
	            Site.initialize();
	            Site.addUserGuide({
	                resourceName: "CommissionGroupList",
	                title: Site.getSingleResource("CommissionGroupList")
	            });
	
	            _me.initEvent();
	            _me.publicFunctions();
	        }
	
	        function initEvent() {
	            $(EXCELBUTTONSELECTOR).click(function () {
	                $(FORMSELECTOR).submit();
	            });
	        }
	
	        function publicFunctions() {
	            window.CustomerCommGroup = {
	                DeleteCommGroup: _me.deleteCommGroup
	            };
	        }
	
	        function deleteCommGroup(groupId) {
	            $.ajax({
	                url: Site.resolveClientUrl("CustomerComGroup/GetDefaultGroup"),
	                type: 'GET',
	                dataType: 'JSon',
	                cache: false,
	                success: function (response) {
	                    deleteGroupData.groupId = groupId;
	                    if (response.id.toString() === groupId.toString()) {
	                        deleteGroupData.isResetDefault = true;
	                        showConfirmDeleteDefaulCommGroupPopup();
	                    }
	                    else {
	                        deleteGroupData.isResetDefault = false;
	                        showOptionDeleteCommGroupPopup();
	                    }
	                }
	            });
	        }
	
	        function doDeleteCommGroup() {
	            $.ajax({
	                type: 'POST',
	                url: Site.resolveClientUrl("CustomerComGroup/Delete"),
	                data: deleteGroupData,
	                success: function (result) {
	                    if (result.ErrorCode > 0 || result.ErrorMessage.lengh > 0) {
	                        showResultPopup(result.ErrorCode > 0 ? "Error" : "Successful", result.ErrorMessage);
	                    } else {
	                        window.location.href = Site.resolveClientUrl("CustomerComGroup/Index");
	                    }
	
	                    Site.unBlockUI();
	                },
	                beforeSend: function () {
	                    Site.blockUI();
	                },
	            });
	        }
	
	        function showConfirmDeleteDefaulCommGroupPopup() {
	            var options = {
	                model: true,
	                title: Site.getSingleResource("Confirm"),
	                height: 150,
	                id: "del-default-comm-group"
	            };
	
	            var buttons = [
	                {
	                    text: "OK",
	                    isClosedOnClick: true,
	                    click: showOptionDeleteCommGroupPopup,
	                    classes: "btn btn-submit"
	                }, {
	                    text: "Cancel",
	                    isClosedOnClick: true,
	                    classes: "btn btn-cancel"
	                }];
	
	            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("DefaultGroupMsg") + '</p></div>', options, buttons);
	        }
	
	        function showOptionDeleteCommGroupPopup() {
	            if (optionDeletePopup === null) {
	                optionDeletePopup = $("#confirm-delete-comm-group");
	                optionDeletePopup.remove();
	                optionDeletePopup.find('input[name="IsRemovePlayerComm"]').on("click", function () {
	                    $(this).prop("checked", true);
	                });
	            }
	
	            var options = {
	                model: true,
	                title: Site.getSingleResource("DeleteCommissionGroup"),
	                height: 150,
	                id: "option-del-comm-group"
	            };
	
	            var buttons = [
	                {
	                    text: "Delete",
	                    isClosedOnClick: true,
	                    click: function () {
	                        deleteGroupData.isRemovePlayerComm = $('input[name="IsRemovePlayerComm"]:checked').val();
	                        showConfirmDeleteCommGroupPopup();
	                    },
	                    classes: "btn btn-submit"
	                }, {
	                    text: "Cancel",
	                    isClosedOnClick: true,
	                    click: function () {
	                        $('#IsRemovePlayerComm').prop("checked", true);
	                    },
	                    classes: "btn btn-cancel"
	                }];
	
	            Popup.openPopupWithButton(optionDeletePopup, options, buttons);
	        }
	
	        function showConfirmDeleteCommGroupPopup() {
	            var options = {
	                model: true,
	                title: Site.getSingleResource("Confirm"),
	                height: 150,
	                id: "confirm-del-comm-group"
	            };
	
	            var buttons = [
	                {
	                    text: "OK",
	                    isClosedOnClick: true,
	                    click: doDeleteCommGroup,
	                    classes: "btn btn-submit"
	                }, {
	                    text: "Cancel",
	                    isClosedOnClick: true,
	                    click: showOptionDeleteCommGroupPopup,
	                    classes: "btn btn-cancel"
	                }];
	
	            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("AreYouSure") + '</p></div>', options, buttons);
	        }
	
	        function showResultPopup(title, message) {
	            var options = {
	                model: true,
	                title: "",
	                height: 150,
	                id: "result-del-comm-group"
	            };
	
	            if ($("#result-del-comm-group").lengh > 0)
	            {
	                $("#result-msg").html(message);
	            }
	
	            var buttons = [
	                {
	                    text: "OK",
	                    isClosedOnClick: true,
	                    classes: "btn btn-cancel",
	                    id: "result-OK"
	                }];
	
	            Popup.openPopupWithButton('<div><p id="result-msg">' + message + '</p></div>', options, buttons);
	        }
	
	        var _me = {
	            initialize: initialize,
	            initEvent: initEvent,
	            deleteCommGroup: deleteCommGroup,
	            publicFunctions: publicFunctions
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }

});
//# sourceMappingURL=commgrouplist.chunk.js.map