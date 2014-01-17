/// <reference path="/@JSense.js" />
//#regions licenseeWL options
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var WinLossByLeagueOption = (function () {
    var setDrillDownLink = function (custId, cellvalue,externalid, systemId) {
        return '<a class="drilldown" href="javascript:;" data-custid="' + custId + '" data-externalid="' + externalid + '" data-systemid="' + systemId + '">' + cellvalue + '</a>';
    };

    //#region Columns


    var indexColumn =
        {
            name: 'Index',
            index: 'Index',
            width: 30,
            align: 'center',
            valign: 'middle',
            sortable: false,
            resizable: false
        };
    var externalIdColumn =
    {
        name: 'ExternalId',
        index: 'ExternalId',
        width: 95,
        align: 'left',
        sortable: false,
        resizable: false,
        formatter: function (cellvalue, options, rowObject) {
            return setDrillDownLink(rowObject.CustomerId, cellvalue, rowObject.ExternalId, rowObject.SystemId);
        }
    };
    var systemIdColumn =
    {
        name: 'SystemId',
        index: 'SystemId',
        width: 120,
        align: 'left',
        sortable: false,
        resizable: false,
        formatter: function (cellvalue, options, rowObject) {
            return setDrillDownLink(rowObject.CustomerId, cellvalue, rowObject.ExternalId, rowObject.SystemId);
        }
    };
    var matchColumn =
     {
         name: 'Match',
         index: 'Match',
         width: 90,
         sortable: false,
         resizable: false,
         hidden: false,
         formatter: function (cellvalue, options, rowObject) {
             return rowObject.HomeName + "<br/>" + rowObject.AwayName;
         }
     };
    var turnoverColumn =
        {
            name: 'TurnOver',
            index: 'TurnOver',
            width: 90,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            hidden: false,
            formatoptions:
            {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: 2
            }
        };
    var betcountColumn =
        {
            name: 'BetCount',
            index: 'BetCount',
            width: 50,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'integer',
            frozen: true,
            formatoptions:
            {
                thousandsSeparator: ","
            }
        };
    var customerWlColumn =
        {
            name: 'CustomerWinLoss',
            index: 'CustomerWinLoss',
            width: 80,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            formatoptions:
            {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: 2
            }
        };

    var licenseeWlColumn =
        {
            name: 'LicenseeWL',
            //classes: 'highlight',
            index: 'LicenseeWL',
            width: 80,
            align: 'right',
            sortable: false,
            resizable: false,
            hidden: false,
            title: true,
            formatter: jqGridAdditional.formatLicenseeWl
        };
   
    var baCommColumn =
        {
            name: 'BaCommission',
            // classes: 'highlight',
            index: 'BaCommission',
            width: 80,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            formatoptions:
            {
                decimalSeparator: ",",
                thousandsSeparator: ",",
                decimalPlaces: 2
            }
        };
    var amtDueToColumn =
        {
            name: 'AmountDue',
            index: 'AmountDue',
            width: 80,
            align: 'right',
            sortable: false,
            resizable: false,
            formatter: 'currency',
            formatoptions:
            {
                decimalSeparator: ".",
                thousandsSeparator: ",",
                decimalPlaces: 2
            }
        };


    //#endregion
    var colNames = [
         ['#', 'Match', 'BetCount', 'Turnover', 'CustomerW_L', 'LicenseeW_L', 'BACommission', 'AmtDueTo'],
         ['#', 'ExternalID', 'SystemID', 'BetCount', 'Turnover', 'CustomerW_L', 'LicenseeW_L'],
         ['#', 'Match', 'BetCount', 'Turnover', 'CustomerW_L', 'LicenseeW_L', 'BACommission', 'AmtDueTo']
     ];

    var colModels = [
        [indexColumn, matchColumn, betcountColumn, turnoverColumn, customerWlColumn, licenseeWlColumn, baCommColumn, amtDueToColumn],
        [indexColumn, externalIdColumn, systemIdColumn, betcountColumn, turnoverColumn, customerWlColumn, licenseeWlColumn],
        [indexColumn, matchColumn, betcountColumn, turnoverColumn, customerWlColumn, licenseeWlColumn, baCommColumn, amtDueToColumn]
    ];


    var me =
        {
            licenseeId: 0,
            currencyIndex: 4,
            systemIdIndex: 3,
            externalIdIndex: 2,
            customerWlTotalIndex: 10,            
            pageSize: 50,
            nonePagingSize: 5000,
            rowNum: 50,
            rowList: [50, 100, 200, 300, 400, 500, 1000, 2000, 3000, 4000, 5000],
            totalColspan: [1, 2, 3],
            //level 0,1,2
            urlData: 'api/report/WinLossByLeague',
            urlExcelData: 'api/report/WinLossByLeague',
            gridOption:
            {
                datatype: 'local',
                colNames: [],
                colModel: [],
                data: [],
                height: 'auto',
                autowidth: true,
                forceFit: false,
                shrinkToFit: true,
                loadtext: 'loading...',
                loadui: 'disable',
                loadonce: true,
                footerrow: true,
                userDataOnFooter: true,
                userData: [],
                viewrecords: true,
                gridview: true
            }
        };

    //# getGidOptionregion

    me.getColName = function (level) {        
        return Site.getResources(colNames[level]);
    };

    me.getColModel = function (level) {       
        return colModels[level];
    };
    
    me.getGridOption = function (level) {
        me.gridOption.colNames = me.getColName(level);
        me.gridOption.colModel = me.getColModel(level);
        return me.gridOption;
    };
    //#end

    me.addLeagueGroupRow = function (leagueid, leagueName) {
        var $grid = $("#ReportGrid");
        var $row = $grid.find("tr.jqgrow:first");
        $row.before("<tr><td colspan=8>&nbsp;&nbsp;&nbsp; <a id='leagueHeader' data-leagueid='" + leagueid + "' href='javascript:void(0)' style='color:blue;text-decoration: underline !important'>* " + leagueName + "</a></td></tr>");
    };

    return me;
})();