import type { ValidationRule } from '@/helpers/ValidationRules';

export function useField(rules: ValidationRule[] = [], initialValue: string = '') {
  const value = ref<any>(initialValue);
  const error = ref<string | null>(null);

  const validate = (): boolean => {
    for (const rule of rules) {
      const result = rule(value.value);
      if (result) {
        error.value = result;
        return false;
      }
    }
    error.value = null;
    return true;
  };

  const reset = (val: string = initialValue) => {
    value.value = val;
    error.value = null;
  };

  return reactive({
    value,
      error: computed(() => error.value ?? undefined),
    validate,
    reset,
  });
}
