export default defineNuxtRouteMiddleware(async (to, from) => {
  const salonStore = useSalonStore();
  const authStore = useAuthStore();
  const router = useRouter();
  const isEmptySalons = salonStore.salons.length === 0;

  if (!authStore.isAuthenticated) {
    return router.push("/");
  }

  if (salonStore.isLoading || salonStore.salons.length === 0) {
    await salonStore.loadSalons();
  }

  if (to.path === "/auth/salon") {
    if (!isEmptySalons) {
      return router.push("/dashboard");
    }
  }

  if (to.path === "/dashboard") {
    console.log("to dashboard");
    if (isEmptySalons) {
      return router.push("/auth/salon");
    }
  }
});
