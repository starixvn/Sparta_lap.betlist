/*exported DataHelper*/
"use strict";
var DataHelper = (function () {
    function compare(x, y) {
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    }

    function compareValues(x, y, order) {
        var result = compare(x, y);

        return order === "asc" ? result : -result;
    }

    function sort(arrayData, sortByFunc, sortOrder, sortThenByFunc, sortThenOrder) {
        var totalData = arrayData.length;

        if (!sortByFunc || totalData <= 1) {
            return arrayData;
        }

        var tempArray = [];
        for (var i = 0; i < totalData; i++) {
            tempArray[i] = arrayData[i];
        }

        var data = tempArray.sort(function (a, b) {
            var x = sortByFunc(a);
            var y = sortByFunc(b);

            var result = compareValues(x, y, sortOrder);
            if (result !== 0 || !sortThenByFunc) {
                return result;
            }

            x = sortThenByFunc(a);
            y = sortThenByFunc(b);

            return compareValues(x, y, sortThenOrder);
        });

        return data;
    }

    return {
        sort: sort
    };
}());