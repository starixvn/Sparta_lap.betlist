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

    function sort(arrayData, sortBy, sortOrder, sortThenBy, sortThenOrder) {
        if (sortBy && arrayData.length > 1) {
            var tempArray = [];
            for (var i = 0; i < arrayData.length; i++) {
                tempArray[i] = arrayData[i];
            }

            var data = tempArray.sort(function (a, b) {
                var x = a[sortBy];
                var y = b[sortBy];

                var result = compareValues(x, y, sortOrder);
                if (result !== 0 || !sortThenBy) {
                    return result;
                }

                x = a[sortThenBy];
                y = b[sortThenBy];

                return compareValues(x, y, sortThenOrder);
            });

            return data;
        }

        return arrayData;
    }

    return {
        sort: sort
    };
}());