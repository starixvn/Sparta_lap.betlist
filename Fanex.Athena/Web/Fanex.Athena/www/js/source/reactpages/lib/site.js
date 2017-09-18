const _rootUrl = window.rootUrl || "/";

export default class Site {
    static resolveClientUrl(relativeUrl) {
        return _rootUrl + relativeUrl;
    }
}