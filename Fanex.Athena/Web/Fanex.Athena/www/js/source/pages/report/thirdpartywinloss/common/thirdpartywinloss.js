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
    "moment",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, Report, ThirdPartyWinLossOption, jqGridAdditional, DatePicker, MultiSelect, DateHelper, moment) {
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
            OrderItemInput: "#OrderItem",
            CanDrilldown: false,
            LevelLinkContainer: "#level-link-container",
            LevelLink: ".level-link",
            DrillDownLicenseeId: "#drillDownLicenseeId",
            DrillDownLink: ".drilldown"
        };

        var _ajaxRequest;
        var _cacheData = [];
        var _submitData;
        var _reportTitle;
        var _canDrilldown = false;
        var _urlExcelData;

        function initialize(memberOption) {
            Site.initialize();

            var option = $.extend({}, defaultOption, memberOption || {});

            Site.addUserGuide({
                resourceName: option.UserGuideResourceName,
                title: Site.getSingleResource(option.UserGuideTitle)
            });

            _me.initSetting(option);

            DatePicker.initDateRangePicker();
            DateHelper.setConstraintMonthRange(option.ValidMonthRange, option.urlExcelData);

            _me.initEvent();
        }

        function initSetting(option) {
            ThirdPartyWinLossOption.urlData = option.UrlData;
            ThirdPartyWinLossOption.urlExcelData = option.UrlExcelData;
            ThirdPartyWinLossOption.licenseeId = parseInt($(_selector.DrillDownLicenseeId).val());
            _reportTitle = Site.getSingleResource(option.ReportTitle);
            _canDrilldown = option.CanDrilldown;
        }

        function initEvent() {
            $(_selector.SubmitButton).click(function () {
                _me.submit();
            });

            $(_selector.ExportToExcel).click(function () {
                $(_selector.ExportExcelInput).val(true);
                var mainForm = $(_selector.Form);
                var url = Site.resolveClientUrl(_urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(_selector.ExportExcelInput).val(false);
                return false;
            });

            $(_selector.LevelLinkContainer).on("click", _selector.LevelLink, function () {
                var level = $(this).data("level");
                if (level < 2) {
                    Report.renderBreadCrumb(level, ThirdPartyWinLossOption.licenseeId);
                    _me.bindData(level, _cacheData[level]);
                    _urlExcelData = _urlExcelData.replace("level=2", "level=1");
                }
                if (level === 0) {
                    _me.setReportTitle(_submitData.CurrencyId, _submitData.FromDate, _submitData.ToDate);
                    setExcelUrl(0);
                }
            });

            $(".main-content").on("click", _selector.DrillDownLink, function () {
                var data = $(this).data("drill-down");
                _me.submit(data);
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function submit(drillDownData) {
            var postdata;
            var url;
            var currencyId = 0;
            var fromDate = "";
            var toDate = "";
            var custId = 0;
            var level = 0;

            if (drillDownData) {
                level = drillDownData.Level;
                postdata = _submitData;
                currencyId = drillDownData.CurrencyId;
                custId = drillDownData.CustId;

                var drilldownDateRange = Report.GetDrilldownDateRange(postdata.ReportBy, postdata.FromDate, postdata.ToDate, drillDownData.Datetime);
                fromDate = moment(drilldownDateRange.FromDate).format("MM/DD/YYYY");
                toDate = moment(drilldownDateRange.ToDate).format("MM/DD/YYYY");
            }
            else {
                postdata = $(_selector.Form).serializeObject();
                currencyId = postdata.CurrencyId;
                fromDate = postdata.FromDate;
                toDate = postdata.ToDate;
            }

            var url = setUpUrl(ThirdPartyWinLossOption.urlData, level, currencyId, custId, fromDate, toDate);
            var option = {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(url),
                success: function (response) {
                    setExcelUrl(level, currencyId, custId, fromDate, toDate);
                    ajaxComplete(level, response, postdata);
                    _me.setReportTitle(currencyId, fromDate, toDate);
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

            if (response == null || numOfRow === 0) {
                jqGridAdditional.setNoInfo();
                Site.unBlockUI();
                return;
            }

            var reportGrid = $("#" + _selector.GridId);

            var gridOption = ThirdPartyWinLossOption.getGridOption(level, _submitData.ReportBy, _canDrilldown);
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
            var columnModel = ThirdPartyWinLossOption.getTotalColModel(level, _submitData.ReportBy);
            var colspan = ThirdPartyWinLossOption.totalColspan[level];
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        }

        function setReportTitle(currencyId, fromdate, todate) {
            var titlePattern = _reportTitle + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyStr = Report.getCurrencyNameForReportTitle(currencyId);

            var title = $.format(titlePattern, fromdate, todate, currencyStr);

            $(_selector.Title).html(title);
            $(_selector.Header).removeClass("hide");
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
                        BaseCurrency: param.BaseCurrency,
                        ProductId: param.ProductId,
                        ReportBy: param.ReportBy
                    };
                }

                Report.renderBreadCrumb(level, ThirdPartyWinLossOption.licenseeId);
                _me.bindData(level, response);
            }
        }

        function setExcelUrl(level, currencyId, custId, fdate, tdate) {
            _urlExcelData = setUpUrl(ThirdPartyWinLossOption.urlExcelData, level, currencyId, custId, fdate, tdate);
        }

        function setUpUrl(url, level, currencyId, custId, fdate, tdate) {
            if (level > 0) {
                url = QueryString.setParam(url, "level", level);
                url = QueryString.setParam(url, "currencyId", currencyId);
                url = QueryString.setParam(url, "custId", custId);
                url = QueryString.setParam(url, "fdate", fdate);
                url = QueryString.setParam(url, "tdate", tdate);
            }

            return url;
        }

        function resizeGrid() {
            var reportGrid = $("#" + _selector.GridId);
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initSetting: initSetting,
            initEvent: initEvent,
            submit: submit,
            bindData: bindData,
            setReportTitle: setReportTitle,
            bindTotal: bindTotal
        };

        return _me;
    });