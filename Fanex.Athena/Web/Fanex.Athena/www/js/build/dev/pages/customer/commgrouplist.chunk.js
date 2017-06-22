webpackJsonp([55],{

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(269);


/***/ }),

/***/ 265:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
	    "use strict";
	
	    function goToCustomerList(groupCommId) {
	        if (isParentSameOrigin()) {
	            // Deactive Commission Group menu (55) and active Customer list menu (43)
	            $("li>#MenuItem55", window.parent.document).parent().removeClass("active");
	            $("li>#MenuItem43", window.parent.document).parent().addClass("active");
	            _me.setPageTitle(Site.getSingleResource("CustomerList"));
	        }
	        Site.redirect(Site.resolveClientUrl("Customer/CustomerList?groupId=" + groupCommId));
	    }
	
	    function setPageTitle(title) {
	        $("#page-title", window.parent.document).text(title);
	    }
	
	    function isParentSameOrigin() {
	        var sameOrigin;
	        try {
	            sameOrigin = window.parent.location.host === window.location.host;
	        }
	        catch (e) {
	            sameOrigin = false;
	        }
	        return sameOrigin;
	    }
	
	    var _me = {
	        goToCustomerList: goToCustomerList,
	        setPageTitle: setPageTitle
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 269:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(270)], __WEBPACK_AMD_DEFINE_RESULT__ = function (CustomerCommGroupList) {
	    "use strict";
	
	    $(CustomerCommGroupList.initialize);
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }),

/***/ 270:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(9), __webpack_require__(265)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, Popup, CustomerCommGroup) {
	        "use strict";
	
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
	        }
	
	        function initEvent() {
	            $("#ExportToExcel").click(function () {
	                $("#CommissionGroupForm").submit();
	            });
	
	            $("#comm-group-list").on("click", ".delete-comm-group", function (e) {
	                _me.confirmDeleteCommGroup($(e.target).data("id"));
	            });
	
	            $("#comm-group-list").on("click", ".customer-list-link", function (e) {
	                CustomerCommGroup.goToCustomerList($(e.target).data("id"));
	            });          
	        }
	
	        function confirmDeleteCommGroup(groupId) {
	            $.ajax({
	                url: Site.resolveClientUrl("CustomerComGroup/GetDefaultGroup"),
	                type: 'GET',
	                dataType: 'JSon',
	                cache: false,
	                success: function (response) {
	                    deleteGroupData.groupId = groupId;
	                    if (response.id.toString() === groupId.toString()) {
	                        deleteGroupData.isResetDefault = true;
	                        _me.showConfirmDeleteDefaulCommGroupPopup();
	                    }
	                    else {
	                        deleteGroupData.isResetDefault = false;
	                        _me.showOptionDeleteCommGroupPopup();
	                    }
	                }
	            });
	        }
	
	        function deleteCommGroup() {
	            $.ajax({
	                type: 'POST',
	                url: Site.resolveClientUrl("CustomerComGroup/Delete"),
	                data: deleteGroupData,
	                success: function (result) {
	                    if (result.ErrorCode > 0 || result.ErrorMessage.length > 0) {
	                        _me.showResultPopup(result.ErrorCode > 0 ? "Error" : "Successful", result.ErrorMessage);
	                    } else {
	                        Site.redirect(Site.resolveClientUrl("CustomerComGroup/Index"));
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
	                modal: true,
	                title: Site.getSingleResource("Confirm"),
	                height: "auto",
	                id: "del-default-comm-group",
	                width: 380
	            };
	
	            var buttons = [
	                {
	                    text: Site.getSingleResource("OK"),
	                    isClosedOnClick: true,
	                    click: _me.showOptionDeleteCommGroupPopup,
	                    classes: "btn btn-submit"
	                }, {
	                    text: Site.getSingleResource("Cancel"),
	                    isClosedOnClick: true,
	                    classes: "btn btn-cancel"
	                }];
	
	            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("DefaultGroupMsg") + '</p></div>', options, buttons);
	        }
	
	        function showOptionDeleteCommGroupPopup() {
	            var popup = $("#option-del-comm-group");
	
	            if (popup.length === 0) {
	                popup = $("#confirm-delete-comm-group");
	                popup.remove();
	                popup.find('input[name="IsRemovePlayerComm"]').on("click", function () {
	                    $(this).prop("checked", true);
	                });
	            }
	
	            var options = {
	                modal: true,
	                title: Site.getSingleResource("DeleteCommissionGroup"),
	                height: "auto",
	                id: "option-del-comm-group",
	                width: 380
	            };
	
	            var buttons = [
	                {
	                    text: Site.getSingleResource("Delete"),
	                    isClosedOnClick: true,
	                    click: function () {
	                        deleteGroupData.isRemovePlayerComm = $('input[name="IsRemovePlayerComm"]:checked').val();
	                        _me.showConfirmDeleteCommGroupPopup();
	                    },
	                    classes: "btn btn-submit"
	                }, {
	                    text: Site.getSingleResource("Cancel"),
	                    isClosedOnClick: true,
	                    click: function () {
	                        $('#IsRemovePlayerComm').prop("checked", true);
	                    },
	                    classes: "btn btn-cancel",
	                    id: "option-del-comm-group-cancel"
	                }];
	
	            Popup.openPopupWithButton(popup, options, buttons);
	        }
	
	        function showConfirmDeleteCommGroupPopup() {
	            var options = {
	                modal: true,
	                title: Site.getSingleResource("Confirm"),
	                height: "auto",
	                id: "confirm-del-comm-group",
	                width: 380
	            };
	
	            var buttons = [
	                {
	                    text: Site.getSingleResource("OK"),
	                    isClosedOnClick: true,
	                    click: _me.deleteCommGroup,
	                    classes: "btn btn-submit"
	                }, {
	                    text: Site.getSingleResource("Cancel"),
	                    isClosedOnClick: true,
	                    click: _me.showOptionDeleteCommGroupPopup,
	                    classes: "btn btn-cancel",
	                    id: "confirm-del-comm-group-cancel"
	                }];
	
	            Popup.openPopupWithButton('<div><p>' + Site.getSingleResource("AreYouSure") + '</p></div>', options, buttons);
	        }
	
	        function showResultPopup(title, message) {
	            var options = {
	                modal: true,
	                title: title,
	                height: "auto",
	                id: "result-del-comm-group",
	                width: 380
	            };
	
	            if ($("#result-del-comm-group").length > 0)
	            {
	                $("#result-msg").html(message);
	            }
	
	            var buttons = [
	                {
	                    text: Site.getSingleResource("OK"),
	                    isClosedOnClick: true,
	                    classes: "btn btn-submit",
	                    id: "result-OK"
	                }];
	
	            Popup.openPopupWithButton('<div><p id="result-msg">' + message + '</p></div>', options, buttons);
	        }
	
	        var _me = {
	            initialize: initialize,
	            initEvent: initEvent,
	            confirmDeleteCommGroup: confirmDeleteCommGroup,
	            showConfirmDeleteDefaulCommGroupPopup: showConfirmDeleteDefaulCommGroupPopup,
	            showOptionDeleteCommGroupPopup: showOptionDeleteCommGroupPopup,
	            showConfirmDeleteCommGroupPopup: showConfirmDeleteCommGroupPopup,
	            showResultPopup: showResultPopup,
	            deleteCommGroup: deleteCommGroup
	        };
	
	        return _me;
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ })

});
//# sourceMappingURL=commgrouplist.chunk.js.map