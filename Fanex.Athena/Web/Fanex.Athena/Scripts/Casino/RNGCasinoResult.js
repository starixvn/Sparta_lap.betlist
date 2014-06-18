/*global Site, RNGCasinoOption, QueryString, jqGridAdditional*/
/// <reference path="/@JSense.js" />
/// //##
"use strict";
var RNGCasino = (function ($) {
    return {
        initialize: function () {
            RNGCasino.initEvent();
            RNGCasino.initSetting();
        },

        initSetting: function () {
            RNGCasinoOption.hasCustomerComm = Site.asBool($("#is-allow-cust-comm").val());
        },

        initEvent: function () {
            $("body").on("click", ".hand-history-detail-link", function () {
                var dataUrl = $(this).attr("data-url");
                RNGCasino.viewCasinoDetail(dataUrl);
            });

            $("#chkIsShowPic").on('click', function () {
                Site.blockUI();
                var isShowPic = $("#chkIsShowPic").is(':checked');
                var url = window.location.href;
                url = QueryString.setParam(url, "IsShowPic", isShowPic);
                window.location.href = url;
            });
        },

        viewCasinoDetail: function (dataUrl) {
            var popH = 550, popW = 720;
            var dialogForm = '<td title="Hand History Detail"><iframe frameborder="0" width="' + popW + '" height="100%" src="' + dataUrl + '" ></iframe></td>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        },

        bindTotal: function (totalData/*, summaryTotal*/) {
            var columnModel = RNGCasinoOption.getTotalColModel();
            var colspan = RNGCasinoOption.totalColspan[0];
            var summaryColspan = colspan + 1;
            jqGridAdditional.assignTotal(totalData, columnModel);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, undefined);

            var grandTotalCell = $('.grandTotal td:first-child');
            grandTotalCell.html(Site.getSingleResource('GrandTotal'));
            grandTotalCell.css('text-align', 'right');

            // set group color
            $('.jqgroup td').css('background-color', '#F4E8D4');
            $('.jqfoot td').css('background-color', '#CEF6CA');
            $('.footrow td').css('background-color', '#B2DFEF');

            // resize when group collapse
            $(".tree-wrap-ltr").click(function () {
                var scrollHeight = $("#ReportGrid").width() > $('.ui-jqgrid-bdiv').width() ? 18 : 0;
                var contentHeight = $("#ReportGrid").height();
                $('.ui-jqgrid-bdiv').height(Math.min(contentHeight + scrollHeight, 778));
            });
        },
        bindData: function (response) {
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            var onPaging = false;
            var gridOption = RNGCasinoOption.getGridOption();
            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;
            gridOption.data = response.Data;
            gridOption.pager = '#ppdata';
            gridOption.onPaging = function (pgButton) {
                if (numOfRow > RNGCasinoOption.pageSize) {
                    onPaging = true;
                    if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                        Site.blockUI();
                    }
                }
                else {
                    return 'stop';
                }
            };

            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);
                if (numOfRow > RNGCasinoOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        RNGCasino.bindTotal(response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                Site.unBlockUI();
            };
            // Show the grid
            reportGrid.jqGrid(gridOption);

            jqGridAdditional.resetLayout(reportGrid);
            jqGridAdditional.showRecommendedBrowser(gridOption.pager);
        },

        ajaxComplete: function (response) {
            RNGCasino.bindData(response);
        },

        loadContent: function () {
            var currentUrl = location.href;
            var urlTokens = currentUrl.split("?");
            $.ajax({
                type: "GET",
                dataType: "json",
                contentType: "application/json",
                url: Site.resolveClientUrl(RNGCasinoOption.urlData) + "?" + urlTokens[1],
                // error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    RNGCasino.ajaxComplete(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });

            return true;
        }
    };
})(jQuery);
$(window).load(function () {
    RNGCasino.initialize();
    RNGCasino.loadContent();
});
//#end
$(window).resize(function () {
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
});

$(window).unload(function () {
    $(window).unbind("resize");
});