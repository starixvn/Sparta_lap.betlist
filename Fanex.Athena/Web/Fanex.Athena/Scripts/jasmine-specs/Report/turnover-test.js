describe("Turnover", function () {
    describe("Turnover.defaultUserName", function () {
        it("Value should be correct", function () {
            expect(Turnover.defaultUserName).toBe(Site.getSingleResource('ExternalIdOrSystemId'));
        });
    });

    describe("initialize()", function () {
        beforeEach(function () {
            affix("input#IsAllowedCustomerCommission[value=true]");
            affix("input#IsAllowedCasinoTransfer[value=true]");
            affix("input#IsAllowBa[value=false]");

            spyOn(Turnover, "initMonthPicker");
            spyOn(Turnover, "initEvent");
            spyOn(Turnover, "initAutoComplete");
            spyOn(Turnover, "setDefaultUsername");
            spyOn(Site, "addUserGuide");

            Turnover.initialize();
        });

        it("Turnover.initMonthPicker() should be called", function () {
            expect(Turnover.initMonthPicker).toHaveBeenCalled();
        });
        it("Turnover.initEvent() should be called", function () {
            expect(Turnover.initEvent).toHaveBeenCalled();
        });
        it("Turnover.initAutoComplete() should be called", function () {
            expect(Turnover.initAutoComplete).toHaveBeenCalled();
        });
        it("Turnover.setDefaultUsername() should be called", function () {
            expect(Turnover.setDefaultUsername).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "Turnover", title: Site.getSingleResource("Turnover") });
        });

        it("Values of TurnoverOption should be correct", function () {
            expect(TurnoverOption.hasCustomerComm).toBe(true);
            expect(TurnoverOption.hasCasinoTransfer).toBe(true);
            expect(TurnoverOption.isAllowBa).toBe(false);
        });
    });

    describe("initMonthPicker()", function () {
        it("monPickr function of jquery should be called", function () {
            spyOn($.fn, "monPickr");

            Turnover.initMonthPicker();

            expect($.fn.monPickr).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        var $username, $input, $submitButton, $exportToExcel, $isExportExcel;
        beforeEach(function () {
            $username = affix("input#Username");

            affix("#TurnoverForm input");
            $input = $("#TurnoverForm input");

            $submitButton = affix("#SubmitButton");
            $isExportExcel = affix("input#IsExportExcel[value=false]");

            $exportToExcel = affix("#ExportToExcel");

            Turnover.initEvent();
        });

        it("Turnover.setDefaultUsername() should be called when '#Username' fired blur event", function () {
            spyOn(Turnover, "setDefaultUsername");

            $username.blur();

            expect(Turnover.setDefaultUsername).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should have value equal false when 'input' fired keypress event", function () {
            var e = $.Event("keypress");
            e.keyCode = 13;
            $input.trigger(e);

            expect($isExportExcel.val()).toBe("false");
        });

        it("Turnover.submit() should be called when 'input' fired keypress event", function () {
            spyOn(Turnover, "submit");

            var e = $.Event("keypress");
            e.keyCode = 13;
            $input.trigger(e);

            expect(Turnover.submit).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should have value equal false when '#SubmitButton' fired click event", function () {
            $submitButton.click();

            expect($isExportExcel.val()).toBe("false");
        });

        it("Turnover.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(Turnover, "submit");

            $submitButton.click();

            expect(Turnover.submit).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should have value equal true when '#ExportToExcel' fired click event", function () {
            $exportToExcel.click();

            expect($isExportExcel.val()).toBe("false");
        });

        it("submit function of jquery should be called when '#ExportToExcel' fired click event", function () {
            spyOn($.fn, "submit");

            $exportToExcel.click();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("initAutoComplete()", function () {
        it("autocomplete function of jquery should be called with correct parameter", function () {
            var $username = affix("input#Username[value=user01]");
            var url = "CustomerAPI/SearchCustomer?licenseeType=&currencyId=0";

            spyOn($.fn, "autocomplete");

            Turnover.initAutoComplete();

            expect($.fn.autocomplete).toHaveBeenCalledWith({ serviceUrl: url, minChars: 2 });
        });
    });

    describe("setDefaultUsername()", function () {
        it("'#Username' should have class named 'default-username' when its value equal empty", function () {
            var $username = affix('input#Username');

            Turnover.setDefaultUsername();

            expect($username).toHaveClass("default-username");
        });
    });

    describe("setUsernameOnFocus()", function () {
        it("'#Username' value should be empty when it fired focus event", function () {
            var $username = affix('input#Username');
            $username.val(Turnover.defaultUserName);

            Turnover.setUsernameOnFocus();

            expect($username.val()).toBe("");
        });
    });

    describe("ajaxComplete(response)", function () {
        beforeEach(function () {
            spyOn(Turnover, "setReportTitle");
            spyOn(Turnover, "bindData");

            Turnover.ajaxComplete({});
        });

        it("Turnover.setReportTitle() should be called", function () {
            expect(Turnover.setReportTitle).toHaveBeenCalled();
        });
        it("Turnover.bindData(response) should be called", function () {
            expect(Turnover.bindData).toHaveBeenCalled();
        });
    });

    describe("bindData(response)", function () {
        var response = { Data: [{ Id: 1, Name: "Test01" }, { Id: 2, Name: "Test02" }, ] };
        var $reportGrid;

        beforeEach(function () {
            $reportGrid = affix("#ReportGrid");

            spyOn(jqGridAdditional, "clearGrid");
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "resetLayout");
            spyOn(Turnover, "resizeGrid");

            Turnover.bindData(response);
        });

        it("jqGridAdditional.clearGrid(gridId) should be called with correct parameter", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
        });
        it("jqGrid function of jquery should be called", function () {
            expect($.fn.jqGrid).toHaveBeenCalled();
        });

        it("jqGridAdditional.resetLayout(grid) should be called with correct parameter", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalledWith($reportGrid);
        });

        it("Turnover.resizeGrid() should be called", function () {
            expect(Turnover.resizeGrid).toHaveBeenCalled();
        });
    });

    describe("bindTotal(totalData)", function () {
        beforeEach(function () {
            spyOn(TurnoverOption, "getColModel").and.returnValue([{ name: "#", index: 0 }]);
            spyOn(jqGridAdditional, "assignTotal");
            spyOn(jqGridAdditional, "setGroupFooters");

            var totalData = { Total: 1000 };
            Turnover.bindTotal(totalData);
        });

        it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameter", function () {
            expect(jqGridAdditional.assignTotal).toHaveBeenCalledWith({ Total: 1000 }, [{ name: "#", index: 0 }]);
        });
        it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling) should be called with correct parameter", function () {
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 0, 0);
        });
    });
});