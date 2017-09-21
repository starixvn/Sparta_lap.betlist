const _rootUrl = window.rootUrl || "/";

export default class Site {
    static resolveClientUrl(relativeUrl) {
        return _rootUrl + relativeUrl;
    }

    static convertDateToString(date) {
        return typeof date === "string" ? date : date.format("MM/DD/YYYY");
    }
}