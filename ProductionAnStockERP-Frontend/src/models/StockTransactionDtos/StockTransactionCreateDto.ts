import type { TransactionType } from "./StockTransaction";

export interface StockTransactionCreateDto {
  productId: number;
  quantity: number;
  transactionType: TransactionType;
  relatedOrderId?: number;
}
