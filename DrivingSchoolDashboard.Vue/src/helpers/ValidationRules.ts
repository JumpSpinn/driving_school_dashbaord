export type ValidationRule = (value: string) => string | null;

export const Rules = {
  required: (): ValidationRule =>
    (value) => !value?.trim() ? "Dieses Feld ist erforderlich" : null,

  minLength: (min: number) : ValidationRule =>
    (value) => value && value.length < min ? `Mindestens ${min} Zeichen erforderlich.` : null,

  maxLength: (max: number): ValidationRule =>
    (value) => value && value.length > max ? `Maximal ${max} Zeichen erlaubt.` : null,

  email: (): ValidationRule =>
    (value) => value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value) ? 'Ungültige E-Mail-Adresse.' : null,

  pattern: (regex: RegExp, message: string): ValidationRule =>
    (value) => value && !regex.test(value) ? message : null,
}
