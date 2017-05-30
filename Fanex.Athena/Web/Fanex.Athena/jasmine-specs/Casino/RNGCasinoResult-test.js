describe("RNGCasino", function () {

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RNGCasino, "initEvent");
            spyOn(RNGCasino, "initSetting");

            RNGCasino.initialize();
        });

        it("RNGCasino.initEvent() should be called", function () {
            expect(RNGCasino.initEvent).toHaveBeenCalled();
        });

        it("RNGCasino.initSetting() should be called", function () {
            expect(RNGCasino.initSetting).toHaveBeenCalled();
        });
    });

    describe("initSetting()", function () {
        it("RNGCasinoOption.hasCustomerComm should be true", function () {
            affix("input#is-allow-cust-comm[value=true]");
            RNGCasino.initSetting();
            expect(RNGCasinoOption.hasCustomerComm).toBe(true);
        });

        it("RNGCasinoOption.hasCustomerComm should be false", function () {
            affix("input#is-allow-cust-comm[value=false]");
            RNGCasino.initSetting();
            expect(RNGCasinoOption.hasCustomerComm).toBe(false);
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix(".hand-history-detail-link[data-url=url-test]");
            affix("input#chkIsShowPic[type='checkbox']");
            affix("#ExportToExcel");
            affix("#ReportGrid .tree-wrap-ltr");

            spyOn(RNGCasino, "viewCasinoDetail");
            spyOn(Site, "redirect");
            spyOn(jqGridAdditional, "gridComplete");

            RNGCasino.initEvent();
        });

        it("RNGCasino.viewCasinoDetail(dataUrl) should be called with correct parameters when $('.hand-history-detail-link') fired click event", function () {
            $(".hand-history-detail-link").click();
            expect(RNGCasino.viewCasinoDetail).toHaveBeenCalledWith('url-test');
        });

        it("Site.redirect(url) should be called when $('#chkIsShowPic') fired click event", function () {
            $("#chkIsShowPic").click();
            expect(Site.redirect).toHaveBeenCalled();
        });

        it("Site.redirect(url) should be called with 'IsExportExcel' query string is true when $('#ExportToExcel') fired click event", function () {
            $("#ExportToExcel").click();
            expect(Site.redirect).toHaveBeenCalledWith(jasmine.stringMatching(/isexportexcel=true/i));
        });

        it("Site.redirect(url) should be called when $('#ExportToExcel') fired click event", function () {
            $("#ExportToExcel").click();
            expect(Site.redirect).toHaveBeenCalled();
        });

        it("jqGridAdditional.gridComplete(grid, gridOption, totalRowCount) should be called when $('.tree-wrap-ltr') fired click event", function () {
            $(".tree-wrap-ltr").click();
            expect(jqGridAdditional.gridComplete).toHaveBeenCalled();
        });
    });

    describe("viewCasinoDetail(dataUrl)", function () {
        it("dialog() function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            RNGCasino.viewCasinoDetail("url-test");

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("bindTotal(totalData)", function () {
        beforeEach(function () {
            affix("table.grandTotal td");
            spyOn(RNGCasinoOption, "getTotalColModel");
            spyOn(jqGridAdditional, "assignTotal");
            spyOn(jqGridAdditional, "setGroupFooters");

            RNGCasino.bindTotal(10);
        });

        it("RNGCasinoOption.getTotalColModel() should be called", function () {
            expect(RNGCasinoOption.getTotalColModel).toHaveBeenCalled();
        });

        it("jqGridAdditional.assignTotal(totalData, columnModel) should be called", function () {
            expect(jqGridAdditional.assignTotal).toHaveBeenCalled();
        });

        it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling) should be called", function () {
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalled();
        });

        it("$('.grandTotal td:first-child') should have correct css property", function () {
            expect($('.grandTotal td:first-child')).toHaveCss({ "text-align": "right" });
        });
    });

    describe("bindData(response)", function () {
        beforeEach(function () {
            affix("#ReportGrid");

            spyOn(jqGridAdditional, "clearGrid");
            spyOn(RNGCasinoOption, "getGridOption").and.returnValue({});
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "resetLayout");

            RNGCasino.bindData({ Data: [] });
        });

        it("jqGridAdditional.clearGrid(reportId) should be called with correct parameters", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
        });

        it("jqGrid(gridOption) function of jquery should be called", function () {
            expect($.fn.jqGrid).toHaveBeenCalled();
        });

        it("jqGridAdditional.resetLayout(reportGrid) should be called", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });
    });

    describe("ajaxComplete(response) should be called", function () {
        it("RNGCasino.bindData(response) should be called with correct parameters", function () {
            spyOn(RNGCasino, "bindData");

            RNGCasino.ajaxComplete({ Data: [] });

            expect(RNGCasino.bindData).toHaveBeenCalledWith({ Data: [] });
        });
    });

    describe("loadContent()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(RNGCasino, "ajaxComplete");
            spyOn(Site, "blockUI");
            spyOn(Site, "unBlockUI");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest(/HandHistory\/RNGCasinoData/)
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Data": [] }'
                });

            RNGCasino.loadContent();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("RNGCasino.ajaxComplete(response) should be called with correct parameters", function () {
            expect(RNGCasino.ajaxComplete).toHaveBeenCalledWith({ Data: [] });
        });

        it("Site.unBlockUI() should be called", function () {
            expect(Site.unBlockUI).toHaveBeenCalled();
        })
    });
});