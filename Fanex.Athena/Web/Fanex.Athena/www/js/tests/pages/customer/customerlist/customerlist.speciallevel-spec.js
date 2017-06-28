define(["jquery",
    "common/site",
    "pages/customer/customerlist/customerlist.state",
    "pages/customer/customerlist/customerlist.service",
    "pages/customer/customerlist/customerlist.speciallevel"],
    function ($, Site, CustomerListState, CustomerListService, CustomerListSpecialLevel) {
        describe("CustomerListSpecialLevel", function () {
            beforeAll(function () {
                spyOn(CustomerListState, "getState");
                spyOn(Site, "getSingleResource").and.callFake(function (str) {
                    return str;
                });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
            });

            describe("initialize()", function () {
                it("get customer list stage", function () {
                    CustomerListSpecialLevel.initialize();

                    expect(CustomerListState.getState).toHaveBeenCalled();
                });

                it("CustomerListService.showGridDropdownById should be called if .specialLevel is click", function () {
                    setFixtures('<div id="report-content"><a class="specialLevel"></a></div>');
                    spyOn(CustomerListService, "showGridDropdownById");

                    CustomerListSpecialLevel.initialize();
                    $(".specialLevel").click();

                    expect(CustomerListService.showGridDropdownById).toHaveBeenCalled();
                });

                it("#LevelListContainer should be hidden if .specialLevel is loss focus", function () {
                    setFixtures('<div id="report-content"><a class="specialLevel"></a><div id="LevelListContainer"></div></div>');
                    spyOn(CustomerListService, "showGridDropdownById");

                    CustomerListSpecialLevel.initialize();
                    $(".specialLevel").focusout();

                    expect($("#LevelListContainer").is(":hidden")).toBe(true);
                });

                it("#LevelListContainer should not be hidden if .specialLevel is loss focus but levelLink is focused", function () {
                    setFixtures('<div id="report-content">' +
                        '<a href="javascript:;" class="specialLevel"></a>' +
                        '<div id="LevelListContainer" style="display:none"><div class="levelLink"></div></div>' +
                        '</div>');
                    spyOn(CustomerListService, "showGridDropdownById").and.callFake(function () {
                        $("#LevelListContainer").show();
                    });

                    CustomerListSpecialLevel.initialize();
                    $(".specialLevel").click();
                    var event = $.Event("focusout");
                    event.relatedTarget = '<div class="levelLink"></div>';
                    $(".specialLevel").trigger(event);

                    expect($("#LevelListContainer").is(":hidden")).toBe(false);
                });

                it("setValue(level) should be called if .levelLink is click", function () {
                    setFixtures('<div id="report-content">' +
                        '<a class="specialLevel"></a>' +
                        '<div id="LevelListContainer" style="display:none"><div class="levelLink" data-level="level1"></div></div>' +
                        '</div>');
                    spyOn(CustomerListSpecialLevel, "setValue");

                    CustomerListSpecialLevel.initialize();
                    $(".levelLink").click();

                    expect(CustomerListSpecialLevel.setValue).toHaveBeenCalledWith("level1");
                });
            });

            describe("setValue(level)", function () {
                it("CustomerListService.updateGridData should be called with correct params if level is 0", function () {
                    var option = {};
                    spyOn(CustomerListService, "updateGridData").and.callFake(function (actualOption) {
                        option = actualOption;
                    });

                    CustomerListSpecialLevel.setValue(0);

                    expect(option.confirmMessage).toEqual("AlertRemoveSpecialLevelForCustomerMsg");
                });

                it("CustomerListService.updateGridData should be called with correct params if level is 1", function () {
                    var option = {};
                    spyOn(CustomerListService, "updateGridData").and.callFake(function (actualOption) {
                        option = actualOption;
                    });

                    CustomerListSpecialLevel.setValue(1);

                    expect(option.confirmMessage).toEqual("AlertSetSpecialLevelForCustomerMsg");
                });
            });
        });
    });