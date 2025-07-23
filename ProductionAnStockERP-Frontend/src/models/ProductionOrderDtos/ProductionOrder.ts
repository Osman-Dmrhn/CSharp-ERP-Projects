import type { ProductionOrderStatus } from "./ProductionOrderStatus";

export interface ProductionOrder {
  productionId: number;
  orderId: number | null;
  productName: string;
  quantity: number;
  status: ProductionOrderStatus;
  createdBy: number;
  createdAt: string;
}