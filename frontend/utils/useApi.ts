import { hash } from "ohash";
import { useToast } from "@/components/ui/toast/use-toast";

export const useApi = function (request: string, opts: Record<string, any>) {
  const config = useRuntimeConfig();
  const { toast } = useToast();
  const authStore = useAuthStore();
  const router = useRouter();

  return useFetch(request, {
    baseURL: config.public.API_URL,
    onRequest({ request, options }) {},
    onRequestError(context) {},
    onResponse({ request, response, options }) {},
    async onResponseError({ request, response, options }) {
      if (response.status === 401) {
        $fetch("/api/Auth/LoginRefreshToken", {
          baseURL: config.public.API_URL,
          credentials: "include",
        })
          .then(async () => {
            return await useApi(String(request), {
              ...options,
              method: options.method as
                | "get"
                | "GET"
                | "HEAD"
                | "PATCH"
                | "POST"
                | "PUT"
                | "DELETE"
                | "CONNECT"
                | "OPTIONS"
                | "TRACE"
                | "head"
                | "patch"
                | "post"
                | "put"
                | "delete"
                | "connect"
                | "options"
                | "trace"
                | undefined,
              headers: {
                ...options.headers,
              },
            });
          })
          .catch(() => {
            $fetch("/api/Auth/Logout", {
              baseURL: config.public.API_URL,
              credentials: "include",
            });
            authStore.clearIsAuthenticated();
            router.push("/");
          });
      } else {
        let errorMessage = "";

        if (response.status === 422) {
          errorMessage = response._data.errors[0].error;
        } else {
          errorMessage = response._data.title;
        }

        toast({
          variant: "destructive",
          description:
            getErrorMessage(errorMessage) ??
            "Coś poszło nie tak :( Juz działamy",
        });
      }
    },
    credentials: "include",
    key: hash([
      "webapi-fetch",
      request,
      opts?.body,
      opts?.params,
      opts?.method,
      opts?.query,
    ]),
    ...opts,
  });
};

const errorMessages: { [key: string]: string } = {
  InvalidPhoneNumberFormat: "Podaj poprawny numer telefonu",
  InvalidEmailOrPassword: "Nie poprawny email lub hasło",
  ClientAlreadyExist: "Klient juz istnieje",
  Unauthorized: "Brak dostępu",
};

function getErrorMessage(errorCode: string): string {
  return errorMessages[errorCode];
}
