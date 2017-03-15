define(['jquery', 'common/site', 'pages/report/livereport/livereport', 'pages/report/livereport/helper', 'moment', "components/pluginhelper/daterangepicker", "components/pluginhelper/monthpicker"],
    function ($, Site, LiveReport, Helper, moment, DatePicker, MonthPicker) {

    describe("livereport.js", function () {
        beforeAll(function () {            
            spyOn(Site, "getSingleResource").and.callFake(function (text) {
                return text;
            });
            spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                return url;
            });
            spyOn(Site, "initialize");
            spyOn(Site, "addUserGuide");

            spyOn(DatePicker, "initDateRangePicker");
            spyOn(MonthPicker, "initMonthPicker");

            setFixtures('<input id="HistoryDate" value="1/1/2016" />');
        });

        describe("initialize()", function () {
            beforeEach(function () {
                LiveReport.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });

            it("DatePicker.initDateRangePicker() should be called", function () {
                expect(DatePicker.initDateRangePicker).toHaveBeenCalled();
            });

            it("MonthPicker.initMonthPicker() should be called", function () {
                expect(MonthPicker.initMonthPicker).toHaveBeenCalled();
            });

            it("Site.addUserGuide(option) should be called", function () {
                expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "LiveReport", title: "LiveReport"});
            });
        });

        describe("when clicking on #switchreport", function () {
            it("Site.redirect() is called with correct url when click", function () {
                spyOn(Site, "redirect");
                setFixtures('<a id="switchreport"></a>');
              
                LiveReport.initialize();
                $('#switchreport').click();
                expect(Site.redirect).toHaveBeenCalledWith('LiveReport?isViewedHistory=False');
            });
        });

        describe("view product detail", function () {
            beforeEach(function () {
                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest('LiveReport/Detail')
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': 'success'
                    });
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("ajax should be called with correct url and parameters when '.product-detail-link' is clicked", function (done) {
                setFixtures("<div class='product-detail-link'></div>");
                appendSetFixtures("<div id='FromDate' data-original-from='1/1/2016'></div>");
                appendSetFixtures("<div id='ToDate' data-original-to='2/2/2016'></div>");
                LiveReport.initialize();
                
                $('.product-detail-link').click();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
                expect(request.url).toBe('LiveReport/Detail');
                expect(request.params).toBe('FromDate=1%2F1%2F2016&ToDate=2%2F2%2F2016&CurrencyName=');
            });
        });

        describe("view sport detail", function () {
            it("Site.redirect() is called with correct url when clicking on sportid = 1", function () {
                spyOn(Site, "redirect");
                setFixtures("<a class='sport-detailt-link' data-drill-down='{\"SportId\":1}'></a>");
                LiveReport.initialize();
                $('.sport-detailt-link').click();
                expect(Site.redirect).toHaveBeenCalledWith('LiveReport/MatchList?currencyname=&sportid=1&showsb=true&showba=false');
            });

            it("Site.redirect() is called with correct url when clicking on sportid = 161", function () {
                spyOn(Site, "redirect");
                spyOn(Helper, "setSumParams").and.callFake(function (url) {
                    return url;
                });
                setFixtures("<a class='sport-detailt-link' data-drill-down='{\"SportId\":161}'></a>");
                LiveReport.initialize();
                $('.sport-detailt-link').click();
                expect(Site.redirect).toHaveBeenCalledWith('LiveReportBetList/NumberGame?currencyname=&showsb=false&showba=false');
            });
        });
    });
});