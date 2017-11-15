"use strict";
define(["lib/linq"], function () {
    function sort(arrayData, sortBy, sortOrder, sortThenBy, sortThenOrder) {
        if (sortBy && arrayData.length > 1) {
            var sortType = sortOrder === "asc" ? "OrderBy" : "OrderByDescending";
            var sortFunction = function (row) {
                return row[sortBy];
            };

            if (!sortThenBy) {
                return Enumerable.From(arrayData)[sortType](sortFunction).ToArray();
            }

            var sortThenType = sortThenOrder === "asc" ? "ThenBy" : "ThenByDescending";
            var sortThenFunction = function (row) {
                return row[sortThenBy];
            };

            return Enumerable.From(arrayData)[sortType](sortFunction)[sortThenType](sortThenFunction).ToArray();
        }

        return arrayData;
    }

    return {
        sort: sort
    };
});