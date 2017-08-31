define([
    "jquery",
    "common/querystring",
    "common/site",
    "pages/report/thirdpartywinloss/common/thirdpartywinloss",
    "pages/report/report",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/daterangepicker",
    "helpers/daterange",
    "common/popup"],
    function ($, QueryString, Site, ThirdPartyWinLoss, Report, jqGridAdditional, DatePicker, DateHelper, Popup) {
        describe("ThirdPartyWinLoss", function () {
            beforeAll(function () {
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
                spyOn(Site, "initialize");
                spyOn(Site, "addUserGuide");
                spyOn(Site, "getSingleResource").and.callFake(function (str) {
                    return str;
                });
                spyOn(DatePicker, "initDateRangePicker");
                spyOn(DateHelper, "setConstraintMonthRange");
                spyOn(Site, "blockUI");
                spyOn(Site, "unBlockUI");
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    spyOn(ThirdPartyWinLoss, "initEvent");
                });

                it("all initial methods should be called", function () {
                    spyOn(ThirdPartyWinLoss, "initSetting");

                    ThirdPartyWinLoss.initialize();

                    expect(Site.initialize).toHaveBeenCalled();
                    expect(DatePicker.initDateRangePicker).toHaveBeenCalled();
                    expect(ThirdPartyWinLoss.initEvent).toHaveBeenCalled();
                    expect(ThirdPartyWinLoss.initSetting).toHaveBeenCalled();
                });

                it("Site.addUserGuide should be called if UserGuideResourceName is set", function () {
                    ThirdPartyWinLoss.initialize({
                        UserGuideResourceName: "UserGuide"
                    });

                    expect(Site.addUserGuide).toHaveBeenCalled();
                });

                it("DateHelper.setConstraintMonthRange should be called if ValidMonthRange is set", function () {
                    ThirdPartyWinLoss.initialize({
                        ValidMonthRange: 3
                    });

                    expect(DateHelper.setConstraintMonthRange).toHaveBeenCalled();
                });
            });
        });
    });