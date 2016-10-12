// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine-jquery', 'jasmine', 'requirejs'],
        files: [
            'bower_components/jasmine/lib/jasmine-core/jasmine-html.js',
		    'bower_components/jasmine-fixture/dist/jasmine-fixture.js',
		    'bower_components/jasmine-ajax/lib/mock-ajax.js',

            { pattern: 'www/js/source/**/*.js', included: false },
            { pattern: 'www/js/tests/common/*-spec.js', included: false },

             'www/js/tests/test-main.js',
        ],
        exclude: [
        ],
        port: 9875,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['PhantomJS'],
        reporters: ['kjhtml', 'dots', 'junit', 'coverage', 'progress'],
        singleRun: true,
        junitReporter: {
            outputFile: 'junit/test-results.xml'
        },
        coverageReporter: {
            type: 'html',
            dir: 'junit/coverage/'
        },
        concurrency: Infinity,
        phantomjsLauncher: {
            // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom)
            exitOnResourceError: true
        }
    })
}