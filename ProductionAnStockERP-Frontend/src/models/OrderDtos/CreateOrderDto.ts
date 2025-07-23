// Sipariş oluşturma DTO'su
export interface CreateOrderDto {
  customerName:string;
  productName: string;
  quantity: number;
}