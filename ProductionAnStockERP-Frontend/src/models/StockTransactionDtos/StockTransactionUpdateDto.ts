import type { TransactionType } from "./StockTransaction";

export interface StockTransactionUpdateDto {
  productId: number;
  quantity: number;
  transactionType: TransactionType;
  relatedOrderId?: number;
}