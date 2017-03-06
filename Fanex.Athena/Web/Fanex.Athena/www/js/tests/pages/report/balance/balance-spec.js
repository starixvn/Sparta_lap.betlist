define(["jquery", "common/site", "pages/report/balance/balance"], function ($, Site, Balance) {
    describe("Balance", function () {
        describe("initialize()", function () {
            beforeEach(function () {
                spyOn(Site, "initialize");
                spyOn(Balance, "initializeParam");
                spyOn(Balance, "initEvent");
                spyOn(Balance, "setHistoryDateConstraint");

                Balance.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("Balance.initializeParam() should be called", function () {
                expect(Balance.initializeParam).toHaveBeenCalled();
            });
            it("Balance.initEvent() should be called", function () {
                expect(Balance.initEvent).toHaveBeenCalled();
            });
            it("Balance.setHistoryDateConstraint() should be called", function () {
                expect(Balance.setHistoryDateConstraint).toHaveBeenCalled();
            });
        });

        describe("initEvent()", function () {
            beforeEach(function () {
                setFixtures('<button id="ExportToExcel"></button>' +
                    '<button id="SubmitButton"></button>' +
                    '<input id="Year" value="12" />' +
                    '<input id="IsExportExcel" value="false" />' +
                    '<form id="BalanceForm"></form>'
                    );

                Balance.initEvent();
            });

            describe("'#ExportToExcel' fire click event", function () {
                var spyEvent;

                beforeEach(function () {
                    spyOn(Balance, "initializeParam");
                    spyEvent = spyOnEvent($('#BalanceForm'), 'submit');
                    $('#BalanceForm').submit(function () { return false; });

                    $('#ExportToExcel').click();
                });

                it("Balance.initializeParam() should be called", function () {
                    expect(Balance.initializeParam).toHaveBeenCalled();
                });
                it("'#IsExportExcel' has correct value", function () {
                    expect($("#IsExportExcel").val()).toBe("true");
                });
                it("$('#BalanceForm') fire submit event", function () {
                    expect(spyEvent).toHaveBeenTriggered();
                });
            });

            it("Balance.setHistoryDateConstraint() should be called when '#Year' fire change event", function () {
                spyOn(Balance, "setHistoryDateConstraint");

                $("#Year").change();

                expect(Balance.setHistoryDateConstraint).toHaveBeenCalled();
            });

            it("Balance.submit() should be called when '#SubmitButton' fire click event", function () {
                spyOn(Balance, "submit");

                $("#SubmitButton").click();

                expect(Balance.submit).toHaveBeenCalled();
            });
        });

        describe("setHistoryDateConstraint()", function () {
            describe("historyDate is equal selected of '#Year'", function () {
                beforeEach(function () {
                    spyOn(Site, "getHistoryDate").and.returnValue(new Date(2017, 2, 2));
                    setFixtures('<select id="Month">' +
                        '<option selected value="0">Jan</option> <option value="1">Feb</option> <option value="2">Mar</option> <option value="3">Apr</option>' +
                        '</select>' +
                        '<input id="Year" value="2017" />'
                        );
                });

                it("'#Month' dropdown should has selected value is 2", function () {
                    Balance.setHistoryDateConstraint();

                    expect($("#Month").val()).toBe("2");
                });

                it("'#Month' dropdown should not change if selected month not lower than history date month", function () {
                    $("#Month").val("3")

                    Balance.setHistoryDateConstraint();

                    expect($("#Month").val()).toBe("3");
                });

                it("option with value 1,2 in '#Month' should be hidden", function () {
                    Balance.setHistoryDateConstraint();
                    var result = $("#Month").find("option[style$='display: none;']").length;
                    var isLastOptionHidden = $("#Month").find("option[value='2']").css("display") == "none";

                    expect(result).toBe(2);
                    expect(isLastOptionHidden).toBe(false);
                });

                it("option with value 1,2 in '#Month' should be disabled", function () {
                    Balance.setHistoryDateConstraint();
                    var result = $("#Month").find("option:disabled").length;

                    expect(result).toBe(2);
                    expect($("#Month").find("option[value='2']").prop("disabled")).toBe(false);
                });

                it("All option of month dropdown list should be disable when current year not equal to history year", function () {
                    $("#Year").val(2016);

                    Balance.setHistoryDateConstraint();
                    var hiddenChildren = $("#Month").find("option[style$='display: none;']").length;

                    expect(hiddenChildren).toBe(0);
                });

                it("All option of month dropdown list should be disable when current year not equal to history year", function () {
                    $("#Year").val("2016");

                    Balance.setHistoryDateConstraint();
                    var disabledChildren = $("#Month").find("option").not(":disabled").length;

                    expect(disabledChildren).toBe(4);
                });
            });
        });

        describe("initializeParam()", function () {
            beforeEach(function () {
                setFixtures('<input id="Month" value="2" />' +
                    '<input id="Year" value="2017" />' +
                    '<input id="FromDate" value="" /><input id="ToDate" value="" />'
                    );

                Balance.initializeParam();
            });

            it("'#FromDate' value should be correct", function () {
                expect($("#FromDate").val()).toBe("Wed Mar 01 2017");
            });

            it("'#ToDate' value should be correct", function () {
                expect($("#ToDate").val()).toBe("Fri Mar 31 2017");
            });
        });

        describe("submit()", function () {
            var spyEvent;
            beforeEach(function () {
                setFixtures('<input id="IsExportExcel" value="true" /> <form id="BalanceForm"></form>');
                spyOn(Site, "blockUI");
                spyOn(Balance, "initializeParam");
                spyEvent = spyOnEvent($('#BalanceForm'), 'submit');
                $('#BalanceForm').submit(function () { return false; });

                Balance.submit();
            });

            it("Site.blockUI() should be called", function () {
                expect(Site.blockUI).toHaveBeenCalled();
            });
            it("Balance.initializeParam() should be called", function () {
                expect(Balance.initializeParam).toHaveBeenCalled();
            });
            it("'#IsExportExcel') should has correct value", function () {
                expect($("#IsExportExcel").val()).toBe("false");
            });
            it("$('#BalanceForm') fire submit event", function () {
                expect(spyEvent).toHaveBeenTriggered();
            });
        });
    });
});