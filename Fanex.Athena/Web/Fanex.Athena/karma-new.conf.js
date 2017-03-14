// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine-jquery', 'jasmine', 'requirejs'],
        files: [
            'bower_components/jasmine-ajax/lib/mock-ajax.js',
            { pattern: "node_modules/squirejs/src/Squire.js", included: false },
            { pattern: "node_modules/hammerjs/hammer.js", included: false },
            { pattern: "node_modules/moment/**/*.js", included: false },
            { pattern: "node_modules/bootstrap-daterangepicker/*.js", included: false },
            { pattern: "node_modules/materialize-autocomplete/**/*.js", included: false },
            { pattern: 'www/js/source/**/*.js', included: false },
            
            { pattern: 'www/js/tests/common/*-spec.js', included: false },
            { pattern: 'www/js/tests/lib/**/jquery.jqgrid.additional-spec.js', included: false },
            { pattern: 'www/js/tests/extensions/**/*-spec.js', included: false },
            { pattern: 'www/js/tests/components/**/*-spec.js', included: false },
            { pattern: 'www/js/tests/pages/**/*-spec.js', included: false },
            'www/js/tests/test-main.js'
        ],
        proxies: {
            '/Test/Index': 'about:blank'
        },
        exclude: [
        ],
        preprocessors: {
            'www/js/source/common/**/*.js': ['coverage'],
            'www/js/source/extensions/**/*.js': ['coverage'],
            'www/js/source/pages/**/*.js': ['coverage'],
            'www/js/source/vendors/**/*.js': ['coverage']
        },
        port: 9875,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: false,
        browsers: ['PhantomJS'],
        reporters: ['kjhtml', 'dots', 'junit', 'coverage', 'progress'],
        singleRun: true,
        junitReporter: {
            outputFile: 'junit/test-results-new.xml'
        },
        coverageReporter: {
            type: 'html',
            dir: 'junit/coverage-new/'
        },
        concurrency: Infinity,
        phantomjsLauncher: {
            // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom)
            exitOnResourceError: true
        }
    });
};