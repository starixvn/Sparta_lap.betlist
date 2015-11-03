describe("site.js > Site", function () {
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

    describe("setLicenseeOptionOriginalParam(urlOrParams)", function (currencyId) {
        it("build an url from 'data-original' attributes of html tags", function () {
            affix("input#LicenseeType[data-original=1]");
            var $currencyId = affix("select#CurrencyId[data-original=2]");
            affix("input#BaseCurrency[data-original=USD]");

            var result = Site.setLicenseeOptionOriginalParam("localhost");
            expect(result).toBe("localhost?licenseetype=1&currencyid=2&currencyname=&basecurrency=USD");
        });
    });

    describe("updateOriginalLicenseeOption()", function (currencyId) {
        it("update values of html tags to 'data-original' attributes", function () {
            affix("input#LicenseeType[value=1]");            
            affix("input#BaseCurrency[value=USD]");
            Site.updateOriginalLicenseeOption();
            var result = $("#LicenseeType").attr("data-original") === "1" && $("#BaseCurrency").attr("data-original") === "USD";
            expect(result).toBe(true);
        });
    });

    describe("setProductOptionParam(url, counter, productCode, status)", function (currencyId) {
        it("build an url with production options", function () {            
            var result = Site.setProductOptionParam("localhost", 1, "SB", true);
            expect(result).toBe("localhost?productoptions[1].code=SB&productoptions[1].name=SB&productoptions[1].status=true");
        });
    });

    describe("initBlockUI()", function (currencyId) {
        it("Site.blockUI() should be called when clicking on .blockui element", function () {
            affix(".blockui");
            Site.initBlockUI();
            spyOn(Site, 'blockUI');
            $('.blockui').click();            
            expect(Site.blockUI).toHaveBeenCalled();
        });
    });

    describe("asBool()", function (currencyId) {
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

    describe("getSingleResource(enghlishName)", function (currencyId) {
        it("Site.blockUI() should be called when clicking on .blockui element", function () {
            affix(".blockui");
            Site.initBlockUI();
            spyOn(Site, 'blockUI');
            $('.blockui').click();
            expect(Site.blockUI).toHaveBeenCalled();
        });
    });

});
