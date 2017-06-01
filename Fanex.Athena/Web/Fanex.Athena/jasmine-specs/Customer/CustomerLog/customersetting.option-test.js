describe("CustomerSettingLogOption", function () {
    it("CustomerSettingLogOption.colNames should has correct value", function () {
        var colNameList = ['LogID', 'LogDate', 'Editedby', 'Action', 'Details'];

        expect(CustomerSettingLogOption.colNames).toEqual(colNameList);
    });

    it("CustomerSettingLogOption.colModel should has 5 items", function () {
        expect(CustomerSettingLogOption.colModel.length).toBe(5);
    });

    it("CustomerSettingLogOption.pager should has value '#CustomerSettingLogPager'", function () {
        expect(CustomerSettingLogOption.pager).toBe("#CustomerSettingLogPager");
    });

    it("CustomerSettingLogOption.sortorder should has value 'desc'", function () {
        expect(CustomerSettingLogOption.sortorder).toBe("desc");
    });
});