/// <binding Clean='default' />
module.exports = function (grunt) {

    grunt.file.defaultEncoding = "utf-8";
    grunt.file.preserveBOM = true;
	
	var configs = grunt.file.readJSON('grunt.json');
    var jsFilters = configs.jsFilters;
	
    grunt.initConfig({
        //pkg: grunt.file.readJSON("package.json"),
        jshint: {
            options: grunt.file.readJSON('jshint.json'),
            beforeconcat: jsFilters,
            afterconcat: ['dist/output.js']
        },
        watch: {
            options: {
                spawn: false,
                nospawn: true,
                event: ['all']
            },
            jshint: {
                files: jsFilters,
                tasks: ['jshint:all']
            }
        }
    });

    grunt.event.on('watch', function (action, filepath) {
        grunt.config('jshint.all.src', filepath);
    });
	
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks('grunt-contrib-jshint');
	
    grunt.registerTask("default", ["watch"]);
};