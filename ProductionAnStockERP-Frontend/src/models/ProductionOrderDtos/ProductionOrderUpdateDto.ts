import type { ProductionOrderStatus } from "./ProductionOrderStatus";

export interface ProductionOrderUpdateDto {
  productName: string;
  quantity: number;
  status: ProductionOrderStatus;
}