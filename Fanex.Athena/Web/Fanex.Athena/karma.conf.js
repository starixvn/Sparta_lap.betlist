// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

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
        'Scripts/Plugin/Toast/toast.js',
        'Scripts/Helper/ProductListHelper.js',
        'Scripts/Helper/downloader.js',
		'jasmine-specs/setup.js',
		'Scripts/Report/report.js',        
		
		// --------- Account --------- //
		 'Scripts/Account/*.js',
		 'jasmine-specs/Account/*-test.js',
		
		// --------- BetList --------- //
		'Scripts/BetList/betlist.js',
		'Scripts/BetList/betlist.option.js',
		'Scripts/BetList/commonbetlist.js',
		'Scripts/BetList/correctscorebetlist.js',
		'Scripts/BetList/customer.dailystatement.js',
		'Scripts/BetList/customer.dailystatement.option.js',
		'Scripts/BetList/customer.outstandingbetlist.js',
		'Scripts/BetList/runningbetlist.js',
		'Scripts/BetList/runningbetlist.multiplebettypes.js',
		
		'jasmine-specs/BetList/betlist-test.js',
		'jasmine-specs/BetList/commonbetlist-test.js',
		'jasmine-specs/BetList/correctscorebetlist-test.js',
		'jasmine-specs/BetList/customer.dailystatement-test.js',
		'jasmine-specs/BetList/customer.dailystatement.option-test.js',
		'jasmine-specs/BetList/customer.outstandingbetlist-test.js',
		 'jasmine-specs/BetList/runningbetlist-test.js',
		'jasmine-specs/BetList/runningbetlist.multiplebettypes-test.js',

		 // --------- Extension --------- //
		'Scripts/Extension/*.js',
		 'jasmine-specs/Extension/*-test.js',
		 
		 // --------- Global --------- //
		//'Scripts/Global/*.js',
		 'jasmine-specs/Global/*-test.js',
		 
		 // --------- Report --------- //
		'Scripts/Report/agcasinobalance*.js',
		'jasmine-specs/Report/agcasinobalance*-test.js',
		
		'Scripts/Report/CasinoProgressiveWins*.js',
		'jasmine-specs/Report/casinoprogressivewins*-test.js',
		
		'Scripts/Report/microgamingwinloss*.js',
		'jasmine-specs/Report/microgamingwinloss*-test.js',
	
		//'Scripts/Report/report*.js',
		 'jasmine-specs/Report/report*-test.js',
		
        // --------- Helper --------- //
	    'jasmine-specs/Helper/*-test.js',

         // --------- Plugin --------- //
		 'jasmine-specs/Plugin/Toast/*-test.js',
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
        browsers: ['PhantomJS'],
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
        concurrency: Infinity,
        phantomjsLauncher: {
            // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom)
            exitOnResourceError: true
        }
    })
}