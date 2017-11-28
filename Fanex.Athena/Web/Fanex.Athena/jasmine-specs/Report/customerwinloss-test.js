describe("CustomerWinLoss", function () {
    
    describe("initialize()", function () {
        it("Expected functions should be call when initialize() is called", function () {
            spyOn(ProductListHelper, 'initProductList');
            spyOn(CustomerWinLoss, 'initEvent');
            spyOn(CustomerWinLoss, 'initSetting');
            spyOn(CustomerWinLoss, 'initMonthPicker');
            spyOn(Site, 'addFilterCollapseExpand');
            spyOn(Site, 'addUserGuide');

            CustomerWinLoss.initialize();

            expect(CustomerWinLoss.initEvent).toHaveBeenCalled();
            expect(CustomerWinLoss.initMonthPicker).toHaveBeenCalled();
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initMonthPicker()", function () {
        it("$.fn.monPickr() should be called on a correct selector.", function () {
            $.fn.monPickr = function () { };
            affix('input#IsHistory[value="True"]');
            affix('#DateControl');
            spyOn($.fn, 'monPickr');

            CustomerWinLoss.initMonthPicker();

            expect($.fn.monPickr.calls.mostRecent().object.selector).toBe('#DateControl');
        });
    });

    describe("initEvent()", function () {
        it("CustomerWinLoss.submit() should be called if #SubmitButton is clicked.", function () {
            var $button = affix('#SubmitButton');
            affix('#CustomerWinlossForm');

            spyOn(Report, "ValidProducts").and.returnValues(true);
            spyOn(CustomerWinLoss, 'submit');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(CustomerWinLoss.submit).toHaveBeenCalled();
        });

        describe("Export excel", function () {
            var ajaxOption;

            beforeEach(function () {
                spyOn(Downloader, "download");
                spyOn(Toast, "show");

                spyOn($, "ajax").and.callFake(function (option) {
                    option.beforeSend();
                    ajaxOption = option;
                });
            });

            it("#ExportToExcel click - ajax settings should be correct", function () {
                setFixtures('<div id="ExportToExcel"></div>');
                spyOn(Report, "ValidProducts").and.returnValues(true);
                var postData = { PageIndex: 1, PageSize: 500, OrderItem: 2, IsAscSort: true, Username: '', isExportExcel: true };

                CustomerWinLoss.initEvent();
                $('#ExportToExcel').click();

                expect(ajaxOption.type).toBe("POST");
                expect(ajaxOption.dataType).toBe("binary");
                expect(ajaxOption.responseType).toBe("blob");
                expect(ajaxOption.url).toBe("api/CustomerWinLossReport/Index");
                expect(ajaxOption.data).toEqual(postData);
            });

            it("#ExportToExcel click - beforeSend - Toast.show should be called", function () {
                setFixtures('<div id="ExportToExcel"></div>');
                spyOn(Report, "ValidProducts").and.returnValues(true);

                CustomerWinLoss.initEvent();
                $('#ExportToExcel').click();

                expect(Toast.show).toHaveBeenCalled();
            });

            it("#ExportToExcel click - call success os ajax function - Downloader.download should be called with correct parameter", function () {
                setFixtures('<div id="ExportToExcel"></div>');
                spyOn(Report, "ValidProducts").and.returnValues(true);

                CustomerWinLoss.initEvent();
                $('#ExportToExcel').click();

                var fileName = "CustomerWinLoss.xls";
                var response = "Binary Data";
                ajaxOption.success(response);

                expect(Downloader.download).toHaveBeenCalledWith(response, fileName);
            });
        });

        it("viewHistoryReport() should be called if #history-report-link is clicked.", function () {
            var $link = affix('#history-report-link');
            spyOn(CustomerWinLoss, 'viewHistoryReport');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewHistoryReport).toHaveBeenCalled();
        });

        it("viewCurrentReport() should be called if #current-report-link is clicked.", function () {
            var $link = affix('#current-report-link');
            spyOn(CustomerWinLoss, 'viewCurrentReport');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewCurrentReport).toHaveBeenCalled();
        });

        it("viewWinlossBetList() should be called with correct param when .bet-list-link is clicked.", function () {
            var $container = affix('.report-content');
            var $link = $container.affix('.bet-list-link');
            $link.attr('data-cust-id', 1);
            $link.attr('data-system-id', 2);
            $link.attr('data-external-id', 3);
            spyOn(CustomerWinLoss, 'viewWinlossBetList');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewWinlossBetList).toHaveBeenCalledWith('1', '2', '3');
        });
    });
});
