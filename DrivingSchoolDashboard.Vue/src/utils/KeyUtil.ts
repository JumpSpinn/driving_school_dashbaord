import { Key } from "@/enums/Key.ts";

export interface KeyCallback {
  onPress?: CallableFunction;
  onUnpress?: CallableFunction;
  onShift?: boolean; // Only triggered when shift is pressed
  onAlt?: boolean; // Only triggered when alt is pressed
  onCtrl?: boolean; // Only triggered when ctrl is pressed
  noModifier?: boolean; // If set no other modifier is allowed to be pressed
}

export class KeyUtil {
  public static getKeyCharForKeyID(k: Key): string | undefined {
    switch (k) {
      case Key.MOUSE_LEFT:
        return "Linke Maustaste";
      case Key.MOUSE_MIDDLE:
        return "Mittlere Maustaste";
      case Key.MOUSE_RIGHT:
        return "Rechte Maustaste";
      case Key.ALT:
        return "Alt";
      case Key.PAUSE:
        return "Pause";
      case Key.BACKSPACE:
        return "Backspace";
      case Key.TAB:
        return "Tab";
      case Key.CLEAR:
        return "Clear";
      case Key.ENTER:
        return "Enter";
      case Key.SHIFT:
        return "Umschalt";
      case Key.CTRL:
        return "Strg";
      case Key.CAPSLOCK:
        return "Capslock";
      case Key.SPACEBAR:
        return "Leertaste";
      case Key.F1:
        return "F1";
      case Key.F2:
        return "F2";
      case Key.F3:
        return "F3";
      case Key.F4:
        return "F4";
      case Key.F5:
        return "F5";
      case Key.F6:
        return "F6";
      case Key.F7:
        return "F7";
      case Key.F8:
        return "F8";
      case Key.F9:
        return "F9";
      case Key.F10:
        return "F10";
      case Key.F11:
        return "F11";
      case Key.F12:
        return "F12";
      case Key.NUMLOCK:
        return "Num Lock";
      case Key.ESC:
        return "Esc";
      case Key.PAGEUP:
        return "Bild hoch";
      case Key.PAGEDOWN:
        return "Bild runter";
      case Key.END:
        return "Ende";
      case Key.HOME:
        return "Pos1";
      case Key.LEFT:
        return "Pfeiltaste Links";
      case Key.UP:
        return "Pfeiltaste Oben";
      case Key.RIGHT:
        return "Pfeiltaste Rechts";
      case Key.DOWN:
        return "Pfeiltaste Unten";
      case Key.PRINT:
        return "Druck";
      case Key.INSERT:
        return "Einf";
      case Key.DELETE:
        return "Entf";
      case Key.D0:
        return "0";
      case Key.D1:
        return "1";
      case Key.D2:
        return "2";
      case Key.D3:
        return "3";
      case Key.D4:
        return "4";
      case Key.D5:
        return "5";
      case Key.D6:
        return "6";
      case Key.D7:
        return "7";
      case Key.D8:
        return "8";
      case Key.D9:
        return "9";
      case Key.A:
        return "A";
      case Key.B:
        return "B";
      case Key.C:
        return "C";
      case Key.D:
        return "D";
      case Key.E:
        return "E";
      case Key.F:
        return "F";
      case Key.G:
        return "G";
      case Key.H:
        return "H";
      case Key.I:
        return "I";
      case Key.J:
        return "J";
      case Key.K:
        return "K";
      case Key.L:
        return "L";
      case Key.M:
        return "M";
      case Key.N:
        return "N";
      case Key.O:
        return "O";
      case Key.P:
        return "P";
      case Key.Q:
        return "Q";
      case Key.R:
        return "R";
      case Key.S:
        return "S";
      case Key.T:
        return "T";
      case Key.U:
        return "U";
      case Key.V:
        return "V";
      case Key.W:
        return "W";
      case Key.X:
        return "X";
      case Key.Y:
        return "Y";
      case Key.Z:
        return "Z";
      case Key.NUM_PAD0:
        return "Numpad 0";
      case Key.NUM_PAD1:
        return "Numpad 1";
      case Key.NUM_PAD2:
        return "Numpad 2";
      case Key.NUM_PAD3:
        return "Numpad 3";
      case Key.NUM_PAD4:
        return "Numpad 4";
      case Key.NUM_PAD5:
        return "Numpad 5";
      case Key.NUM_PAD6:
        return "Numpad 6";
      case Key.NUM_PAD7:
        return "Numpad 7";
      case Key.NUM_PAD8:
        return "Numpad 8";
      case Key.NUM_PAD9:
        return "Numpad 9";
      case Key.NUM_MUL:
        return "Numpad *";
      case Key.NUM_ADD:
        return "Numpad +";
      case Key.NUM_SUBTRACT:
        return "Numpad -";
      case Key.NUM_DIVIDE:
        return "Numpad /";
      case Key.NUM_DEC:
        return "Numpad ,";
      case Key.NUM_DEC_2:
        return "Numpad .";
      case Key.ROLL:
        return "Rollen";
      case Key.AE:
        return "Ä";
      case Key.UE:
        return "Ü";
      case Key.SHARP_S:
        return "ẞ";
      case Key.HASHTAG:
        return "#";
      case Key.NORMAL_ADD:
        return "+";
      case Key.MINUS:
        return "-";
      case Key.COMMA:
        return ",";
      case Key.PERIOD:
        return ".";
      case Key.OE:
        return "Ö";
      case Key.CIRCUMFLEX:
        return "^";
      case Key.PLUS:
        return "+";
      case Key.DASH:
        return "-";
      case Key.BACKTICK:
        return "`";
      case Key.HASHTAG_2:
        return "#";
      case Key.TAG:
        return "<";
      default:
        return undefined;
    }
  }

  //#region KEY PRESS
  private static pressedKeys: Set<Key> = new Set();

  private static callbacks: Map<Key, KeyCallback[]> = new Map();
  private static anyCallbacks: ((id: Key) => void)[] = [];

  /**
   * @description Takes a key id and a function
   * Registers the function if the key is pressed.
   * @static
   * @param {KEYID} key
   * @param {Function} callback
   * @return {*}  {() => void}
   * @memberof Key
   */
  static register(key: Key, callback: KeyCallback): () => void {
    if (!KeyUtil.callbacks.has(key)) KeyUtil.callbacks.set(key, []);
    KeyUtil.callbacks.get(key)?.push(callback);

    return () => {
      const index = KeyUtil.callbacks.get(key)?.indexOf(callback) ?? -1;
      if (index !== -1) KeyUtil.callbacks.get(key)?.splice(index, 1);
    };
  }

  /**
   * @description The document callback.
   * Use @function register !
   * @static
   * @param {KeyboardEvent} ev
   * @return {*}
   * @memberof Key
   */
  static async _onkeydown(ev: KeyboardEvent): Promise<void> {
    KeyUtil.pressedKeys.add(ev.keyCode);
    KeyUtil.anyCallbacks.forEach((v) => {
      v(ev.keyCode);
    });
    if (!KeyUtil.callbacks.has(ev.keyCode)) return;
    else KeyUtil.callbacks.get(ev.keyCode)!.forEach((v) => {
      if (v.onAlt && !ev.altKey) return;
      else if (v.onCtrl && !ev.ctrlKey) return;
      else if (v.onShift && !ev.shiftKey) return;
      else if (v.noModifier && (ev.altKey || ev.ctrlKey || ev.shiftKey)) return;
      else if (v.onPress) v.onPress();
    });
  }

  static async _onkeyUp(ev: KeyboardEvent): Promise<void> {
    KeyUtil.pressedKeys.delete(ev.keyCode);
    if (!KeyUtil.callbacks.has(ev.keyCode)) return;
    else KeyUtil.callbacks.get(ev.keyCode)!.forEach((v) => {
      if (v.onUnpress) v.onUnpress();
    });
  }

  static isKeyPressed(id: Key): boolean {
    return KeyUtil.pressedKeys.has(id);
  }
  //#endregion
}

document.onkeydown = KeyUtil._onkeydown;
document.onkeyup = KeyUtil._onkeyUp;
