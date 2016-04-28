// Karma configuration
// Generated on Wed Apr 20 2016 16:28:26 GMT+0700 (SE Asia Standard Time)

module.exports = function(config) {
    config.set({
    // basePath: 'D:/GITLab/lrf/Fanex.Athena/Web/Fanex.Athena/',
    frameworks: ['jasmine'],
    files: [
		'Scripts/Library/jquery.min.js',
		'Scripts/Library/jquery.jqGrid.min.js',
	//	'Scripts/jasmine/jasmine.js',
		'Scripts/jasmine/jasmine-html.js',
		//'Scripts/jasmine/boot.js',		
		'Scripts/jasmine/jasmine-jquery.js',
		'Scripts/jasmine/jasmine-fixture.js',	
		'Scripts/jasmine/mock-ajax.js',
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
		'Scripts/jasmine-specs/setup.js',
		'Scripts/Report/report.js',
		
		// --------- Account --------- //
		 'Scripts/Account/*.js',
		 'Scripts/jasmine-specs/Account/*-test.js',
		
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
		// need to refactor userGuideName
		// 'Scripts/BetList/runningbetlist.js',
		//'Scripts/BetList/runningbetlist.multiplebettypes.js',
		
		'Scripts/jasmine-specs/BetList/betlist-test.js',
		'Scripts/jasmine-specs/BetList/cancelledbetsbetlist-test.js',
		'Scripts/jasmine-specs/BetList/commonbetlist-test.js',
		'Scripts/jasmine-specs/BetList/correctscorebetlist-test.js',
		'Scripts/jasmine-specs/BetList/customer.dailystatement-test.js',
		'Scripts/jasmine-specs/BetList/customer.dailystatement.option-test.js',
		'Scripts/jasmine-specs/BetList/customer.outstandingbetlist-test.js',
		'Scripts/jasmine-specs/BetList/dailybetlist-test.js',
		'Scripts/jasmine-specs/BetList/lastbetsmonitornew-test.js',
		// 'Scripts/jasmine-specs/BetList/runningbetlist-test.js',
		//'Scripts/jasmine-specs/BetList/runningbetlist.multiplebettypes-test.js',
		
		// --------- Casino --------- //
		'Scripts/Casino/**.js',
		 'Scripts/jasmine-specs/Casino/*-test.js',
		 
		 // --------- CustomerLog --------- //
		'Scripts/Customer/BetSetting/*.js',
		 'Scripts/jasmine-specs/Customer/BetSetting/*-test.js',
		 
		 // --------- CustomerComm --------- //
		'Scripts/Customer/CustomerComm/*.js',
		 'Scripts/jasmine-specs/Customer/CustomerComm/*-test.js',
		 
		  // --------- CustomerLog --------- //
		'Scripts/Customer/CustomerLog/*.js',
		 'Scripts/jasmine-specs/Customer/CustomerLog/*-test.js',
		 
		  // --------- Customer --------- //
		'Scripts/Customer/customers.js',
		'Scripts/Customer/customers.option.js',
		'Scripts/jasmine-specs/Customer/customers-test.js',
		
		 // --------- Extension --------- //
		'Scripts/Extension/*.js',
		 'Scripts/jasmine-specs/Extension/*-test.js',
		 
		 // --------- Forecast --------- //
		'Scripts/Forecast/*.js',
		 'Scripts/jasmine-specs/Forecast/*-test.js',
		 
		 // --------- Global --------- //
		//'Scripts/Global/*.js',
		 'Scripts/jasmine-specs/Global/*-test.js',
		 
		 // --------- Report --------- //
		'Scripts/Report/activecustomersummary*.js',
		 'Scripts/jasmine-specs/Report/activecustomersummary*-test.js',
		 
		'Scripts/Report/agcasinobalance*.js',
		'Scripts/jasmine-specs/Report/agcasinobalance*-test.js',
		
		'Scripts/Report/agcasinowinloss*.js',
		'Scripts/jasmine-specs/Report/agcasinowinloss*-test.js',
		
		'Scripts/Report/balance*.js',
		'Scripts/jasmine-specs/Report/balance*-test.js',
		
		'Scripts/Report/CasinoProgressiveWins*.js',
		'Scripts/jasmine-specs/Report/casinoprogressivewins*-test.js',
		
		'Scripts/Report/customerbalance*.js',
		'Scripts/jasmine-specs/Report/customerbalance*-test.js',
		
		'Scripts/Report/customerpendingfunds*.js',
		'Scripts/jasmine-specs/Report/customerpendingfunds*-test.js',
		
		'Scripts/Report/customerturnover*.js',
		'Scripts/jasmine-specs/Report/customerturnover*-test.js',
		
		'Scripts/Report/customerwinloss*.js',
		'Scripts/jasmine-specs/Report/customerwinloss*-test.js',
			
		'Scripts/Report/dailyracingwinloss*.js',
		'Scripts/jasmine-specs/Report/dailyracingwinloss*-test.js',
		
		'Scripts/Report/golddeluxewinloss*.js',
		'Scripts/jasmine-specs/Report/golddeluxewinloss*-test.js',
		
		'Scripts/Report/licenseewinloss*.js',
		'Scripts/jasmine-specs/Report/licenseewinloss*-test.js',
		
		'Scripts/Report/livereport*.js',
		'Scripts/jasmine-specs/Report/livereport*-test.js',
		
		'Scripts/Report/matchwinloss*.js',
		'Scripts/jasmine-specs/Report/matchwinloss*-test.js',
		
		'Scripts/Report/microgamingwinloss*.js',
		'Scripts/jasmine-specs/Report/microgamingwinloss*-test.js',
		
		// have not been implemented
		//'Scripts/Report/platformwinloss*.js',
		//'Scripts/jasmine-specs/Report/platformwinloss*-test.js',
		
		'Scripts/Report/playtechcasino*.js',
		'Scripts/jasmine-specs/Report/playtechcasino*-test.js',
		
		'Scripts/Report/racingcommission*.js',
		'Scripts/jasmine-specs/Report/racingcommission*-test.js',
		
		//'Scripts/Report/report*.js',
		 'Scripts/jasmine-specs/Report/report*-test.js',
		
		// need to refactor getData()
		//'Scripts/Report/Result*.js',
		//'Scripts/jasmine-specs/Report/Result*-test.js',
		
		'Scripts/Report/thirdpartywinloss*.js',
		'Scripts/jasmine-specs/Report/thirdpartywinloss*-test.js',
		 
		'Scripts/Report/turnover*.js',
		'Scripts/jasmine-specs/Report/turnover*-test.js',
		 
		'Scripts/Report/winlossbybettype*.js',
		'Scripts/jasmine-specs/Report/winlossbybettype*-test.js',
		 
		'Scripts/Report/winlossbyleague*.js',
		'Scripts/jasmine-specs/Report/winlossbyleague*-test.js',
		 
		'Scripts/Report/winlossbyproduct*.js',
		'Scripts/jasmine-specs/Report/winlossbyproduct*-test.js',
		
		 // --------- TotalBets --------- //
		'Scripts/TotalBets/*.js',
		 'Scripts/jasmine-specs/TotalBets/*-test.js',
		 
		  // --------- WareHouse --------- //
		'Scripts/WareHouse/*.js',
		 'Scripts/jasmine-specs/WareHouse/*-test.js',
    ],
    exclude: [
    ],
    preprocessors: {
    },
    reporters: ['progress'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['PhantomJS'],
    singleRun: false,
    concurrency: Infinity,
	phantomjsLauncher: {
      // Have phantomjs exit if a ResourceError is encountered (useful if karma exits without killing phantom) 
      exitOnResourceError: true
    }
  })
}
