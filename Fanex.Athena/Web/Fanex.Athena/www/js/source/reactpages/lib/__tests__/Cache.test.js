import Cache from '../Cache'
jest.useFakeTimers();
describe('Cache ', () => {
    afterEach(() =>{
        Cache.clear();
    });

    it('set/get - works properly', () => {
        Cache.set("abc", 123);

        const result = Cache.get("abc");

        expect(result).toBe(123);
    })

    it('get with default value - works properly', () => {
        const result = Cache.get("abc", 999);

        expect(result).toBe(999);
    })

    it('Cache should be expired properly', () => {
        Cache.set("abc", 999, 2);

        jest.runAllTimers();

        expect(Cache.get('abc')).toBe(null);
        expect(Cache.size()).toBe(0);
        expect(setTimeout).toHaveBeenLastCalledWith(expect.any(Function), 120000);
    })
})