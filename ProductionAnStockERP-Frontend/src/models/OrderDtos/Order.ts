import type { OrderStatus } from "./OrderStatus";

export interface Order {
  orderId: number;
  productName: string;
  customerName: string;
  quantity: number;
  status: OrderStatus;
  createdAt: string;
  userId: number;
}


