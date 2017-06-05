"use strict";
define(["jquery", "common/site",
    "lib/jqgrid/jquery.jqgrid.additional", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, jqGridAdditional) {
        var GRID_ID = "ReportGrid";

        var _me = {
            initialize: function () {
                Site.initialize();
                Site.addUserGuide({
                    resourceName: "CustomerLog",
                    title: Site.getSingleResource("CustomerLog")
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            loadGrid: function (gridOption, hasCheckbox) {
                Site.blockUI();
                jqGridAdditional.clearGrid(GRID_ID);
                gridOption.gridComplete = function () {
                    if (hasCheckbox) {
                        _me.checkShowAll();
                    }
                    _me.gridComplete();
                };
                $('#ReportGrid').jqGrid(gridOption);
            },

            gridComplete: function () {
                jqGridAdditional.gridComplete();
                var $grid = $("#" + GRID_ID);

                // Check noinfo
                var recCount = $grid.getGridParam("reccount");
                var hasData = recCount !== 0;
                if (hasData) {
                    _me.resizeGrid();
                }
                else {
                    jqGridAdditional.setNoInfo();
                }

                // Can not clear grid because jqgrid's bug
                // https://stackoverflow.com/questions/37917624/error-cannot-read-property-bdiv-of-null-after-unloading-loading-jqgrid
                $("#gbox_ReportGrid").toggle(hasData);
                Site.unBlockUI();
            },

            resizeGrid: function () {
                var $grid = $("#" + GRID_ID);
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth($grid);
            },

            checkShowAll: function () {
                var checked = $('#showall').is(':checked');
                $('.old-value').toggleClass("hide", !checked);
            }
        };

        return _me;
    });