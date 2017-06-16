define(["jquery", "common/site", "pages/customer/customercomm/commgroup"],
    function ($, Site, CustomerCommGroup) {
        describe("CustomerCommGroup", function () {
            describe("setPageTitle(title)", function () {
                it("change page title with correct param", function () {
                    var expectedTitle = "title";
                    spyOn($.fn, "text");
                    spyOn(Site, "getSingleResource").and.callFake(function (str) {
                        return str;
                    });

                    CustomerCommGroup.setPageTitle(expectedTitle);

                    expect($("#page-title", window.parent.document).text).toHaveBeenCalledWith(expectedTitle);
                });
            });

            describe("goToSite(relativeUrl, paramString)", function () {
                beforeAll(function () {
                    spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                        return url;
                    });
                });

                it("Site.redirect should be called with correct params", function () {
                    var expectedHref = "url?param=0";
                    spyOn(Site, "redirect");

                    CustomerCommGroup.goToSite("url", "?param=0");

                    expect(Site.redirect).toHaveBeenCalledWith(expectedHref);
                });

                it("Site.redirect should be called correctly without paramString", function () {
                    var expectedHref = "url";
                    spyOn(Site, "redirect");

                    CustomerCommGroup.goToSite("url");

                    expect(Site.redirect).toHaveBeenCalledWith(expectedHref);
                });
            });

            describe("goToCustomerList(groupCommId)", function () {
                beforeAll(function () {
                    spyOn(Site, "getSingleResource").and.callFake(function (str) {
                        return str;
                    });
                });

                it("setPageTitle should be called if parent host is the same with current host", function () {
                    setFixtures('<ul></ul>');
                    $("ul").append('<li><a id="MenuItem55"></a></li>');
                    $("ul").append('<li><a id="MenuItem43"></a></li>');
                    window.parent.document = "ul";
                    spyOn(CustomerCommGroup, "setPageTitle");
                    spyOn(CustomerCommGroup, "goToSite");

                    CustomerCommGroup.goToCustomerList(1);

                    expect(CustomerCommGroup.setPageTitle).toHaveBeenCalledWith("CustomerList");
                    expect(CustomerCommGroup.goToSite).toHaveBeenCalledWith("Customer/CustomerList", "?groupId=1");
                });

                it("setPageTitle should not be called if parent host is not the same with current host", function () {
                    spyOn(CustomerCommGroup, "setPageTitle");
                    spyOn(CustomerCommGroup, "goToSite");
                    window.parent = {
                        location: {
                            host: new Error()
                        }

                    };
                    window = {
                        location: {
                            host: "child"
                        }
                    };

                    CustomerCommGroup.goToCustomerList(1);

                    expect(CustomerCommGroup.setPageTitle).not.toHaveBeenCalledWith("CustomerList");
                    expect(CustomerCommGroup.goToSite).toHaveBeenCalledWith("Customer/CustomerList", "?groupId=1");
                });
            });
        });
    });