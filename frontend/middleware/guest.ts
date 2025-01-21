export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore();
  const router = useRouter();
  if (authStore.isAuthenticated) {
    return router.push("/dashboard");
  }
});
