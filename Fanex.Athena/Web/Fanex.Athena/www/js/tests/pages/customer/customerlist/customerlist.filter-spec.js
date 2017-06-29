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
                spyOn(CustomerListState, "getState");
            });

            describe("initialize", function () {
                it("initializes components for the page", function () {
                    spyOn(CustomerListFilter, "initEvent");

                    CustomerListFilter.initialize();

                    expect(CustomerListState.getState).toHaveBeenCalled();
                    expect(Site.addUserGuide).toHaveBeenCalled();
                    expect(AutoComplete.initialize).toHaveBeenCalled();
                    expect(CustomerListFilter.initEvent).toHaveBeenCalled();
                });
            });
        });
    });