"use strict";
define(["jquery", "common/site",
    "lib/jqgrid/jquery.jqgrid.additional", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, jqGridAdditional) {
        var GRID_ID = "ReportGrid";
        var _ajaxRequest = null;

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

            loadGrid: function (url, gridOption, hasCheckbox) {
                var option = {
                    type: "GET",
                    url: url,
                    dataType: "json",
                    contentType: "application/json",
                    cache: false,
                    success: function (response) {
                        jqGridAdditional.clearGrid(GRID_ID);

                        var data = response.rows;
                        if (data.length === 0) {
                            jqGridAdditional.setNoInfo();
                            return;
                        }

                        _me.bindData(data, gridOption, hasCheckbox);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                }

                _me.stopAjaxRequest();
                _ajaxRequest = $.ajax(option);
            },

            bindData: function (data, gridOption, hasCheckbox) {
                gridOption.data = data;
                gridOption.gridComplete = function () {
                    if (hasCheckbox) {
                        _me.checkShowAll();
                    }

                    jqGridAdditional.gridComplete();
                    _me.resizeGrid();
                };

                $('#ReportGrid').jqGrid(gridOption);
            },

            resizeGrid: function () {
                var $grid = $("#" + GRID_ID);
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth($grid);
            },

            checkShowAll: function () {
                var checked = $('#showall').is(':checked');
                $('.old-value').toggleClass("hide", !checked);
            },

            stopAjaxRequest: function () {
                if (_ajaxRequest !== null) {
                    _ajaxRequest.abort();
                }
            }
        };

        return _me;
    });