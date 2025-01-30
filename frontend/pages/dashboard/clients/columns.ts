import { h } from "vue";
import type { ColumnDef } from "@tanstack/vue-table";
import type { Client } from "~/types/type";
import DataTableDropDown from "./DataTableDropDown.vue";

export const columns: ColumnDef<Client>[] = [
  {
    accessorKey: "firstName",
    header: () => h("div", { class: "text-right" }, "Imię"),
    cell: ({ row }) => {
      return h(
        "div",
        { class: "text-right font-medium" },
        row.getValue("firstName")
      );
    },
  },
  {
    accessorKey: "lastName",
    header: () => h("div", { class: "text-right" }, "Nazwisko"),
    cell: ({ row }) => {
      return h(
        "div",
        { class: "text-right font-medium" },
        row.getValue("lastName")
      );
    },
  },
  {
    accessorKey: "phoneNumber",
    header: () => h("div", { class: "text-right" }, "Numer telefonu"),
    cell: ({ row }) => {
      const phoneNumber = formatPhoneNumber(row.getValue("phoneNumber"));

      return h("div", { class: "text-right font-medium" }, phoneNumber);
    },
  },
  {
    accessorKey: "email",
    header: () => h("div", { class: "text-right" }, "Adres email"),
    cell: ({ row }) => {
      const emailAddress = formatEmailAddress(row.getValue("email"));

      return h("div", { class: "text-right font-medium" }, emailAddress);
    },
  },
  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const client = row.original;

      return h(
        "div",
        { class: "relative" },
        h(DataTableDropDown, {
          client,
        })
      );
    },
  },
];
