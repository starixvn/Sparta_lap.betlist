define(["jquery", "common/site", "common/popup"], function ($, Site, Popup) {
    "use strict";
    var REPORT_BY_MONTHLY = 2;
    function lastDateOfMonth(year, month) {
        return new Date(year, month + 1, 0);
    }

    var _me = {
        GetDrilldownDateRange: function (reportBy, fromDate, toDate, drilldownDate) {
            var fdate = (typeof drilldownDate === "string") ? Date.parse(drilldownDate) : drilldownDate;
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
                var message = Site.getSingleResource("AlertChooseProductMsg");
                var $dialog = $('<div title="' + title + '"><p class="notification">' + message + '</p></div>');

                Popup.openPopup($dialog, {
                    width: 380,
                    height: 150,
                    title: title,
                    className: "popup-notification",
                    onOpen: function () {
                        $('#SubmitButton').prop('disabled', true);
                    },
                    onClose: function () {
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
            if (!summaryTotal || summaryTotal.length === 0) {
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
        },

        getCurrencyNameForReportTitle: function (currencyId) {
            var viewinKey = Site.getSingleResource("Viewin");
            var currencyTextList = {
                "-3": viewinKey.replace("{0}", "GBP"),
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            return currencyTextList[currencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                : currencyTextList[currencyId];
        },

        renderBreadCrumb: function (level, licenseeId) {
            level = Number(level);
            if (level === 0) {
                $(".breadCrumbHolder").hide();
            }
            else {
                $(".breadCrumbHolder").show();
                $("#level-link-container .level0").show();

                var isLevel2 = level === 2;
                $("#level-link-container .level2").toggle(isLevel2).toggleClass("active", isLevel2);
                
                var allowShowLevel1 = licenseeId === 1;
                $("#level-link-container .level1").toggle(allowShowLevel1);
                $("#level-link-container .level1").toggleClass("active", allowShowLevel1 && !isLevel2);
            }
        }
    };

    return _me;
});