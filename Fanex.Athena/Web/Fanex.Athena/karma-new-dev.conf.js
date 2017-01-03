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
            { pattern: "node_modules/malihu-custom-scrollbar-plugin/*.js", included: false },
            { pattern: 'www/js/source/**/*.js', included: false },
            { pattern: 'www/js/tests/test-helper.js', included: false },

            //{ pattern: 'www/js/tests/pages/betlist/betlist-spec.js', included: false },
            //{ pattern: 'www/js/tests/pages/betlist/betlist.option-spec.js', included: false },
            //{ pattern: 'www/js/tests/pages/betlist/betlistcorrectscore-spec.js', included: false },
            //{ pattern: 'www/js/tests/pages/betlist/betlistrunning-spec.js', included: false },
            //{ pattern: 'www/js/tests/pages/betlist/betlistrunning.multiplebettypes-spec.js', included: false },
            //{ pattern: 'www/js/tests/pages/betlist/commonbetlist-spec.js', included: false },
            { pattern: 'www/js/tests/components/refreshcountdown-spec.js', included: false },
            'www/js/tests/test-main.js'
        ],
        proxies: {
            '/Test/Index': 'about:blank'
        },
        exclude: [
        ],
        port: 9874,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['PhantomJS'],
     //   reporters: ['progress', 'kjhtml'],
        singleRun: true,
        concurrency: Infinity,
        phantomjsLauncher: {
            // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom)
          //  exitOnResourceError: true
        }
    });
};