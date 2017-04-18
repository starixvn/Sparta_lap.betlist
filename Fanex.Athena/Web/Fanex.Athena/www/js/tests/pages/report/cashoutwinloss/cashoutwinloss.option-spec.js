"use strict";
define(["jquery", "common/site", "lib/jqgrid/jquery.jqgrid.additional", "pages/report/cashoutwinloss/cashoutwinloss.option", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, jqGridAdditional, CashoutWinLossOption) {
        describe("CashoutWinLossOption", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
            });

            describe("initial properties should be correct", function () {
                it("Default values are correct", function () {
                    expect(CashoutWinLossOption.licenseeId).toBe(0);
                    expect(CashoutWinLossOption.isLicenseeOfferBA).toBe(false);
                    expect(CashoutWinLossOption.buyBackAmountColumnIndex).toBe(7);
                    expect(CashoutWinLossOption.totalColspan).toEqual([2, 3]);
                    expect(CashoutWinLossOption.urlData).toBe("api/CashOutWinLossReport/Index");
                    expect(CashoutWinLossOption.urlExcelData).toBe("api/CashOutWinLossReport/Index");
                });

                it("gridOption should be correct with default value", function () {
                    var gridOption = {
                        datatype: 'local',
                        colNames: [],
                        colModel: [],
                        data: [],
                        userDataOnFooter: true,
                        userData: [],
                        height: '100%',
                        autowidth: false,
                        forceFit: false,
                        shrinkToFit: true,
                        loadtext: 'loading...',
                        loadui: 'disable',
                        loadonce: true,
                        footerrow: true,
                        rowNum: 2000,
                        grouping: true,
                        viewrecords: true,
                        gridview: true,
                        pager: '#ppdata',
                        rownumbers: true
                    };

                    expect(CashoutWinLossOption.gridOption).toEqual(gridOption);
                });
            });

            describe("setGroupHeaders($grid)", function () {
                var originalValue = CashoutWinLossOption.isLicenseeOfferBA;
                var method, option;

                beforeEach(function () {
                    spyOn($.fn, "jqGrid").and.callFake(function (param1, param2) { method = param1, option = param2; });
                    setFixtures('<div id="grid"></div>');
                });

                afterEach(function () {
                    CashoutWinLossOption.isLicenseeOfferBA = originalValue;
                });

                it("jqGrid should be called with correct parameter when Licensee do not offer BA", function () {
                    var expectedResult = {
                        useColSpanStyle: true,
                        groupHeaders: [
                            {},
                            {
                                startColumnName: "SettleAmountTicketPayout",
                                numberOfColumns: 3,
                                titleText: "SettleAmount"
                            }
                        ]
                    };

                    CashoutWinLossOption.setGroupHeaders($("#grid"));

                    expect($.fn.jqGrid).toHaveBeenCalled();
                    expect(method).toBe("setGroupHeaders");
                    expect(option).toEqual(expectedResult);
                });

                it("jqGrid should be called with correct parameter when Licensee offer BA", function () {
                    CashoutWinLossOption.isLicenseeOfferBA = true;
                    var expectedResult = {
                        useColSpanStyle: true,
                        groupHeaders: [
                            {
                                startColumnName: "BuyBackAmountSB",
                                numberOfColumns: 3,
                                titleText: "BuyBackAmount"
                            },
                            {
                                startColumnName: "SettleAmountTicketPayout",
                                numberOfColumns: 3,
                                titleText: "SettleAmount"
                            }
                        ]
                    };

                    CashoutWinLossOption.setGroupHeaders($("#grid"));

                    expect($.fn.jqGrid).toHaveBeenCalled();
                    expect(method).toBe("setGroupHeaders");
                    expect(option).toEqual(expectedResult);
                });
            });

            describe("getColName()", function () {
                var originalValue = CashoutWinLossOption.isLicenseeOfferBA;

                beforeEach(function () {
                    spyOn(Site, "getResources").and.callFake(function (resource) { return resource });
                });

                afterEach(function () {
                    CashoutWinLossOption.isLicenseeOfferBA = originalValue;
                });

                it("ColName should be returned correctly when Licensee do not offer BA", function () {
                    var expected = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
                          'CashOutTimes', 'CashOutTickets', 'CashOutValue', 'TicketPayout', 'CashOutCommission',
                          'TicketPayout', 'BBAmtComm', 'Total', 'CurrencyId', 'CustomerId'];

                    var result = CashoutWinLossOption.getColName();

                    expect(result).toEqual(expected);
                });

                it("ColName should be returned correctly when Licensee offer BA", function () {
                    CashoutWinLossOption.isLicenseeOfferBA = true;
                    var expected = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
                          'CashOutTimes', 'CashOutTickets', 'CashOutValue', 'Sportsbook', 'BA', 'Total', 'TicketPayout', 'CashOutCommission',
                          'TicketPayout', 'BBAmtComm', 'Total', 'CurrencyId', 'CustomerId'];

                    var result = CashoutWinLossOption.getColName();

                    expect(result).toEqual(expected);
                });
            });
        });
    }
);