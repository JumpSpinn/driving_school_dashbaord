export function useForm(fields: { validate: () => boolean }[]) {
  const validate = (): boolean => {
    const results = fields
      .map((field) =>
        field.validate());

    return results.every(Boolean);
  };

  const reset = () =>
    fields.forEach((f) => ('reset' in f ? (f as any).reset() : null));

  return {
    validate,
    reset
  };
}
