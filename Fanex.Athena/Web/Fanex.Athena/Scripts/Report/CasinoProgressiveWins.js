/*global jqGridAdditional, QueryString, CasinoProgressiveWinsOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var CasinoProgressiveWins = (function ($) {
    "use strict";
    var VALID_MONTH_RANGE = 12;
    var ajaxRequest = null;

    function getMicrogamingResultUrl(gameid, custid) {
        var url = Site.resolveClientUrl('ResultReport/MicrogamingResultPopup');
        url = QueryString.setParam(url, "gameid", gameid);
        url = QueryString.setParam(url, "custid", custid);

        return url;
    }

    function showMicrogamingResultPopup(gameid, custid) {
        var url = getMicrogamingResultUrl(gameid, custid);
        var title = Site.getSingleResource("Result");
        var dialogForm = '<div title="' + title + '"><iframe frameborder="0" width="720" height="100%" src="' + url + '" ></iframe></div></div>';
        $(dialogForm).dialog({
            modal: true,
            width: 750,
            height: 630
        });
    }

    return {
        initialize: function () {
            CasinoProgressiveWins.initEvent();

            Site.addUserGuide({
                resourceName: "CasinoProgressiveWins",
                title: Site.getSingleResource("CasinoProgressiveWins")
            });

            Site.setConstraintMonthRange(VALID_MONTH_RANGE, CasinoProgressiveWinsOption.urlExcelData);
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
                var gameid = $this.attr("data-gameid");
                var custid = $this.attr("data-custid");
                showMicrogamingResultPopup(gameid, custid);
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
                    CasinoProgressiveWins.bindData(response);
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

            var $reportGrid = $("#ReportGrid");
            
            var gridOption = CasinoProgressiveWinsOption.getGridOption();
            gridOption.data = response.Data;

            gridOption.gridComplete = function () {
                if (response == null || response.Data.length === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    jqGridAdditional.resizeGrid();
                }

                Site.unBlockUI();
            };

            // Show the grid
            $reportGrid.jqGrid(gridOption);

            $reportGrid.jqGrid('showCol', 'rn');

            jqGridAdditional.resetLayout($reportGrid);

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
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});