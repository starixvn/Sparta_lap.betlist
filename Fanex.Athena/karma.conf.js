// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

const puppeteer = require('puppeteer');
process.env.CHROME_BIN = puppeteer.executablePath();

module.exports = function (config) {
    config.set({
        // basePath: 'D:/GITLab/lrf/Fanex.Athena/Web/Fanex.Athena/',
        frameworks: ['jasmine'],
        files: [
            'Scripts/Library/jquery.min.js',
            'Scripts/Library/jquery.jqGrid.min.js',
            'node_modules/jasmine-jquery/lib/jasmine-jquery.js',
            'node_modules/jasmine-fixture/dist/jasmine-fixture.min.js',
            'node_modules/jasmine-ajax/lib/mock-ajax.js',
            'Scripts/Library/moment.min.js',
            'Scripts/Library/jquery-ui.min.js',
            'Scripts/Extension/jquery.extension.js',
            'Scripts/Global/site.js',
            'Scripts/Global/query.string.js',
            'Scripts/Global/prototype.js',
            'Scripts/Global/Paging.js',
            'Scripts/Global/jquery.jqgrid.additional.js',
            'Scripts/Plugin/LinqJs/JSLINQ.js',
            'Scripts/Plugin/MultiSelect/jquery.multiselect.js',
            'Scripts/Plugin/FormatNumber/jshashtable.js',
            'Scripts/Plugin/FormatNumber/jquery.numberformatter.js',
            'Scripts/Plugin/ColorPicker/spectrum.js',
            'Scripts/Plugin/MonthPickr/monpickr.js',
            'Scripts/Plugin/Refresh/refreshcountdown.js',
            'Scripts/Plugin/DateRangePicker/date.min.js',
            'Scripts/Helper/ProductListHelper.js',
            'jasmine-specs/setup.js',
            'Scripts/Report/report.js',

            // --------- Account --------- //
             'Scripts/Account/*.js',
             'jasmine-specs/Account/*-test.js',

             // --------- Extension --------- //
            'Scripts/Extension/*.js',
             'jasmine-specs/Extension/*-test.js',

             // --------- Global --------- //
            //'Scripts/Global/*.js',
             'jasmine-specs/Global/*-test.js',

             // --------- Report --------- //
            'Scripts/Report/agcasinobalance*.js',
            'jasmine-specs/Report/agcasinobalance*-test.js',

            //'Scripts/Report/report*.js',
             'jasmine-specs/Report/report*-test.js',

            // --------- Helper --------- //
            'jasmine-specs/Helper/*-test.js'
        ],
        exclude: [
        ],
        preprocessors: {
            ['Scripts/!(Library|Plugin)/**/!(ajaxBinaryTransporting)*.js']: ['coverage']
        },
        port: 9876,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: false,
        browsers: ['ChromeHeadless'],
        reporters: ['kjhtml', 'dots', 'junit', 'coverage'],
        singleRun: true,
        junitReporter: {
            outputFile: 'junit/test-results.xml'
        },
        coverageReporter: {
            type: 'html',
            dir: 'junit/coverage/',
            reporters: [
                { type: 'html', subdir: 'report-html' }
            ]
        },
        concurrency: Infinity
    })
}
