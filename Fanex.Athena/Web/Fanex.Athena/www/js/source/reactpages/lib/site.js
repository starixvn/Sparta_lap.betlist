const _rootUrl = window.rootUrl || "/";

export function resolveClientUrl(relativeUrl) {
    return _rootUrl + relativeUrl;
}