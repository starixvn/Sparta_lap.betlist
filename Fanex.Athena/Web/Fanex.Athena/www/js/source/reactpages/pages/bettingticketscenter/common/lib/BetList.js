import Site from '../../../../lib/site'
import QueryString from '../../../../lib/QueryString'

const BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29, BETTYPE_MIXPARLAY_BETRADAR = 2799;
const MIN_BETTYPE_FASTMARKET = 221;
const MAX_BETTYPE_FASTMARKET = 227;

export default class BetList {

    isFastMarket(bettype) {
        return bettype >= MIN_BETTYPE_FASTMARKET && bettype <= MAX_BETTYPE_FASTMARKET;
    }

    getMixParlayResultUrl(refno, winlostdate, bettype) {
        let url = Site.resolveClientUrl('ResultReport/ResultPopup');
        url = QueryString.setParam(url, "refno", refno);
        url = QueryString.setParam(url, "winlossdate", winlostdate);
        url = QueryString.setParam(url, "bettype", bettype);
        url = QueryString.setParam(url, "matchid", 1);

        return url;
    }

    getSportBookResultUrl (matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
        let url = Site.resolveClientUrl('ResultReport/ResultPopup');
        url = QueryString.setParam(url, "matchid", matchid);
        url = QueryString.setParam(url, "bettype", bettype);
        url = QueryString.setParam(url, "raceno", raceNo);
        url = QueryString.setParam(url, "sporttype", sporttype);
        url = QueryString.setParam(url, "isoutright", isOutright);

        if (this.isFastMarket(bettype)) {
            url = QueryString.setParam(url, "handicap1", extension1);
            url = QueryString.setParam(url, "handicap2", extension2);
        } else {
            url = QueryString.setParam(url, "betid", extension1);
        }

        return url;
    }

    getResultUrl (matchid, raceNo, bettype, sporttype, refno,
        username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) {
            bettype = Number(bettype);
        console.log(matchid);
        let isParlay = bettype === BETTYPE_MIXPARLAY || bettype === BETTYPE_SYSTEMPARLAY || bettype === BETTYPE_MIXPARLAY_BETRADAR;
        let url = isParlay
                ? this.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype)
                : this.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

            return url;
   }
}