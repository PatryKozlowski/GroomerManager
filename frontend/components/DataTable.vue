<template>
  <div class="border rounded-md">
    <!-- <div class="flex items-center p-4">
      <Input
        class="max-w-sm"
        placeholder="Filter emails..."
        :model-value="table.getColumn('phoneNumber')?.getFilterValue() as string"
        @update:model-value="
          table.getColumn('phoneNumber')?.setFilterValue($event)
        "
      />
    </div> -->
    <Table>
      <TableHeader>
        <TableRow
          v-for="headerGroup in table.getHeaderGroups()"
          :key="headerGroup.id"
        >
          <TableHead v-for="header in headerGroup.headers" :key="header.id">
            <FlexRender
              v-if="!header.isPlaceholder"
              :render="header.column.columnDef.header"
              :props="header.getContext()"
            />
          </TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        <template v-if="table.getRowModel().rows?.length">
          <TableRow
            v-for="row in table.getRowModel().rows"
            :key="row.id"
            :data-state="row.getIsSelected() ? 'selected' : undefined"
          >
            <TableCell v-for="cell in row.getVisibleCells()" :key="cell.id">
              <FlexRender
                :render="cell.column.columnDef.cell"
                :props="cell.getContext()"
              />
            </TableCell>
          </TableRow>
        </template>
        <template v-else>
          <TableRow>
            <TableCell :colspan="columns.length" class="h-24 text-center">
              Brak danych
            </TableCell>
          </TableRow>
        </template>
      </TableBody>
    </Table>
    <div class="flex w-full py-4">
      <div class="space-x-2 flex w-full justify-center">
        <Button
          variant="outline"
          size="sm"
          :disabled="!table.getCanPreviousPage()"
          @click="table.previousPage()"
        >
          <ArrowBigLeft />
        </Button>
        <Button
          variant="outline"
          size="sm"
          :disabled="!table.getCanNextPage()"
          @click="table.nextPage()"
        >
          <ArrowBigRight />
        </Button>
      </div>
      <div class="mr-4">
        <DropdownMenu>
          <DropdownMenuTrigger as-child>
            <Button variant="outline">
              {{ table.getState().pagination.pageSize }}</Button
            >
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuItem
              :key="pageSize"
              :value="pageSize.toString"
              v-for="pageSize in pageSizes"
              @click="table.setPageSize(pageSize)"
            >
              {{ pageSize }}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts" generic="TData, TValue">
import { ArrowBigLeft, ArrowBigRight } from "lucide-vue-next";

import type {
  ColumnDef,
  ColumnFiltersState,
  PaginationOptions,
  PaginationState,
} from "@tanstack/vue-table";

import { FlexRender, getCoreRowModel, useVueTable } from "@tanstack/vue-table";
import { valueUpdater } from "~/lib/utils";

const props = defineProps<{
  columns: ColumnDef<TData, TValue>[];
  data: TData[];
  pagination: PaginationState;
  paginationChange: Pick<PaginationOptions, "onPaginationChange">;
  pageCount: number;
}>();

const columnFilters = ref<ColumnFiltersState>([]);

const table = useVueTable({
  get data() {
    return props.data;
  },
  get columns() {
    return props.columns;
  },
  get pageCount() {
    return props.pageCount ?? 0;
  },
  getCoreRowModel: getCoreRowModel(),
  onPaginationChange: props.paginationChange.onPaginationChange,

  state: {
    get columnFilters() {
      return columnFilters.value;
    },

    pagination: props.pagination,
  },
});

const pageSizes = [5, 10, 20, 30, 40, 50];
</script>
