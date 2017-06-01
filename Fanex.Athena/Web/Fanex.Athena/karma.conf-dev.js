// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

module.exports = function (config) {
    config.set({
        // basePath: 'D:/GITLab/lrf/Fanex.Athena/Web/Fanex.Athena/',
        frameworks: ['jasmine'],
        files: [
            'Scripts/Library/jquery.min.js',
            'Scripts/Library/jquery.jqGrid.min.js',
            'bower_components/jasmine/lib/jasmine-core/jasmine-html.js',
            'bower_components/jasmine-jquery/lib/jasmine-jquery.js',
            'bower_components/jasmine-fixture/dist/jasmine-fixture.min.js',
            'bower_components/jasmine-ajax/lib/mock-ajax.js',
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
            'jasmine-specs/setup.js',

            // --------- Customer --------- //
		    'Scripts/Customer/CustomerList/*.js',
		    'jasmine-specs/Customer/CustomerList/*-test.js'
        ],
        port: 9870,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: false,
        browsers: ['PhantomJS'],
        singleRun: true,
        concurrency: Infinity,
        phantomjsLauncher: {
            // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom)
            exitOnResourceError: true
        }
    })
}