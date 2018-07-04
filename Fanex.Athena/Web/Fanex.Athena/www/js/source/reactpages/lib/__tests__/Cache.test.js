import Cache from '../Cache'

describe('Cache ', () => {
    it('set/get - works properly', () => {
        Cache.set("abc", 123);

        const result = Cache.get("abc");

        expect(result).toBe(123);
    })
})