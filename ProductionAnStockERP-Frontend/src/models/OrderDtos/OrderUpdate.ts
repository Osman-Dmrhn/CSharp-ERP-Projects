import type { OrderStatus } from "./OrderStatus";

export interface OrderUpdateDto {
  productName?: string;
  customerName:string;
  status?: OrderStatus;
  quantity: number;
}