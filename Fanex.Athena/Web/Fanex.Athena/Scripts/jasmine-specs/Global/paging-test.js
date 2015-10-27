/// <reference path="../../jasmine/jasmine-jquery.js" />
/// <reference path="../../jasmine/jasmine-fixture.js" />
/// <reference path="../../Global/Paging.js" />

describe("Paging.js > pagingHeper", function () {
    describe("render(option)", function () {
        it("does not render the pager if totalrow less than page size", function () {
            window.Site =
            {
                getSingleResource: function (text) { return text; }
            };

            var $pager = affix("#page");
            pagingHeper.render({ totalrow: 10, pagesize: 50 });
            expect($pager).not.toContainElement(".pagingContainer");
        });

        it("render a pager if totalrow larger than page size", function () {
            window.Site =
            {
                getSingleResource: function (text) { return text; }
            };
            
            var $pager = affix("#page");
            pagingHeper.render({ totalrow: 100 });
            expect($pager).toContainElement(".pagingContainer");
        });
    });

    describe("getSetting()", function () {
        it("does not render the pager if totalrow less than page size", function () {
            var result = pagingHeper.getSetting();
            expect(result).toEqual({
                pageindex: 1,
                pagesize: 50,
                pagelist: [50, 100, 1000],
                totalrow: 1,
                formId: "formreport",
                elementcontainer: "#page",
                method: "AJAX",
                ajaxfunction: "",
                alwayshow: false
            });
        });
    });
});
