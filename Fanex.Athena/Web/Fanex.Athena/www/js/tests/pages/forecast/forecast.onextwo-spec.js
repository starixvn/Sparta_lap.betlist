define(['jquery', 'test-helper', 'common/site', 'components/refreshcountdown', 'common/popup', 'pages/forecast/forecast.onextwo'],
    function ($, TestHelper, Site, RefreshCountDown, Popup, ForecastOneXTwo) {
        describe("ForecastOneXTwo", function () {
            beforeAll(function () {
                //   TestHelper.initGlobalSetting();
                spyOn(Site, "initialize");
                spyOn(ForecastOneXTwo, "resizePopup");
                spyOn(RefreshCountDown, "initialize");
                spyOn(RefreshCountDown, "initEvent");
                spyOn(Site, "resolveClientUrl").and.callFake(function (text) {
                    return text;
                });
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });
            });

            describe("initialize()", function () {
                it("Relevant methods should be called.", function () {
                    spyOn(ForecastOneXTwo, "initEvent");
                    spyOn(Site, "addUserGuide");

                    ForecastOneXTwo.initialize();

                    expect(ForecastOneXTwo.initEvent).toHaveBeenCalled();
                    expect(Site.addUserGuide).toHaveBeenCalledWith({
                        resourceName: "Forecast1x2",
                        title: "Forecast - OneXTwo"
                    });
                    expect(RefreshCountDown.initialize).toHaveBeenCalled();
                    expect(RefreshCountDown.initEvent).toHaveBeenCalled();

                });
            });

            describe("initEvent()", function () {
                it("loadSelectedLeague() should be called with correct params when .league-list raises event change.", function () {
                    setFixtures('<input type="text" class="league-list" value="999" />');
                    spyOn(ForecastOneXTwo, "loadSelectedLeague");

                    ForecastOneXTwo.initEvent();
                    $(".league-list").change();

                    expect(ForecastOneXTwo.loadSelectedLeague).toHaveBeenCalledWith('999');
                });

                it("openForecast() should be called with correct params when .button-add is clicked.", function () {
                    setFixtures('<div id="reportContent"><span class="button-add" data-match-id="123" data-bettype="456"></span></div>');
                    spyOn(ForecastOneXTwo, "openForecast");

                    ForecastOneXTwo.initEvent();
                    $(".button-add").click();

                    expect(ForecastOneXTwo.openForecast).toHaveBeenCalledWith('123', 456);
                });

                it("submit() should be called when #SubmitButton is clicked.", function () {
                    setFixtures('<input id="SubmitButton" type="button" />');
                    spyOn(ForecastOneXTwo, "submit");

                    ForecastOneXTwo.initEvent();
                    $("#SubmitButton").click();

                    expect(ForecastOneXTwo.submit).toHaveBeenCalled();
                });
            });

            describe("openForecast(matchId, betType)", function () {
                it("Popup.updateCurrentPopup() should be called with correct params.", function () {
                    setFixtures('<input type="text" id="IsPopUp" value="true" />');
                    spyOn(Popup, "updateCurrentPopup");

                    ForecastOneXTwo.initialize();

                    ForecastOneXTwo.openForecast(123, 456);

                    expect(Popup.updateCurrentPopup).toHaveBeenCalledWith("RunningMatchBetList/Index?matchid=123&bettype=456",
                        { width: 900, height: 200, title: 'BetList' });
                });
            });
        });
    });