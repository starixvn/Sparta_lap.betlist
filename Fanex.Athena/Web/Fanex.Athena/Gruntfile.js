var webpack = require("webpack");
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
		bundle: {
			context: __dirname + "/www/js/",
			resolve: {
				alias: {
					jquery: __dirname + "/www/js/source/vendors/jquery/jquery.min.js",
					bootstrap: __dirname + "/www/js/source/vendors/bootstrap/bootstrap.min.js"
				}
			},
			entry: {
				// "vendors/vendors": [ "bootstrap"],
				 "vendors/vendors": "./source/vendors/vendors.js",
				"pages/livereport/livereport": "./source/pages/livereport/livereport.js"
			},
			output: {
				path: __dirname + "/www/js/build/",
				filename: "[name].chunk.js"
			},
			plugins: [ 
				/*new webpack.optimize.CommonsChunkPlugin({
				  name: "commons",
				  filename: "commons.js",
				}),*/
				/*new webpack.ProvidePlugin({ 
					$: "jquery",
					jQuery: "jquery"
				})*/
			//new webpack.optimize.UglifyJsPlugin({ compress: { warnings: false } }) 
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