define(["jquery", "common/site", "common/popup"], function ($, Site, Popup) {
    "use strict";
    var REPORT_BY_MONTHLY = 2;
    var LICENSEE_12BET = 1;
    function lastDateOfMonth(year, month) {
        return new Date(year, month + 1, 0);
    }

    var _me = {
        GetDrilldownDateRange: function (reportBy, fromDate, toDate, drilldownDate) {
            var fdate = typeof drilldownDate === "string" ? new Date(drilldownDate) : drilldownDate;
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
                    id: "product-invalid",
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
            if (!summaryTotal || summaryTotal.length === 0) {
                return;
            }

            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            var rowCount = 0;
            var currencyList = [];

            function processColumnModel(dataItem) {
                var row = mainFooter.clone();
                var cellList = $("td", row);

                $(columnModel).each(function (cellIndex, modelElement) {
                    var isCustomFormater = typeof modelElement.formatter === "function";
                    var rawValue = dataItem[modelElement.name];
                    var value = isCustomFormater ? modelElement.formatter(rawValue, modelElement.formatoptions, row) :
                                $.fn.fmatter(modelElement.formatter, rawValue, modelElement.formatoptions);

                    var $cell = $(cellList.get(cellIndex));
                    if (cellIndex === 0) {
                        var content = "Total in " + currency;
                        $cell.html(content)
                            .addClass("group-" + currency)
                            .attr("title", content);
                    }
                    else {
                        var title = $.isNumeric(rawValue) && rawValue < 0 ? undefined : value;
                        $cell.html(value).attr("title", title);
                    }
                });

                row.insertAfter(mainFooter);
                rowCount++;
            }

            for (var currency in summaryTotal) {
                currencyList.push(currency);
                var summaryData = summaryTotal[currency];
                if (Array.isArray(summaryData)) {
                    summaryData.forEach(processColumnModel);
                }
                else {
                    processColumnModel(summaryData);
                }
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

                var allowShowLevel1 = licenseeId ? licenseeId === LICENSEE_12BET : true;
                $("#level-link-container .level1").toggle(allowShowLevel1);
                $("#level-link-container .level1").toggleClass("active", allowShowLevel1 && !isLevel2);
            }
        }
    };

    return _me;
});