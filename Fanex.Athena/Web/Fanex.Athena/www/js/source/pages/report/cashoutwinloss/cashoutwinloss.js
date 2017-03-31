"use strict";
define(["jquery", "common/querystring", "common/site", "components/pluginhelper/daterangepicker", "./cashoutwinloss.option"],
    function ($, QueryString, Site, DatePicker, CashOutWinLossOption) {
        var cacheData = {};
        var ajaxRequest = null;
        var totalRowCount = 0;
        var _level = 0;
        var VALID_MONTH_RANGE = 3;
        var _submitData;

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initDateRangePicker();
                Site.addUserGuide({
                    resourceName: "AGCasinoWL",
                    title: Site.getSingleResource("AGCasinoWinLoss")
                });
                Site.setConstraintMonthRange(VALID_MONTH_RANGE, CashOutWinLossOption.urlExcelData);

                CashOutWinLossOption.isLicenseeOfferBA = $("#IsLicenseeOfferBA").val() === "1";
                _me.initEvent();
                
            },

            initEvent: function () {

            }
        };

        return _me;
    });