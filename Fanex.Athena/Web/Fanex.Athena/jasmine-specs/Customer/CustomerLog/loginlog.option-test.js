describe("LogInLogOption", function () {
    it("LogInLogOption.gridOption.colNames should has correct value", function () {
        var colNameList = ['Username', 'CreatedDate', 'LastTxn'];

        expect(LogInLogOption.gridOption.colNames).toEqual(colNameList);
    });

    it("LogInLogOption.gridOption.colModel should has 3 items", function () {
        expect(LogInLogOption.gridOption.colModel.length).toBe(3);
    });

    it("LogInLogOption.gridOption.pager should has value 'null'", function () {
        expect(LogInLogOption.gridOption.pager).toBe(null);
    });

    it("LogInLogOption.gridOption.sortorder should has value 'desc'", function () {
        expect(LogInLogOption.gridOption.sortorder).toBe("desc");
    });
});