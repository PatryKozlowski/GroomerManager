export function formatPhoneNumber(phoneNumber: string) {
  return `+48 ${phoneNumber.replace(/(\d{3})(\d{3})(\d{3})/, "$1 $2 $3")}`;
}

export function formatEmailAddress(email: string) {
  return email ? email : "-";
}

interface DebounceFunction {
  (...args: any[]): void;
}

interface Debounce {
  (func: DebounceFunction, timeout: number): DebounceFunction;
}

export const debounce: Debounce = (func, timeout) => {
  let timeoutId: NodeJS.Timeout;
  return (...args: any[]) => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => {
      func(...args);
    }, timeout);
  };
};
