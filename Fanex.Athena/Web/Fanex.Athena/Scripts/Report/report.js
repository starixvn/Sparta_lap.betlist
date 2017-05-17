/*global Site*/
/*exported Report*/
/// <reference path="/@JSense.js" />
"use strict";
var Report = (function ($) {
    var REPORT_BY_MONTHLY = 2;
    function lastDateOfMonth(year, month) {
        return new Date(year, month + 1, 0);
    }

    return {
        CleanProductOptions: function (postData, productCount) {
            var productList = [];
            for (var i = 0; i < productCount; i++) {
                if (postData["ProductOptions[" + i + "].Status"] === "True") {
                    productList.push(postData["ProductOptions[" + i + "].Code"]);
                }

                delete postData["ProductOptions[" + i + "].Code"];
                delete postData["ProductOptions[" + i + "].Name"];
                delete postData["ProductOptions[" + i + "].Status"];
            }

            postData.ProductList = productList;
        },
        GetDrilldownDateRange: function (reportBy, fromDate, toDate, drilldownDate) {
            var fdate = (typeof drilldownDate === "string") ? new Date(drilldownDate) : drilldownDate;
            var tdate = fdate;
            if (Number(reportBy) === REPORT_BY_MONTHLY) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(fromDate);
                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = new Date(toDate);

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = lastDateOfMonth(year, month);
                }
            }

            return {
                FromDate: fdate,
                ToDate: tdate
            };
        },
        ValidProducts: function () {
            var temp = true;
            if ($(".childCheckbox:checked").length === 0) {
                temp = false;
                var title = Site.getSingleResource("LrfNotification");
                $('<div title="' + title + '">' + Site.getSingleResource("AlertChooseProductMsg") + '</div>').dialog({
                    open: function () {
                        $('#SubmitButton').prop('disabled', true);
                    },
                    close: function () {
                        $('#SubmitButton').removeAttr('disabled');
                    }
                });
            }

            return temp;
        },
        setOriginalProduct: function () {
            $(".childCheckbox[type='checkbox']", "#ProductList").each(function (index, element) {
                var isCheck = $(this).is(":checked");
                $(element).attr({ 'data-original': isCheck });
            });
        },
        bindSummaryTotal: function (columnModel, summaryTotal) {
            /// <summary>
            /// Bind summary total data to footer rows
            /// </summary>
            /// <param name="columnModel"></param>
            /// <param name="summaryTotal"></param>
            /// <returns type="int">Number of iserted row</returns>
            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            if (summaryTotal === undefined || summaryTotal == null || summaryTotal.length === 0) {
                return;
            }

            function processColumnModel(columnModel) {
                var firstCell = null;
                $(columnModel).each(function (cellIndex, modelElement) {
                    var cell = cellList.get(cellIndex);
                    var isCustomFormater = typeof (modelElement.formatter) === "function";
                    var rawValue = summaryData[modelElement.name];
                    var value = isCustomFormater ? modelElement.formatter(rawValue, modelElement.formatoptions, row) :
                                $.fn.fmatter(modelElement.formatter, rawValue, modelElement.formatoptions);
                    var titleValue = value;
                    if ($.isNumeric(rawValue) && rawValue < 0) {
                        var wrapper = document.createElement('div');
                        wrapper.innerHTML = value;
                        titleValue = wrapper.outerHtml;
                    }

                    $(cell).html(value).attr("title", titleValue);
                    firstCell = !firstCell ? $(cell) : firstCell;
                });

                return firstCell;
            }

            var rowCount = 0;
            var currencyList = [];
            for (var currency in summaryTotal) {
                currencyList.push(currency);
                var summaryData = summaryTotal[currency];

                var row = mainFooter.clone();
                row.addClass("total-eur total-eur-none-top-se");
                var cellList = $("td", row);
                var firstCell = processColumnModel(columnModel);
                firstCell.html("Total in " + currency);
                firstCell.addClass("group-" + currency);
                row.insertAfter(mainFooter);
                rowCount++;
            }

            $(".ui-jqgrid-ftable").attr("data-total-currency-list", currencyList.join());
            mainFooter.remove();

            return rowCount;
        }
    };
})(jQuery);