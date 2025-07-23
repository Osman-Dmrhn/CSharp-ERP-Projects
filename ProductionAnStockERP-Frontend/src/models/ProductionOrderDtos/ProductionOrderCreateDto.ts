export interface ProductionOrderCreateDto {
  orderId?: number | null;
  productName: string;
  quantity: number;
}