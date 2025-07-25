import type { ProductionOrderStatus } from "./ProductionOrderStatus";

export interface ProductionOrderUpdateDto {
  productId: number;
  quantity: number;
  status: ProductionOrderStatus;
}