define(
    ["breakpoints", "asscroll", "asscrollable", "ashoverscroll", "waves", "core", "menu", "menubar", "sidebar", "homesite"],
    function (breakpoints, asscroll, asscrollable, ashoverscroll, Waves, core, menu, menubar, sidebar, homesite) {
        Breakpoints();
        $(document).ready(function ($) {        
            Site.run(), Waves.attach(".page-content .btn-floating", ["waves-light"])
        });
    }
);