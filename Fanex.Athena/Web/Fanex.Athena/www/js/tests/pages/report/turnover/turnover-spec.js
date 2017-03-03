define(["jquery", "common/site", "pages/report/turnover/turnover.option", "lib/jqgrid/jquery.jqgrid.additional",
        "moment", "components/pluginhelper/monthpicker", "pages/report/turnover/turnover",
        "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "extensions/jquery.extension", "materialize-autocomplete"],
    function ($, Site, TurnoverOption, jqGridAdditional, moment, MonthPicker, Turnover) {
        describe("Turnover", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { if (resource === "Viewin") { return resource + " EUR"; } return resource; });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            it("defaultUserName should be correct", function () {
                expect(Turnover.defaultUserName).toBe("");
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    setFixtures('<input id="productsData" value="SB,RC" />' +
                    '<input id="IsAllowedCustomerCommission" value="true" /> <input id="IsAllowedCasinoTransfer" value="true" /> <input id="IsAllowBa" value="true" />'
                    );
                    spyOn(Site, "initialize");
                    spyOn(Turnover, "initEvent");
                    spyOn(MonthPicker, "initMonthPicker");
                    spyOn(Site, "setMultiSelectForDropDownList");
                    spyOn(Site, "addUserGuide");
                    spyOn(Turnover, "initAutoComplete");
                    Turnover.initialize();
                });

                it("Site.initialize() should be called", function () {
                    expect(Site.initialize).toHaveBeenCalled();
                });
                it("Turnover.initEvent() should be called", function () {
                    expect(Turnover.initEvent).toHaveBeenCalled();
                });
                it("Turnover.initAutoComplete() should be called", function () {
                    expect(Turnover.initAutoComplete).toHaveBeenCalled();
                });
                it("MonthPicker.initMonthPicker() should be called", function () {
                    expect(MonthPicker.initMonthPicker).toHaveBeenCalled();
                });
                it("Site.setMultiSelectForDropDownList() should be called with correct parameter", function () {
                    expect(Site.setMultiSelectForDropDownList).toHaveBeenCalledWith('grid-products-multiselect', "SB,RC", 'Code', 'Name', 'Status');
                });
                it("Site.addUserGuide() should be called with correct parameter", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "Turnover", title: "Turnover" });
                });
                it("TurnoverOption has correct value", function () {
                    expect(TurnoverOption.hasCustomerComm).toBe(true);
                    expect(TurnoverOption.hasCasinoTransfer).toBe(true);
                    expect(TurnoverOption.isAllowBa).toBe(true);
                });
            });

            describe("initEvent()", function () {
                beforeEach(function () {
                    $("*").off();

                    setFixtures('<form id="TurnoverForm"><input id="Username" /></form>' +
                        '<input id="IsExportExcel" value="" />' +
                        '<button id="ExportToExcel">Export Excel</button>' +
                        '<div id="dropdown">Dropdown</div>' +
                        '<button id="SubmitButton">Submit</button>'
                        );
                });
                afterEach(function () {
                    $("*").off();
                });

                describe("Events relate with autocomplete", function () {
                    var $autocomplete, $dropdown;
                    beforeEach(function () {
                        spyOn(Site, "initialize");
                        spyOn(MonthPicker, "initMonthPicker");
                        spyOn(Site, "setMultiSelectForDropDownList");
                        spyOn(Site, "addUserGuide");

                        $autocomplete = $("#Username");
                        $dropdown = $("#dropdown");
                        $autocomplete.$dropdown = $dropdown;
                        spyOn($.fn, "materialize_autocomplete").and.returnValue($autocomplete);
                        Turnover.initialize();
                    });

                    describe("'#TurnoverForm input' fire keypress event", function () {
                        beforeEach(function () {
                            var e = $.Event('keypress', { keyCode: 13, which: 13 });
                            spyOn(Turnover, "submit");

                            $("#TurnoverForm input").trigger(e);
                        });

                        it("'#IsExportExcel' has value is false", function () {
                            expect($("#IsExportExcel").val()).toBe("false");
                        });

                        it("Turnover.submit() should be called", function () {
                            expect(Turnover.submit).toHaveBeenCalled();
                        });

                        it("Dropdown of auto complete should be hidden", function () {
                            expect($dropdown).toBeHidden();
                        });
                    });

                    describe("body fire click event", function () {
                        it("Dropdown of auto complete should be hidden", function () {
                            $("body").click();
                            expect($dropdown).toBeHidden();
                        });
                    });
                });

                describe("Events that not relate with autocomplete", function () {
                    beforeEach(function () {
                        Turnover.initEvent();
                        $("body").off("click");
                    });

                    describe("'#SubmitButton' fire click event", function () {
                        beforeEach(function () {
                            spyOn(Turnover, "submit");
                            $("#SubmitButton").click();
                        });
                        it("'#IsExportExcel' should has false value", function () {
                            expect($("#IsExportExcel").val()).toBe("false");
                        });
                        it("Turnover.submit() should be called", function () {
                            expect(Turnover.submit).toHaveBeenCalled();
                        });
                    });

                    describe("'#ExportToExcel' fire click event", function () {
                        var spyEvent;
                        beforeEach(function () {
                            spyOn($.fn, "val");

                            spyEvent = spyOnEvent($('#TurnoverForm'), 'submit');
                            $('#TurnoverForm').submit(function () { return false; });

                            $("#ExportToExcel").click();
                        });

                        it("'#IsExportExcel' is set value 2 times with correct parameter", function () {
                            expect($.fn.val.calls.argsFor(0)[0]).toBe(true);
                            expect($.fn.val.calls.argsFor(1)[0]).toBe(false);
                        });

                        it("'#TurnoverForm' should has correct value of action attribute", function () {
                            expect($('#TurnoverForm').attr("action")).toBe("TurnoverReport/Excel");
                        });

                        it("'#TurnoverForm' fired submit event", function () {
                            expect(spyEvent).toHaveBeenTriggered();
                        });
                    });

                    describe("window fire resize event", function () {
                        beforeEach(function () {
                            spyOn(Turnover, "resizeGrid");
                            spyOn(Turnover, "updateWidthOfAutoCompleteDropdown");

                            $(window).resize();
                        });

                        it("Turnover.resizeGrid() should be called", function () {
                            expect(Turnover.resizeGrid).toHaveBeenCalled();
                        });
                        it("Turnover.updateWidthOfAutoCompleteDropdown() should be called", function () {
                            expect(Turnover.updateWidthOfAutoCompleteDropdown).toHaveBeenCalled();
                        });
                    });

                    it(".off function should be called with correct parameter when window fire unload event", function () {
                        spyOn($.fn, "off");

                        $(window).unload();

                        expect($.fn.off).toHaveBeenCalledWith("resize");
                    });
                });
            });

            describe("initAutoComplete()", function () {
                var option;
                beforeEach(function () {
                    spyOn($.fn, "materialize_autocomplete").and.callFake(function (opt) { option = opt; });
                    spyOn(Turnover, "updateWidthOfAutoCompleteDropdown");

                    Turnover.initAutoComplete();
                });

                it("Auto complete should be called with correct parameter", function () {
                    expect(option.ignoreCase).toBe(false);
                    expect(option.multiple).toEqual({ enable: false });
                    expect(option.dropdown).toEqual({ el: "#suggestionDropdown" });
                });

                it("Turnover.updateWidthOfAutoCompleteDropdown() should be called", function () {
                    expect(Turnover.updateWidthOfAutoCompleteDropdown).toHaveBeenCalled();
                });

                describe("option.getData(value, callback)", function () {
                    var request, callback;
                    beforeEach(function (done) {
                        var value = "a";
                        callback = jasmine.createSpy('callback');
                        jasmine.Ajax.install();

                        jasmine.Ajax
                            .stubRequest(/.*CustomerAPI\/SearchCustomer\?licenseeType\=\&currencyId\=0\&query=a.*/)
                            .andReturn({
                                'status': 200,
                                'contentType': 'application/json',
                                'responseText': '{ "suggestions": ["abc", "asd"] }'
                            });

                        option.getData(value, callback);
                        request = jasmine.Ajax.requests.mostRecent();

                        setTimeout(done, 10);
                    });

                    afterEach(function () {
                        jasmine.Ajax.uninstall();
                    });

                    it("Ajax option should be correct", function () {
                        var url = "CustomerAPI/SearchCustomer?licenseeType=&currencyId=0&query=a";
                        expect(request.method).toBe("GET");
                        expect(request.url.indexOf(url) > -1).toBe(true);
                    });

                    it("callback function should be called with correct parameter", function () {
                        expect(callback).toHaveBeenCalledWith("a", [{ id: "abc", text: "abc" }, { id: "asd", text: "asd" }]);
                    });
                });
            });

            describe("updateWidthOfAutoCompleteDropdown()", function () {
                beforeEach(function () {
                    spyOn(Site, "initialize");
                    spyOn(Turnover, "initEvent");
                    spyOn(Turnover, "initAutoComplete");
                    spyOn(MonthPicker, "initMonthPicker");
                    spyOn(Site, "setMultiSelectForDropDownList");
                    spyOn(Site, "addUserGuide");

                    setFixtures('<div id="suggestionDropdown" style="width: 200px;"></div> <input id="Username" />');
                });

                it("'#suggestionDropdown' should has correct width value when its width not equal $autoCompleteTarget width", function () {
                    $("#Username").width(300);
                    Turnover.initialize();
                    Turnover.updateWidthOfAutoCompleteDropdown();

                    var result = $("#suggestionDropdown").width();

                    expect(result).toBe(300);
                });

                it("'#suggestionDropdown' should has correct width value when its width equal $autoCompleteTarget width", function () {
                    $("#Username").width(200);
                    Turnover.initialize();
                    Turnover.updateWidthOfAutoCompleteDropdown();

                    var result = $("#suggestionDropdown").width();

                    expect(result).toBe(200);
                });
            });

            describe("setReportTitle()", function () {
                beforeEach(function () {
                    setFixtures('<input id="Username" />' +
                        '<select id="CurrencyId"><option value="undefined">EUR</option></select>' +
                        '<div class="panel-heading hide"><div class="report-title"></div></div>' +
                        '<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />'
                        );
                    Turnover.setDateParam();
                });

                it("'.report-title' should have correct text when '#Username' is empty", function () {
                    var expected = "Turnover, 03/01/2017 → 03/31/2017 - Viewin EUR";
                    Turnover.setReportTitle();

                    expect($(".report-title").html()).toBe(expected);
                });

                it("'.report-title' should have correct text when '#Username' has value", function () {
                    $("#Username").val("Walter");
                    var expected = "Turnover, 03/01/2017 → 03/31/2017 - Customer Walter (EUR)";
                    Turnover.setReportTitle();

                    expect($(".report-title").html()).toBe(expected);
                });

                it("'.panel-heading' remove class 'hide'", function () {
                    Turnover.setReportTitle();
                    expect($(".panel-heading").hasClass("hide")).toBe(false);
                });
            });

            describe("ajaxComplete(response)", function () {
                beforeEach(function () {
                    spyOn(Turnover, "setReportTitle");
                    spyOn(Turnover, "bindData");
                });

                it("Turnover.setReportTitle() should be called when response param not null", function () {
                    Turnover.ajaxComplete("data");
                    expect(Turnover.setReportTitle).toHaveBeenCalled();
                });

                it("Turnover.bindData() should be called with correct parameter when response param not null", function () {
                    Turnover.ajaxComplete("data");
                    expect(Turnover.bindData).toHaveBeenCalledWith("data");
                });

                it("Turnover.setReportTitle() is not called when response param is null", function () {
                    Turnover.ajaxComplete(null);
                    expect(Turnover.setReportTitle).not.toHaveBeenCalled();
                });

                it("Turnover.bindData() is not called when response param is null", function () {
                    Turnover.ajaxComplete(null);
                    expect(Turnover.bindData).not.toHaveBeenCalled();
                });
            });

            describe("setDateParam()", function () {
                beforeEach(function () {
                    setFixtures('<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />' +
                        '<input id="FromDay" /> <input id="ToDay" /> <input id="Month" /> <input id="Year" />'
                        );

                    Turnover.setDateParam();
                });

                it("inputs value should be correct", function () {
                    expect($("#FromDay").val()).toBe("1");
                    expect($("#ToDay").val()).toBe("31");
                    expect($("#Month").val()).toBe("3");
                    expect($("#Year").val()).toBe("2017");
                });
            });

            describe("submit()", function () {
                var request;
                beforeEach(function (done) {
                    setFixtures('<div id="ProductList"><input type="checkbox" checked class="childCheckbox" /></div>' +
                        '<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />' +
                        '<input id="FromDay" /> <input id="ToDay" /> <input id="Month" /> <input id="Year" />' +
                        '<input id="CurrencyName" value="EUR" /> <input id="CurrencyId" value="1" />'
                        );

                    spyOn(Site, "blockUI");
                    spyOn(Site, "unBlockUI");
                    spyOn(Turnover, "setDateParam");
                    spyOn($.fn, "serializeObject").and.returnValue({ Username: "Walter", BaseCurrency: "EUR" });
                    spyOn(Turnover, "ajaxComplete");

                    jasmine.Ajax.install();

                    jasmine.Ajax
                        .stubRequest("reportapi/Turnover")
                        .andReturn({
                            'status': 200,
                            'contentType': 'application/json',
                            'responseText': '{ "Data": ["abc", "asd"] }'
                        });

                    Turnover.submit();
                    request = jasmine.Ajax.requests.mostRecent();

                    setTimeout(done, 10);
                });

                afterEach(function () {
                    jasmine.Ajax.uninstall();
                });

                it("ajax should be called with correct parameter", function () {
                    expect(request.method).toBe("POST");
                    expect(request.data()).toEqual({ Username: ["Walter"], BaseCurrency: ["EUR"] });
                    expect(request.url).toBe("reportapi/Turnover");
                });

                it("Turnover.ajaxComplete(response) should be called with correct parameter", function () {
                    expect(Turnover.ajaxComplete).toHaveBeenCalledWith({ Data: ["abc", "asd"] });
                });

                it("Turnover.setDateParam() should be called", function () {
                    expect(Turnover.setDateParam).toHaveBeenCalled();
                });

                it("Site.blockUI() should be called", function () {
                    expect(Site.blockUI).toHaveBeenCalled();
                });

                it("Site.unBlockUI() should be called", function () {
                    expect(Site.unBlockUI).toHaveBeenCalled();
                });
            });

            describe("bindData(response)", function () {
                var option;
                beforeEach(function () {
                    setFixtures('<div id="ReportGrid"></div>');
                    spyOn(jqGridAdditional, "clearGrid");
                    spyOn(jqGridAdditional, "setNoInfo");
                    spyOn(TurnoverOption, "getGridOption").and.returnValue({});
                    spyOn($.fn, "jqGrid").and.callFake(function (opt) { if (typeof opt === "string") { return; } option = opt; });
                });

                it("jqGridAdditional.clearGrid() should be called", function () {
                    Turnover.bindData({ Data: [] });
                    expect(jqGridAdditional.clearGrid).toHaveBeenCalled();
                });
                it("jqGridAdditional.setNoInfo() should be called when have no data", function () {
                    Turnover.bindData({ Data: [] });
                    expect(jqGridAdditional.setNoInfo).toHaveBeenCalled();
                });
                it(".jqgrid function should be called with correct parameters", function () {
                    Turnover.bindData({ Data: [1, 2] });

                    expect($.fn.jqGrid).toHaveBeenCalled();
                    expect(option.data).toEqual([1, 2]);
                });

                describe("gridOption.gridComplete()", function () {
                    beforeEach(function () {
                        spyOn(jqGridAdditional, "gridComplete");
                        spyOn(Turnover, "bindTotal");
                        spyOn(Turnover, "resizeGrid");
                        spyOn(Site, "unBlockUI");

                        Turnover.bindData({ Data: [1, 2], TotalData: [3, 4] });
                    });

                    it("jqGridAdditional.gridComplete() should be called", function () {
                        option.gridComplete();
                        expect(jqGridAdditional.gridComplete).toHaveBeenCalled();
                    });
                    it("Turnover.bindTotal(totaolData) should be called with correct parameter", function () {
                        option.gridComplete();
                        expect(Turnover.bindTotal).toHaveBeenCalledWith([3, 4]);
                    });

                    it(".jqGrid function should be called with correct parameter when TurnoverOption.hasCustomerComm is true", function () {
                        TurnoverOption.hasCustomerComm = true;
                        option.gridComplete();
                        var groupHeader = [{
                            startColumnName: "TurnoverBA",
                            numberOfColumns: 3,
                            titleText: "BA"
                        },{
                            startColumnName: "WinLoss",
                            numberOfColumns: 3,
                            titleText: "CustomerW_L"
                        }];

                        expect($.fn.jqGrid).toHaveBeenCalledWith("setGroupHeaders", { useColSpanStyle: true, groupHeaders: groupHeader });
                    });
                    it(".jqGrid function should be called with correct parameter when TurnoverOption.hasCustomerComm is false", function () {
                        TurnoverOption.hasCustomerComm = false;
                        option.gridComplete();
                        var groupHeader = [{
                            startColumnName: "TurnoverBA",
                            numberOfColumns: 3,
                            titleText: "BA"
                        }];

                        expect($.fn.jqGrid).toHaveBeenCalledWith("setGroupHeaders", { useColSpanStyle: true, groupHeaders: groupHeader });
                    });

                    it("Turnover.resizeGrid should be called", function () {
                        option.gridComplete();
                        expect(Turnover.resizeGrid).toHaveBeenCalled();
                    });
                    it("Site.unBlockUI() should be called", function () {
                        option.gridComplete();
                        expect(Site.unBlockUI).toHaveBeenCalled();
                    });
                });
            });

            describe("bindTotal(totalData)", function () {
                beforeEach(function () {
                    setFixtures('<div class="ui-jqgrid-sdiv"><table>' +
                        '<td></td><td aria-describedby=ReportGrid_OpeningBalance></td><td aria-describedby=ReportGrid_ClosingBalance></td><td></td>' +
                        '</table></div>'
                        );
                    spyOn(TurnoverOption, "getColModel").and.returnValue(["col1", "col2"]);
                    spyOn(jqGridAdditional, "assignTotal");
                    spyOn(jqGridAdditional, "setGroupFooters");

                    Turnover.bindTotal([3, 4]);
                });

                it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameter", function () {
                    expect(jqGridAdditional.assignTotal).toHaveBeenCalledWith([3, 4], ["col1", "col2"]);
                });

                it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling) should be called with correct parameter", function () {
                    expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 0, 0);
                });

                it("cell OpeningBalance and ClosingBalance should have correct text", function () {
                    var $tr = $(".ui-jqgrid-sdiv table tr");
                    var htmlOpeningBalance = $tr.find("td[aria-describedby=ReportGrid_OpeningBalance]").html();
                    var htmlClosingBalance = $tr.find("td[aria-describedby=ReportGrid_ClosingBalance]").html();

                    expect(htmlOpeningBalance).toBe("-");
                    expect(htmlClosingBalance).toBe("-");
                });

                it("cell OpeningBalance and ClosingBalance should have correct text-align css", function () {
                    var $tr = $(".ui-jqgrid-sdiv table tr");
                    var textAlignOpeningBalance = $tr.find("td[aria-describedby=ReportGrid_OpeningBalance]").css("text-align");
                    var textAlignClosingBalance = $tr.find("td[aria-describedby=ReportGrid_ClosingBalance]").css("text-align");

                    expect(textAlignOpeningBalance).toBe("center");
                    expect(textAlignClosingBalance).toBe("center");
                });
            });

            describe("resizeGrid()", function () {
                beforeEach(function () {
                    setFixtures('<div id="ReportGrid"></div>');
                    spyOn(jqGridAdditional, "updateGridHeight");
                    spyOn(jqGridAdditional, "updateGridWidth");
                    Turnover.resizeGrid();
                });

                it("jqGridAdditional.updateGridHeight() should be called", function () {
                    expect(jqGridAdditional.updateGridHeight).toHaveBeenCalled();
                });
                it("jqGridAdditional.updateGridWidth(grid) should be called with correct parameter", function () {
                    expect(jqGridAdditional.updateGridWidth).toHaveBeenCalledWith($("#ReportGrid"));
                });
            });
        });
    });