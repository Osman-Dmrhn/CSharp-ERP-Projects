import type { OrderStatus } from "./OrderStatus";

export interface OrderUpdateDto {
  productId: number; 
  customerName: string;
  status: OrderStatus;
  quantity: number;
}