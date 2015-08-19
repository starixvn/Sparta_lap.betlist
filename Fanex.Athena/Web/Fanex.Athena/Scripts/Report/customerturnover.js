/*global Site, JSLINQ, jqGridAdditional, pagingHeper, CustTurnoverOption */
/// <reference path="/@JSense.js" />
var CustTurnover = (function ($) {
    "use strict";
    return {
        defaultUserName: Site.getSingleResource('All'),
        defaultOdds: 0,
        sports: [],
        checkedSports: [],
        bettypes: [],
        checkedBettypes: [],
        ajaxRequestData: null,
        ajaxRequestGetBettype: null,
        isExportExcel: false,
        sunGameLicensee: 8,
        currentLicensee: 0,
        excelParam:
        {
            fromdate: "",
            todate: "",
            reportby: 1,
            showwin: true,
            showloss: true,
            showdraw: true,
            odds: 0,
            SportProducts: [],
            bettypes: [],
            username: "",
            currencyid: 0,
            pageindex: 1,
            pagesize: 50,
            isexportexcel: false
        },
        param: {
            fromdate: "",
            todate: "",
            reportby: 1,
            showwin: true,
            showloss: true,
            showdraw: true,
            odds: 0,
            SportProducts: [],
            bettypes: [],
            username: "",
            currencyid: 0,
            pageindex: 1,
            pagesize: 50,
            isexportexcel: false
        },
        pagingOption:
        {
            pageindex: 1,
            pagesize: 25,
            pagelist: [25, 50, 100, 500],
            totalrow: 1,
            elementcontainer: "#paging",
            method: "AJAX",
            ajaxfunction: "CustTurnover.ajaxPaging()",
            alwayshow: true
        },
        sportUrl: Site.resolveClientUrl("report/GetSportsAndProducts"),
        bettypeUrl: Site.resolveClientUrl("ReportAPI/GetBettypesBySport"),
        urlData: Site.resolveClientUrl("report/CustomerTurnover"),
        urlExcelData: Site.resolveClientUrl("report/CustomerTurnoverExcel"),
        initialize: function () {
            /*init setting*/
            CustTurnover.currentLicensee = parseInt($("#LicenseeId").val());
            CustTurnover.initEvent();
            CustTurnover.setDefaultUsername();
            CustTurnover.initAutoComplete();
            Site.addUserGuide({
                resourceName: "CustomerTurnover",
                title: Site.getSingleResource("CustomerTurnover")
            });
            setTimeout(function () {
                Site.addFilterCollapseExpand();
            }, 500);
        },
        resizeGrid: function () {
            setTimeout(function () {
                if ($(".ui-jqgrid-bdiv").length === 0) {
                    return;
                }
                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var pagingHeight = $("#paging:visible").height();
                if (pagingHeight > 0) {
                    pagingHeight += 15;
                }

                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - pagingHeight - 20;
                gridHeight = Math.max(120, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
            }, 100);
        },
        initEvent: function () {
            /*product list*/

            CustTurnover.initSportProduct();

            CustTurnoverOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());

            $("#SubmitButton").click(function () {
                CustTurnover.clearExcelExport();
                CustTurnover.param.pageindex = 1;
                CustTurnover.submitForm();

                return false;
            });

            $('#ExportToExcel').click(function () {
                CustTurnover.clearExcelExport();
                CustTurnover.isExportExcel = true;
                CustTurnover.param.pageindex = -1;
                CustTurnover.submitForm();
                Site.unBlockUI();
                return false;
            });
            $("#Username").bind("focus focusout", function () {
                CustTurnover.setUsernameOnFocus();
            });

            $("#TickerFilter input[type=checkbox]").checkboxgroup();

            $("#Odds").keypress(function (event) {
                /*--------------------------------------------------------------
                Backspace, tab, enter, end, home, left, right
                Don't support the del key in Opera because del == . == 46.
                --------------------------------------------------------------*/
                var controlKeys = [8, 9, 13, 35, 36, 37, 39, 46];

                /*--------------------------------------------------------------
                IE doesn't support indexOf
                --------------------------------------------------------------*/
                var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

                /*--------------------------------------------------------------
                Some browsers just don't raise events for control keys. Easy.
                e.g. Safari backspace.
                --------------------------------------------------------------*/
                if (!event.which ||
                // Control keys in most browsers. e.g. Firefox tab is 0
                 (48 <= event.which && event.which <= 57) ||
                // Always 0 through 9
                //(48 == event.which && $(this).attr("value")) ||   // No 0 first digit
                    isControlKey) {
                    // Opera assigns values for control keys.
                    return;
                }
                else {
                    event.preventDefault();
                }
            });
        },
        initSportProduct: function () {
            var createComboboxTree = function () {
                var groupData = [];
                var products = JSLINQ(CustTurnover.sports).Where(function (item) {
                    return item.ParentId === -1;
                }).ToArray();
                $.each(products, function (index, value) {
                    var groupItem = {
                        Id: index.toString(),
                        Value: "-1-" + value.ID,
                        Text: value.Name,
                        Checked: value.Checked,
                        ParrentId: -1,
                        Enable: true,
                        Css: "",
                        CssClass: "",
                        Items: []
                    };
                    var childItems = JSLINQ(CustTurnover.sports).Where(function (item) {
                        return item.ParentId === value.ID && item.ID > 0 && item.ParentId > -1;
                    }).ToArray();
                    $.each(childItems, function (cindex, cvalue) {
                        var item = {
                            Id: groupItem.Id + "-" + cindex,
                            Value: cvalue.ParentId + "-" + cvalue.ID,
                            Text: cvalue.Name,
                            ParrentId: cvalue.ParentId,
                            Checked: cvalue.Checked,
                            Enable: true,
                            Css: "",
                            CssClass: "",
                            GroupName: groupItem.Name
                        };
                        groupItem.Items.push(item);
                    });
                    groupData.push(groupItem);
                });

                function addCheckedSport(checkedValue) {
                    var modelItem = JSLINQ(CustTurnover.sports).First(function (item)
                    { return (item.ParentId + "-" + item.ID) === checkedValue; });
                    if (modelItem != null) {
                        modelItem.Checked = true;
                        CustTurnover.checkedSports.push(modelItem);
                        /*add parent item*/

                        if (modelItem.ParentId > -1) {
                            var parent = JSLINQ(CustTurnover.checkedSports).First(function (item)
                            { return item.ID === modelItem.ParentId && item.ParentId === -1; });

                            /* if not found parrent item in checked sports
                            * Continue find parent item in sports and add it to checked sports.
                            */
                            if (parent == null) {
                                parent = JSLINQ(CustTurnover.sports).First(function (item)
                                { return item.ID === modelItem.ParentId && item.ParentId === -1; });
                                if (parent != null) {
                                    CustTurnover.checkedSports.push(parent);
                                }
                            }
                        }
                    }
                }

                $("#sports").multiSelect({
                    showCheckAll: true,
                    maxHeight: 705,
                    listData: groupData,
                    hasOptGroup: true,
                    onCheck: function () {
                        var numOfCheckedItem = $("#sports").multiSelect("getCheckedItems").length;
                        var checkedItems = $("#sports").multiSelect("getCheckedItems");
                        /*update checked sports*/
                        CustTurnover.checkedSports = [];
                        for (var i = 0; i < numOfCheckedItem; i++) {
                            addCheckedSport(checkedItems[i]);
                        }

                        /*Load bet types*/
                        CustTurnover.loadBettypes();
                    },
                    isUsedCustomCheckAllText: true,
                    customCheckAllText: Site.getSingleResource('All'),
                    checkAllText: Site.getSingleResource('All'),
                    noItemText: Site.getSingleResource('NoProductFound'),
                    unCheckAllText: Site.getSingleResource('UncheckAll'),
                    updateResult: '',
                    noneSelectedText: Site.getSingleResource('SelectOptions'),
                    selectedText: Site.getSingleResource('SelectedItemCount'),
                    tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                    selectedList: 0,
                    isCheckAll: true,
                    isRetrieveDataAfterClose: false,
                    multiple: true,
                    showFooter: false,
                    showHeader: true,
                    showTooltip: true
                });
            };
            /*load sport and products*/
            $.getJSON(CustTurnover.sportUrl, function (data) {
                CustTurnover.sports = data;
                CustTurnover.checkedSports = data;
                createComboboxTree();
                /*loadd all bettype*/
                CustTurnover.loadBettypes();
            });
        },
        creatBettypeComboBoxTree: function () {
            var dataList = [];
            $.each(CustTurnover.bettypes, function (index, value) {
                var item = {
                    Id: index.ID,
                    Value: value.ID,
                    Text: value.Name,
                    Checked: value.Checked,
                    Enable: true,
                    Css: "",
                    CssClass: ""
                };
                dataList.push(item);
            });

            function addCheckedBettype(checkedValue) {
                var modelItem = JSLINQ(CustTurnover.bettypes).First(function (item)
                { return item.ID.toString() === checkedValue.toString(); });
                modelItem.Checked = true;
                CustTurnover.checkedBettypes.push(modelItem);
            }

            $("#bettypes").multiSelect({
                showCheckAll: true,
                maxHeight: 705,
                listData: dataList,
                hasOptGroup: false,
                onCheck: function () {
                    var numOfCheckedItem = $("#bettypes").multiSelect("getCheckedItems").length;
                    var checkedItems = $("#bettypes").multiSelect("getCheckedItems");
                    /*update checked bet types*/
                    CustTurnover.checkedBettypes = [];
                    for (var i = 0; i < numOfCheckedItem; i++) {
                        addCheckedBettype(checkedItems[i]);
                    }
                },
                isUsedCustomCheckAllText: true,
                customCheckAllText: Site.getSingleResource('All'),
                checkAllText: Site.getSingleResource('All'),
                noItemText: Site.getSingleResource('NoBettypeFound'),
                unCheckAllText: Site.getSingleResource('UncheckAll'),
                updateResult: '',
                noneSelectedText: Site.getSingleResource('SelectOptions'),
                selectedText: Site.getSingleResource('SelectedItemCount'),
                tooltipNoItem: Site.getSingleResource('ThereIsNoSelectedItem'),
                selectedList: 0,
                isCheckAll: true,
                isRetrieveDataAfterClose: false,
                multiple: true,
                showFooter: false,
                showHeader: true,
                showTooltip: true
            });
        },
        loadBettypes: function () {
            /* load bet types by checked sports.*/
            var option = {
                type: "POST",
                data: JSON.stringify(CustTurnover.checkedSports),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: CustTurnover.bettypeUrl,
                success: function (response) {
                    CustTurnover.bettypes = response;
                    CustTurnover.checkedBettypes = response;
                    CustTurnover.creatBettypeComboBoxTree();
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            };
            if (CustTurnover.ajaxRequestGetBettype != null) {
                CustTurnover.ajaxRequestGetBettype.abort();
            }
            CustTurnover.ajaxRequestGetBettype = $.ajax(option);
        },
        initAutoComplete: function () {
            var txtUserName = $('#Username');
            var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({
                    serviceUrl: serUrl,
                    minChars: 2
                });
            }
        },
        submitForm: function () {
            var username = $('#Username').val();
            var odds = parseFloat($('#Odds').val());
            var isValid = true;
            if (username === '' || username === CustTurnover.defaultUserName) {
                username = "";
            }
            if (isNaN(odds) || odds == null || odds < 0) {
                odds = 0;
            }
            /* check date range ,only support two month.*/
            var fromDateTokens = $("#FromDate").val().split("/");

            var fromDate = new Date(fromDateTokens[2], fromDateTokens[0], fromDateTokens[1]);
            var toDateTokens = $("#ToDate").val().split("/");
            var toDate = new Date(toDateTokens[2], toDateTokens[0], toDateTokens[1]);
            var oneDay = 24 * 60 * 60 * 1000;

            var diffDays = Math.round(Math.abs((fromDate.getTime() - toDate.getTime()) / (oneDay)));
            if (diffDays > 31) {
                isValid = false;
                return alert(Site.getSingleResource("CustomerTurnOverNote"));
            }
            if (CustTurnover.checkedSports.length === 0) {
                isValid = false;
                $("#sport-null").removeClass("hide");
                return;
            } else {
                $("#sport-null").addClass("hide");
            }

            if (CustTurnover.checkedBettypes.length === 0) {
                isValid = false;
                $("#bettype-null").removeClass("hide");
                return;
            } else {
                $("#bettype-null").addClass("hide");
            }
            if ($("#TickerFilter input:checked").length === 0) {
                isValid = false;
                return $("#ticket-null").removeClass("hide");
            } else {
                $("#ticket-null").addClass("hide");
            }

            if (isValid) {
                //get parameters
                CustTurnover.excelParam.fromdate = $("#FromDate").val();
                CustTurnover.excelParam.todate = $("#ToDate").val();
                CustTurnover.excelParam.reportby = $("input[name='ReportBy']:checked").val();
                CustTurnoverOption.reportBy = CustTurnover.excelParam.reportby;
                CustTurnover.excelParam.showwin = $("#ShowWin").is(':checked');
                CustTurnover.excelParam.showloss = $("#ShowLoss").is(':checked');
                CustTurnover.excelParam.showdraw = $("#ShowDraw").is(':checked');
                CustTurnover.excelParam.odds = odds;
                CustTurnover.excelParam.username = username;
                CustTurnover.excelParam.currencyid = $("#CurrencyId").val();
                CustTurnover.excelParam.isexportexcel = CustTurnover.isExportExcel;
                CustTurnover.excelParam.SportProducts = CustTurnover.checkedSports;
                CustTurnover.excelParam.bettypes = CustTurnover.checkedBettypes;
                /*reset page index*/
                CustTurnover.pagingOption.pageindex = 1;
                if (CustTurnover.isExportExcel) {
                    CustTurnover.exportExcel();
                } else {
                    CustTurnover.param = Site.clone(CustTurnover.excelParam);
                    CustTurnover.loadData();
                    $(".report-title").removeClass("hide");
                }
            }
        },
        exportExcel: function () {
            var form = $("#CustomerTurnoverForm");
            /*update paging*/
            CustTurnover.excelParam.pageindex = CustTurnover.pagingOption.pageindex;
            CustTurnover.excelParam.pagesize = CustTurnover.pagingOption.pagesize;
            $("#excelparam").val(JSON.stringify(CustTurnover.excelParam));
            form.submit();
        },
        clearExcelExport: function () {
            CustTurnover.isExportExcel = false;
            $("#excelparam").val("");
        },
        loadData: function () {
            /*update paging*/
            CustTurnover.param.pageindex = CustTurnover.pagingOption.pageindex;
            CustTurnover.param.pagesize = CustTurnover.pagingOption.pagesize;
            var option = {
                type: "POST",
                data: JSON.stringify(CustTurnover.param),
                dataType: "json",
                contentType: "application/json",
                cache: false,
                url: CustTurnover.urlData,
                success: function (response) {
                    CustTurnover.gridBiding(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                }
            };
            if (CustTurnover.ajaxRequestData != null) {
                CustTurnover.ajaxRequestData.abort();
            }
            CustTurnover.ajaxRequestData = $.ajax(option);
        },
        gridBiding: function (response) {
            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");
            /*clear paging control*/
            $(CustTurnover.pagingOption.elementcontainer).html("");
            /*end*/
            var gridOption = CustTurnoverOption.getGridOption();
            CustTurnover.setReportTitle();

            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = response.Data.length,
            gridOption.rowTotal = response.Data.length;
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, 1);
                $("#pg_ppdata").hide();

                if (response == null ||
                 $.isEmptyObject(response) || response.length === 0 || response.Data.length === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
                    jqGridAdditional.setGroupFooters(CustTurnoverOption.totalColspan);
                }
                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);
            /*group header*/
            if (CustTurnoverOption.hasCustomerComm) {
                jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
            }
            else {
                jqGridAdditional.resetHeaderHeight(reportGrid);
            }
            jqGridAdditional.resetLayout(reportGrid);
            CustTurnover.pagingOption.totalrow = parseInt(response.TotalRow);
            if (CustTurnover.pagingOption.totalrow > 0) {
                pagingHeper.render(CustTurnover.pagingOption);
            }

            CustTurnover.resizeGrid();
        },
        ajaxPaging: function () {
            CustTurnover.clearExcelExport();
            CustTurnover.pagingOption = pagingHeper.getSetting();
            CustTurnover.loadData();
        },
        setReportTitle: function () {
            var titlePattern = Site.getSingleResource('Customer') + ' '
              + Site.getSingleResource('Turnover')
               + ', {0} → {1} - ' + Site.getSingleResource("Currency") + ': {2} - ' +
               Site.getSingleResource('MinimumOdds') + ': {3} {4}';

            var currency = $('#CurrencyId option[value="' + CustTurnover.param.currencyid + '"]').text();

            var tempUserName = CustTurnover.param.username.length > 0 ?
             " - " + Site.getSingleResource("Username") + ": " + CustTurnover.param.username : "";

            var title = $.format(titlePattern, CustTurnover.param.fromdate, CustTurnover.param.todate, currency, CustTurnover.param.odds, tempUserName);
            $("div.report-title").html(title);
        },
        setDefaultUsername: function () {
            var username = $('#Username').val();
            if (username === '' || username === CustTurnover.defaultUserName) {
                $('#Username').val(CustTurnover.defaultUserName);
                $('#Username').attr('class', 'default-username');
            }
            else {
                $('#Username').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#Username').val();
            if (username === CustTurnover.defaultUserName) {
                $('#Username').val('');
            } else if (username === '') {
                $('#Username').val(CustTurnover.defaultUserName);
            }
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustTurnover.initialize();
});
$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    CustTurnover.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});