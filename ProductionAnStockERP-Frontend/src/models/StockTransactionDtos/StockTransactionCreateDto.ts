// StockTransactionCreateDto model
export interface StockTransactionCreateDto {
  productName: string;
  quantity: number;
  transactionType: 'Entry' | 'Exit';  // Giriş veya çıkış
  relatedOrderId?: number | null;
}
