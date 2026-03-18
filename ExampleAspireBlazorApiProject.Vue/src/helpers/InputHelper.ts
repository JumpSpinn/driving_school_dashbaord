import { BaseUtil } from "@/utils/BaseUtil";

export class InputHelper {
  private static MIN_TEXTAREA_ROWS = 4;
  private static MAX_TEXTAREA_ROWS = 20;

  public static getInputId(id: string | undefined) {
    if (BaseUtil.isNullOrUndefined(id))
      // return crypto.randomUUID();
      // TODO: own UUID stuff writen alla
      return 'unknown';
    return id;
  }

  public static getInputPlaceholder(
    placeholder: string | undefined,
    label: string | undefined,
    disabled: boolean,
    required: boolean
  ) {
    let val: string = placeholder ?? label ?? '';

    val = val.replace(':', '');

    if (disabled) val = val.concat(' (deaktiviert)');
    else if (required) val = val.concat(' (erforderlich)');
    else val += '..';

    return val;
  }

  public static getInputResize(both: boolean, horizontal: boolean, vertical: boolean) {
    if (both || (vertical && horizontal)) return 'resizeBoth';

    if (vertical) return 'resizeVertical';

    if (horizontal) return 'resizeHorizontal';

    return '';
  }

  public static getInputRows(rows: number | undefined) {
    if (BaseUtil.isNullOrUndefined(rows)) return this.MIN_TEXTAREA_ROWS;
    return BaseUtil.clamp(rows, this.MAX_TEXTAREA_ROWS, this.MIN_TEXTAREA_ROWS);
  }

  static clampValue(value: number, min?: number | string, max?: number | string): number {
    let clamped = value;
    if (min !== undefined && min !== '') clamped = Math.max(Number(min), clamped);
    if (max !== undefined && max !== '') clamped = Math.min(Number(max), clamped);
    return clamped;
  }
}
