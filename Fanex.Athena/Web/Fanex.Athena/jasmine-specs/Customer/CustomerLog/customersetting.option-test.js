describe("CustomerSettingLogOption", function () {
    xit("CustomerSettingLogOption.colNames should has correct value", function () {
        var colNameList = ['LogID', 'LogDate', 'Editedby', 'Action', 'Details'];

        expect(CustomerSettingLogOption.colNames).toEqual(colNameList);
    });

    xit("CustomerSettingLogOption.colModel should has 5 items", function () {
        expect(CustomerSettingLogOption.colModel.length).toBe(5);
    });

    describe("getColumnNames()", function () {
        it("Site.getResources(arrayText) should be called with correct parameters", function () {
            var colNameList = ['LogID', 'LogDate', 'Editedby', 'Action', 'Details'];
            spyOn(Site, "getResources");

            CustomerSettingLogOption.getColumnNames();

            expect(Site.getResources).toHaveBeenCalledWith(colNameList);
        });
    });

    it("CustomerSettingLogOption.pager should has value '#CustomerSettingLogPager'", function () {
        expect(CustomerSettingLogOption.pager).toBe("#CustomerSettingLogPager");
    });

    it("CustomerSettingLogOption.sortorder should has value 'desc'", function () {
        expect(CustomerSettingLogOption.sortorder).toBe("desc");
    });
});