define(['jquery', 'common/site'], function ($, Site) {
    describe("Site", function () {
        describe("getLicenseeId()", function () {
            it("return a value from input#LicenseeId", function () {
                setFixtures('<input id="LicenseeId" value="2" />');
                var result = Site.getLicenseeId();

                expect(result).toBe(2);
            });
        });

        describe("asNull(text)", function () {
            it("return null if the param 'text' is an empty string", function () {
                var result = Site.asNull("");
                expect(result).toBe(null);
            });
        });

        describe("blockUI()", function () {
            it("div.block-body and div.loading will be display", function () {
                setFixtures('<div class="block-body" style="display:none"></div>');
                appendSetFixtures('<div class="loading" style="display:none"></div>');
                Site.blockUI();
                var result = $('div.block-body').is(':visible') && $('div.loading').is(':visible');
                expect(result).toBe(true);
            });
        });

        describe("setLicenseeOptionParam(urlOrParams)", function () {
            beforeEach(function () {
                setFixtures('<input id="LicenseeType" value="1" />');
                appendSetFixtures('<select id="CurrencyId"><option value="1" /><option value="2" /></select>');
                appendSetFixtures('<input id="BaseCurrency" value="USD" />');
            });

            it("return an url with query string if 'urlOrParams' is a string", function () {
                var result = Site.setLicenseeOptionParam("localhost");
                expect(result).toBe("localhost?licenseetype=1&currencyid=1&currencyname=&basecurrency=USD");
            });

            it("return an object with query string if 'urlOrParams' is an object", function () {
                var result = Site.setLicenseeOptionParam({});
                expect(result).toEqual({ LicenseeType: '1', CurrencyId: '1', CurrencyName: '', BaseCurrency: 'USD' });
            });
        });

        describe("setLicenseeOptionOriginalParam(urlOrParams)", function () {
            it("build an url from 'data-original' attributes of html tags", function () {
                setFixtures('<input id="LicenseeType" data-original="1" />');
                appendSetFixtures('<select id="CurrencyId" data-original="2" />');
                appendSetFixtures('<input id="BaseCurrency" data-original="USD" />');

                var result = Site.setLicenseeOptionOriginalParam("localhost");
                expect(result).toBe("localhost?licenseetype=1&currencyid=2&currencyname=&basecurrency=USD");
            });
        });

        describe("updateOriginalLicenseeOption()", function () {
            it("update values of html tags to 'data-original' attributes", function () {
                setFixtures('<input id="LicenseeType" value="1">');
                appendSetFixtures('<input id="BaseCurrency" value="USD">');
                Site.updateOriginalLicenseeOption();
                var result = $("#LicenseeType").attr("data-original") === "1" && $("#BaseCurrency").attr("data-original") === "USD";
                expect(result).toBe(true);
            });
        });

        describe("setProductOptionParam(url, counter, productCode, status)", function () {
            it("build an url with production options", function () {
                var result = Site.setProductOptionParam("localhost", 1, "SB", true);
                expect(result).toBe("localhost?productoptions[1].code=SB&productoptions[1].name=SB&productoptions[1].status=true");
            });
        });

        describe("initBlockUI()", function () {
            it(".block-body should be display when clicking on .blockui element.", function () {
                setFixtures('<div class="blockui"></div>');
                appendSetFixtures('<div class="block-body" style="display:none"></div>');

                Site.initialize();
                $('.blockui').click();
                expect($('.block-body').is(':visible')).toBe(true);
            });
        });

        describe("asBool()", function () {
            it("convert a string 'TRUE' to true (boolean)", function () {
                var result = Site.asBool("TRUE");
                expect(result).toBe(true);
            });

            it("convert a string '123' to false (boolean)", function () {
                var result = Site.asBool("123");
                expect(result).toBe(false);
            });

            it("convert a number 123 to true (boolean)", function () {
                var result = Site.asBool(123);
                expect(result).toBe(true);
            });

            it("convert an object to true (boolean)", function () {
                var result = Site.asBool({});
                expect(result).toBe(true);
            });

            it("convert null to false (boolean)", function () {
                var result = Site.asBool(null);
                expect(result).toBe(false);
            });
        });
    });
});