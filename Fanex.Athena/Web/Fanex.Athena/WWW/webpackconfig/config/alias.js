var projectRoot = require('../config/path').projectRoot

var alias = {
    // vendors
    jquery: projectRoot + "/www/js/source/vendors/jquery/jquery.min.js",
    bootstrap: projectRoot + "/www/js/source/vendors/bootstrap/bootstrap.min.js",

    // plugins
    moment: projectRoot + "/www/js/source/plugins/moment/moment.min.js",
    daterangepicker: projectRoot + "/www/js/source/plugins/daterangepicker/daterangepicker.js",

    // common
    querystring: projectRoot + "/www/js/source/common/querystring.js",
    site: projectRoot + "/www/js/source/common/site.js",

    // home
    breakpoints: projectRoot + "/www/js/source/plugins/breakpoints/breakpoints.min.js",
    asscroll: projectRoot + "/www/js/source/plugins/asscroll/jquery-asScroll.min.js",
    asscrollable: projectRoot + "/www/js/source/plugins/asscrollable/jquery.asScrollable.all.min.js",
    ashoverscroll: projectRoot + "/www/js/source/plugins/ashoverscroll/jquery-asHoverScroll.min.js",
    waves: projectRoot + "/www/js/source/plugins/waves/waves.min.js",

    core: projectRoot + "/www/js/source/pages/home/menu/core.min.js",
    menu: projectRoot + "/www/js/source/pages/home/menu/menu.min.js",
    menubar: projectRoot + "/www/js/source/pages/home/menu/menubar.min.js",
    sidebar: projectRoot + "/www/js/source/pages/home/menu/sidebar.min.js",
    homesite: projectRoot + "/www/js/source/pages/home/site.min.js"
}

module.exports = alias