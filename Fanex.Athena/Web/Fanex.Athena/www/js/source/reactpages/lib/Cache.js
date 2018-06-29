/*
  cache.set('foo', 'bar');
  cache.get('foo'); //'bar'
*/
let _cache = {};

export default class Cache {      
    static get(key, defaultValue = null) {
        return _cache[key] || defaultValue;
    }

    static set(key, value) {
        key && (_cache[key] = value);
    }
};