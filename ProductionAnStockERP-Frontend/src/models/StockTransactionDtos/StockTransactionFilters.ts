import type { TransactionType } from "./StockTransaction";
export interface StockTransactionFilters {
  pageNumber?: number;
  pageSize?: number;
  productId?: number;
  transactionType?: TransactionType;
}