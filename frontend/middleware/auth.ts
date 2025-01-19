export default defineNuxtRouteMiddleware((to, from) => {
  if (import.meta.client) {
    const authStore = useAuthStore();
    const router = useRouter();
    if (!authStore.isAuthenticated) {
      return router.push("/");
    }
  }

  if (import.meta.server) {
    const token = useCookie("GROOMER-AUTH");
    if (!token.value) {
      return navigateTo("/");
    }
  }
});
