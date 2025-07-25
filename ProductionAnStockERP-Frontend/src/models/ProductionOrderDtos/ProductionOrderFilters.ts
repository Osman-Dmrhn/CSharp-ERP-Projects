import type { ProductionOrderStatus } from "./ProductionOrderStatus";

export interface ProductionOrderFilters {
  pageNumber?: number;
  pageSize?: number;
  productId?: number;
  status?: ProductionOrderStatus;
}