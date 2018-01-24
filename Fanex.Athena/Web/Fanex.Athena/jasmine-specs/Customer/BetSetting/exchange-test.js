describe("Exchange", function () {
    describe("initialize", function () {
        it("#btnSubmit fire click event - updateComm() should be called", function () {
            var htmlButton = '<button id="btnSubmit" data-cust-id="1234"></button>' +
                '<select id="ddlCommission" data-original-value="123"><option value="234"></option></select>';
            setFixtures(htmlButton);
            spyOn(Exchange, "updateComm");

            Exchange.initialize();
            $("#btnSubmit").click();

            expect(Exchange.updateComm).toHaveBeenCalledWith("1234", 123, "234");
        });
    });

    describe("updateComm(custId, oldCommission, newCommission)", function () {
        var custId = "1234";
        var oldCommission = "2.5";
        var newCommission = "4.5"
        var url = "localhost/ExchangeDcsBetSetting/UpdateComm";
        var ajaxOption;

        beforeAll(function () {
            spyOn(Site, "getSingleResource").and.callFake(function (key) { return key; });
            spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return "localhost/" + url; });
            spyOn($, "ajax").and.callFake(function (option) { ajaxOption = option; });
        });

        it("ajax option should be correct", function () {
            Exchange.updateComm(custId, oldCommission, newCommission);

            expect(ajaxOption.type).toBe("POST");
            expect(ajaxOption.cache).toBe(false);
            expect(ajaxOption.url).toBe(url);
            expect(ajaxOption.data).toEqual({ custid: custId, oldCommission: oldCommission, newCommission: newCommission });
            expect(ajaxOption.dataType).toBe("json");
        });

        it("call success in ajax option - Code in response is 0 (success) - BetSetting.showUpdateSettingResult should be called with correct parameter", function () {
            spyOn(BetSetting, "showUpdateSettingResult");
            var message = "Success";
            var response = { Code: 0, Message: message };

            Exchange.updateComm(custId, oldCommission, newCommission);
            ajaxOption.success(response);

            expect(BetSetting.showUpdateSettingResult).toHaveBeenCalledWith(response);
        });
    });
});