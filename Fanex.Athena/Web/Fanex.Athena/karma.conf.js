// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

module.exports = function(config) {
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
		'Scripts/Report/report.js',
		
		// --------- Account --------- //
		 'Scripts/Account/*.js',
		 'jasmine-specs/Account/*-test.js',
		
		// --------- BetList --------- //
		'Scripts/BetList/betlist.js',
		'Scripts/BetList/betlist.option.js',
		'Scripts/BetList/cancelledbetsbetlist.js',
		'Scripts/BetList/commonbetlist.js',
		'Scripts/BetList/correctscorebetlist.js',
		'Scripts/BetList/customer.dailystatement.js',
		'Scripts/BetList/customer.dailystatement.option.js',
		'Scripts/BetList/customer.outstandingbetlist.js',
		'Scripts/BetList/dailybetlist.js',
		'Scripts/BetList/lastbetsmonitornew.js',
		'Scripts/BetList/runningbetlist.js',
		'Scripts/BetList/runningbetlist.multiplebettypes.js',
		
		'jasmine-specs/BetList/betlist-test.js',
		'jasmine-specs/BetList/cancelledbetsbetlist-test.js',
		'jasmine-specs/BetList/commonbetlist-test.js',
		'jasmine-specs/BetList/correctscorebetlist-test.js',
		'jasmine-specs/BetList/customer.dailystatement-test.js',
		'jasmine-specs/BetList/customer.dailystatement.option-test.js',
		'jasmine-specs/BetList/customer.outstandingbetlist-test.js',
		'jasmine-specs/BetList/dailybetlist-test.js',
		'jasmine-specs/BetList/lastbetsmonitornew-test.js',
		 'jasmine-specs/BetList/runningbetlist-test.js',
		'jasmine-specs/BetList/runningbetlist.multiplebettypes-test.js',
		
		// --------- Casino --------- //
		'Scripts/Casino/**.js',
		 'jasmine-specs/Casino/*-test.js',
		 
		 // --------- Customer/BetSetting --------- //
		'Scripts/Customer/BetSetting/*.js',
		 'jasmine-specs/Customer/BetSetting/*-test.js',
		 
		 // --------- Customer/CustomerComm --------- //
		'Scripts/Customer/CustomerComm/*.js',
		 'jasmine-specs/Customer/CustomerComm/*-test.js',
		 
		  // --------- Customer/CustomerLog --------- //
		'Scripts/Customer/CustomerLog/*.js',
        'jasmine-specs/Customer/CustomerLog/customerlog-test.js',
        'jasmine-specs/Customer/CustomerLog/customersetting.option-test.js',
        'jasmine-specs/Customer/CustomerLog/customersetting-test.js',
        'jasmine-specs/Customer/CustomerLog/groupcommission-test.js',
        'jasmine-specs/Customer/CustomerLog/loginlog.option-test.js',
        'jasmine-specs/Customer/CustomerLog/loginlog-test.js',
		 
		  // --------- Customer --------- //
		'Scripts/Customer/customers.js',
		'Scripts/Customer/customers.option.js',
		'jasmine-specs/Customer/customers-test.js',
		
		 // --------- Extension --------- //
		'Scripts/Extension/*.js',
		 'jasmine-specs/Extension/*-test.js',
		 
		 // --------- Forecast --------- //
		'Scripts/Forecast/forecastpopup.js',
		'Scripts/Forecast/forecast.ah.js',
		'Scripts/Forecast/forecast.handicapoverunder.js',
		'Scripts/Forecast/forecast.newforecasthdpou.js',
		'Scripts/Forecast/forecast.onextwo.js',
		
		 'jasmine-specs/Forecast/*-test.js',
		 
		 // --------- Global --------- //
		//'Scripts/Global/*.js',
		 'jasmine-specs/Global/*-test.js',
		 
		 // --------- Report --------- //
		'Scripts/Report/activecustomersummary*.js',
		 'jasmine-specs/Report/activecustomersummary*-test.js',
		 
		'Scripts/Report/agcasinobalance*.js',
		'jasmine-specs/Report/agcasinobalance*-test.js',
		
		'Scripts/Report/agcasinowinloss*.js',
		'jasmine-specs/Report/agcasinowinloss*-test.js',
		
		'Scripts/Report/balance*.js',
		'jasmine-specs/Report/balance*-test.js',
		
		'Scripts/Report/CasinoProgressiveWins*.js',
		'jasmine-specs/Report/casinoprogressivewins*-test.js',
		
		'Scripts/Report/customerbalance*.js',
		'jasmine-specs/Report/customerbalance*-test.js',
		
		'Scripts/Report/customerpendingfunds*.js',
		'jasmine-specs/Report/customerpendingfunds*-test.js',
		
		'Scripts/Report/customerturnover*.js',
		'jasmine-specs/Report/customerturnover*-test.js',
		
		'Scripts/Report/customerwinloss*.js',
		'jasmine-specs/Report/customerwinloss*-test.js',
			
		'Scripts/Report/dailyracingwinloss*.js',
		'jasmine-specs/Report/dailyracingwinloss*-test.js',
		
		'Scripts/Report/golddeluxewinloss*.js',
		'jasmine-specs/Report/golddeluxewinloss*-test.js',
		
		'Scripts/Report/licenseewinloss*.js',
		'jasmine-specs/Report/licenseewinloss*-test.js',
		
		'Scripts/Report/livereport*.js',
		'jasmine-specs/Report/livereport*-test.js',
		
		'Scripts/Report/matchwinloss*.js',
		'jasmine-specs/Report/matchwinloss*-test.js',
		
		'Scripts/Report/microgamingwinloss*.js',
		'jasmine-specs/Report/microgamingwinloss*-test.js',
	
		'Scripts/Report/platformwinloss*.js',
		'jasmine-specs/Report/platformwinloss*-test.js',
		
		'Scripts/Report/playtechcasino*.js',
		'jasmine-specs/Report/playtechcasino*-test.js',
		
		'Scripts/Report/racingcommission*.js',
		'jasmine-specs/Report/racingcommission*-test.js',
		
		//'Scripts/Report/report*.js',
		 'jasmine-specs/Report/report*-test.js',
		
		// need to refactor getData()
		//'Scripts/Report/Result*.js',
		//'jasmine-specs/Report/Result*-test.js',
		
		'Scripts/Report/thirdpartywinloss*.js',
		'jasmine-specs/Report/thirdpartywinloss*-test.js',
		 
		'Scripts/Report/turnover*.js',
		'jasmine-specs/Report/turnover*-test.js',
		 
		'Scripts/Report/winlossbybettype*.js',
		'jasmine-specs/Report/winlossbybettype*-test.js',
		 
		'Scripts/Report/winlossbyleague*.js',
		'jasmine-specs/Report/winlossbyleague*-test.js',
		
		'Scripts/Report/winlossbyproduct*.js',
		'jasmine-specs/Report/winlossbyproduct*-test.js',

        'Scripts/Report/weeklylivematch*.js',
		'jasmine-specs/Report/weeklylivematch*-test.js',
		
		 // --------- TotalBets --------- //
		'Scripts/TotalBets/*.js',
		 'jasmine-specs/TotalBets/*-test.js',
		 
		  // --------- WareHouse --------- //
		'Scripts/WareHouse/*.js',
		 'jasmine-specs/WareHouse/*-test.js',
    ],
    exclude: [
    ],
    preprocessors: {
        'Scripts/**/*.js': ['coverage']
    },
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['PhantomJS'],
    reporters: ['kjhtml', 'dots', 'junit', 'coverage', 'progress'],
    singleRun: true,
    junitReporter : {
        outputFile: 'junit/test-results.xml'
    },
    coverageReporter: {
        type : 'html',
        dir : 'junit/coverage/'
    },
    concurrency: Infinity,
	phantomjsLauncher: {
      // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom) 
      exitOnResourceError: true
    }
  })
}
