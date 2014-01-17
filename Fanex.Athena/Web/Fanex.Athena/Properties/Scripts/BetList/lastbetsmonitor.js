/// <reference path="/@JSense.js" />
/**
* Takes seven arguments,
* 
* @param {licenseeType} - could be not null
* @param {currency} - could be not null
* @param {bettype} -could be not null
* @param {limitRowcout}-default value =10 , value in range {10,20,30}
* @param {transiId} -default value=0,
* @param {containerTable} -table report,default value="betlist_monitor"
* @param {tableLastRowHeaderId} -header row report,default value="monitor_header"
*/

/*---------------------------------------------------------------------------------
* Report monitor bet list display lastest when user bet
- If none currency or special level is selected then doesn't display data
Note: licenseeType: consist such as normal, affiliate, media, income access
currencyId: consist such as MYR, USD ...
betType:      consist susch as Handicap Over/Under ...
---------------------------------------------------------------------------------*/

var MonitorBetList = function () {
    this.serviceUrl = Site.resolveClientUrl('api/betlist/LastMonitorBetList');
    this.betListRows = { "Updated": false, "Rows": "" };
    this.licenseeType = "";
    this.currency = "";
    this.betType = "";
    this.specialLevel = "0";
    this.limitRowCount = 10;
    this.lastTransId = "";
    this.tableContainerId = "betlist_monitor";
    this.tableLastRowHeaderId = "monitor_header";
    this.highlightClass = "monitor-betlist-hightlight";
    this.statusHighLightClass = "betlist_status_hightlight";
    this.highlightKeepClass = "monitor-betlist-hight-keep";
    this.intervalId = 0;
    this.transIdArray = null;
    this.errorMessage = null;
    this.isLoading = false;
    this.isCriteriaChange = false;
    this.intervalMiliseconds = 3000;
    this.minLoadingMiliseconds = 1000;
    this.loadingTickBegin = 0;

    /*----------------------------------------------------------------------
    Method set init value for report, get from cookie if have values
    else get values from another funtions
    ----------------------------------------------------------------------*/
    this.init = function (licenseeType, currency, bettype, special, limitRowCount, transiId, containerTable, tableLastRowHeaderId) {
        this.licenseeType = licenseeType;
        this.currency = currency;
        this.betType = bettype;
        this.specialLevel = special;
        this.limitRowCount = limitRowCount;
        this.lastTransId = transiId;
        this.tableContainerId = containerTable;
        this.tableLastRowHeaderId = tableLastRowHeaderId;
    };

    this.SetTransId = function (transId) {
        this.lastTransId = transId == undefined || transId == null || transId == ""
                            ? 0
                            : transId; ;
    };

    this.setLicenseeType = function (licenseeType) {
        this.licenseeType = licenseeType;
    };
    this.setCurrencyId = function (currency) {
        this.currency = currency;
    };
    this.setSpecialLevel = function (specialLevel) {
        this.specialLevel = specialLevel;
    };
    this.setBetType = function (bettype) {
        this.betType = bettype;
    }
    this.setLimitRowCount = function (limitrowcount) {
        this.limitRowCount = limitrowcount;
    };

    this.bind = function () {

        var context = this;

        /*----------------------------------------------------------------------
        Display when any error or no information, remove all rows have data
        ----------------------------------------------------------------------*/
        if (context.errorMessage != null && context.errorMessage.length > 0) {
            var errorRow = '<tr class="no-info-row"><td colspan="7">' + context.errorMessage + '</td></tr>';
            $("#" + context.tableContainerId + " tr:gt(0)").remove();
            $(errorRow).appendTo("#" + context.tableContainerId);
            return;
        }

        // Do nothing if there is no new bet
        if (!context.betListRows.Updated) { context.isLoading = false; return; };

        /*--------------------------------------------------------------------------------------------
        Insert new rows from database
        Remove old rows
        Update last transId
        --------------------------------------------------------------------------------------------*/

        $("#" + context.tableContainerId + " tr.no_info").remove();

        var header = "#" + context.tableLastRowHeaderId;
        var newRowCount = context.betListRows.Rows.length;
        var currentRowCount = $("#" + context.tableContainerId + " tr").length;
        for (var i = 0, k = newRowCount; i < k; i++) {
            var newRow = context.betListRows.Rows[i].replace("<tr>", "<tr onclick='javascript:MonitorBetListUI.hightlightRow(" + context.transIdArray[i] + ");' id='" + context.transIdArray[i] + "' class='align-top " + context.highlightClass + "'>");
            $(newRow).insertAfter(header);
        }

        if (currentRowCount + newRowCount > context.limitRowCount) {
            $("#" + context.tableContainerId + " tr:gt(" + context.limitRowCount + ")").remove();
        };

        //Set new value for Last TransId
        if (newRowCount > 0) {
            this.SetTransId(context.transIdArray[0]);
        };

        setTimeout(function () { return context.unHighlight(); }, 6000);
        context.isLoading = false;
    };

    /*------------------------------------------------------------------------------------------
    Method unhigh light row when user click
    ------------------------------------------------------------------------------------------*/
    this.unHighlight = function () {
        var context = this;
        var contextTr = $("#" + this.tableContainerId + " tr");
        contextTr.removeClass(this.highlightClass);
        $.each(contextTr, function (index, value) {
            $(this).find("td").filter(":last").removeClass(context.statusHighLightClass);
        });

    };

    /*------------------------------------------------------------------------------------------
    Get paramaters
    ------------------------------------------------------------------------------------------*/
    this.getParams = function () {
        var params = {
            "licenseeType": this.licenseeType,
            "currency": this.currency,
            "betType": this.betType,
            "limitRowCount": this.limitRowCount,
            "transid": this.lastTransId,
            "licenseeId": $("#LicenseeId").val(),
            "speciallevel": this.specialLevel
        };
        return params;
    };

    /*------------------------------------------------------------------------------------------
    Get data from database
    ------------------------------------------------------------------------------------------*/
    this.getData = function () {

        var context = this;
        var needGetData = !context.isLoading;
        needGetData = needGetData && $('#special-list input[type=checkbox]:checked').length > 0;
        needGetData = needGetData && $('#currency-list input[type=checkbox]:checked').length > 0;

        if (!needGetData) {
            context.doMonitor();
            return;
        }

        context.isLoading = true;

        var loadingTickBegin = (new Date()).getTime();
        var ajax = $.ajax({
            url: context.serviceUrl,
            data: $.param(context.getParams()),
            dataType: "json",
            async: true,
            cache: false,
            timeout: 50000,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
                $("#div-loading").show();
                context.isLoading = true;
                context.isCriteriaChange = false;
            },
            complete: function () {
                $("#div-loading").hide();
                var numOfRow = $("#" + context.tableContainerId + " tr:gt(0)").length - $(".no-info-row").length;
                if (numOfRow > 0) {
                    $(".no-info-row").remove();
                }
            },
            success: function (data) {

                if (context.isCriteriaChange) {
                    context.isLoading = false;
                    context.doMonitor();
                    return;
                }

                if (context.lastTransId == 0) {
                    context.clearContent();
                }

                // Only bind data if current mode == mode at the begining of request
                var result = jQuery.parseJSON(data);
                context.betListRows.Updated = result.NewRow;

                context.betListRows.Rows = result.Rows;
                context.transIdArray = result.TransIdArray;
                //                context.errorMessage = result.Error;

                if (this.isCriteriaChange) {
                    context.clearContent();
                }
                context.bind();

                var loadingTime = (new Date()).getTime() - loadingTickBegin;
                context.intervalMiliseconds = loadingTime > context.minLoadingMiliseconds
                ? context.intervalMiliseconds + 1000
                : 3000;
                context.doMonitor();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#div-loading").hide();

                context.betListRows.Updated = false;
                context.betListRows.Rows = "";
                context.isLoading = false;
                context.intervalMiliseconds += 1000;
                context.doMonitor();
            }
        });

    };

    this.trim = function (str) {
        return str != null ? str.replace(" ", "") : "";
    };

    this.clearContent = function () {
        this.lastTransId = 0;
        this.StopRender = true;
        $("#" + this.tableContainerId + " tr:gt(0)").remove();
        var noInfoMessage = Site.getSingleResource("NoInfo");
        var noinfo = "<tr class='no-info-row' style='text-align:center'><td colspan='7'>" + noInfoMessage + "</td></tr>";
        var header = "#" + this.tableLastRowHeaderId;

        $(noinfo).insertAfter(header);
        this.transIdArray = null;
        this.StopRender = false;
    };
    this.betTypeHandicapNames = []
    this.betTypeOverUnderNames = [];

    this.setBetTypeNames = function (str) {
        var temp = str.split(",");
        var temp2 = [];
        for (var i = 0, k = temp.length; i < k; i++) {
            if (temp[i] != null) {
                temp2.push(this.trim(temp[i]));
            }
        }

        return temp2;
    };

    this.getCurrencyList = function () {
        if ($("#currency-list input[type=checkbox]").eq(0).is(":checked")) return "";

        var selectedCurrency = [];
        $("#currency-list input.ckh-currency:checked").each(function (index, value) {
            var value = $(this).attr("data");
            if (value > 0) { selectedCurrency.push(value); };
        });

        return selectedCurrency.join(",");
    };

    this.getSpecialList = function () {

        if ($("#special-list input[type=checkbox]").eq(0).is(":checked")) return "";

        var selectedLevel = [];
        $("#special-list input.ckh-special:gt(0):checked").each(function (index, value) {

            var value = $(this).attr("data");
            selectedLevel.push(value);
        });

        return selectedLevel.join(",");
    };


    this.startMonitor = function () {
        this.stopMonitor();

        var context = this;
        context.lastTransId = 0;
        context.intervalMiliseconds = 3000;
        context.doMonitor();
    };

    this.stopMonitor = function () {
        if (this.intervalId > 0) {
            clearInterval(this.intervalId);
        }
    };

    this.doMonitor = function () {

        var context = this;
        if (context.intervalId > 0) {
            clearTimeout(this.intervalId);
        }

        context.intervalId = setTimeout(function () { context.getData(); }, context.intervalMiliseconds);
    };
};

var MonitorBetListUI = function (monitorObj) {

    this.monitor = monitorObj;
    this.initMonitorObject = function () {

        var containerTable = "hor-minimalist-a";            //Id of table
        var tableLastRowHeaderId = "tr-betlist-header";     //Id of tag tr

        this.monitor.init("ALL", "", "ALL", "", 10, 0, containerTable, tableLastRowHeaderId);
        this.monitor.highlightClass = "monitor-betlist-hightlight";

        this.monitor.betTypeHandicapNames = this.monitor.setBetTypeNames($("#HiddenFieldBettypeHandicap").val());
        this.monitor.betTypeOverUnderNames = this.monitor.setBetTypeNames($("#HiddenFieldOverUnder").val());
    };


    this.initMonitorControl = function () {

        //bind filter
        $("#LicenseeType option:selected").val("ALL");

        //limit row
        $("#filter-limit-row-count .limit-row").removeAttr("disabled");
        $("#filter-limit-row-count .limit-row[value='10']").attr("disabled", "disabled");

        //currency list
        $("#currency-list .ckh-currency").removeAttr("checked");

        $("#currency-list .ckh-currency").prop("checked", "checked");
        $("#special-list .ckh-special").attr("checked", "checked");

        //bettype
        $("#filter-licensee-bettype .filter-by-bettype").removeClass("selected");
        $("#filter-licensee-bettype .filter-by-bettype[data='ALL']").addClass("selected");
    };


    this.initSpecialLevelCheckbox = function () {
        /*-------------------------------------------------------------------------------------
        Change function of checkbox
        -------------------------------------------------------------------------------------*/
        var monitor = this.monitor;
        this.initializeCheckboxList($("#special-list input.ckh-special").eq(0), $("#special-list input.ckh-special:gt(0)"));

        $("#special-list .ckh-special").change(function () {

            monitor.stopMonitor();

            var specialList = monitor.getSpecialList();
            monitor.setSpecialLevel(specialList);
            monitor.isCriteriaChange = true;

            monitor.startMonitor();
        });
    };

    this.initPageSizeControl = function () {

        var monitor = this.monitor;

        $("#filter-limit-row-count .limit-row").click(function () {

            monitor.stopMonitor();

            $("#filter-limit-row-count .limit-row").removeAttr("disabled");
            var value = $(this).attr("value");
            $(this).attr("disabled", "disabled");
            monitor.setLimitRowCount(value);
            monitor.isCriteriaChange = true;

            monitor.startMonitor();
        });


    };

    this.initBetTypeControl = function () {

        var monitor = this.monitor;

        $("#filter-licensee-bettype .filter-by-bettype").click(function () {

            monitor.stopMonitor();

            $("#filter-licensee-bettype .filter-by-bettype").removeClass("selected");
            $(this).addClass("selected");
            var value = $(this).attr("data");
            monitor.setBetType(value);
            monitor.isCriteriaChange = true;

            monitor.startMonitor();
        });
    };

    this.initLicenseeTypeControl = function () {

        var monitor = this.monitor;

        $("#LicenseeType").change(function () {

            monitor.stopMonitor();

            var value = $(this).val();
            monitor.setLicenseeType(value);
            monitor.isCriteriaChange = true;

            monitor.startMonitor();
        });
    };

    this.initCurrencyIdControl = function () {

        var monitor = this.monitor;

        this.initializeCheckboxList($("#currency-list input.ckh-currency").eq(0), $("#currency-list input.ckh-currency:gt(0)"));

        $("#currency-list .ckh-currency").change(function () {

            monitor.stopMonitor();

            var currencyList = monitor.getCurrencyList();
            monitor.setCurrencyId(currencyList);
            monitor.isCriteriaChange = true;

            monitor.startMonitor();
        });
    };

    this.initializeCheckboxList = function (allChk, elseChk) {
        allChk.attr('checked', elseChk.filter(":checked").length >= elseChk.length);

        allChk.bind('change', function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
        });

        elseChk.bind('change', function () {
            if ($(this).is(':checked')) {
                allChk.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
            } else {
                allChk.prop('checked', false);
            }
        });
    };

    this.initialize = function () {
        this.initMonitorObject();
        this.initMonitorControl();
        this.initLicenseeTypeControl();
        this.initBetTypeControl();
        this.initCurrencyIdControl();
        this.initSpecialLevelCheckbox();
        this.initPageSizeControl();
    };
};


/*--------------------------------------------------------------------------
Method use change color of row when user click
--------------------------------------------------------------------------*/
MonitorBetListUI.hightlightRow = function (target) {
    var context = $("#" + target);
    if (context.hasClass("monitor-betlist-hight-keep")) {
        context.removeClass("monitor-betlist-hight-keep");
    }
    else {
        context.addClass("monitor-betlist-hight-keep");
    }
};

/*******************/
var monitor = null;
var monitorUi = null;
$(window).load(function () {
    monitor = new MonitorBetList();
    monitorUi = new MonitorBetListUI(monitor);

    monitorUi.initialize();
});


$(window).load(function () {
    monitor.startMonitor();
});

$(window).unload(function () {
    monitor.stopMonitor();
    monitor = null;
});
