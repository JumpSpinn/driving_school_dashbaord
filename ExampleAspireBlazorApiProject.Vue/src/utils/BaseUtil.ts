// noinspection SpellCheckingInspection

export class BaseUtil {
  //#region NULL, UNDEFINED, OBJECT
  public static isNullOrUndefined<T = any>(o: T | undefined | null): o is undefined | null {
    return o === null || o === undefined;
  }

  public static isUndefinedOrEmptyObject(obj: any) {
    if (BaseUtil.isNullOrUndefined(obj)) return true;
    else if (typeof obj != 'object') return true;
    else !Object.keys(obj).length;
  }

  public static isAnyNullOrUndefined(a: any[]): boolean {
    if (!Array.isArray(a)) return true;
    return a.includes(null) || a.includes(undefined);
  }

  public static clamp(val: number, max: number, min: number): number {
    return Math.min(Math.max(val, min), max);
  }

  public static convertRanges(
    oldVal: number,
    oldMin: number,
    oldMax: number,
    newMin: number,
    newMax: number
  ): number {
    return ((oldVal - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
  }

  public static convertMsToTime(milliseconds: number): string {
    const ts: number = Math.floor(milliseconds / 1_000); // total seconds
    const ms: number = milliseconds % 1_000; // ms
    const m: number = Math.floor(ts / 60); // minutes
    const s: number = ts % 60; // seconds
    return `${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}:${String(ms).padStart(3, '0')}`;
  }

  public static deepCopy<T = any>(a: T): T {
    // Prevent undefined objects
    if (BaseUtil.isNullOrUndefined(a)) return a;
    else if (typeof a != 'object') return a;
    else {
      const bObject: any = Array.isArray(a) ? [] : {};
      let value;
      for (const key in a) {
        if (Object.is(a[key], a)) continue;

        value = a[key];
        bObject[key] = typeof value === 'object' ? BaseUtil.deepCopy(value) : value;
      }
      return bObject;
    }
  }

  public static jsonCopy<T = any>(a: T): T {
    return JSON.parse(JSON.stringify(a));
  }

  public static merge<T = any>(a: T, b: T): T {
    const result = { ...a };
    for (const key in b) {
      if (Object.prototype.hasOwnProperty.call(b, key)) {
        const aValue = a[key];
        const bValue = b[key];
        if (BaseUtil.isNullOrUndefined(aValue)) continue;
        else if (BaseUtil.isNullOrUndefined(bValue)) result[key] = aValue;
        else if (typeof aValue != typeof bValue) result[key] = aValue;
        else if (Array.isArray(aValue) && !Array.isArray(bValue)) result[key] = aValue;
        else if (Array.isArray(aValue) && Array.isArray(bValue)) result[key] = bValue;
        else if (aValue && typeof bValue === 'object' && typeof aValue === 'object')
          result[key] = BaseUtil.merge(aValue, bValue);
        else result[key] = bValue;
      }
    }
    return result as T;
  }

  public static satisfiesInterface<T extends object>(value: object): value is T {
    return Object.keys(value).every((key) => {
      if (key in (value as T)) {
        // Check the type of each property
        return typeof (value as any)[key] === typeof (value as any)[key];
      }
      return false;
    });
  }
  //#endregion

  //#region STRING
  public static makeStringSafeForHTML(str: string): string {
    if (BaseUtil.isNullOrUndefined(str)) return '';
    else {
      const map: Record<string, string> = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;',
      };
      // @ts-ignore
      return str.replace(/[&<>"']/g, (m) => map[m]);
    }
  }

  public static randomString(
    length: number,
    numbers = true,
    lowercase = true,
    uppercase = true
  ): string {
    let result = '';
    let chars = '';
    if (numbers) chars += '0123456789';
    if (lowercase) chars += 'abcdefghijklmnopqrstuvwxyz';
    if (uppercase) chars += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    for (let i = length; i > 0; --i) {
      result += chars[Math.round(Math.random() * (chars.length - 1))];
    }
    return result;
  }

  public static stringContainsOnlyWhiteSpace(str: string): boolean {
    // eslint-disable-next-line quotes
    if (!str.replace(/\s/g, '').length) {
      return true;
    }
    return false;
  }

  public static isStringOnlyNumbers(s: string, allowNegative = false): boolean {
    if (!allowNegative) return /^\d+$/.test(s);
    else return /^-?\d+$/.test(s);
  }

  public static insert(str: string, index: number, value: string) {
    return str.substring(0, index) + value + str.substring(index);
  }

  public static parseAmount(am: number | undefined): string {
    if (am == undefined) return '';
    let amStr = am.toString();
    if (am >= 0) {
      while (amStr.length < 3) {
        amStr = BaseUtil.insert(amStr, 0, '0');
      }
      amStr = BaseUtil.insert(amStr, amStr.length - 2, ',');
    } else {
      amStr = amStr.substring(1);
      while (amStr.length < 3) {
        amStr = BaseUtil.insert(amStr, 0, '0');
      }
      amStr = BaseUtil.insert(amStr, amStr.length - 2, ',');
      amStr = '-' + amStr;
    }
    return amStr;
  }

  public static substringStringFromZeroToLength(str: string, length: number): string {
    return str.length > length ? `${str.substring(0, length)}..` : str;
  }

  public static parseKilometerAmount(am: number): string {
    const kilometers = am / 1_000;
    let formattedKilometers: string;
    if (kilometers > 9999) {
      formattedKilometers = kilometers.toFixed(0);
    } else if (kilometers > 999) {
      formattedKilometers = kilometers.toFixed(1);
    } else {
      formattedKilometers = kilometers.toFixed(2);
    }
    return formattedKilometers.padStart(5, ' ');
  }

  public static checkStringLength(s: string, l: number): boolean {
    if (BaseUtil.isAnyNullOrUndefined([s, l])) return false;
    return !(s.length < 1 || s.length > l);
  }

  public static hammingDistance(str1: string, str2: string): number {
    const len1 = str1.length;
    const len2 = str2.length;
    const maxLength = Math.max(len1, len2);

    let distance = Math.abs(len1 - len2);
    for (let i = 0; i < maxLength; i++) {
      if (str1[i] !== str2[i]) {
        distance++;
      }
    }

    return distance;
  }
  //#endregion

  //#region NUMBERS
  public static padNumber(num: number, length: number): string {
    let s = num + '';
    if (s.length > length) {
      return s.slice(0, length);
    }
    while (s.length < length) s = '0' + s;
    return s;
  }

  public static twoDigits(d: number): string {
    if (0 <= d && d < 10) return '0' + d.toString();
    if (-10 < d && d < 0) return '-0' + (-1 * d).toString();
    return d.toString();
  }

  public static round(value: any, exp: any) {
    if (typeof exp === 'undefined' || +exp === 0) {
      return Math.round(value);
    }
    value = +value;
    exp = +exp;
    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
      return NaN;
    }
    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? +value[1] + exp : exp)));
    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? +value[1] - exp : -exp));
  }

  public static numberToBoundary(n: number, lower: number, upper: number): number {
    return n < lower ? lower : n > upper ? upper : n;
  }

  public static randomInt(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1) + min);
  }

  public static randomBool(): boolean {
    return this.randomInt(0, 1) == 1;
  }

  public static probableSuccess(percentage: number): boolean {
    return Math.random() <= percentage / 100;
  }

  public static isInt32(value: number): boolean {
    return value >= -2147483648 && value <= 2147483647;
  }

  public static modifyPercental(val: number, percentage: number): number {
    return (val / 100) * percentage + val;
  }

  public static getPercental(val: number, max: number): number {
    if (val <= 0) return 0;
    else if (max <= 0) return 100;
    else return Math.floor((val / max) * 100);
  }
  //#endregion

  //#region ENUM
  public static enumKeys<O extends Record<string, unknown>, K extends keyof O = keyof O>(
    obj: O
  ): K[] {
    return Object.keys(obj).filter((k) => Number.isNaN(+k)) as K[];
  }

  public static enumToArray<O extends Record<string, unknown>, K extends keyof O = keyof O>(
    obj: O
  ): O[K][] {
    const keys = Object.keys(obj).filter((k) => Number.isNaN(+k)) as K[];
    return keys.map((k) => obj[k]);
  }

  public static isValidEnumValue<O extends Record<string, unknown>>(obj: O, value: any): boolean {
    return Object.values(obj).includes(value);
  }
  //#endregion

  //#region MAP
  public static getKeyOfMapByValue<T>(m: Map<T, any>, val: any): T | undefined {
    if (BaseUtil.isNullOrUndefined(m)) return undefined;
    else {
      for (const [k, v] of m) {
        if (v == val) return k;
      }
      return undefined;
    }
  }

  public static filterMap<T, U>(map: Map<T, U>, predicate: (value: U) => boolean): Map<T, U> {
    return new Map(Array.from(map).filter(([_key, value]) => predicate(value)));
  }

  public static findMap<T, U>(
    map: Map<T, U>,
    predicate: (value: U) => boolean
  ): { key: T; value: U } | undefined {
    const res = Array.from(map).find(([_key, value]) => predicate(value));
    if (!res) return undefined;

    return {
      key: res[0],
      value: res[1],
    };
  }

  public static existsInMap(map: Map<any, any>, val: any) {
    for (const v of map.values()) {
      if (v === val) {
        return true;
      }
    }
    return false;
  }

  //#endregion

  //#region ARRAY
  public static chunkArray<T = any>(a1: T[], chunkSize: number): T[][] {
    const chunkedArray: any[][] = [];
    for (let i = 0; i < a1.length; i += chunkSize) {
      const chunk = a1.slice(i, i + chunkSize);
      chunkedArray.push(chunk);
    }
    return chunkedArray;
  }

  public static randomFromArray<T = any>(a: T[]): [T, number] {
    const random = Math.floor(Math.random() * a.length);
    // @ts-ignore
    return [a[random], random];
  }

  public static shuffleArray<T = any>(a: T[], copy = false): T[] {
    let j, x, i: number;
    for (i = a.length - 1; i > 0; i--) {
      j = Math.floor(Math.random() * (i + 1));
      x = a[i];
      // @ts-ignore
      a[i] = a[j];
      // @ts-ignore
      a[j] = x;
    }
    return copy ? JSON.parse(JSON.stringify(a)) : a;
  }

  public static areArraysDifferent(a1: any[], a2: any[]): boolean {
    if (BaseUtil.isNullOrUndefined(a1) || BaseUtil.isNullOrUndefined(a2)) {
      return true;
    }
    if (!Array.isArray(a1) || !Array.isArray(a2)) {
      return true;
    }
    if (a1.length != a2.length) {
      return true;
    }
    for (let i = 0; i < a1.length; i++) {
      if (typeof a1[i] != typeof a2[i]) {
        return true;
      }
      if (Array.isArray(a1[i]) && Array.isArray(a2[i])) {
        if (BaseUtil.areArraysDifferent(a1[i], a2[i])) {
          return true;
        }
      } else if (typeof a1[i] == 'object') {
        if (BaseUtil.areObjectsDifferent(a1[i], a2[i])) {
          return true;
        }
      } else {
        if (a1[i] != a2[i]) {
          return true;
        }
      }
    }
    return false;
  }

  public static makeArrayUnique<T = any>(a: T[]): T[] {
    return Array.from(new Set(a));
  }

  public static swapElements<T>(array: T[], index1: number, index2: number): T[] {
    // @ts-ignore
    [array[index1], array[index2]] = [array[index2], array[index1]];
    return array;
  }

  public static moveElementAfter<T>(a: T[], index1: number, index2: number): T[] {
    if (index1 < 0 || index2 < 0 || index1 >= a.length || index2 >= a.length) return a;
    else {
      const element = a[index1];
      a.splice(index1, 1);
      if (index1 < index2) index2--;
      // @ts-ignore
      a.splice(index2 + 1, 0, element);
      return a;
    }
  }

  public static moveElementBefore<T>(a: T[], index1: number, index2: number): T[] {
    if (index1 < 0 || index2 < 0 || index1 >= a.length || index2 >= a.length) return a;
    else {
      const element = a[index1];
      a.splice(index1, 1);
      if (index1 < index2) index2--;
      // @ts-ignore
      a.splice(index2, 0, element);
      return a;
    }
  }

  public static elementsRemovedOrAdded<T>(before: T[], after: T[]): { added: T[]; removed: T[] } {
    const added: T[] = [];
    const removed: T[] = [];

    const beforeMap = new Map<T, number>();

    for (const element of before) {
      beforeMap.set(element, (beforeMap.get(element) ?? 0) + 1);
    }

    for (const element of after) {
      if (beforeMap.has(element)) {
        const count = beforeMap.get(element)! - 1;
        if (count === 0) beforeMap.delete(element);
        else beforeMap.set(element, count);
      } else added.push(element);
    }

    beforeMap.forEach((count, element) => {
      for (let i = 0; i < count; i++) {
        removed.push(element);
      }
    });

    return { added, removed };
  }
  //#endregion

  //#region SET
  public static areSetsEqual(s1: Set<any>, s2: Set<any>): boolean {
    if (BaseUtil.isAnyNullOrUndefined([s1, s2])) return false;
    else if (s1.size != s2.size) return false;
    else return [...s1].every((v) => s2.has(v));
  }
  //#endregion

  //#region OBJECT
  public static areObjectsDifferent(o1: object, o2: object): boolean {
    if (BaseUtil.isNullOrUndefined(o1) || BaseUtil.isNullOrUndefined(o2)) {
      return true;
    }
    for (const [key1, val1] of Object.entries(o1)) {
      if (!Object.keys(o2).includes(key1)) {
        return true;
      }
      for (const [key2, val2] of Object.entries(o2)) {
        if (key1 == key2) {
          if (typeof val1 != typeof val2) {
            return true;
          } else if (Array.isArray(val1) && Array.isArray(val2)) {
            if (BaseUtil.areArraysDifferent(val1, val2)) {
              return true;
            }
          } else if (typeof val1 == 'object' && typeof val2 == 'object') {
            if (BaseUtil.areObjectsDifferent(val1, val2)) {
              return true;
            }
          } else {
            if (val1 != val2) {
              return true;
            }
          }
        }
      }
    }
    return false;
  }
  //#endregion

  public static replaceAll(str: string, find: string, replace: string): string {
    return str.replace(new RegExp(find, 'g'), replace);
  }

  public static replaceWith(str: string, replace: string): string {
    return str
      .split('')
      .map(() => replace)
      .join('');
  }
}
