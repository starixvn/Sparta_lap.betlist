define(["jquery", "common/site", "pages/report/customerpendingfunds/customerpendingfunds.option", ],
    function ($, Site, CustomerPedingFundsOption) {
        describe("CustomerPedingFundsOption", function () {
            beforeAll(function () {
                spyOn(Site, "clone").and.callFake(function (obj) {
                    return obj;
                });

                spyOn(Site, "getResources").and.callFake(function (resources) {
                    return resources;
                });

                spyOn(Site, "getSingleResource").and.callFake(function (resource) {
                    return resource;
                });
            });

            describe("getGridOption(totalRows)", function () {
                it("calls function to get column names and models", function () {
                    spyOn(CustomerPedingFundsOption, "getColName");
                    spyOn(CustomerPedingFundsOption, "getColModel");

                    CustomerPedingFundsOption.getGridOption(0);

                    expect(CustomerPedingFundsOption.getColName).toHaveBeenCalled();
                    expect(CustomerPedingFundsOption.getColModel).toHaveBeenCalled();
                });
            });

            describe("getColName()", function () {
                it("returns list of column name", function () {
                    var result = CustomerPedingFundsOption.getColName();

                    expect(result.length).toBeGreaterThan(0);
                });
            });

            describe("getColModel(totalRows)", function () {
                it("returns list of column model", function () {
                    var result = CustomerPedingFundsOption.getColModel();

                    expect(result.length).toBeGreaterThan(0);
                });

                it("disables sorting if total rows is 1", function () {
                    var result = CustomerPedingFundsOption.getColModel(1);

                    $.each(result, function (index, item) {
                        expect(item.sortable).toBe(false);
                    });
                });

                it("returns correct attribute for externalId cell", function () {
                    var models = CustomerPedingFundsOption.getColModel(2);
                    var externalIdColumn = models[1];
                    var result = externalIdColumn.cellattr();

                    expect(result).toBe(' style="text-overflow: ellipsis"');
                });

                it("returns correct formatter for externalId cell", function () {
                    var expectedFormat = '<a class="bet-list-link" href="javascript:;" data-drill-down={"CustId":1,"ExternalId":2,"SystemId":3}>0</a>';

                    var models = CustomerPedingFundsOption.getColModel(2);
                    var externalIdColumn = models[1];
                    var result = externalIdColumn.formatter(0, {}, { CustId: 1, ExternalId: 2, SystemId: 3 });

                    expect(result).toEqual(expectedFormat);
                });

                it("sets index number for Index column", function () {
                    var models = CustomerPedingFundsOption.getColModel(2);
                    var indexColumn = models[0];
                    var result = indexColumn.formatter();

                    expect(result).toBe(1);
                });
            });
        });
    });