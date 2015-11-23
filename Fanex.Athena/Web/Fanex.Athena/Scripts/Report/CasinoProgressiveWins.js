/*global jqGridAdditional, QueryString, CasinoProgressiveWinsOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var CasinoProgressiveWins = (function ($) {
    "use strict";

    var ajaxRequest = null;
    var _resizeTimer = null;

    function getMicrogamingResultUrl(gameid, custid) {
        var url = Site.resolveClientUrl('Report/MicrogamingResultPopup');
        url = QueryString.setParam(url, "gameid", gameid);
        url = QueryString.setParam(url, "custid", custid);

        return url;
    }

    function showMicrogamingResultPopup(gameid, custid) {
        var url = getMicrogamingResultUrl(gameid, custid);
        var title = Site.getSingleResource("Result");
        var dialogForm = '<div title="' + title + '"><iframe frameborder="0" width="720" height="450" src="' + url + '" ></iframe></div></div>';
        $(dialogForm).dialog({
            modal: true,
            width: 750,
            height: 500
        });
    }

    return {
        resizeGrid: function () {
            if ($(".ui-jqgrid-bdiv").length === 0) {
                return;
            }
            clearTimeout(_resizeTimer);
            _resizeTimer = setTimeout(function () {
                var $bdiv = $(".ui-jqgrid-bdiv");

                var gridTop = $bdiv.offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                            - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(120, gridHeight);

                var gridContainer = $("#gview_ReportGrid");
                var bodyHeight = window.innerHeight - gridContainer.offset().top;
                var actualHeight = $("#ReportGrid").height();
                var contentHeight = actualHeight > bodyHeight ? bodyHeight : actualHeight;
                var tableWidth = $("#ReportGrid").width();
                var contentWidth = $bdiv.width();
                var newHeight = tableWidth > contentWidth ? contentHeight + 18 : contentHeight;
                $bdiv.height(Math.min(newHeight, gridHeight))
                                    .css("max-height", gridHeight);
            }, 200);
        },

        initialize: function () {
            CasinoProgressiveWins.initEvent();

            Site.addUserGuide({
                resourceName: "CasinoProgressiveWins",
                title: Site.getSingleResource("CasinoProgressiveWins")
            });
        },

        initEvent: function () {
            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                CasinoProgressiveWins.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#mainForm");
                var url = Site.resolveClientUrl(CasinoProgressiveWinsOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });

            $(".report-content").on("click", ".refno-link", function () {
                var $this = $(this);
                var bettype = $this.attr("data-bettype");
                var custid = $this.attr("data-custid");
                showMicrogamingResultPopup(bettype, custid);
            });
        },

        //#region Layout
        setExcelUrl: function (currencyId, custId, fdate, tdate, currencyName) {
            var temp = 'api/CasinoProgressiveWinsReport/index';
            temp += "?currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy") + "&currencyName=" + currencyName;

            CasinoProgressiveWinsOption.urlExcelData = temp;
        },

        //#end

        //#Get Data
        ajaxComplete: function (response) {
            if (response != null) {
                CasinoProgressiveWins.bindData(response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#mainForm");
            var option = {
                type: "POST",
                data: mainForm.serialize(),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CasinoProgressiveWinsOption.urlData),
                success: function (response) {
                    CasinoProgressiveWins.setExcelUrl(0, 0, "", "", "");
                    CasinoProgressiveWins.ajaxComplete(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        bindData: function (response) {
            var $gridBody = $(".ui-jqgrid-bdiv");

            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");
            var numOfRow = response.Data.length;

            var gridOption = CasinoProgressiveWinsOption.getGridOption();
            gridOption.rownumbers = true;
            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowList = CasinoProgressiveWinsOption.rowList;
            gridOption.rowTotal = numOfRow;

            gridOption.gridComplete = function () {
                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    CasinoProgressiveWins.resizeGrid();
                }

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            reportGrid.jqGrid('showCol', 'rn');

            jqGridAdditional.resetLayout(reportGrid);

            $gridBody = $(".ui-jqgrid-bdiv");
            if ($gridBody.length > 0) {
                $gridBody[0].scrollLeft = scrollLeft;
            }
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CasinoProgressiveWins.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    CasinoProgressiveWins.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});