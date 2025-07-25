import type { ProductionOrderStatus } from "./ProductionOrderStatus";

export interface ProductionOrder {
  productionId: number;
  productId: number;
  productName: string;
  quantity: number;
  status: ProductionOrderStatus;
  createdAt: string;
  createdBy: number;
  createdByUserName: string;
  orderId?: number;
}