/// <reference path="/@JSense.js" />
var note = (function ($) {
    return {
        initialize: function () {
            var noteContent = $("#note-content").html();
            $("#note-content").remove();

            $("#note-icon").click(function () {
                if ($("#note-wrapper").is(":visible")) {
                    $("#note-icon").hideBalloon();
                }
                else {
                    $("#note-icon").showBalloon({
                        contents: noteContent,
                        position: 'left bottom',
                        offsetX: 0,
                        offsetY: 0,
                        hideDuration: 120,
                        css: {
                            backgroundColor: "#333333",
                            color: "#FFF",
                            opacity: "0.9",
                            borderRadius: "3px",
                            boxShadow: "3px 3px 3px #555"
                        }
                    });
                }

                return false;
            });

            $(".note-close").click(function () {
                $("#note-icon").hideBalloon();
                return false;
            });

            $(document).click(function () {
                if ($("#note-wrapper").is(":visible")) {
                    $("#note-icon").hideBalloon();
                }
            });
        }
    };
} (jQuery));

jQuery(window).load(function () {
    note.initialize();
});