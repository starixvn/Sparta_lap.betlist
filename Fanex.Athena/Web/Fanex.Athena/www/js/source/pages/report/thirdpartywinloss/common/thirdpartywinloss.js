define([
    "jquery",
    "common/querystring",
    "common/site",
    "../../report",
    "./thirdpartywinloss.option",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/daterangepicker",
    "components/pluginhelper/multiselect",
    "helpers/daterange",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, Report, ThirdPartyWinLossOption, jqGridAdditional, DatePicker, MultiSelect, DateHelper) {
        "use strict";
        var defaultOption = {
            UserGuideResourceName: "",
            UserGuideTitle: "",
            UrlExcelData: "api/ThirdPartyWinLossReport/Index",
            UrlData: "api/ThirdPartyWinLossReport/Index",
            ValidMonthRange: 0,
            ReportTitle: ""
        };

        var _selector = {
            SubmitButton: "#SubmitButton",
            ExportExcelInput: "#IsExportExcel",
            ExportToExcel: "#ExportToExcel",
            Form: "#ThirdPartyWinLossForm",
            Title: ".report-title",
            Header: ".panel-heading",
            GridId: "ReportGrid",
            OrderItemInput: "#OrderItem"
        };

        var _ajaxRequest;
        var _cacheData = [];
        var _submitData;
        var _reportTitle;

        function initialize(memberOption) {
            Site.initialize();

            var option = $.extend({}, defaultOption, memberOption || {});
            ThirdPartyWinLossOption.urlData = option.UrlData;
            ThirdPartyWinLossOption.urlExcelData = option.UrlExcelData;
            _reportTitle = Site.getSingleResource(option.ReportTitle);
            
            Site.addUserGuide({
                resourceName: option.UserGuideResourceName,
                title: Site.getSingleResource(option.UserGuideTitle)
            });

            DatePicker.initDateRangePicker();
            DateHelper.setConstraintMonthRange(option.ValidMonthRange, ThirdPartyWinLossOption.urlExcelData);

            _me.initEvent();
        }

        function initEvent() {
            $(_selector.SubmitButton).click(function () {
                //_level = 0;
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                $(_selector.ExportExcelInput).val(true);
                var mainForm = $(_selector.Form);
                var url = Site.resolveClientUrl(ThirdPartyWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(_selector.ExportExcelInput).val(false);
                return false;
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function submit() {
            var mainForm = $(_selector.Form);
            var postdata = mainForm.serializeObject();

            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(ThirdPartyWinLossOption.urlData),
                success: function (response) {
                    //ThirdPartyWinLoss.setExcelUrl(0, 0, 0, "", "");
                    //ThirdPartyWinLoss.ajaxComplete(0, response);
                    ajaxComplete(0, response, postdata);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            };
            if (_ajaxRequest != null) {
                _ajaxRequest.abort();
            }
            _ajaxRequest = $.ajax(option);

            return true;
        }

        function bindData(level, response) {
            Site.blockUI();
            jqGridAdditional.clearGrid(_selector.GridId);

            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;

            if (response == null || numOfRow === 0) {
                jqGridAdditional.setNoInfo();
                Site.unBlockUI();
                return;
            }

            var reportGrid = $("#" + _selector.GridId);

            var gridOption = ThirdPartyWinLossOption.getGridOption(level, _canDrilldown);
            gridOption.data = response.Data;
            gridOption.rowNum = numOfRow;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete();
                _me.bindTotal(level, response.Total, response.SummaryTotal);
                resizeGrid();
                Site.unBlockUI();
            };

            reportGrid.jqGrid(gridOption);
        }

        function bindTotal(level, totalData, summaryTotal) {

        }

        function setReportTitle(currencyId, fromdate, todate) {
            var titlePattern = _reportTitle + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyStr = Report.getCurrencyNameForReportTitle(currencyId);

            var title = $.format(titlePattern, fromdate.toString("MM/dd/yyyy"), todate.toString("MM/dd/yyyy"), currencyStr);

            $(_selector.Title).html(title);
            $(_selector.Header).removeClass("hide");
        }

        function renderBreadCrumb() {
        }

        function ajaxComplete(level, response, param) {
            if (response != null) {
                _cacheData[level] = response;
                if (level === 0) {
                    _submitData = {
                        IsExcludedTestData: param.IsExcludedTestData,
                        CurrencyId: param.CurrencyId,
                        FromDate: param.FromDate,
                        ToDate: param.ToDate,
                        CurrencyName: param.CurrencyName,
                        BaseCurrency: param.BaseCurrency,
                        ProductId: param.ProductId,
                        ReportBy: param.reportBy
                    };
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                }

                _me.renderBreadCrumb(level);
                _me.bindData(level, response);
            }
        }

        function resizeGrid() {
            var reportGrid = $("#" + _selector.ReportGrid);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent,
            submit: submit,
            bindData: bindData,
            setReportTitle: setReportTitle,
            renderBreadCrumb: renderBreadCrumb,
            bindTotal: bindTotal
        };

        return _me;
    });