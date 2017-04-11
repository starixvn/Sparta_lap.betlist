define(['jquery', 'common/site', 'components/selectlist'], function ($, Site) {
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
                setFixtures('<select id="CurrencyId"><option value="1" /><option value="2" /></select> <input id="BaseCurrency" value="USD" />');
            });

            it("return an url with query string if 'urlOrParams' is a string", function () {
                var result = Site.setLicenseeOptionParam("localhost");
                expect(result).toBe("localhost?currencyid=1&currencyname=&basecurrency=USD");
            });

            it("return an object with query string if 'urlOrParams' is an object", function () {
                var result = Site.setLicenseeOptionParam({});
                expect(result).toEqual({ CurrencyId: '1', CurrencyName: '', BaseCurrency: 'USD' });
            });
        });

        describe("setLicenseeOptionOriginalParam(urlOrParams)", function () {
            it("build an url from 'data-original' attributes of html tags", function () {
                setFixtures('<select id="CurrencyId" data-original="2" /> <input id="BaseCurrency" data-original="USD" />');

                var result = Site.setLicenseeOptionOriginalParam("localhost");
                expect(result).toBe("localhost?currencyid=2&currencyname=&basecurrency=USD");
            });
        });

        describe("updateOriginalLicenseeOption()", function () {
            it("update values of html tags to 'data-original' attributes", function () {
                setFixtures('<input id="CurrencyId" value="1"> <input id="BaseCurrency" value="USD"> <input id="CurrencyName" value="USD">');
                Site.updateOriginalLicenseeOption();

                expect($("#CurrencyId").attr("data-original")).toBe("1");
                expect($("#BaseCurrency").attr("data-original")).toBe("USD");
                expect($("#CurrencyName").attr("data-original")).toBe("USD");
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

        describe("getSingleResource(enghlishName)", function () {
            describe("resource is exist", function () {
                beforeEach(function () {
                    setFixtures('<input id="PageLanguage" value="en-US" />');
                    window["_language_en_US"] = { "Test": "en-Test" };
                    spyOn($.fn, "selectList");
                });

                it("get correct value from resource", function () {
                    Site.initialize();
                    var result = Site.getSingleResource("Test");
                    expect(result).toEqual("en-Test");
                });

                it("get key value when resource not contain", function () {
                    Site.initialize();
                    var result = Site.getSingleResource("NotContain");
                    expect(result).toEqual("NotContain");
                });

                it("return null when passed param is null", function () {
                    var result = Site.getSingleResource(null);
                    expect(result).toEqual(null);
                });

                it("return undefined when param is not passed", function () {
                    var result = Site.getSingleResource();
                    expect(result).toEqual(undefined);
                });

                it("get correct value from resource when value match with form that is defined", function () {
                    Site.initialize();
                    var result = Site.getSingleResource("@[Test]<br/>@[Commission]");
                    expect(result).toEqual("en-Test<br/>Commission");
                });

                it("get correct value from resource with special character @ and not match with form that is defined", function () {
                    Site.initialize();
                    var result = Site.getSingleResource("@Test<br/>@Commission]");
                    expect(result).toEqual("@Test<br/>@Commission]");
                });
            });
        });

        describe("getResources(arrayText)", function () {
            it("return correct array text", function () {
                setFixtures('<input id="PageLanguage" value="_language_en_US" />');
                window["_language_en_US"] = { "Test1": "en-Test1", "Test2": "en-Test2", "Test3": "en-Test3" };
                spyOn($.fn, "selectList");
                Site.initialize();
                var arrayText = ["Test1", "Test2", "Test3", "Test4"];
                var result = Site.getResources(arrayText);
                expect(result).toEqual(["en-Test1", "en-Test2", "en-Test3", "Test4"]);
            });

            it("return null when passed array is null", function () {
                var result = Site.getResources(null);

                expect(result).toBe(null);
            });

            it("return null when passed value is not array", function () {
                var result = Site.getResources({});

                expect(result).toBe(null);
            });
        });

        describe("isNumber(text)", function () {
            it("return true when pass text is integer", function () {
                var result = Site.isNumber(16);
                expect(result).toBe(true);
            });

            it("return true when pass text is digit string", function () {
                var result = Site.isNumber("20");
                expect(result).toBe(true);
            });

            it("return true when pass text is float", function () {
                var result = Site.isNumber(16.002);
                expect(result).toBe(true);
            });

            it("return false when pass text is not digit", function () {
                var result = Site.isNumber("ABC");
                expect(result).toBe(false);
            });

            it("return false when we don't pass text value", function () {
                var result = Site.isNumber();
                expect(result).toBe(false);
            });
        });

        describe("clone(obj)", function () {
            it("return null when passed param is null", function () {
                var result = Site.clone(null);
                expect(result).toBe(null);
            });

            it("return itself when passed param is not object type", function () {
                var result = Site.clone(1);
                expect(result).toBe(1);
            });

            it("return correct date value when passed param is Date type", function () {
                var date = new Date(2017, 0, 1);

                var result = Site.clone(date);

                expect(result).not.toBe(date);
                expect(result).toEqual(date);
            });
        });

        describe("createSortingIcon(sortHandler)", function () {
            describe("when Row Count less or equal 1", function () {
                it(".sort-heading class should be removed", function () {
                    setFixtures('<input id="RowCount" value="1" />' +
                        '<div id="header-content"><div class="sortable sort-heading"><div></div>'
                        );

                    Site.createSortingIcon();

                    var result = $("#header-content .sortable").hasClass("sort-heading");

                    expect(result).toBe(false);
                });
            });

            describe("when Row Count greater than 1", function () {
                beforeEach(function () {
                    setFixtures('<input id="RowCount" value="2" /> <input id="SortColumn" value="Col1" /> <input id="SortDirection" value="DESC" />' +
                        '<div id="header-content">' +
                        '<div class="sortable"><div class="sort-heading" data-column-name="Col1"></div></div>' +
                        '<div class="sortable"><div class="sort-heading" data-column-name="Col2"></div></div>' +
                        '</div>' +
                        '<select id="CurrencyId"><option>USD<option></select>' +
                        '<input id="CurrencyName" value="" />'
                        );
                });

                it("Col1 should has class 'sorted-heading'", function () {
                    var handler = function () { };
                    Site.createSortingIcon();

                    var result = $("#header-content .sortable .sort-heading[data-column-name='Col1']").hasClass("sorted-heading");

                    expect(result).toBe(true);
                });

                it("Col1 should has correct html content", function () {
                    var handler = function () { };
                    Site.createSortingIcon();

                    var expected = '<span class="sort-icon sort-icon-desc"></span>';
                    var result = $("#header-content .sortable .sort-heading[data-column-name='Col1']").html();

                    expect(result).toBe(expected);
                });
            });
        });
    });
});