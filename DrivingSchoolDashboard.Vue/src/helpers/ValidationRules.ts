export type ValidationRule = (value: string) => string | null;

export const Rules = {
  required: (): ValidationRule =>
    (value) => (value === null || value === undefined || String(value).trim() === "")
      ? "Dieses Feld ist erforderlich"
      : null,

  minLength: (min: number) : ValidationRule =>
    (value) => value && value.length < min ? `Mindestens ${min} Zeichen erforderlich.` : null,

  maxLength: (max: number): ValidationRule =>
    (value) => value && value.length > max ? `Maximal ${max} Zeichen erlaubt.` : null,

  minValue: (min: number): ValidationRule =>
    (value) => value && Number(value) < min ? `Mindestens ${min} erforderlich.` : null,

  maxValue: (max: number): ValidationRule =>
    (value) => value && Number(value) > max ? `Maximal ${max} erlaubt.` : null,

  email: (): ValidationRule =>
    (value) => value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value) ? 'Ungültige E-Mail-Adresse.' : null,

  timeHHmmss: (): ValidationRule =>
    (value) => value && !/^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/.test(value)
      ? "Ungültiges Zeitformat. Erwartet: HH:mm:ss"
      : null,

  pattern: (regex: RegExp, message: string): ValidationRule =>
    (value) => value && !regex.test(value) ? message : null,
}
