define(['jquery', 'common/site', 'common/popup', 'pages/report/report'],
    function ($, Site, Popup, Report) {
        describe("Report", function () {
            describe("ValidProducts()", function () {
                var $checkbox;
                beforeEach(function () {
                    spyOn(Site, "getSingleResource").and.callFake(function (text) {
                        return text;
                    });
                    spyOn(Site, "initialize");
                    spyOn(Popup, "openPopup");
                    $checkbox = setFixtures("<input class='childCheckbox' type='checkbox'></input>");
                });

                describe("No checkboxed were checked", function () {
                    it("Report.ValidProducts() should return false", function () {
                        var expected = false;

                        var result = Report.ValidProducts();

                        expect(result).toBe(expected);
                    });

                    it("Popup.openPopup() should be called", function () {
                        Report.ValidProducts();

                        expect(Popup.openPopup).toHaveBeenCalled();
                    });
                });

                it("Report.ValidProducts() should return value equal true when there are at least checkbox was checked", function () {
                    $(".childCheckbox").prop("checked", true);
                    var expected = true;

                    var result = Report.ValidProducts();

                    expect(result).toBe(expected);
                });
            });

            describe("setOriginalProduct()", function () {
                it("data-original attribute of checkbox should have correct value", function () {
                    var $productList = setFixtures("<div id='ProductList'>" + 
                        "<input id='chk1' checked class='childCheckbox' type='checkbox'></input>" +
                        "<input id='chk2' class='childCheckbox' type='checkbox'></input>" +
                        "</div>");

                    Report.setOriginalProduct();

                    expect($('#chk1').attr("data-original")).toBe("true");
                    expect($('#chk2').attr("data-original")).toBe("false");
                });
            });
        });
    });