describe("jquery.jqgrid.additional.js > jqGridAdditional", function () {
    beforeEach(function () {
        spyOn(Site, "getSingleResource").and.callFake(function (text) {
            return text;
        });
    });

    describe("formatInteger(value)", function () {
        it("format number by adding commas", function () {
            var result = jqGridAdditional.formatInteger(1234567);
            expect(result).toBe("1,234,567");
        });

        it("format number (as string) by adding commas", function () {
            var result = jqGridAdditional.formatInteger("12345");
            expect(result).toBe("12,345");
        });
    });

    describe("excludeCol(colName, colModel, excludedCol)", function () {
        it("return a column model object after excluding some columns", function () {
            var columnNames = [
                "A", "B", "C"
            ];
            var columnModels = [
                { name: "A" }, { name: "C" }, { name: "B" }
            ];
            var excludeColumns = ["C", "B"];
            var result = jqGridAdditional.excludeCol(columnNames, columnModels, excludeColumns);
            expect(result).toEqual({ colName: ["A"], colModel: [{ name: "A" }] });
        });
    });

    describe("rowSpanSetting(rowId, val, rawObject)", function () {
        it('when RowSpan < 0, returns \' style="display:none;" \'', function () {
            var result = jqGridAdditional.rowSpanSetting(null, null, { RowSpan: -1 });
            expect(result).toBe(' style="display:none;" ');
        });

        it('when RowSpan = 0, returns \'\'', function () {
            var result = jqGridAdditional.rowSpanSetting(null, null, { RowSpan: 0 });
            expect(result).toBe('');
        });

        it('when RowSpan > 1, returns \' rowspan="{RowSpan}"\'', function () {
            var result = jqGridAdditional.rowSpanSetting(null, null, { RowSpan: 2 });
            expect(result).toBe(' rowspan="2"');
        });
    });

    describe("setNoInfo(columnModel, message)", function () {
        it("div.ui-jqgrid-sdiv should be hidden", function () {
            affix("div.ui-jqgrid-sdiv");
            jqGridAdditional.setNoInfo([]);
            expect($("div.ui-jqgrid-sdiv").is(":visible")).toBe(false);
        });

        it("table.ui-jqgrid-htable should contains a td.hide", function () {
            affix("div.ui-jqgrid-sdiv");
            affix("table.ui-jqgrid-htable");
            jqGridAdditional.setNoInfo([], "NoInfo");
            expect($("table.ui-jqgrid-htable td.hide").length).toBe(1);
        });

        it("first td contains text of param 'message'", function () {
            affix(".ui-jqgrid-ftable");
            affix("table.ui-jqgrid-htable");
            jqGridAdditional.setNoInfo([], "No Info Message");
            expect($("table.ui-jqgrid-htable td:first").text()).toBe('No Info Message');
        });
    });

    describe("setNoInfoWithColspan(columnModel, message)", function () {
        it("div.ui-jqgrid-sdiv should be hidden", function () {
            affix("div.ui-jqgrid-sdiv");
            jqGridAdditional.setNoInfoWithColspan(1);
            expect($("div.ui-jqgrid-sdiv").is(":visible")).toBe(false);
        });

        it("table.ui-jqgrid-htable should not contains a td.hide", function () {
            affix("div.ui-jqgrid-sdiv");
            affix("table.ui-jqgrid-htable");
            jqGridAdditional.setNoInfoWithColspan(1);
            expect($("table.ui-jqgrid-htable td.hide").length).toBe(0);
        });

        it("first td contains text of 'NoInfo' (from resource)", function () {
            affix(".ui-jqgrid-ftable");
            affix("table.ui-jqgrid-htable");
            jqGridAdditional.setNoInfoWithColspan(1);
            expect($("table.ui-jqgrid-htable td:first").text()).toBe('NoInfo');
        });
    });

    describe("showRecommendedBrowser(pagingContainer)", function () {
        it("pager element should contain a div having text resource 'RecommendBrowserOnReportPageMsg'", function () {
            var $pagingContainer = affix("table#pager td#ppdata_left");
            jqGridAdditional.showRecommendedBrowser("table#pager");
            expect($pagingContainer.find("#ppdata_left").html()).toBe('<div class="recommend-browser">RecommendBrowserOnReportPageMsg</div>');
        });
    });
});