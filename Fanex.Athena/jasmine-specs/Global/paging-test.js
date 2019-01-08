describe("Paging.js > PagingHeper", function () {

    beforeEach(function () {
        spyOn(Site, "getSingleResource").and.callFake(function (text) {
            return text;
        });
    });

    describe("render(option)", function () {        
        it("does not render the pager if totalrow less than page size", function () {
            var $pager = affix("#page");

            PagingHeper.render({ totalrow: 10, pagesize: 50 });

            expect($pager.find(".pagingContainer").length > 0).toBe(false);
        });

        it("render a pager if totalrow larger than page size", function () {            
            var $pager = affix("#page");

            PagingHeper.render({ totalrow: 100 });

            expect($pager.find(".pagingContainer").length > 0).toBe(true);
        });
    });

    describe("getSetting()", function () {
        it("return setting object", function () {
            affix('div.pagingContainer input[name="PageIndex"][value="1"]');
            affix('div.pagingContainer input[name="PageSize"][value="2"]');
            var result = PagingHeper.getSetting({ totalrow: 100 });

            expect(result).toEqual({
                pageindex: 1,
                pagesize: 2,
                pagelist: [50, 100, 1000],
                totalrow: 100,
                formId: "formreport",
                elementcontainer: "#page",
                method: "AJAX",
                ajaxfunction: "",
                alwayshow: false
            });
        });
    });
});
