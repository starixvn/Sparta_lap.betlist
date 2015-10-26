/// <reference path="../../jasmine/jasmine-jquery.js" />
/// <reference path="../../jasmine/jasmine-fixture.js" />
/// <reference path="../../Global/site.js" />

describe("site.js", function () {
    describe("getLicenseeId()", function () {
        it("return a value from input#LicenseeId", function () {
            affix("input#LicenseeId[value=1]");             
            var result = Site.getLicenseeId();
            
            expect(result).toBe(1);
        });
    });

    describe("asNull(text)", function (currencyId) {
        it("return null if the param 'text' is an empty string", function () {
            var result = Site.asNull("");
            expect(result).toBe(null);
        });
    });

    describe("blockUI()", function (currencyId) {
        it("div.loading will be display", function () {
            affix("div.block-body[style=display:none]");
            affix("div.loading[style=display:none]");
            Site.blockUI();
            var result = $('div.block-body').is(':visible') && $('div.loading').is(':visible');
            expect(result).toBe(true);            
        });
    });

    describe("setLicenseeOptionParam(urlOrParams)", function (currencyId) {
        it("return an url with query string if 'urlOrParams' is a string", function () {
            affix("input#LicenseeType[value=1]");
            var $currencyId = affix("select#CurrencyId"); 
            $currencyId.affix("option[value=1]");
            $currencyId.affix("option[value=2]");
            affix("input#BaseCurrency[value=USD]");

            var result = Site.setLicenseeOptionParam("localhost");
            expect(result).toBe("localhost?licenseetype=1&currencyid=1&currencyname=&basecurrency=USD");
        });

        it("return an object with query string if 'urlOrParams' is an object", function () {
            affix("input#LicenseeType[value=1]");
            var $currencyId = affix("select#CurrencyId");
            $currencyId.affix("option[value=1]");
            $currencyId.affix("option[value=2]");
            affix("input#BaseCurrency[value=USD]");

            var result = Site.setLicenseeOptionParam({});
            expect(result).toEqual({ LicenseeType: '1', CurrencyId: '1', CurrencyName: '', BaseCurrency: 'USD' });
        });
    });
});
