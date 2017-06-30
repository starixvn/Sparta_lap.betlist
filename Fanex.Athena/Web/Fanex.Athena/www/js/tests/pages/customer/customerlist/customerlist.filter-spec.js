define(["jquery",
    "common/site",
    "pages/customer/customerlist/customerlist.state",
    "pages/customer/customerlist/customerlist.service",
    "components/pluginhelper/autocomplete",
    "pages/customer/customerlist/customerlist.filter", ],
    function ($, Site, CustomerListState, CustomerListService, AutoComplete, CustomerListFilter) {
        describe("CustomerListFilter", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (str) {
                    return str;
                });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
                spyOn(Site, "addUserGuide");
                spyOn(AutoComplete, "initialize");
            });

            describe("initialize", function () {
                it("initializes components for the page", function () {
                    spyOn(CustomerListFilter, "initEvent");
                    spyOn(CustomerListState, "getState");

                    CustomerListFilter.initialize();

                    expect(CustomerListState.getState).toHaveBeenCalled();
                    expect(Site.addUserGuide).toHaveBeenCalled();
                    expect(AutoComplete.initialize).toHaveBeenCalled();
                    expect(CustomerListFilter.initEvent).toHaveBeenCalled();
                });
            });

            describe("initEvent()", function () {
                it("initializes Grid column option and Odd group option", function () {
                    spyOn(CustomerListFilter, "initGridColumnOption");
                    spyOn(CustomerListFilter, "initOddGroupOption");

                    CustomerListFilter.initEvent();

                    expect(CustomerListFilter.initGridColumnOption).toHaveBeenCalled();
                    expect(CustomerListFilter.initOddGroupOption).toHaveBeenCalled();
                });

                it("CustomerListService.submit() should be called if SubmitButton is clicked", function () {
                    setFixtures('<button id="SubmitButton"></button>');
                    spyOn(CustomerListFilter, "initGridColumnOption");
                    spyOn(CustomerListFilter, "initOddGroupOption");
                    spyOn(CustomerListService, "submit");
                    spyOn(CustomerListState, "getState").and.returnValue({ pagingOption: { pageindex: 1 } });

                    CustomerListFilter.initialize();
                    CustomerListFilter.initEvent();
                    $("#SubmitButton").click();

                    expect(CustomerListService.submit).toHaveBeenCalled();
                });
            });

            describe("initGridColumnOption()", function () {
                var selectOption;
                var multiSelect;

                beforeAll(function () {
                    multiSelect = spyOn($.fn, "multiSelect").and.callFake(function (option) {
                        selectOption = option;
                    });
                });

                it("initializes grid column multiselect without checking all columns", function () {
                    setFixtures('<input id="AllowedGridColumns"'
                        + 'value="[{&quot;Name&quot;:&quot;Col1&quot;,&quot;ID&quot;:0,&quot;Checked&quot;:true}'
                        + ',{&quot;Name&quot;:&quot;Col2&quot;,&quot;ID&quot;:0,&quot;Checked&quot;:false}]">');

                    CustomerListFilter.initGridColumnOption();

                    expect(selectOption.isCheckAll).toBe(false);
                });

                it("disables SubmitButton if no column is choosen", function () {
                    setFixtures('<button id="SubmitButton"></button>');
                    multiSelect.and.returnValue([]);

                    selectOption.onCheck();

                    expect($("#SubmitButton").is(":disabled")).toBe(true);
                });

                it("enables SubmitButton if a column is choosen", function () {
                    setFixtures('<button id="SubmitButton" disabled></button>');
                    multiSelect.and.returnValue(["Col1"]);

                    selectOption.onCheck();

                    expect($("#SubmitButton").is(":disabled")).toBe(false);
                });
            });

            describe("initOddGroupOption()", function () {
                var selectOption;
                var multiSelect;

                beforeAll(function () {
                    multiSelect = spyOn($.fn, "multiSelect").and.callFake(function (option) {
                        selectOption = option;
                    });
                });

                it("does not initialize odd group multiselect if #OddsGroups is not provided", function () {
                    CustomerListFilter.initOddGroupOption();

                    expect($.fn.multiSelect).not.toHaveBeenCalled();
                });

                it("initializes odd group multiselect without checking all columns", function () {
                    setFixtures('<input id="OddsGroups"'
                        + 'value="[{&quot;Name&quot;:&quot;Col1&quot;,&quot;ID&quot;:0,&quot;Checked&quot;:true}'
                        + ',{&quot;Name&quot;:&quot;Col2&quot;,&quot;ID&quot;:0,&quot;Checked&quot;:false}]">');

                    CustomerListFilter.initOddGroupOption();

                    expect(selectOption.isCheckAll).toBe(false);
                });

                it("disables SubmitButton if no column is choosen", function () {
                    setFixtures('<button id="SubmitButton"></button>');
                    multiSelect.and.returnValue([]);

                    selectOption.onCheck();

                    expect($("#SubmitButton").is(":disabled")).toBe(true);
                });

                it("enables SubmitButton if a column is choosen", function () {
                    setFixtures('<button id="SubmitButton" disabled></button>');
                    multiSelect.and.returnValue(["Col1"]);

                    selectOption.onCheck();

                    expect($("#SubmitButton").is(":disabled")).toBe(false);
                });
            });
        });
    });