export type TransactionType = 'Entry' | 'Exit';

export interface StockTransaction {
  stockTxnId: number;
  productId: number;
  productName: string;
  quantity: number;
  transactionType: TransactionType;
  relatedOrderId?: number;
  createdAt: string;
}