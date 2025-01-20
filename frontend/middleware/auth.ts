export default defineNuxtRouteMiddleware(async (to, from) => {
  const salonStore = useSalonStore();

  if (import.meta.server) {
    const token = useCookie("GROOMER-AUTH");
    if (!token.value) {
      return navigateTo("/");
    }
  }

  if (import.meta.client) {
    const authStore = useAuthStore();
    const router = useRouter();
    if (!authStore.isAuthenticated) {
      return router.push("/");
    }
  }

  if (to.path === "/dashboard") {
    await salonStore.loadSalons();
  }
});
