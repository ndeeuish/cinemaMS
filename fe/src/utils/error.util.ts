export const getErrorMessage = (error: any, defaultMessage = 'Có lỗi xảy ra'): string => {
  if (!error || !error.response || !error.response.data) {
    return defaultMessage;
  }

  const data = error.response.data;

  // 1. FluentValidation errors
  if (data.errors && typeof data.errors === 'object') {
    const firstKey = Object.keys(data.errors)[0];
    if (firstKey && Array.isArray(data.errors[firstKey]) && data.errors[firstKey].length > 0) {
      return data.errors[firstKey][0];
    }
  }

  // 2. Custom Business exceptions (details)
  if (data.details && typeof data.details === 'string') {
    return data.details;
  }

  // 3. Fallback message
  if (data.message && typeof data.message === 'string') {
    return data.message;
  }
  
  if (data.title && typeof data.title === 'string') {
    return data.title;
  }

  return defaultMessage;
};
