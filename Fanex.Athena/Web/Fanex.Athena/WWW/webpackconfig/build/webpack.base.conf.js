var path = require('path')
var webpack = require('webpack')
const CleanWebpackPlugin = require('clean-webpack-plugin');

var projectRoot = path.resolve(__dirname, '../../../')

const PATHS = {
    style: path.join(projectRoot, '/www/css/build/test'),
    build: path.join(projectRoot, '/www/js/build/test')
};

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
    },
    entry: {
        "vendors": ["jquery", "bootstrap"],
        //"pages/home/home": "./www/js/source/pages/home/home.js",
        "pages/livereport/livereport": "./www/js/source/pages/livereport/livereport.js"
    },
    output: {
        path: PATHS.build,
        filename: "[name].chunk.js",
        publicPath: "/www/js/build/"
    },
    module: {
        loaders: [
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract('style', 'css'),
                include: PATHS.style
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin([PATHS.build], {
            root: process.cwd()
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: "vendors",
            filename: "vendors.chunk.js",
            minChunks: Infinity
        }),
		new webpack.ProvidePlugin({
			$: "jquery",
			jQuery: "jquery"
		}),
        new ExtractTextPlugin('[name].css')
    ]
}