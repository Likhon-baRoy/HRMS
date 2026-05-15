export function getApiError(err: any, fallback = 'Operation failed'): string {
  const errors = err?.error?.errors;

  if (errors) {
    const firstKey = Object.keys(errors)[0];

    if (firstKey && errors[firstKey] && errors[firstKey][0]) {
      return errors[firstKey][0];
    }
  }

  return fallback;
}
