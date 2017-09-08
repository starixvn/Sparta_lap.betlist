const DEFAULT_RESOURCE_NAME = "_language_en_US";
const langCode = window.PageLanguage || "en-US";
const resourceName = `_language_${langCode.replace("-", "_")}`;

export default class LangHelper {
    static get currentLanguage() {
        return langCode;
    }

    static getSingleResource(key) {
        var langHandler = window[resourceName];

        return langHandler[key] || window[DEFAULT_RESOURCE_NAME][key] || key;
    }
}