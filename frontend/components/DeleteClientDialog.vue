<template>
  <AlertDialog>
    <AlertDialogTrigger as-child>
      <Button :variant="buttonVariant" :class="buttonClass">
        <Trash2 class="w-4 h-4 text-red-500" />
        Usuń klienta
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent>
      <AlertDialogHeader>
        <AlertDialogTitle>Czy aby na pewno?</AlertDialogTitle>
        <AlertDialogDescription>
          Tej akcji nie można cofnąć. Spowoduje to trwałe usunięcie klienta:
          <span class="text-red-500 font-semibold">
            {{ clientsStore.getStoredClientById(props.clientId!)?.firstName }}

            {{ clientsStore.getStoredClientById(props.clientId!)?.lastName }}
          </span>
        </AlertDialogDescription>
      </AlertDialogHeader>
      <AlertDialogFooter>
        <Button
          variant="destructive"
          @click="
            props.clientId !== undefined &&
              deleteClient(props.clientId.toString())
          "
        >
          <Spinner v-if="clientsStore.isLoading" />
          <Save v-else />
          Usuń
        </Button>
        <AlertDialogCancel @click="open = false">Anuluj</AlertDialogCancel>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>

<script setup lang="ts">
import { Trash2, Save } from "lucide-vue-next";
import { string } from "zod";

const { open } = useDialogHelper();

const clientsStore = useClientsStore();

const successSubmitting = () => {
  open.value = false;
  props.onSuccessCallback();
};

const deleteClient = async (clientId: string) => {
  await clientsStore.deleteClient(clientId, successSubmitting);
};

const props = defineProps({
  clientId: {
    type: String,
  },
  buttonVariant: {
    type: String as PropType<
      "link" | "default" | "destructive" | "outline" | "secondary" | "ghost"
    >,
    default: "default",
  },
  buttonClass: {
    type: String,
    default: "",
  },
  onSuccessCallback: {
    type: Function as PropType<(args?: any) => void>,
    default: () => {},
  },
});
</script>
