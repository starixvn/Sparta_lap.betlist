var path = require('path')
var webpack = require('webpack')

var projectRoot = path.resolve(__dirname, '../../../')

console.log(__dirname);
console.log("Project root: " + projectRoot);

module.exports = {
    resolve: {
        alias: {
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
            breakpoints: __dirname + "/www/js/source/plugins/breakpoints/breakpoints.min.js",
            asscroll: __dirname + "/www/js/source/plugins/asscroll/jquery-asScroll.min.js",
            asscrollable: __dirname + "/www/js/source/plugins/asscrollable/jquery.asScrollable.all.min.js",
            ashoverscroll: __dirname + "/www/js/source/plugins/ashoverscroll/jquery-asHoverScroll.min.js",
            waves: __dirname + "/www/js/source/plugins/waves/waves.min.js",

            core: __dirname + "/www/js/source/pages/home/menu/core.min.js",
            menu: __dirname + "/www/js/source/pages/home/menu/menu.min.js",
            menubar: __dirname + "/www/js/source/pages/home/menu/menubar.min.js",
            sidebar: __dirname + "/www/js/source/pages/home/menu/sidebar.min.js",
            homesite: __dirname + "/www/js/source/pages/home/site.min.js"
        }
    },
    entry: {
        "vendors": ["jquery", "bootstrap"],
        //"pages/home/home": "./www/js/source/pages/home/home.js",
        "pages/livereport/livereport": "./www/js/source/pages/livereport/livereport.js"
    },
    output: {
        path: projectRoot + "/www/js/build/test",
        filename: "[name].chunk.js",
        publicPath: "/www/js/build/"
    },
    plugins: [
                new webpack.optimize.CommonsChunkPlugin({
                    name: "vendors",
                    filename: "vendors.chunk.js",
                    minChunks: Infinity
                }),
				new webpack.ProvidePlugin({
				    $: "jquery",
				    jQuery: "jquery"
				})
    ]
}