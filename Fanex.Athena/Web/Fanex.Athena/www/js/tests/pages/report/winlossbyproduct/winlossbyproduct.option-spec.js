define(["jquery", "common/site", "pages/report/winlossbyproduct/winlossbyproduct.option"],
    function ($, Site, WinLossByProductOption) {
        
        describe("WinLossByProductOption", function () {
            beforeAll(function () {
                spyOn(Site, "getResources").and.callFake(function (resource) { return resource; });
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
            });

            describe("Default Option", function () {
                it("Initial values should be correct", function () {
                    var expectedGridOption = {
                        datatype: 'local',
                        colNames: [],
                        colModel: [],
                        data: [],
                        height: 'auto',
                        autowidth: false,
                        forceFit: false,
                        shrinkToFit: true,
                        loadtext: 'loading...',
                        loadui: 'disable',
                        loadonce: true,
                        footerrow: true,
                        pager: '#ppdata',
                        userDataOnFooter: true,
                        userData: [],
                        viewrecords: true,
                        gridview: true,
                        sortname: "Index",
                        rowList: [50, 100, 200, 300, 400, 500, 1000, 2000]
                    };
                    expect(WinLossByProductOption.currencyIndex).toBe(4);
                    expect(WinLossByProductOption.systemIdIndex).toBe(3);
                    expect(WinLossByProductOption.externalIdIndex).toBe(2);
                    expect(WinLossByProductOption.customerWlTotalIndex).toBe(10);

                    expect(WinLossByProductOption.pageSize).toBe(50);
                    expect(WinLossByProductOption.nonePagingSize).toBe(2000);
                    expect(WinLossByProductOption.rowNum).toBe(50);
                    expect(WinLossByProductOption.totalColspan).toEqual([1, 2, 3]);
                    expect(WinLossByProductOption.urlData).toBe("api/WinLossByProductReport/Index");
                    expect(WinLossByProductOption.urlExcelData).toBe("api/WinLossByProductReport/Index");

                    expect(WinLossByProductOption.gridOption).toEqual(expectedGridOption);
                });
            });

            describe("getColName()", function () {
                it("colNameList length should equal 14", function () {
                    expect(WinLossByProductOption.getColName().length).toBe(14);
                });

                it("index of customerWlTotalIndex should has value 'Total' when 'hasCustomerComm' equal true", function () {
                    WinLossByProductOption.hasCustomerComm = true;
                    var colNameList = WinLossByProductOption.getColName();
                    expect(colNameList[WinLossByProductOption.customerWlTotalIndex]).toBe("Total");
                });

                it("index of customerWlTotalIndex should has value 'CustomerW_L' when 'hasCustomerComm' equal false", function () {
                    WinLossByProductOption.hasCustomerComm = false;
                    var colNameList = WinLossByProductOption.getColName();
                    expect(colNameList[WinLossByProductOption.customerWlTotalIndex]).toBe("CustomerW_L");
                });
            });

            describe("getColModel(level, reportBy)", function () {
                it("colModelList length should equal 14", function () {
                    expect(WinLossByProductOption.getColModel(0, 2).length).toBe(14);
                });

                it("dateColumn of colModel should be hidden when level greater than 0", function () {
                    var colModel = WinLossByProductOption.getColModel(1, 2);
                    var dateColumn = colModel[1];
                    expect(dateColumn.hidden).toBe(true);
                });

                it("dateColumn of colModel should be showed when level equal 0", function () {
                    var colModel = WinLossByProductOption.getColModel(0, 2);
                    var dateColumn = colModel[1];
                    expect(dateColumn.hidden).toBe(false);
                });

                it("format option of dateColumn should be 'm/Y' when reportBy equal 2", function () {
                    var colModel = WinLossByProductOption.getColModel(0, 2);
                    var dateColumn = colModel[1];
                    expect(dateColumn.formatoptions.newformat).toBe("m/Y");
                });

                it("format option of dateColumn should be 'm/d/Y' when reportBy not equal 2", function () {
                    var colModel = WinLossByProductOption.getColModel(0, 1);
                    var dateColumn = colModel[1];
                    expect(dateColumn.formatoptions.newformat).toBe("m/d/Y");
                });
            });

            describe("getGridOption(level, reportBy)", function () {
                it("gridOption.grouping should be true when level equal 0", function () {
                    var gridOption = WinLossByProductOption.getGridOption(0, 1);

                    expect(gridOption.grouping).toBe(true);
                });

                it("gridOption.groupingView should be correct object when level equal 0", function () {
                    var gridOption = WinLossByProductOption.getGridOption(0, 1);

                    expect(gridOption.groupingView).toEqual({
                        groupField: ['Date'],
                        groupColumnShow: [false],
                        plusicon: "ui-icon-circlesmall-plus icon-arrow",
                        minusicon: "ui-icon-circlesmall-minus icon-arrow"
                    });
                });

                it("gridOption.grouping should be false when level not equal 0", function () {
                    var gridOption = WinLossByProductOption.getGridOption(1, 1);

                    expect(gridOption.grouping).toBe(false);
                });

                it("gridOption.groupingView should be null when level not equal 0", function () {
                    var gridOption = WinLossByProductOption.getGridOption(1, 1);

                    expect(gridOption.groupingView).toEqual(null);
                });
            });

            describe("getTotalColModel(level)", function () {
                it("formatter of currencyColumn should be null", function () {
                    var colModel = WinLossByProductOption.getTotalColModel(0);

                    expect(colModel[4].formatter).toBe(null);
                });
            });
        });
    });