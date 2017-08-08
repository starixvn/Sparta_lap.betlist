
export default class LangHelper {
    static get currentLanguage() {
        var langCode = window.PageLanguage || "en-US";
        return "_language_" + langCode.replace("-", "_");

    }
    static single(key) {
        var langHandler = window[currentLanguage.currentLanguage];

        return langHandler[key] || key;
    }
}