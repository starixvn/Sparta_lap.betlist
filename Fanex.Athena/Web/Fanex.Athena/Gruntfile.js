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
     }
  });

  grunt.loadNpmTasks('grunt-karma');
  grunt.loadNpmTasks('grunt-contrib-jshint');

  // Default task(s).
  // grunt.registerTask('default', ['karma']);

};