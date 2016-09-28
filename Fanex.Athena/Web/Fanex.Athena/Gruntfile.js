var webpack = require("webpack");
var webpackDevelopment = require("./www/webpackconfig/build/webpack.dev.conf.js");
var webpackProduction = require("./www/webpackconfig/build/webpack.pro.conf.js");

module.exports = function(grunt) {

	grunt.file.defaultEncoding = "utf-8";
	grunt.file.preserveBOM = true;

	var configs = grunt.file.readJSON('GruntFilters.json');
	var jsFilters = configs.jsFilters;

  // Project configuration.
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    karma: {
	  unit: {
		configFile: 'karma.conf.js',
		/*port: 9999,
		singleRun: true,
		browsers: ['PhantomJS'],
		logLevel: 'ERROR'*/
	  }
	},
	jshint: {
		options: grunt.file.readJSON('jshint.json'),
		beforeconcat: jsFilters,
		afterconcat: ['dist/output.js']
    },
	webpack: {
	    dev: webpackDevelopment,
	    pro: webpackProduction,
	    bundle: {
	        debug: true,
	        devtool: 'hidden-source-map',
			context: __dirname + "/www/js/",
			resolve: {
			    alias: {
                    // vendors
			        jquery: __dirname + "/www/js/source/vendors/jquery/jquery.min.js",
			        bootstrap: __dirname + "/www/js/source/vendors/bootstrap/bootstrap.min.js",

                    // plugins
			        moment: __dirname + "/www/js/source/plugins/moment/moment.min.js",
			        daterangepicker: __dirname + "/www/js/source/plugins/daterangepicker/daterangepicker.js",

			        // common
			        querystring: __dirname + "/www/js/source/common/querystring.js",
			        site: __dirname + "/www/js/source/common/site.js",

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
			    "pages/home/home": "./source/pages/home/home.js",
			  //  "common": ["querystring", "site"],
				"pages/livereport/livereport": "./source/pages/livereport/livereport.js"
			},
			output: {
				path: __dirname + "/www/js/build/",
				filename: "[name].chunk.js",
				publicPath: "/www/js/build/"
			},
			plugins: [
                new webpack.optimize.CommonsChunkPlugin({
                    name: "vendors", 
                    filename: "vendors.chunk.js",
                 //   chunks: ["vendors", "common"],
                    minChunks: Infinity
                }),
				//new webpack.optimize.CommonsChunkPlugin({
				//    name: "common",
				//    filename: "common.chunk.js",
				//    chunks: ["common"],
				//    minChunks: Infinity
				//}),
				new webpack.ProvidePlugin({ 
					$: "jquery",
					jQuery: "jquery"
				}),
			new webpack.optimize.UglifyJsPlugin({ compress: { warnings: false } }) 
			]
		}
	}
  });

	grunt.loadNpmTasks('grunt-karma');
	grunt.loadNpmTasks('grunt-contrib-jshint');
	grunt.loadNpmTasks('grunt-webpack');
	
  // Default task(s).
  // grunt.registerTask('default', ['karma']);
};