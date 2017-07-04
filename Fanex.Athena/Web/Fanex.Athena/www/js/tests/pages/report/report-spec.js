define(['jquery', 'common/site', 'common/popup', 'pages/report/report'],
    function ($, Site, Popup, Report) {
        var popupOption;
        describe("Report", function () {
            beforeEach(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });
                spyOn(Site, "initialize");
                spyOn(Popup, "openPopup").and.callFake(function ($element, option) { popupOption = option; });
                $checkbox = setFixtures("<input class='childCheckbox' type='checkbox'></input>");
            });

            describe("GetDrilldownDateRange()", function () {
                it("if report by = 1, return fromdate and todate is same as drilldowndate", function () {
                    var result = Report.GetDrilldownDateRange(1, new Date(2017, 1, 2), new Date(2017, 1, 3), new Date(2017, 1, 4));

                    expect(result.FromDate).toEqual(new Date(2017, 1, 4));
                    expect(result.ToDate).toEqual(new Date(2017, 1, 4));
                });

                it("if report by = 2, return correct fromdate and todate", function () {
                    var result = Report.GetDrilldownDateRange(2, new Date(2017, 1, 2), new Date(2017, 1, 3), "02/04/2017");

                    expect(result.FromDate).toEqual(new Date(2017, 1, 2));
                    expect(result.ToDate).toEqual(new Date(2017, 1, 3));
                });
            });

            describe("ValidProducts()", function () {
                var $checkbox;

                describe("No checkboxed were checked", function () {
                    it("Report.ValidProducts() should return false", function () {
                        var expected = false;

                        var result = Report.ValidProducts();

                        expect(result).toBe(expected);
                    });

                    it("Popup.openPopup() should be called", function () {
                        Report.ValidProducts();

                        expect(Popup.openPopup).toHaveBeenCalled();
                    });
                });

                it("Report.ValidProducts() should return value equal true when there are at least checkbox was checked", function () {
                    $(".childCheckbox").prop("checked", true);
                    var expected = true;

                    var result = Report.ValidProducts();

                    expect(result).toBe(expected);
                });

                it("call onOpen() function in Popup - #SubmitButton should be disabled", function () {
                    setFixtures('<button id="SubmitButton"></button>');

                    popupOption.onOpen();
                    var result = $("#SubmitButton").prop("disabled");

                    expect(result).toBe(true);
                });

                it("call onClose() function in Popup - #SubmitButton should be enabled", function () {
                    setFixtures('<button id="SubmitButton" disabled></button>');

                    popupOption.onClose();
                    var result = $("#SubmitButton").prop("disabled");

                    expect(result).toBe(false);
                });
            });

            describe("setOriginalProduct()", function () {
                it("data-original attribute of checkbox should have correct value", function () {
                    var $productList = setFixtures("<div id='ProductList'>" +
                        "<input id='chk1' checked class='childCheckbox' type='checkbox'></input>" +
                        "<input id='chk2' class='childCheckbox' type='checkbox'></input>" +
                        "</div>");

                    Report.setOriginalProduct();

                    expect($('#chk1').attr("data-original")).toBe("true");
                    expect($('#chk2').attr("data-original")).toBe("false");
                });
            });

            describe("getCurrencyNameForReportTitle(currencyId)", function () {
                it("return correct title for correct currencyid", function () {
                    Site.getSingleResource = jasmine.createSpy().and.returnValue("View in {0}");

                    var result = Report.getCurrencyNameForReportTitle("-3");

                    expect(result).toBe("View in GBP");
                });
                it("return correct title for currencyid from dropdownlist", function () {
                    setFixtures('<select id="CurrencyId"><option value="1">UUS</option></select>');
                    Site.getSingleResource = jasmine.createSpy().and.returnValue("View in {0}");

                    var result = Report.getCurrencyNameForReportTitle("1");

                    expect(result).toBe("View in UUS");
                });
            });

            describe("renderBreadCrumb(level)", function () {
                beforeEach(function () {
                    setFixtures('<div class="breadCrumbHolder"></div>');
                });
                it(".breadCrumbHolder should be hide if param level is 0.", function () {
                    Report.renderBreadCrumb(0);

                    expect($('.breadCrumbHolder').is(':visible')).toBe(false);
                });

                it(".level0 should show if param level > 0.", function () {
                    appendSetFixtures('<div id="level-link-container"><div class="level0"></div></div>');

                    Report.renderBreadCrumb(1);

                    expect($('.level0').is(':visible')).toBe(true);
                });

                it(".level2 should be hide if param level = 1.", function () {
                    appendSetFixtures('<div id="level-link-container"><div class="level2"></div></div>');

                    Report.renderBreadCrumb(1);

                    expect($('.level2').is(':visible')).toBe(false);
                });

                it(".level2 should show if param level = 2.", function () {
                    appendSetFixtures('<div id="level-link-container"><div class="level2"></div></div>');

                    Report.renderBreadCrumb(2);

                    expect($('.level2').is(':visible')).toBe(true);
                });
            });

            describe("bindSummaryTotal(columnModel, summaryTotal)", function () {
                var col0 = {
                    name: "Col0",
                    formatter: "format"
                }
                var col1 = {
                    name: "Col1",
                    formatter: function (value) { return value; }
                }
                var col2 = {
                    name: "Col2",
                    formatter: function (value) { return value; }
                }

                var columnModel = [col0, col1, col2];

                beforeAll(function () {
                    spyOn($.fn, "fmatter").and.callFake(function (formatter, value) { return value; });
                });

                beforeEach(function () {
                    var htmlTableFooter = '<table class="ui-jqgrid-ftable"><tr><td></td><td></td><td></td></tr></table>';

                    setFixtures(htmlTableFooter);
                });
            });
        });
    });